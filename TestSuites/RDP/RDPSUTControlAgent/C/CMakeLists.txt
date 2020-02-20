cmake_minimum_required(VERSION 3.11)
project(control_agent)

set(SOURCE_FILES control_agent.c)
add_executable(${PROJECT_NAME} ${SOURCE_FILES})
