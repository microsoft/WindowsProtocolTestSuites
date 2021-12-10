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
        private string _ignoredAssemblyName;
        private string _missedAssemblyName;

        public CollectibleAssemblyLoadContext(string mainAssemblyToLoadPath,string ignoredAssemblyName,string missedAssemblyName) : base(isCollectible: true)
        {
            _resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
            _ignoredAssemblyName = ignoredAssemblyName;
            _missedAssemblyName = missedAssemblyName;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (_ignoredAssemblyName.Split().Any(x=>assemblyName.Name.Contains(x)))
            {
                return null;
            }

            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                if (_missedAssemblyName.Split().Any(x => assemblyPath.Contains(x)))
                {
                    if (AssemblyLoadContext.Default.Assemblies.Any(ass => _missedAssemblyName.Split().Any(x => ass.FullName.Contains(x))))
                        return null;
                    return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                }

                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
