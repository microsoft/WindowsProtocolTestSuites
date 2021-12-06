// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public CollectibleAssemblyLoadContext(string mainAssemblyToLoadPath) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (assemblyName.Name.Contains("PropertyValueDetector"))
            {
                return null;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                if(assemblyPath.Contains("Rdma"))
                {
                    if (AssemblyLoadContext.Default.Assemblies.Any(ass => ass.FullName.Contains("Rdma")))
                        return null;
                    return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                }

                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
