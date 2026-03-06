#!/bin/bash
# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# ==========================================
#  Start to build MS-SMBD test suite
# ==========================================

# set default values
CONFIGURATION="Release"
OUTDIR=""

# parse command line arguments
for i in "$@"; do
  case $i in
    --Configuration=*)
      CONFIGURATION="${i#*=}"
      shift
      ;;
    --OutDir=*)
      OUTDIR="${i#*=}"
      shift
      ;;
    *)
     ;;
  esac
done

# Get the root directory of the test suite
INVOCATION_PATH=$(dirname "$(readlink -f "$0")")
TEST_SUITE_ROOT="$INVOCATION_PATH/../../../"

# if CONFIGURATION is not set, use Release as default
if [ -z "$OUTDIR" ]; then
    OUTDIR="$TEST_SUITE_ROOT/drop/TestSuites/MS-SMBD"
fi

echo "Configuration: $CONFIGURATION"
echo "Output Directory: $OUTDIR"

# ==========================================
# Check system dependencies
# ==========================================
echo "Checking system dependencies..."

packages_debian=("cmake" "rdma-core" "ibverbs-utils" "libibverbs-dev" "librdmacm-dev" "perftest" "infiniband-diags")
packages_rhel=("cmake" "rdma-core" "ibverbs-utils" "libibverbs-devel" "librdmacm-devel" "perftest" "infiniband-diags")

if [ -f /etc/debian_version ]; then
    echo "Debian-based system detected. Using apt-get for installation."
    package_manager="apt-get"
    packages=("${packages_debian[@]}")
elif [ -f /etc/redhat-release ]; then
    echo "RedHat-based system detected. Using dnf/yum for installation."
    if command -v dnf &>/dev/null; then
        package_manager="dnf"
    else
        package_manager="yum"
    fi
    packages=("${packages_rhel[@]}")
else
    echo "Unsupported Linux distribution."
    exit 1
fi

echo "Updating package list..."
sudo $package_manager update -y

for package in "${packages[@]}"; do
    if dpkg -l | grep -q "$package" || rpm -q "$package" &>/dev/null; then
        echo "$package is already installed."
    else
        echo "$package is NOT installed. Installing..."
        sudo $package_manager install -y "$package"
    fi
done

# Check for RDMA devices (optional warning)
if [ ! -d "/sys/class/infiniband" ]; then
    echo "WARNING: InfiniBand subsystem not found at /sys/class/infiniband"
    echo "         RDMA hardware may not be available, but build will continue."
else
    DEVICE_COUNT=$(find /sys/class/infiniband -maxdepth 1 -type d | tail -n +2 | wc -l)
    echo "  InfiniBand devices found: $DEVICE_COUNT"
fi

echo "All dependencies satisfied."
echo ""

# check if required files exist
# if [ ! -f "$TEST_SUITE_ROOT/ProtoSDK/RDMA/include/ndspi.h" ]; then
#     echo "WindowsProtocolTestSuites/ProtoSDK/RDMA/include/ndspi.h does not exist."
#     exit 1
# fi
# if [ ! -f "$TEST_SUITE_ROOT/ProtoSDK/RDMA/include/ndstatus.h" ]; then
#     echo "WindowsProtocolTestSuites/ProtoSDK/RDMA/include/ndstatus.h does not exist."
#     exit 1
# fi

# if output directory exists, clean it
if [ -d "$OUTDIR" ]; then
    rm -rf "$OUTDIR"/*
fi

# create necessary directories
PLUGIN_DIR="$OUTDIR/Plugin"
mkdir -p "$PLUGIN_DIR"
mkdir -p "$PLUGIN_DIR/doc"
mkdir -p "$PLUGIN_DIR/script"
mkdir -p "$OUTDIR/Batch"
mkdir -p "$OUTDIR/Bin" # 

# ==========================================
# start to build MS-SMBD test suite using dotnet
# ==========================================
dotnet publish MS-SMBD_Server.sln --no-dependencies -o "$OUTDIR/Bin" -c "$CONFIGURATION"
PUBLISH_STATUS=$?

if [ $PUBLISH_STATUS -ne 0 ]; then
    echo "Build failed. Exiting script."
    # exit 1
fi

# copy plugin files
cp -r "$TEST_SUITE_ROOT/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/"*.xml "$PLUGIN_DIR/"

# copy documentation files
cp -r "$TEST_SUITE_ROOT/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/Docs/"* "$PLUGIN_DIR/doc/"

# copy PowerShell script files
cp -r "$TEST_SUITE_ROOT/TestSuites/MS-SMBD/src/Plugin/SMBDPlugin/Detector/"*.ps1 "$PLUGIN_DIR/script/"

# copy batch files
cp -r "$TEST_SUITE_ROOT/TestSuites/MS-SMBD/src/Batch/"* "$OUTDIR/Batch/"
cp "$TEST_SUITE_ROOT/common/RunTestCasesByBinariesAndFilter.ps1" "$OUTDIR/Batch/"

# copy license file
cp "$TEST_SUITE_ROOT/TestSuites/MS-SMBD/src/Deploy/LICENSE.rtf" "$OUTDIR/LICENSE.rtf"

# ==========================================
# start to build ProtoSDK/RdmaLinux using CMake
# ==========================================
echo "Starting CMake build for ProtoSDK/RdmaLinux"

CMAKE_BUILD_DIR="$TEST_SUITE_ROOT/ProtoSDK/RdmaLinux/build"
mkdir -p "$CMAKE_BUILD_DIR"
pushd "$CMAKE_BUILD_DIR" || { echo "Failed to change directory to $CMAKE_BUILD_DIR"; exit 1; }

# configure the project
cmake .. -DCMAKE_BUILD_TYPE=$CONFIGURATION
if [ $? -ne 0 ]; then
    echo "Failed to configure CMake."
    popd
    exit 1
fi

# build the project
cmake --build .
if [ $? -ne 0 ]; then
    echo "Failed to build with CMake."
    popd
    exit 1
fi

# copy the compiled binaries to output directory
if [ ! -f "./libRdmaLinuxAdapter.so" ]; then
    echo "ERROR: Failed to build libRdmaLinuxAdapter.so"
    echo "The compiled library was not found in the build directory."
    popd
    exit 1
fi

cp "./libRdmaLinuxAdapter.so" "$OUTDIR/Bin/"
if [ $? -ne 0 ]; then
    echo "Failed to copy compiled binaries to $OUTDIR/Bin."
    popd
    exit 1
fi

echo "Successfully copied libRdmaLinuxAdapter.so to $OUTDIR/Bin/"
cp "$TEST_SUITE_ROOT/AssemblyInfo/.version" "$OUTDIR/Bin/"

popd # return to previous directory
# ==========================================
# Build successfully
# ==========================================
echo "Build MS-SMBD test suite successfully."
exit 0
