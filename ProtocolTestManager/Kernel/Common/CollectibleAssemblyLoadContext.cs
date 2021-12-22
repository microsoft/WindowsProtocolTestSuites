// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Microsoft.Protocols.TestManager.Kernel
{
    public class CollectibleAssemblyLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver resolver;
        private string[] ignoredAssemblies;
        private string[] mixedAssemblies;

        public CollectibleAssemblyLoadContext(string mainAssemblyToLoadPath, string[] ignoredAssemblyName = null, string[] missedAssemblyName = null) : base(isCollectible: true)
        {
            resolver = new AssemblyDependencyResolver(mainAssemblyToLoadPath);
            ignoredAssemblies = ignoredAssemblyName;
            mixedAssemblies = missedAssemblyName;
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            if (ignoredAssemblies != null && ignoredAssemblies.Any(x=>assemblyName.Name.Contains(x)))
            {
                return null;
            }

            string assemblyPath = resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                if (mixedAssemblies != null && mixedAssemblies.Any(x => assemblyPath.Contains(x)))
                {
                    if (AssemblyLoadContext.Default.Assemblies.Any(ass => mixedAssemblies.Any(x => ass.FullName.Contains(x))))
                        return null;
                    return AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);
                }

                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }
    }
}
