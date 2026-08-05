// RDMA Client for SMB Direct Compatibility
// Compile with: g++ -o rdma_client rdma_client.cpp -libverbs -lrdmacm -lpthread
#include "rdma_wrapper.h"
#include <rdma/rdma_cma.h>
#include <rdma/rdma_verbs.h>
#include <iostream>
#include <cstring>
#include <cerrno>
#include <mutex>
#include <condition_variable>
#include <vector>
#include <queue>
#include <fstream>
#include <ctime>
#include <iomanip>
#include <sstream>
#include <thread>
#include <unordered_map>
#include <unordered_set>
#include <cstdint>
#include <cstdlib>
#include <atomic>
#include <algorithm>
#include <arpa/inet.h>
#include <memory>
#include <map>

// ------------------- Logger ------------------- //
class Logger {
public:
    static Logger& instance() {
        static Logger logger;
        return logger;
    }

    void log(const std::string& level, const std::string& func, const std::string& msg) {
        std::lock_guard<std::mutex> lock(mutex_);
        if (!file_.is_open()) return;

        auto now = std::chrono::system_clock::now();
        auto ms = std::chrono::duration_cast<std::chrono::milliseconds>(
            now.time_since_epoch()) % 1000;
        std::time_t t = std::chrono::system_clock::to_time_t(now);
        std::tm tm;
#if defined(__unix__) || defined(__APPLE__)
        localtime_r(&t, &tm);
#else
        tm = *std::localtime(&t);
#endif
        std::ostringstream oss;
        oss << std::put_time(&tm, "%Y-%m-%d %H:%M:%S");
        oss << '.' << std::setfill('0') << std::setw(3) << ms.count();

        std::string output = "[" + oss.str() + "] [" + level + "] [T:"
            + get_thread_id_str() + "] [" + func + "] " + msg;

        file_ << output << std::endl;
    }

private:
    Logger() {
        const char* log_path = std::getenv("RDMA_LOG_PATH");
        if (!log_path) log_path = "rdma_debug.log";
        file_.open(log_path, std::ios::out | std::ios::app);
    }
    ~Logger() {
        if (file_.is_open()) file_.close();
    }

    std::string get_thread_id_str() {
        std::ostringstream ss;
        ss << std::this_thread::get_id();
        return ss.str();
    }

    std::mutex mutex_;
    std::ofstream file_;
};

#define LOG_DEBUG(msg) Logger::instance().log("DEBUG", __func__, (msg))
#define LOG_ERROR(msg) Logger::instance().log("ERROR", __func__, (msg))

enum class WrType : uint64_t {
    SEND = 0x1000000000000000ULL,
    RECV = 0x2000000000000000ULL,
    READ = 0x3000000000000000ULL,
    WRITE = 0x4000000000000000ULL,
    BIND = 0x5000000000000000ULL,
    INVALIDATE = 0x6000000000000000ULL
};

static std::atomic<uint64_t> s_seq{ 1 };

uint64_t generate_wr_id(WrType type) {
    uint64_t seq = s_seq.fetch_add(1, std::memory_order_relaxed) & 0x0FFFFFFFFFFFFFFFULL;
    return static_cast<uint64_t>(type) | seq;
}

// ------------------- Helper: Hex Dump ------------------- //
static void log_buffer_hex(const std::string& tag, const void* data, size_t len) {
    const size_t max_dump = 256;
    std::ostringstream oss;
    const uint8_t* bytes = static_cast<const uint8_t*>(data);
    size_t dump_len = std::min(len, max_dump);
    oss << tag << " (" << len << " bytes): ";
    oss << std::hex << std::setfill('0');
    for (size_t i = 0; i < dump_len; ++i) {
        oss << std::setw(2) << static_cast<int>(bytes[i]) << " ";
    }
    if (len > max_dump) oss << "...";
    LOG_DEBUG(oss.str());
}

// ------------------- Per-client MR Registry ------------------- //
struct MrEntry {
    struct ibv_mr* mr;
    uintptr_t start;
    size_t length;
};

struct MwContext {
    struct ibv_mw* mw;
    struct ibv_mr* mr;
    bool is_type_2;
    std::atomic<bool> invalidated{false};
};

struct RdmaClient {
    rdma_event_channel* ec{};
    rdma_cm_id* id{};
    ibv_pd* pd{};
    ibv_cq* cq{};
    ibv_qp* qp{};
    ibv_mr* send_mr{};
    ibv_mr* recv_mr{};

    void* send_buf_aligned{ nullptr };
    void* recv_buf_aligned{ nullptr };

    static constexpr size_t BUF_SZ = 1024 * 1024;

    ibv_comp_channel* comp_channel{};
    std::queue<RdmaCompletion> send_completion_queue;
    std::queue<RdmaCompletion> recv_completion_queue;
    std::mutex completion_mutex;

    std::unordered_set<uint64_t> active_recv_slots;
    std::mutex slot_mutex;

    // Per-client MR registry for fast lookup
    std::mutex mr_mutex;
    std::map<uintptr_t, MrEntry> active_mrs; // sorted by start address for range search

    // General QP-operation mutex
    std::mutex qp_mutex;

    // Connection lifecycle flag
    std::atomic<bool> disconnecting{ false };

    bool valid{ false };

    RdmaClient() {
        if (posix_memalign(&send_buf_aligned, 4096, BUF_SZ) != 0) {
            LOG_ERROR("Failed to allocate aligned send buffer");
            return;
        }
        if (posix_memalign(&recv_buf_aligned, 4096, BUF_SZ) != 0) {
            LOG_ERROR("Failed to allocate aligned recv buffer");
            free(send_buf_aligned);
            send_buf_aligned = nullptr;
            return;
        }
        memset(send_buf_aligned, 0, BUF_SZ);
        memset(recv_buf_aligned, 0, BUF_SZ);
        valid = true;
    }

    ~RdmaClient();

    bool is_valid() const { return valid; }

    void track_mr(struct ibv_mr* mr) {
        if (!mr) return;
        std::lock_guard<std::mutex> lock(mr_mutex);
        MrEntry entry{ mr, reinterpret_cast<uintptr_t>(mr->addr), mr->length };
        active_mrs[entry.start] = entry;
        LOG_DEBUG("Tracked MR: Addr=0x" + std::to_string(entry.start) +
            " Len=" + std::to_string(entry.length) + " LKey=" + std::to_string(mr->lkey));
    }

    void untrack_mr(struct ibv_mr* mr) {
        if (!mr) return;
        std::lock_guard<std::mutex> lock(mr_mutex);
        active_mrs.erase(reinterpret_cast<uintptr_t>(mr->addr));
    }

    bool find_lkey(uintptr_t addr, size_t len, uint32_t& out_lkey) {
        std::lock_guard<std::mutex> lock(mr_mutex);
        // Exact match first
        auto it = active_mrs.find(addr);
        if (it != active_mrs.end()) {
            if (len > it->second.length) return false;
            out_lkey = it->second.mr->lkey;
            return true;
        }
        // Range search using sorted map
        auto candidate = active_mrs.upper_bound(addr);
        if (candidate != active_mrs.begin()) {
            --candidate;
            uintptr_t start = candidate->second.start;
            uintptr_t end = start + candidate->second.length;
            if (addr >= start && (addr + len) <= end) {
                out_lkey = candidate->second.mr->lkey;
                return true;
            }
        }
        return false;
    }

    void clear_mrs() {
        std::lock_guard<std::mutex> lock(mr_mutex);
        active_mrs.clear();
    }
};

#pragma pack(push, 1)
struct SmbDirectNegotiateReq {
    uint16_t MinVersion;
    uint16_t MaxVersion;
    uint16_t Reserved;
    uint16_t CreditsRequested;
    uint16_t PreferredSendSize;
    uint32_t MaxReceiveSize;
    uint32_t MaxFragmentedSize;
};
#pragma pack(pop)

// ------------------- Destructor ------------------- //
RdmaClient::~RdmaClient() {
    disconnecting.store(true, std::memory_order_release);

    if (send_mr) {
        untrack_mr(send_mr);
        ibv_dereg_mr(send_mr);
        send_mr = nullptr;
    }
    if (recv_mr) {
        untrack_mr(recv_mr);
        ibv_dereg_mr(recv_mr);
        recv_mr = nullptr;
    }

    if (id && id->qp) {
        rdma_destroy_qp(id);
    }

    if (id) {
        rdma_destroy_id(id);
        id = nullptr;
    }

    if (cq) {
        ibv_destroy_cq(cq);
        cq = nullptr;
    }

    if (comp_channel) {
        ibv_destroy_comp_channel(comp_channel);
        comp_channel = nullptr;
    }

    if (pd) {
        ibv_dealloc_pd(pd);
        pd = nullptr;
    }

    if (ec) {
        rdma_destroy_event_channel(ec);
        ec = nullptr;
    }

    if (send_buf_aligned) { free(send_buf_aligned); send_buf_aligned = nullptr; }
    if (recv_buf_aligned) { free(recv_buf_aligned); recv_buf_aligned = nullptr; }
}

// ------------------- Helper for SGE ------------------- //
static bool prepare_sge_for_buf(RdmaClient* client, struct ibv_sge& sge, const void* buf, size_t len) {
    if (!client || !buf || len == 0) return false;
    uintptr_t addr = reinterpret_cast<uintptr_t>(buf);
    sge.addr = addr;
    sge.length = static_cast<uint32_t>(len);

    if (client->find_lkey(addr, len, sge.lkey)) {
        return true;
    }

    LOG_ERROR("Buffer NOT registered in client MR map: 0x" + std::to_string(addr) +
        " Len: " + std::to_string(len));
    return false;
}

// ------------------- Poll Helper ------------------- //
static void poll_and_distribute_completions(RdmaClient* client) {
    if (!client || !client->cq) return;

    struct ibv_wc wc[32];
    int num_comp = ibv_poll_cq(client->cq, 32, wc);

    if (num_comp > 0) {
        std::atomic_thread_fence(std::memory_order_acquire);

        std::lock_guard<std::mutex> lock(client->completion_mutex);
        for (int i = 0; i < num_comp; ++i) {
            if (wc[i].status != IBV_WC_SUCCESS) {
                LOG_ERROR("WC failed with status: " + std::to_string(wc[i].status) +
                    " (" + ibv_wc_status_str(wc[i].status) + ")" +
                    " vendor_err: " + std::to_string(wc[i].vendor_err) +
                    " wr_id: " + std::to_string(wc[i].wr_id) +
                    " opcode: " + std::to_string(wc[i].opcode));
            }
            else {
                std::string type = (wc[i].opcode & IBV_WC_RECV) ? "RECV" : "SEND";
                LOG_DEBUG("WC Success: wr_id=" + std::to_string(wc[i].wr_id) +
                    " type=" + type + " bytes=" + std::to_string(wc[i].byte_len));
            }

            RdmaCompletion comp = {};
            comp.wr_id = wc[i].wr_id;
            comp.status = static_cast<uint32_t>(wc[i].status);
            comp.byte_len = (wc[i].status == IBV_WC_SUCCESS) ? wc[i].byte_len : 0;
            comp.qp_num = wc[i].qp_num;
            comp.op_code = static_cast<uint32_t>(wc[i].opcode);
            comp.vendor_err = wc[i].vendor_err;

            if (wc[i].opcode & IBV_WC_RECV) {
                {
                    std::lock_guard<std::mutex> sl(client->slot_mutex);
                    client->active_recv_slots.erase(wc[i].wr_id);
                }
                client->recv_completion_queue.push(comp);
            }
            else {
                client->send_completion_queue.push(comp);
            }
        }
    }
    else if (num_comp < 0) {
        LOG_ERROR("ibv_poll_cq returned error");
    }
}

// ------------------- Resource Creation ------------------- //
static RdmaStatus create_rdma_resources(RdmaClient* client) {
    client->pd = ibv_alloc_pd(client->id->verbs);
    if (!client->pd) {
        LOG_ERROR("Failed to allocate PD");
        return RDMA_ERR_RESOURCE;
    }

    client->comp_channel = ibv_create_comp_channel(client->id->verbs);
    if (!client->comp_channel) {
        LOG_ERROR("Failed to create completion channel");
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }

    client->cq = ibv_create_cq(client->id->verbs, 128, client, client->comp_channel, 0);
    if (!client->cq) {
        LOG_ERROR("Failed to create CQ");
        ibv_destroy_comp_channel(client->comp_channel);
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }

    struct ibv_qp_init_attr qp_attr = {};
    qp_attr.qp_context = client;
    qp_attr.send_cq = client->cq;
    qp_attr.recv_cq = client->cq;
    qp_attr.qp_type = IBV_QPT_RC;
    qp_attr.cap = { .max_send_wr = 512, .max_recv_wr = 512, .max_send_sge = 4, .max_recv_sge = 4 };

    if (rdma_create_qp(client->id, client->pd, &qp_attr)) {
        LOG_ERROR("Failed to create QP");
        ibv_destroy_cq(client->cq);
        ibv_destroy_comp_channel(client->comp_channel);
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }
    client->qp = client->id->qp;

    int access = IBV_ACCESS_LOCAL_WRITE | IBV_ACCESS_REMOTE_READ | IBV_ACCESS_REMOTE_WRITE;
    client->send_mr = ibv_reg_mr(client->pd, client->send_buf_aligned, RdmaClient::BUF_SZ, access);
    if (!client->send_mr) {
        LOG_ERROR("Failed to register send MR");
        rdma_destroy_qp(client->id);
        ibv_destroy_cq(client->cq);
        ibv_destroy_comp_channel(client->comp_channel);
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }

    client->recv_mr = ibv_reg_mr(client->pd, client->recv_buf_aligned, RdmaClient::BUF_SZ, access);
    if (!client->recv_mr) {
        LOG_ERROR("Failed to register recv MR");
        ibv_dereg_mr(client->send_mr);
        rdma_destroy_qp(client->id);
        ibv_destroy_cq(client->cq);
        ibv_destroy_comp_channel(client->comp_channel);
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }

    client->track_mr(client->send_mr);
    client->track_mr(client->recv_mr);

    if (ibv_req_notify_cq(client->cq, 0)) {
        LOG_ERROR("Failed to request CQ notification");
        client->untrack_mr(client->send_mr);
        client->untrack_mr(client->recv_mr);
        ibv_dereg_mr(client->send_mr);
        ibv_dereg_mr(client->recv_mr);
        rdma_destroy_qp(client->id);
        ibv_destroy_cq(client->cq);
        ibv_destroy_comp_channel(client->comp_channel);
        ibv_dealloc_pd(client->pd);
        return RDMA_ERR_RESOURCE;
    }

    return RDMA_OK;
}

// ------------------- C ABI Implementation ------------------- //

extern "C" RdmaStatus rdma_connect_client(const char* host, const char* port, RdmaClientHandle* out_handle) {
    if (!host || !port || !out_handle) return RDMA_ERR_INVALID_ARGUMENT;

    LOG_DEBUG(std::string("Connecting to ") + host + ":" + port);
    struct rdma_addrinfo hints = {}, * addrinfo = nullptr;
    hints.ai_port_space = RDMA_PS_TCP;

    int ret = rdma_getaddrinfo(host, port, &hints, &addrinfo);
    if (ret) {
        LOG_ERROR(std::string("rdma_getaddrinfo failed: ") + strerror(ret));
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    auto client = new (std::nothrow) RdmaClient();
    if (!client) {
        rdma_freeaddrinfo(addrinfo);
        return RDMA_ERR_RESOURCE;
    }

    if (!client->is_valid()) {
        delete client;
        rdma_freeaddrinfo(addrinfo);
        return RDMA_ERR_RESOURCE;
    }

    client->ec = rdma_create_event_channel();
    if (!client->ec) {
        delete client;
        rdma_freeaddrinfo(addrinfo);
        return RDMA_ERR_RESOURCE;
    }

    ret = rdma_create_id(client->ec, &client->id, client, RDMA_PS_TCP);
    if (ret) {
        delete client;
        rdma_freeaddrinfo(addrinfo);
        return RDMA_ERR_RESOURCE;
    }

    if (rdma_resolve_addr(client->id, nullptr, addrinfo->ai_dst_addr, 2000)) {
        LOG_ERROR("rdma_resolve_addr failed");
        delete client;
        rdma_freeaddrinfo(addrinfo);
        return RDMA_ERR_TIMEOUT;
    }

    struct rdma_cm_event* event = nullptr;
    bool connected = false;
    RdmaStatus result = RDMA_ERR_GENERAL;

    while (rdma_get_cm_event(client->ec, &event) == 0) {
        struct rdma_cm_event e = *event;
        rdma_ack_cm_event(event);

        switch (e.event) {
        case RDMA_CM_EVENT_ADDR_RESOLVED:
            LOG_DEBUG("Address resolved, resolving route...");
            if (rdma_resolve_route(client->id, 2000)) {
                LOG_ERROR("Failed to resolve route");
                result = RDMA_ERR_GENERAL;
                connected = false;
                goto connect_done;
            }
            break;

        case RDMA_CM_EVENT_ROUTE_RESOLVED: {
            LOG_DEBUG("Route resolved, creating QP...");
            ret = create_rdma_resources(client);
            if (ret != RDMA_OK) {
                result = RDMA_ERR_GENERAL;
                connected = false;
                goto connect_done;
            }

            LOG_DEBUG("Initial credits posted. Constructing Negotiate Request...");

            SmbDirectNegotiateReq priv_data = {};
            priv_data.MinVersion = 0x0100;
            priv_data.MaxVersion = 0x0100;
            priv_data.CreditsRequested = 20;
            priv_data.PreferredSendSize = 1364;
            priv_data.MaxReceiveSize = RdmaClient::BUF_SZ;
            priv_data.MaxFragmentedSize = RdmaClient::BUF_SZ;

            LOG_DEBUG("MinVersion Value: " + std::to_string(priv_data.MinVersion));
            log_buffer_hex("Negotiate Packet Payload (Check Endianness)", &priv_data, sizeof(priv_data));

            rdma_conn_param param = {};
            param.private_data = &priv_data;
            param.private_data_len = sizeof(priv_data);
            param.responder_resources = 1;
            param.initiator_depth = 1;
            param.retry_count = 7;
            param.rnr_retry_count = 7;

            if (rdma_connect(client->id, &param)) {
                LOG_ERROR("rdma_connect failed");
                result = RDMA_ERR_GENERAL;
                connected = false;
                goto connect_done;
            }
            break;
        }

        case RDMA_CM_EVENT_ESTABLISHED:
            LOG_DEBUG("RC connection established successfully!");
            connected = true;
            result = RDMA_OK;
            goto connect_done;

        case RDMA_CM_EVENT_REJECTED:
        case RDMA_CM_EVENT_UNREACHABLE:
        case RDMA_CM_EVENT_CONNECT_ERROR:
            LOG_ERROR("Connection failed/rejected: " + std::string(rdma_event_str(e.event)));
            result = RDMA_ERR_GENERAL;
            connected = false;
            goto connect_done;

        default:
            break;
        }
    }

connect_done:
    rdma_freeaddrinfo(addrinfo);

    if (!connected) {
        LOG_ERROR("Failed to establish connection");
        delete client;
        return result;
    }

    *out_handle = reinterpret_cast<RdmaClientHandle>(client);
    return RDMA_OK;
}

extern "C" RdmaStatus disconnect(RdmaClientHandle handle) {
    if (!handle) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    LOG_DEBUG("Disconnecting client handle...");
    client->disconnecting.store(true, std::memory_order_release);

    if (client->id) {
        rdma_disconnect(client->id);
    }

    client->clear_mrs();
    delete client;
    return RDMA_OK;
}

extern "C" RdmaStatus rdma_send(RdmaClientHandle handle, const void* data, size_t len) {
    if (!handle || !data || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, data, len)) {
        return RDMA_ERR_GENERAL;
    }

    std::atomic_thread_fence(std::memory_order_release);

    struct ibv_send_wr wr = {};
    wr.wr_id = generate_wr_id(WrType::SEND);
    wr.sg_list = &sge;
    wr.num_sge = 1;
    wr.opcode = IBV_WR_SEND;
    wr.send_flags = IBV_SEND_SIGNALED;

    struct ibv_send_wr* bad_wr = nullptr;

    LOG_DEBUG("Posting SEND. wr_id=" + std::to_string(wr.wr_id) + " len=" + std::to_string(len));

    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("ibv_post_send failed. errno=" + std::to_string(errno));
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

extern "C" RdmaStatus post_receive(RdmaClientHandle handle, void* buf, size_t len, uint64_t recv_slot_id) {
    if (!handle || !buf || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    if (len > RdmaClient::BUF_SZ) {
        LOG_ERROR("post_receive: length too large " + std::to_string(len));
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    std::lock_guard<std::mutex> lock(client->slot_mutex);
    if (client->active_recv_slots.count(recv_slot_id) > 0) {
        LOG_ERROR("CRITICAL WARNING: Overwriting existing pending receive for slot " + std::to_string(recv_slot_id) + ". Operation rejected.");
        return RDMA_ERR_RESOURCE;
    }

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, buf, len)) {
        return RDMA_ERR_GENERAL;
    }

    client->active_recv_slots.insert(recv_slot_id);

    std::atomic_thread_fence(std::memory_order_release);
    struct ibv_recv_wr wr = {};
    wr.wr_id = recv_slot_id;
    wr.sg_list = &sge;
    wr.num_sge = 1;

    struct ibv_recv_wr* bad_wr = nullptr;

    LOG_DEBUG("Posting RECV. wr_id=" + std::to_string(recv_slot_id) + " len=" + std::to_string(len));

    std::lock_guard<std::mutex> qp_lock(client->qp_mutex);
    if (ibv_post_recv(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("ibv_post_recv failed");
        client->active_recv_slots.erase(recv_slot_id);
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

extern "C" RdmaStatus poll_completion(
    RdmaClientHandle handle,
    RdmaCompletion* completion_ptr,
    int timeout_ms,
    int completion_type)
{
    if (!handle || !completion_ptr) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    auto start_time = std::chrono::steady_clock::now();
    auto deadline = start_time + std::chrono::milliseconds(timeout_ms);

    std::queue<RdmaCompletion>* target_queue = nullptr;

    if (completion_type == 0) {
        target_queue = &client->recv_completion_queue;
    }
    else if (completion_type == 1) {
        target_queue = &client->send_completion_queue;
    }
    else {
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    while (true) {
        {
            std::lock_guard<std::mutex> lock(client->completion_mutex);
            if (!target_queue->empty()) {
                *completion_ptr = target_queue->front();
                target_queue->pop();
                return RDMA_OK;
            }
        }

        poll_and_distribute_completions(client);

        {
            std::lock_guard<std::mutex> lock(client->completion_mutex);
            if (!target_queue->empty()) {
                *completion_ptr = target_queue->front();
                target_queue->pop();
                return RDMA_OK;
            }
        }

        auto now = std::chrono::steady_clock::now();
        if (now >= deadline) {
            return RDMA_ERR_TIMEOUT;
        }

        std::this_thread::sleep_for(std::chrono::microseconds(100));
    }
}

extern "C" RdmaStatus write(RdmaClientHandle handle, const void* local_buf, size_t len, uint64_t remote_addr, uint32_t rkey) {
    if (!handle || !local_buf || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, local_buf, len)) return RDMA_ERR_GENERAL;

    std::atomic_thread_fence(std::memory_order_release);

    struct ibv_send_wr wr = {};
    wr.wr_id = generate_wr_id(WrType::WRITE);
    wr.sg_list = &sge;
    wr.num_sge = 1;
    wr.opcode = IBV_WR_RDMA_WRITE;
    wr.send_flags = IBV_SEND_SIGNALED;
    wr.wr.rdma.remote_addr = remote_addr;
    wr.wr.rdma.rkey = rkey;

    struct ibv_send_wr* bad_wr = nullptr;
    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("write failed");
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

extern "C" RdmaStatus read(RdmaClientHandle handle, void* local_buf, size_t len, uint64_t remote_addr, uint32_t rkey) {
    if (!handle || !local_buf || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, local_buf, len)) return RDMA_ERR_GENERAL;

    std::atomic_thread_fence(std::memory_order_acquire);

    struct ibv_send_wr wr = {};
    wr.wr_id = generate_wr_id(WrType::READ);
    wr.sg_list = &sge;
    wr.num_sge = 1;
    wr.opcode = IBV_WR_RDMA_READ;
    wr.send_flags = IBV_SEND_SIGNALED;
    wr.wr.rdma.remote_addr = remote_addr;
    wr.wr.rdma.rkey = rkey;

    struct ibv_send_wr* bad_wr = nullptr;
    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("read failed");
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

extern "C" RdmaStatus post_write(RdmaClientHandle handle,
    const void* local_buf,
    size_t len,
    uint64_t remote_addr,
    uint32_t rkey,
    uint64_t wr_id)
{
    if (!handle || !local_buf || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, local_buf, len)) return RDMA_ERR_GENERAL;

    std::atomic_thread_fence(std::memory_order_release);

    struct ibv_send_wr wr = {};
    wr.wr_id = wr_id;
    wr.sg_list = &sge;
    wr.num_sge = 1;
    wr.opcode = IBV_WR_RDMA_WRITE;
    wr.send_flags = IBV_SEND_SIGNALED;
    wr.wr.rdma.remote_addr = remote_addr;
    wr.wr.rdma.rkey = rkey;

    struct ibv_send_wr* bad_wr = nullptr;
    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("post_write failed");
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

extern "C" RdmaStatus post_read(RdmaClientHandle handle,
    void* local_buf,
    size_t len,
    uint64_t remote_addr,
    uint32_t rkey,
    uint64_t wr_id)
{
    if (!handle || !local_buf || len == 0) return RDMA_ERR_INVALID_ARGUMENT;
    auto client = reinterpret_cast<RdmaClient*>(handle);

    struct ibv_sge sge = {};
    if (!prepare_sge_for_buf(client, sge, local_buf, len)) return RDMA_ERR_GENERAL;

    std::atomic_thread_fence(std::memory_order_acquire);

    struct ibv_send_wr wr = {};
    wr.wr_id = wr_id;
    wr.sg_list = &sge;
    wr.num_sge = 1;
    wr.opcode = IBV_WR_RDMA_READ;
    wr.send_flags = IBV_SEND_SIGNALED;
    wr.wr.rdma.remote_addr = remote_addr;
    wr.wr.rdma.rkey = rkey;

    struct ibv_send_wr* bad_wr = nullptr;
    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        LOG_ERROR("post_read failed");
        return RDMA_ERR_GENERAL;
    }
    return RDMA_OK;
}

static bool is_qp_ready(RdmaClient* client) {
    if (!client || !client->qp) return false;

    struct ibv_qp_attr attr;
    struct ibv_qp_init_attr init_attr;
    if (ibv_query_qp(client->qp, &attr, IBV_QP_STATE, &init_attr) != 0) {
        LOG_ERROR("Failed to query QP state");
        return false;
    }

    if (attr.qp_state != IBV_QPS_RTS) {
        LOG_ERROR("QP not ready! Current state: " + std::to_string(attr.qp_state) +
            " (1=RESET, 2=INIT, 3=RTR, 4=RTS, 5=SQD, 6=SQE, 7=ERR)");
        return false;
    }
    return true;
}

extern "C" RdmaStatus register_memory_window(
    RdmaClientHandle handle,
    void* buf,
    size_t len,
    uint32_t accessFlags,
    intptr_t* out_mwHandle,
    uint32_t* out_rkey)
{
    if (!handle || !buf || !out_mwHandle || !out_rkey)
        return RDMA_ERR_INVALID_ARGUMENT;

    auto client = reinterpret_cast<RdmaClient*>(handle);
    if (!is_qp_ready(client)) {
        LOG_ERROR("register_memory_window: QP is not in RTS state, cannot register MW");
        return RDMA_ERR_CONNECTION_CLOSED;
    }
    int mr_access = IBV_ACCESS_LOCAL_WRITE | accessFlags;

    struct ibv_mr* mr = ibv_reg_mr(client->pd, buf, len, mr_access);
    if (!mr) {
        LOG_ERROR("register_memory_window: ibv_reg_mr failed: " + std::string(strerror(errno)));
        return RDMA_ERR_RESOURCE;
    }

    bool use_type_2 = true;
    struct ibv_mw* mw = nullptr;

    mw = ibv_alloc_mw(client->pd, IBV_MW_TYPE_2);
    if (!mw) {
        LOG_DEBUG("register_memory_window: Type 2 MW alloc failed, falling back to Type 1: " + std::string(strerror(errno)));
        use_type_2 = false;
    }

    if (!mw) {
        mw = ibv_alloc_mw(client->pd, IBV_MW_TYPE_1);
        if (!mw) {
            LOG_ERROR("register_memory_window: Type 1 MW alloc failed: " + std::string(strerror(errno)));
            ibv_dereg_mr(mr);
            return RDMA_ERR_RESOURCE;
        }
    }

    int mw_access = accessFlags & (IBV_ACCESS_REMOTE_READ | IBV_ACCESS_REMOTE_WRITE | IBV_ACCESS_REMOTE_ATOMIC);
    struct ibv_mw_bind_info bind_info = {};
    bind_info.mr = mr;
    bind_info.addr = reinterpret_cast<uintptr_t>(buf);
    bind_info.length = len;
    bind_info.mw_access_flags = mw_access;

    uint64_t bind_wr_id = generate_wr_id(WrType::BIND);
    int bind_ret = 0;

    if (use_type_2) {
        struct ibv_send_wr wr = {};
        struct ibv_send_wr* bad_wr = nullptr;
        wr.wr_id = bind_wr_id;
        wr.opcode = IBV_WR_BIND_MW;
        wr.send_flags = IBV_SEND_SIGNALED;
        wr.bind_mw.mw = mw;
        wr.bind_mw.rkey = mw->rkey;
        wr.bind_mw.bind_info = bind_info;

        std::lock_guard<std::mutex> lock(client->qp_mutex);
        bind_ret = ibv_post_send(client->qp, &wr, &bad_wr);
        if (bind_ret != 0) {
            LOG_DEBUG("register_memory_window: Type 2 bind via ibv_post_send failed, return=" +
                std::to_string(bind_ret) + " errno=" + std::to_string(errno));
        }
    }
    else {
        struct ibv_mw_bind mw_bind = {};
        mw_bind.wr_id = bind_wr_id;
        mw_bind.send_flags = IBV_SEND_SIGNALED;
        mw_bind.bind_info = bind_info;

        std::lock_guard<std::mutex> lock(client->qp_mutex);
        bind_ret = ibv_bind_mw(client->qp, mw, &mw_bind);
        if (bind_ret != 0) {
            LOG_ERROR("register_memory_window: ibv_bind_mw failed, return=" + std::to_string(bind_ret) +
                " errno=" + std::to_string(errno));
        }
    }

    if (bind_ret != 0) {
        if (use_type_2) {
            ibv_dealloc_mw(mw);
            mw = ibv_alloc_mw(client->pd, IBV_MW_TYPE_1);
            if (!mw) {
                LOG_ERROR("register_memory_window: Type 1 MW alloc failed on retry");
                ibv_dereg_mr(mr);
                return RDMA_ERR_RESOURCE;
            }
            struct ibv_mw_bind mw_bind = {};
            mw_bind.wr_id = bind_wr_id;
            mw_bind.send_flags = IBV_SEND_SIGNALED;
            mw_bind.bind_info = bind_info;

            std::lock_guard<std::mutex> lock(client->qp_mutex);
            bind_ret = ibv_bind_mw(client->qp, mw, &mw_bind);
            if (bind_ret != 0) {
                LOG_ERROR("register_memory_window: Type 1 bind also failed");
                ibv_dealloc_mw(mw);
                ibv_dereg_mr(mr);
                return RDMA_ERR_GENERAL;
            }
            use_type_2 = false;
        }
        else {
            ibv_dealloc_mw(mw);
            ibv_dereg_mr(mr);
            return RDMA_ERR_GENERAL;
        }
    }

    struct ibv_wc wc;
    bool bind_completed = false;
    int poll_count = 0;
    const int max_polls = 50000;

    while (!bind_completed && poll_count < max_polls) {
        int num = ibv_poll_cq(client->cq, 1, &wc);

        if (num > 0) {
            if (wc.wr_id == bind_wr_id) {
                bind_completed = true;
                if (wc.status != IBV_WC_SUCCESS) {
                    LOG_ERROR("register_memory_window: Bind CQE failed, status=" +
                        std::to_string(wc.status));
                    ibv_dealloc_mw(mw);
                    ibv_dereg_mr(mr);
                    return RDMA_ERR_GENERAL;
                }
            }
            else {
                RdmaCompletion comp = {};
                comp.wr_id = wc.wr_id;
                comp.status = wc.status;
                comp.byte_len = wc.byte_len;
                comp.qp_num = wc.qp_num;
                comp.op_code = static_cast<uint32_t>(wc.opcode);
                comp.vendor_err = wc.vendor_err;

                std::lock_guard<std::mutex> lock(client->completion_mutex);
                if (wc.opcode & IBV_WC_RECV) {
                    {
                        std::lock_guard<std::mutex> sl(client->slot_mutex);
                        client->active_recv_slots.erase(wc.wr_id);
                    }
                    client->recv_completion_queue.push(comp);
                }
                else {
                    client->send_completion_queue.push(comp);
                }
            }
        }
        else if (num < 0) {
            LOG_ERROR("register_memory_window: ibv_poll_cq failed");
            ibv_dealloc_mw(mw);
            ibv_dereg_mr(mr);
            return RDMA_ERR_GENERAL;
        }

        if (!bind_completed) {
            std::this_thread::sleep_for(std::chrono::microseconds(100));
            poll_count++;
        }
    }

    if (!bind_completed) {
        LOG_ERROR("register_memory_window: Bind timeout");
        ibv_dealloc_mw(mw);
        ibv_dereg_mr(mr);
        return RDMA_ERR_TIMEOUT;
    }

    MwContext* ctx = new (std::nothrow) MwContext();
    if (!ctx) {
        ibv_dealloc_mw(mw);
        ibv_dereg_mr(mr);
        return RDMA_ERR_RESOURCE;
    }

    ctx->mw = mw;
    ctx->mr = mr;
    ctx->is_type_2 = use_type_2;
    ctx->invalidated = false;

    client->track_mr(mr);

    *out_mwHandle = reinterpret_cast<intptr_t>(ctx);
    *out_rkey = mw->rkey;

    return RDMA_OK;
}

extern "C" RdmaStatus deregister_memory_window(RdmaClientHandle handle, intptr_t mwHandle) {
    if (!handle || !mwHandle) {
        LOG_ERROR("deregister_memory_window: Invalid arguments (handle=" +
            std::to_string(reinterpret_cast<uintptr_t>(handle)) +
            ", mwHandle=" + std::to_string(mwHandle) + ")");
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    auto client = reinterpret_cast<RdmaClient*>(handle);
    auto ctx = reinterpret_cast<MwContext*>(mwHandle);

    if (!ctx || (!ctx->mr && !ctx->mw)) {
        LOG_ERROR("deregister_memory_window: Corrupted context");
        delete ctx;
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    RdmaStatus status = RDMA_OK;

    if (ctx->mw) {
        if (ctx->is_type_2 && ctx->invalidated.load()) {
            LOG_DEBUG("deregister_memory_window: Type 2 MW already invalidated, skipping ibv_dealloc_mw");
        }
        else {
            int ret = ibv_dealloc_mw(ctx->mw);
            if (ret != 0) {
                if (ctx->is_type_2) {
                    LOG_DEBUG("deregister_memory_window: ibv_dealloc_mw failed for Type 2 MW (may already be invalidated): " +
                        std::string(strerror(errno)) + " (ret=" + std::to_string(ret) + ")");
                }
                else {
                    LOG_ERROR("deregister_memory_window: ibv_dealloc_mw failed: " +
                        std::string(strerror(errno)) + " (ret=" + std::to_string(ret) + ")");
                    status = RDMA_ERR_GENERAL;
                }
            }
        }
    }

    if (ctx->mr) {
        client->untrack_mr(ctx->mr);
        int ret = ibv_dereg_mr(ctx->mr);
        if (ret != 0) {
            LOG_ERROR("deregister_memory_window: ibv_dereg_mr failed: " +
                std::string(strerror(errno)));
            status = RDMA_ERR_GENERAL;
        }
    }

    delete ctx;
    return status;
}

extern "C" RdmaStatus wait_for_disconnect(RdmaClientHandle handle, int timeout_seconds) {
    if (!handle || timeout_seconds <= 0) {
        LOG_DEBUG("wait_for_disconnect: Invalid arguments");
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    auto client = reinterpret_cast<RdmaClient*>(handle);
    rdma_event_channel* ec = client->ec;
    if (!ec) {
        LOG_DEBUG("wait_for_disconnect: No event channel available");
        return RDMA_ERR_GENERAL;
    }

    LOG_DEBUG("wait_for_disconnect: Starting to listen for disconnect events, timeout=" + std::to_string(timeout_seconds) + "s");

    auto start = std::chrono::steady_clock::now();

    while (true) {
        if (client->disconnecting.load(std::memory_order_acquire)) {
            LOG_DEBUG("wait_for_disconnect: Client is disconnecting, aborting wait");
            return RDMA_ERR_CONNECTION_CLOSED;
        }

        auto now = std::chrono::steady_clock::now();
        auto elapsed = std::chrono::duration_cast<std::chrono::seconds>(now - start).count();
        if (elapsed >= timeout_seconds) {
            LOG_DEBUG("wait_for_disconnect: Timeout reached");
            return RDMA_ERR_TIMEOUT;
        }

        fd_set read_fds;
        FD_ZERO(&read_fds);
        FD_SET(ec->fd, &read_fds);

        struct timeval tv;
        tv.tv_sec = 1;
        tv.tv_usec = 0;

        int ret = select(ec->fd + 1, &read_fds, NULL, NULL, &tv);
        if (ret <= 0) {
            continue;
        }

        if (!FD_ISSET(ec->fd, &read_fds)) {
            continue;
        }

        struct rdma_cm_event* event;
        ret = rdma_get_cm_event(ec, &event);
        if (ret) {
            continue;
        }

        rdma_cm_event_type type = event->event;
        LOG_DEBUG("wait_for_disconnect: Received event type=" + std::to_string(type));

        rdma_ack_cm_event(event);

        switch (type) {
        case RDMA_CM_EVENT_DISCONNECTED:
            LOG_DEBUG("Disconnection detected");
            return RDMA_OK;
        case RDMA_CM_EVENT_REJECTED:
        case RDMA_CM_EVENT_UNREACHABLE:
        case RDMA_CM_EVENT_CONNECT_ERROR:
            LOG_DEBUG("Connection error: " + std::string(rdma_event_str(type)));
            return RDMA_ERR_CONNECTION_CLOSED;
        default:
            LOG_DEBUG("Unhandled event: " + std::string(rdma_event_str(type)));
            break;
        }
    }

    return RDMA_ERR_GENERAL;
}

extern "C" RdmaStatus invalidate_memory_window(
    RdmaClientHandle handle,
    intptr_t mwHandle,
    uint64_t* out_result_id)
{
    if (!handle || !mwHandle || !out_result_id)
        return RDMA_ERR_INVALID_ARGUMENT;

    auto client = reinterpret_cast<RdmaClient*>(handle);
    auto ctx = reinterpret_cast<MwContext*>(mwHandle);

    if (!ctx || !ctx->mw) {
        LOG_ERROR("invalidate_memory_window: Invalid MW context");
        return RDMA_ERR_INVALID_ARGUMENT;
    }

    uint64_t result_id = generate_wr_id(WrType::INVALIDATE);

    struct ibv_send_wr wr = {};
    struct ibv_send_wr* bad_wr = nullptr;

    if (ctx->invalidated.load()) {
        LOG_DEBUG("invalidate_memory_window: MW already invalidated, returning success");
        *out_result_id = result_id;
        return RDMA_OK;
    }

    if (ctx->is_type_2) {
        wr.wr_id = result_id;
        wr.opcode = IBV_WR_SEND_WITH_INV;
        wr.send_flags = IBV_SEND_SIGNALED;
        wr.invalidate_rkey = ctx->mw->rkey;
        wr.num_sge = 0;
        wr.sg_list = nullptr;

        LOG_DEBUG("invalidate_memory_window: Posting SEND_WITH_INV for Type 2 MW rkey=" +
            std::to_string(ctx->mw->rkey));
    }
    else {
        wr.wr_id = result_id;
        wr.opcode = IBV_WR_SEND;
        wr.send_flags = IBV_SEND_SIGNALED;
        wr.num_sge = 0;

        LOG_DEBUG("invalidate_memory_window: Type 1 MW, posting empty SEND as invalidate signal");
    }

    std::lock_guard<std::mutex> lock(client->qp_mutex);
    if (ibv_post_send(client->id->qp, &wr, &bad_wr)) {
        if (ctx->is_type_2 && errno == EINVAL) {
            LOG_DEBUG("invalidate_memory_window: ibv_post_send failed with EINVAL, assuming Type 2 MW already invalidated by remote peer");
            ctx->invalidated = true;
            *out_result_id = result_id;
            return RDMA_OK;
        }
        LOG_ERROR("invalidate_memory_window: ibv_post_send failed, errno=" + std::to_string(errno));
        return RDMA_ERR_GENERAL;
    }

    ctx->invalidated = true;
    *out_result_id = result_id;
    LOG_DEBUG("invalidate_memory_window: Success, result_id=" + std::to_string(result_id));
    return RDMA_OK;
}

extern "C" RdmaStatus check_invalidate_completion(
    RdmaClientHandle handle,
    uint64_t result_id,
    int timeout_ms)
{
    if (!handle || timeout_ms <= 0) return RDMA_ERR_INVALID_ARGUMENT;

    auto client = reinterpret_cast<RdmaClient*>(handle);
    auto start = std::chrono::steady_clock::now();

    while (true) {
        poll_and_distribute_completions(client);

        {
            std::lock_guard<std::mutex> lock(client->completion_mutex);
            auto& queue = client->send_completion_queue;

            std::queue<RdmaCompletion> temp_queue;
            bool found = false;

            while (!queue.empty()) {
                auto comp = queue.front();
                queue.pop();

                if (comp.wr_id == result_id) {
                    found = true;
                    if (comp.status == IBV_WC_SUCCESS) {
                        return RDMA_OK;
                    }
                    else {
                        return RDMA_ERR_GENERAL;
                    }
                }
                else {
                    temp_queue.push(comp);
                }
            }

            while (!temp_queue.empty()) {
                queue.push(temp_queue.front());
                temp_queue.pop();
            }

            if (found) break;
        }

        auto elapsed = std::chrono::duration_cast<std::chrono::milliseconds>(
            std::chrono::steady_clock::now() - start).count();
        if (elapsed > timeout_ms) {
            return RDMA_ERR_TIMEOUT;
        }

        std::this_thread::sleep_for(std::chrono::microseconds(100));
    }

    return RDMA_OK;
}

// ------------------- Aligned Memory Allocation Helpers ------------------- //

extern "C" void* rdma_alloc_aligned(size_t size) {
    void* ptr = nullptr;
    if (posix_memalign(&ptr, 4096, size) != 0) {
        return nullptr;
    }
    return ptr;
}

extern "C" void rdma_free_aligned(void* ptr) {
    free(ptr);
}
