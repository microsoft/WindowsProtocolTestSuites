# CMakeList.txt : Top-level CMake project file, do global configuration
# and include sub-projects here.
#
cmake_minimum_required (VERSION 3.16)
project("RdmaLinuxAdapter")

set(CMAKE_CXX_STANDARD 17)

find_package(PkgConfig QUIET)
if(PkgConfig_FOUND)
    pkg_check_modules(RDMACM librdmacm)
    pkg_check_modules(IBVERBS libibverbs)
endif()

if (NOT RDMACM_FOUND)
    find_path(RDMACM_INCLUDE_DIR rdma/rdma_cma.h)
    find_library(RDMACM_LIBRARY rdmacm)
    if (RDMACM_INCLUDE_DIR AND RDMACM_LIBRARY)
        set(RDMACM_LIBRARIES ${RDMACM_LIBRARY})
        set(RDMACM_INCLUDE_DIRS ${RDMACM_INCLUDE_DIR})
    else()
        message(FATAL_ERROR "librdmacm not found. Please install librdmacm-dev")
    endif()
endif()

if (NOT IBVERBS_FOUND)
    find_path(IBVERBS_INCLUDE_DIR infiniband/verbs.h)
    find_library(IBVERBS_LIBRARY ibverbs)
    if (IBVERBS_INCLUDE_DIR AND IBVERBS_LIBRARY)
        set(IBVERBS_LIBRARIES ${IBVERBS_LIBRARY})
        set(IBVERBS_INCLUDE_DIRS ${IBVERBS_INCLUDE_DIR})
    else()
        message(FATAL_ERROR "libibverbs not found. Please install libibverbs-dev")
    endif()
endif()

include_directories(${RDMACM_INCLUDE_DIRS} ${IBVERBS_INCLUDE_DIRS})

add_library(${PROJECT_NAME} SHARED 
    src/rdma_wrapper.cpp 
    src/rdma_wrapper.h
)

#target_include_directories(${PROJECT_NAME} PRIVATE ${PROJECT_SOURCE_DIR}/rdma_adapter)
target_link_libraries(${PROJECT_NAME} PRIVATE ibverbs rdmacm)

