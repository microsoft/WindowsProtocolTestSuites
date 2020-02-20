#!/usr/bin/env python

import binascii
import logging
import threading
import subprocess
import struct
import socket
import sys
import datetime
from struct import *
from jinja2 import Environment

try:
    from configparser import ConfigParser
except ImportError:
    from ConfigParser import ConfigParser  # ver. < 3.0

logging.basicConfig(
    format='%(asctime)s - %(levelname)s - %(message)s', level=logging.INFO)

message_type = {'SUT_CONTROL_REQUEST': 0x0000,
                'SUT_CONTROL_RESPONSE': 0x0001}

command_id = {'START_RDP_CONNECTION': 0x0001,
              'CLOSE_RDP_CONNECTION': 0x0002,
              'AUTO_RECONNECT': 0x0003,
              'BASIC_INPUT': 0x0004,
              'SCREEN_SHOT': 0x0005,
              'TOUCH_EVENT_SINGLE': 0x0101,
              'TOUCH_EVENT_MULTIPLE': 0x0102,
              'TOUCH_EVENT_DISMISS_HOVERING_CONTACT': 0x0103,
              'DISPLAY_UPDATE_RESOLUTION': 0x0201,
              'DISPLAY_UPDATE_MONITORS': 0x0202,
              'DISPLAY_FULLSCREEN': 0x0203}

payload_type = {'RDP_FILE': 0x0000,
                'PARAMETERS_STRUCT': 0x0001}

testsuite_id = {'RDP_TESTSUITE': 0x0001}

result_code = {'SUCCESS': 0x00000000}

basic_input_flag = {'KEYBOARD_EVENT': 0x00000001,
                    'UNICODE_KEYBOARD_EVENT': 0x00000002,
                    'MOUSE_EVENT': 0x00000004,
                    'EXTENDED_MOUSE_EVENT': 0x00000008,
                    'CLIENT_SYNCHRONIZE_EVENT': 0x00000010,
                    'CLIENT_REFRESH_RECT': 0x00000020,
                    'CLIENT_SUPPRESS_OUTPUT': 0x00000040}

screen_type = {'NORMAL': 0x0000,
               'FULL_SCREEN': 0x0001}

connect_approach = {'NEGOTIATE': 0x0000,
                    'DIRECT': 0x0001}

monitor_action = {'ADD_MONITOR': 0x00000001,
                  'REMOVE_MONITOR': 0x00000002,
                  'MOVE_MONITOR_POSITION': 0x00000004}


class Payload:
    def __init__(self):
        self.type = ""
        self.content = ""
        self.port = 0
        self.desktop_width = 0
        self.desktop_height = 0
        self.connect_approach = ""
        self.screen_type = ""
        self.address = ""

    def encode(self):
        pass

    def decode(self, raw_payload):
        self.type = unpack('<i', raw_payload[:4])
        self.type = self.type[0]
        logging.debug("payload type:", self.type)
        if self.type == payload_type['RDP_FILE']:
            content_length = len(raw_payload) - 4
            self.content = unpack('<%sc' % content_length, raw_payload[4:])
            logging.debug("content", self.content)
        elif self.type == payload_type['PARAMETERS_STRUCT']:
            start = 4
            self.port = unpack('<h', raw_payload[start:start + 2])
            self.port = self.port[0]
            logging.debug("port", self.port)
            start += 2
            self.screen_type = unpack('<h', raw_payload[start:start + 2])
            self.screen_type = self.screen_type[0]
            logging.debug("screen_type", self.screen_type)
            start += 2
            self.desktop_width = unpack('<h', raw_payload[start:start + 2])
            self.desktop_width = self.desktop_width[0]
            logging.debug("desktop_width", self.desktop_width)
            start += 2
            self.desktop_height = unpack('<h', raw_payload[start:start + 2])
            self.desktop_height = self.desktop_height[0]
            logging.debug("desktop_height", self.desktop_height)
            start += 2
            self.connect_approach = unpack('<h', raw_payload[start:start + 2])
            self.connect_approach = self.connect_approach[0]
            logging.debug("connect_approach", self.connect_approach)
            start += 2
            address_length = unpack('<h', raw_payload[start:start + 2])
            address_length = address_length[0]
            if address_length:
                start += 2
                self.address = unpack(
                    '<%ss' % address_length, raw_payload[start:start + address_length])
                self.address = self.address[0].decode('utf-8', errors="ignore")
                logging.debug("address", self.address)
        else:
            logging.error("wrong payload type")


class Message:
    def __init__(self):
        self.type = 0
        self.rc = result_code['SUCCESS']
        self.request_id = 0
        self.testsuite_id = 0
        self.command_id = 0
        self.testcase_name = ""
        self.help_message = ""
        self.payload = ""
        self.error_message = ""
        self.monitor_action = 0

    def encode(self):
        packer = struct.Struct('< h h h i %ss h i i i' %
                               len(self.testcase_name))
        fields = (self.type, self.testsuite_id, self.command_id,
                  len(self.testcase_name), self.testcase_name,
                  self.request_id, self.rc, 0, 0)
        packed_data = packer.pack(*fields)
        logging.debug('sending "%s"' % binascii.hexlify(packed_data))
        logging.debug('encoded!')
        return packed_data

    def decode(self, request):
        header_length = 10
        self.type, self.testsuite_id, self.command_id, name_length = \
            unpack('<hhhi', request[:header_length])
        if name_length:
            self.testcase_name = unpack(
                '<%sc' % name_length, request[header_length:name_length + header_length])
            self.testcase_name = b''.join(self.testcase_name)
        start = header_length + name_length
        self.request_id, help_message_length = unpack(
            '<hi', bytes(request[start:start + 6]))
        if help_message_length:
            start += 6
            self.help_message = unpack(
                '<%sc' % help_message_length, request[start:start + help_message_length])
            self.help_message = b''.join(self.help_message)
        start += help_message_length
        payload_length = unpack('<i', request[start:start + 4])[0]
        if payload_length:
            start += 4
            raw_payload = unpack('<%sc' % payload_length,
                                 request[start:start + payload_length])
            if self.command_id == command_id['START_RDP_CONNECTION']:
                self.payload = Payload()
                self.payload.decode(b''.join(raw_payload))
            elif self.command_id == command_id['DISPLAY_UPDATE_MONITORS']:
                self.monitor_action = unpack(
                    '<i', request[start + payload_length:])
            elif self.command_id == command_id['DISPLAY_UPDATE_RESOLUTION']:
                logging.debug("DISPLAY_UPDATE_RESOLUTION")
                # TODO: parse payload structure
            elif self.command_id == command_id['TOUCH_EVENT_MULTIPLE']:
                logging.debug("TOUCH_EVENT_MULTIPLE")
                # TODO: parse payload structure
            elif self.command_id == command_id['TOUCH_EVENT_SINGLE']:
                logging.debug("TOUCH_EVENT_SINGLE")
                # TODO: parse payload structure
            elif self.command_id == command_id['BASIC_INPUT']:
                logging.debug("BASIC_INPUT")
                # TODO: parse payload structure
            else:
                self.payload = ''.join(raw_payload)
                logging.debug(self.payload)


def build_client_cmd(cmd, ip_address, ip_port):

    if ip_port == 0:
        address = ip_address
    else:
        address = "%s:%s" % (ip_address, ip_port)
    client_cmd = Environment().from_string(cmd).render(address=address)

    return client_cmd


def handle_connection(client_socket, config):
    processes = []

    while True:
        buffer_size = int(config.get('general', 'buffer_size'))
        request = client_socket.recv(buffer_size)
        if len(request) == 0:
            continue

        msg = Message()
        msg.decode(request)
        logging.info("testcase: %s" % msg.testcase_name)
        logging.debug("command: %s" % msg.command_id)
        if msg.type != message_type['SUT_CONTROL_REQUEST'] or msg.testsuite_id != testsuite_id['RDP_TESTSUITE']:
            logging.error("message is not a control request")
            continue

        response = Message()
        response.type = message_type['SUT_CONTROL_RESPONSE']
        response.testsuite_id = testsuite_id['RDP_TESTSUITE']
        response.command_id = msg.command_id
        response.testcase_name = msg.testcase_name
        response.request_id = msg.request_id

        logging.info("Command ID: %s", msg.command_id)
        if msg.command_id == command_id['START_RDP_CONNECTION']:
            config_cmd = config.get('client', 'Negotiate')
            cmd = build_client_cmd(
                config_cmd, msg.payload.address, msg.payload.port)
            logging.info("Executing client: %s" % cmd)
            processes.append(subprocess.Popen(cmd.split(' ')))
        elif msg.command_id == command_id['CLOSE_RDP_CONNECTION']:
            for p in processes:
                logging.debug("Terminate the client %s" % p)
                p.terminate()
        elif msg.command_id == command_id['AUTO_RECONNECT']:
            # TODO
            pass
        elif msg.command_id == command_id['SCREEN_SHOT']:
            # TODO
            pass
        elif msg.command_id == command_id['BASIC_INPUT']:
            # TODO
            pass
        elif msg.command_id == command_id['TOUCH_EVENT_SINGLE']:
            # TODO
            pass
        elif msg.command_id == command_id['TOUCH_EVENT_MULTIPLE']:
            # TODO
            pass
        elif msg.command_id == command_id['TOUCH_EVENT_DISMISS_HOVERING_CONTACT']:
            # TODO
            pass
        elif msg.command_id == command_id['DISPLAY_UPDATE_RESOLUTION']:
            # TODO
            pass
        elif msg.command_id == command_id['DISPLAY_UPDATE_MONITORS']:
            # TODO
            pass
        elif msg.command_id == command_id['DISPLAY_FULLSCREEN']:
            config_cmd = config.get('client', 'NegotiateFullScreen')
            cmd = build_client_cmd(
                config_cmd, msg.payload.address, msg.payload.port)
            logging.info("Executing client: %s" % cmd)
            processes.append(subprocess.Popen(cmd.split(' ')))

        # FIXME: response.request_message = ""
        # FIXME: response.error_message = ""
        logging.debug("Response has been sent")
        client_socket.sendall(response.encode())


def main():

    CONFIG_PATH = 'settings.ini'

    config = ConfigParser()
    config.read(CONFIG_PATH)
    ip_address = config.get('general', 'bind_ip_adress')
    port = int(config.get('general', 'bind_port'))

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    while True:
        try:
            server.bind((ip_address, port))
        except socket.error:
            continue
        break
    server.listen(5)  # max connections
    logging.info("Listening on %s:%s" % (ip_address, port))

    while True:
        connection, address = server.accept()
        logging.info("Accepted connection from %s:%s" %
                     (address[0], address[1]))
        client_handler = threading.Thread(
            target=handle_connection,
            args=(connection, config)
        )
        client_handler.start()


if __name__ == '__main__':
    sys.exit(main())
