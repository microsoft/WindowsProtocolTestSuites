#include <arpa/inet.h>
#include <errno.h>
#include <fcntl.h> /* Added for the nonblocking socket */
#include <netinet/in.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <sys/mman.h>
#include <sys/socket.h>
#include <sys/types.h>
#include <sys/wait.h>
#include <unistd.h>

#include "config.h"

enum message_type {
  SUT_CONTROL_REQUEST = 0x0000,
  SUT_CONTROL_RESPONSE = 0x0001
};

enum command_id {
  START_RDP_CONNECTION = 0x0001,
  CLOSE_RDP_CONNECTION = 0x0002,
  AUTO_RECONNECT = 0x0003,
  BASIC_INPUT = 0x0004,
  SCREEN_SHOT = 0x0005,
  TOUCH_EVENT_SINGLE = 0x0101,
  TOUCH_EVENT_MULTIPLE = 0x0102,
  TOUCH_EVENT_DISMISS_HOVERING_CONTACT = 0x0103,
  DISPLAY_UPDATE_RESOLUTION = 0x0201,
  DISPLAY_UPDATE_MONITORS = 0x0202,
  DISPLAY_FULLSCREEN = 0x0203
};

enum payload_type { RDP_FILE = 0x0000, PARAMETERS_STRUCT = 0x0001 };

enum testsuite_id { RDP_TESTSUITE = 0x0001 };

enum result_code { SUCCESS = 0x00000000 };

enum basic_input_flag {
  KEYBOARD_EVENT = 0x00000001,
  UNICODE_KEYBOARD_EVENT = 0x00000002,
  MOUSE_EVENT = 0x00000004,
  EXTENDED_MOUSE_EVENT = 0x00000008,
  CLIENT_SYNCHRONIZE_EVENT = 0x00000010,
  CLIENT_REFRESH_RECT = 0x00000020,
  CLIENT_SUPPRESS_OUTPUT = 0x00000040
};

enum screen_type { NORMAL = 0x0000, FULL_SCREEN = 0x0001 };

enum connect_approach { NEGOTIATE = 0x0000, DIRECT = 0x0001 };

enum monitor_action {
  ADD_MONITOR = 0x00000001,
  REMOVE_MONITOR = 0x00000002,
  MOVE_MONITOR_POSITION = 0x00000004
};

enum operation {
  UPDATE_RESOLUTION = 0x00000001,
  UPDATE_ORIENTATION = 0x00000002
};

typedef struct request_msg request_msg;
typedef struct response_msg response_msg;
typedef struct connection_parameters connection_parameters;
typedef struct pixel_data pixel_data;
typedef struct graphic_data graphic_data;
typedef struct position position;
typedef struct touch_event_single touch_event_single;
typedef struct touch_event_multiple touch_event_multiple;
typedef struct display_update_resolution display_update_resolution;

struct connection_parameters {
  uint16_t port;
  uint16_t screen_type;
  uint16_t desktop_width;
  uint16_t desktop_height;
  uint16_t connect_approach;
  uint16_t address_len;
  char *address;
} __attribute__((packed));

struct position {
  uint32_t X;
  uint32_t Y;

} __attribute__((packed));

struct touch_event_single {
  uint32_t touch_event_times;
  position positions[];
} __attribute__((packed));

struct touch_event_multiple {
  uint32_t touch_position_count;
  position positions[];
} __attribute__((packed));

struct display_update_resolution {
  uint32_t operation;
  uint16_t width;
  uint16_t height;
  uint32_t orientation;
} __attribute__((packed));

struct graphic_data {
  uint32_t width;
  uint32_t height;
  char *data;
} __attribute__((packed));

struct pixel_data {
  uint8_t red;
  uint8_t green;
  uint8_t blue;
} __attribute__((packed));

struct request_msg {
  uint16_t type;
  uint16_t testsuite_id;
  uint16_t command_id;
  uint32_t testcase_name_len;
  char *testcase_name;
  uint16_t request_id;
  uint32_t help_message_len;
  char *help_message;
  uint32_t payload_len;
  /* payload */
  uint32_t basic_input;
  connection_parameters *connection;
  touch_event_single *event_single;
  touch_event_multiple *event_multiple;
  display_update_resolution *display_update_resolution;
  uint32_t *display_update_monitors;
} __attribute__((packed));

struct response_msg {
  uint16_t type;
  uint16_t testsuite_id;
  uint16_t command_id;
  uint32_t testcase_name_len;
  char *testcase_name;
  uint16_t request_id;
  uint32_t result_code;
  uint32_t error_message_len;
  char *error_message;
  uint32_t payload_len;
} __attribute__((packed));

const void *read_uint8(const void *buffer, uint8_t *value) {
  const uint8_t *vptr = buffer;
  *value = *vptr;
  vptr++;

  return vptr;
}

const void *read_uint16(const void *buffer, uint16_t *value) {
  const uint16_t *vptr = buffer;
  *value = *vptr;
  vptr++;

  return vptr;
}

const void *read_uint32(const void *buffer, uint32_t *value) {
  const uint32_t *vptr = buffer;
  *value = *vptr;
  vptr++;

  return vptr;
}

const void *read_str(const void *buffer, char **value, size_t len) {
  char *str;
  str = calloc(len + 1, sizeof(char));
  memcpy(str, buffer, len);
  str[len + 1] = '\0';
  buffer += sizeof(char) * len;
  *value = str;

  return buffer;
}

const void *read_touch_event_single_payload(const void *buffer,
                                            touch_event_single *event) {
  buffer = read_uint32(buffer, &event->touch_event_times);
}

const void *read_touch_event_multiple_payload(const void *buffer,
                                              touch_event_multiple *event) {
  buffer = read_uint32(buffer, &event->touch_position_count);
}

const void *
read_display_update_resolution_payload(const void *buffer,
                                       display_update_resolution *update) {
  buffer = read_uint32(buffer, &update->operation);
  buffer = read_uint16(buffer, &update->width);
  buffer = read_uint16(buffer, &update->height);
  buffer = read_uint32(buffer, &update->orientation);
}

const void *read_rdp_connection_payload(const void *buffer,
                                        connection_parameters *conn) {
  uint32_t payload_type;
  buffer = read_uint32(buffer, &payload_type);
  switch (payload_type) {
  case RDP_FILE:
    printf("RDP_FILE is unsupported\n");
    break;
  case PARAMETERS_STRUCT:
    buffer = read_uint16(buffer, &conn->port);
    buffer = read_uint16(buffer, &conn->screen_type);
    buffer = read_uint16(buffer, &conn->desktop_width);
    buffer = read_uint16(buffer, &conn->desktop_height);
    buffer = read_uint16(buffer, &conn->connect_approach);
    buffer = read_uint16(buffer, &conn->address_len);
    buffer = read_str(buffer, &conn->address, conn->address_len);
    break;
  default:
    printf("unknown payload type\n");
  };

  return buffer;
};

void print_request_msg(request_msg *msg) {
  printf("Message type: %d\n", msg->type);
  printf("Testsuite ID: %d\n", msg->testsuite_id);
  printf("Command ID: %d\n", msg->command_id);
  printf("Testcase: %s\n", msg->testcase_name);
  printf("Request ID: %d\n", msg->request_id);
  printf("Payload length: %d\n", msg->payload_len);
  printf("Payload: ");
  if (msg->connection) {
    connection_parameters *conn = msg->connection;
    printf("Connection parameters: %s:%d", conn->address, conn->port);
    printf(" %d x %d\n", conn->desktop_width, conn->desktop_height);
  }

  /* touch_event_single *event_single; */
  /* touch_event_multiple *event_multiple; */

  if (msg->basic_input != 0) {
    printf("Basic input: %d\n", msg->basic_input);
  }
  if (msg->display_update_monitors != 0) {
    printf("Display update monitors: %ls\n", msg->display_update_monitors);
  }
  if (msg->display_update_resolution) {
    display_update_resolution *update = msg->display_update_resolution;
    printf("Display update resolution: %d, %d x %d, %d\n", update->operation,
           update->width, update->height, update->orientation);
  }
  printf("\n");
}

void print_response_msg(response_msg *msg) {
  printf("Message type: %d\n", msg->type);
  printf("Testsuite ID: %d\n", msg->testsuite_id);
  printf("Command ID: %d\n", msg->command_id);
  printf("Testcase: %s\n", msg->testcase_name);
  printf("Request ID: %d\n", msg->request_id);
  printf("Payload length: %d\n", msg->payload_len);
}

int decode_request_msg(const void *buffer, request_msg *msg) {
  buffer = read_uint16(buffer, &msg->type);
  if (msg->type != SUT_CONTROL_REQUEST) {
    printf("unknown message type\n");
    return -1;
  }
  buffer = read_uint16(buffer, &msg->testsuite_id);
  if (msg->testsuite_id != RDP_TESTSUITE) {
    printf("unknown testsuite_id\n");
    return -1;
  }

  buffer = read_uint16(buffer, &msg->command_id);
  buffer = read_uint32(buffer, &msg->testcase_name_len);
  buffer = read_str(buffer, &msg->testcase_name, msg->testcase_name_len);
  buffer = read_uint16(buffer, &msg->request_id);
  buffer = read_uint32(buffer, &msg->help_message_len);
  buffer = read_str(buffer, &msg->help_message, msg->help_message_len);
  buffer = read_uint32(buffer, &msg->payload_len);

  msg->connection = malloc(sizeof(connection_parameters));
  switch (msg->command_id) {
  case START_RDP_CONNECTION:
    printf("START_RDP_CONNECTION\n");
    buffer = read_rdp_connection_payload(buffer, msg->connection);
    break;
  case CLOSE_RDP_CONNECTION:
    printf("CLOSE_RDP_CONNECTION\n");
    break;
  case BASIC_INPUT:
    printf("BASIC_INPUT\n");
    buffer = read_uint32(buffer, &msg->basic_input);
    break;
  case TOUCH_EVENT_SINGLE:
    printf("TOUCH_EVENT_SINGLE (not implelented)\n");
    break;
  case TOUCH_EVENT_MULTIPLE:
    printf("TOUCH_EVENT_MULTIPLE (not implelented)\n");
    break;
  case DISPLAY_UPDATE_RESOLUTION:
    printf("DISPLAY_UPDATE_RESOLUTION (not implelented)\n");
    break;
  case DISPLAY_UPDATE_MONITORS:
    printf("DISPLAY_UPDATE_MONITORS (not implelented)\n");
    break;
  default:
    printf("unknown command_id\n");
    return -1;
  };
  printf("REQUEST: %s (%d)\n", msg->testcase_name, msg->command_id);

  return 0;
}

void free_request_msg(request_msg *msg) {
  free(msg->display_update_resolution);
  free(msg->event_single);
  free(msg->event_multiple);
  free(msg->testcase_name);
  free(msg->help_message);
  free(msg->connection->address);
  free(msg->connection);
}

void free_response_msg(response_msg *msg) {
  free(msg->testcase_name);
  free(msg->error_message);
}

void *encode_response_msg(response_msg *msg, size_t *sz) {
  printf("RESPONSE: %s (%d)\n", msg->testcase_name, msg->command_id);

  size_t size = 0;
  size = sizeof(msg->type);
  size += sizeof(msg->testsuite_id);
  size += sizeof(msg->command_id);
  size += sizeof(msg->testcase_name_len);
  if (msg->testcase_name) {
    size += strlen(msg->testcase_name);
  }
  size += strlen(msg->testcase_name);
  size += sizeof(msg->request_id);
  size += sizeof(msg->result_code);
  size += sizeof(msg->error_message_len);
  if (msg->error_message) {
    size += strlen(msg->error_message);
  }
  size += sizeof(msg->payload_len);
  /* TODO: size += sizeof(msg->payload); */

  void *pa = NULL;
  printf("allocate response buffer %ld\n", size);
  pa = calloc(1, size);
  if (!pa) {
    perror("calloc");
    return NULL;
  }
  explicit_bzero(pa, size);

  size_t offset = 0;
  memcpy(pa, &msg->type, sizeof(msg->type));
  offset = sizeof(msg->type);
  memcpy(pa + offset, &msg->testsuite_id, sizeof(msg->testsuite_id));
  offset += sizeof(msg->testsuite_id);
  memcpy(pa + offset, &msg->command_id, sizeof(msg->command_id));
  offset += sizeof(msg->command_id);
  memcpy(pa + offset, &msg->testcase_name_len, sizeof(msg->testcase_name_len));
  offset += sizeof(msg->testcase_name_len);
  if (msg->testcase_name_len > 0) {
    printf("testcase name %s \n", msg->testcase_name);
    memcpy(pa + offset, msg->testcase_name, strlen(msg->testcase_name));
    offset += strlen(msg->testcase_name);
  }
  memcpy(pa + offset, &msg->request_id, sizeof(msg->request_id));
  offset += sizeof(msg->request_id);
  memcpy(pa + offset, &msg->result_code, sizeof(msg->result_code));
  offset += sizeof(msg->result_code);
  memcpy(pa + offset, &msg->error_message_len, sizeof(msg->error_message_len));
  offset += sizeof(msg->error_message_len);
  memcpy(pa + offset, &msg->payload_len, sizeof(msg->payload_len));
  offset += sizeof(msg->payload_len);

  *sz = size;

  printf("encode_response_msg()\n");
  return pa;
}

int main(int argc, char *argv[]) {
  int server_fd, client_fd, err;
  struct sockaddr_in server, client;
  char buf[BUFFER_SIZE];

  server_fd = socket(AF_INET, SOCK_STREAM, 0);
  if (server_fd < 0) {
    perror("socket");
  }

  server.sin_family = AF_INET;
  server.sin_port = htons(TCP_PORT);
  server.sin_addr.s_addr = htonl(INADDR_ANY);

  int opt_val = 1;
  setsockopt(server_fd, SOL_SOCKET, SO_REUSEADDR, &opt_val, sizeof opt_val);

  err = bind(server_fd, (struct sockaddr *)&server, sizeof(server));
  if (err < 0) {
    perror("bind");
  }

  err = listen(server_fd, 128);
  if (err < 0) {
    perror("listen");
  }

  printf("Start commands processing on port %d\n", TCP_PORT);
  socklen_t client_len = sizeof(client);
  client_fd = accept(server_fd, (struct sockaddr *)&client, &client_len);

  if (client_fd < 0) {
    perror("accept");
  }

  while (1) {
    int read = recv(client_fd, buf, BUFFER_SIZE, 0);
    if (read < 0) {
      perror("read");
      break;
    }
    if (read == 0) {
      continue;
    }

    printf("Line %d\n", __LINE__);
    request_msg request_msg = {0};
    int rc = decode_request_msg(buf, &request_msg);
    if (rc != 0) {
      printf("decode_request_msg() failed\n");
      continue;
    }

    switch (request_msg.command_id) {
    case START_RDP_CONNECTION:
      printf("%s START_RDP_CONNECTION\n", __func__);
      system(CMD_CLOSE_CLIENT);
      rc = system(CMD_CONNECT_CLIENT);
      printf("connect result = %d\n", rc);
      break;
    case CLOSE_RDP_CONNECTION:
      printf("%s CLOSE_RDP_CONNECTION\n", __func__);
      rc = system(CMD_CLOSE_CLIENT);
      printf("close connection result = %d", rc);
      break;
    case AUTO_RECONNECT:
      printf("%s AUTO_RECONNECT\n", __func__);
      break;
    case BASIC_INPUT:
      printf("%s BASIC_INPUT\n", __func__);
      break;
    case SCREEN_SHOT:
      printf("%s SCREEN_SHOT\n", __func__);
      break;
    case TOUCH_EVENT_SINGLE:
      printf("%s TOUCH_EVENT_SINGLE\n", __func__);
      break;
    case TOUCH_EVENT_MULTIPLE:
      printf("%s TOUCH_EVENT_MULTIPLE\n", __func__);
      break;
    case TOUCH_EVENT_DISMISS_HOVERING_CONTACT:
      printf("%s TOUCH_EVENT_DISMISS_HOVERING_CONTACT\n", __func__);
      break;
    case DISPLAY_UPDATE_RESOLUTION:
      printf("%s DISPLAY_UPDATE_RESOLUTION\n", __func__);
      break;
    case DISPLAY_UPDATE_MONITORS:
      printf("%s DISPLAY_UPDATE_MONITORS\n", __func__);
      break;
    case DISPLAY_FULLSCREEN:
      printf("%s DISPLAY_FULLSCREEN\n", __func__);
      break;
    default:
      printf("Unknown command id in request message");
    };

    response_msg response_msg = {0};
    response_msg.type = SUT_CONTROL_RESPONSE;
    response_msg.testsuite_id = RDP_TESTSUITE;
    response_msg.command_id = request_msg.command_id;
    response_msg.testcase_name_len = request_msg.testcase_name_len;
    response_msg.testcase_name = strdup(request_msg.testcase_name);
    response_msg.request_id = request_msg.request_id;
    response_msg.result_code = SUCCESS;
    response_msg.error_message_len = 0;
    response_msg.payload_len = 0;

    size_t response_sz;
    void *response = encode_response_msg(&response_msg, &response_sz);
    if (!response) {
      printf("encode_response_msg() failed\n");
      continue;
    }

    ssize_t err;
    err = send(client_fd, response, response_sz, 0);
    if (err < 0) {
      perror("send");
    }
    free(response);
    free_request_msg(&request_msg);
    free_response_msg(&response_msg);
  }

  return 0;
}
