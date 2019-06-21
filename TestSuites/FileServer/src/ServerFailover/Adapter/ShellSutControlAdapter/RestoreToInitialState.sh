# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# This script is used to restore all cluster nodes to the initial state.

# The following example code starts ctdb services on all cluster nodes.
#
# node1=$PTFProp_Cluster_ClusterNode01
# node2=$PTFProp_Cluster_ClusterNode02
#
# echo "Reset node $node1..."
# ssh $node1 sudo systemctl start ctdb
# echo "Done!"
#
# echo "Reset node $node2..."
# ssh $node2 sudo systemctl start ctdb
# echo "Done!"
