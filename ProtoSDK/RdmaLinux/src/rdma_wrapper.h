#pragma once
#include <cstdint>
#include <cstddef>


#ifdef __cplusplus
extern "C" {
#endif
	// ——————————— Public C ABI ———————————
	typedef struct RdmaClient* RdmaClientHandle;

	// Status codes
	typedef enum RdmaStatus {
		RDMA_OK = 0,
		RDMA_ERR_GENERAL = -1,
		RDMA_ERR_TIMEOUT = -2,
		RDMA_ERR_INVALID_ARGUMENT = -3,
		RDMA_ERR_CONNECTION_CLOSED = -4,
		RDMA_ERR_NO_COMPLETION = -5,
		RDMA_ERR_RESOURCE = -6,
	} RdmaStatus;

	// RDMA Access rights
	enum RdmaAccess : uint32_t {
		RDMA_ACCESS_LOCAL_WRITE = 1u << 0,
		RDMA_ACCESS_REMOTE_READ = 1u << 1,
		RDMA_ACCESS_REMOTE_WRITE = 1u << 2
	};
	#pragma pack(push, 1)
	typedef struct RdmaCompletion {
		uint64_t wr_id;
		uint32_t status;
		uint32_t byte_len;
		uint32_t qp_num;
		uint32_t op_code;
		uint32_t vendor_err;
	} RdmaCompletion;
	#pragma pack(pop)
	// Connection
	RdmaStatus rdma_connect_client(const char* host, const char* port, RdmaClientHandle* out_handle);
	RdmaStatus disconnect(RdmaClientHandle handle);

	// Send
	RdmaStatus rdma_send(RdmaClientHandle hanle, const void* data, size_t len);

	// post receive
	// Post a receive buffer. The out_recv_slot_id can be used to identify the buffer in a completion event.
	RdmaStatus post_receive(RdmaClientHandle handle, void* buf, size_t len, uint64_t out_recvSlotId);

	// Poll for a single completion. Returns RDMA_ERR_NO_COMPLETION if no completion is found.
	// The completion_ptr must be a valid pointer to a RdmaCompletion struct.
	RdmaStatus poll_completion(RdmaClientHandle handle, RdmaCompletion* completion_ptr, int timeout_ms, int completion_type);

	// Wait for a RECV to complete
	//RdmaStatus poll_receive(RdmaClientHandle handle, int timeout_ms, int* out_recvSlotId, int* out_received);

	// Memory Window
	// Returns a handle for the memory window and the remote key (rkey)
	RdmaStatus register_memory_window(
		RdmaClientHandle handle,
		void* buf,
		size_t len,
		uint32_t accessFlags,
		intptr_t* out_mwHandle,
		uint32_t* out_rkey);

	RdmaStatus deregister_memory_window(RdmaClientHandle handle, intptr_t mwHandle);

	// RDMA WRITE/READ
	// Use a local buffer to write to a remote address
	RdmaStatus write(
		RdmaClientHandle handle,
		const void* local_buf,
		size_t len,
		uintptr_t remote_addr,
		uint32_t rkey);

	// Use a local buffer to read from a remote address
	RdmaStatus read(
		RdmaClientHandle handle,
		void* local_buf,
		size_t len,
		uintptr_t remote_addr,
		uint32_t rkey);

	// Asynchronous versions of RDMA operations
	// These functions post the work request and return immediately.
	// The caller must use poll_completion to check for completion.
	RdmaStatus post_write(RdmaClientHandle handle,
		const void* local_buf,
		size_t len,
		uint64_t remote_addr,
		uint32_t rkey,
		uint64_t wr_id);

	RdmaStatus post_read(RdmaClientHandle handle,
		void* local_buf,
		size_t len,
		uint64_t remote_addr,
		uint32_t rkey,
		uint64_t wr_id);

	RdmaStatus wait_for_disconnect(RdmaClientHandle handle, int timeout_seconds);

#ifdef __cplusplus
} // extern "C"
#endif