using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace KellermanSoftware.CompareNetObjects
{
    /// <summary>
    /// Used internally to verify the config settings before comparing
    /// </summary>
    public class VerifyConfig
    {
        /// <summary>
        /// Verifies the specified configuration.
        /// </summary>
        /// <param name="config">The configuration.</param>
        public void Verify(ComparisonConfig config)
        {
            VerifySpec(config);
        }

        /// <summary>
        /// Verifies the collection matching spec.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <exception cref="Exception">
        /// </exception>
        public void VerifySpec(ComparisonConfig config)
        {
            if (config.CollectionMatchingSpec == null)
                return;

            foreach (var kvp in config.CollectionMatchingSpec)
            {
                if (TypeHelper.IsIList(kvp.Key))
                {
                    string msg = string.Format(
                        "Collection Matching Spec Type {0} should be a class, not a List.  Expected something like Customer, not List<Customer>.  See https://github.com/GregFinzer/Compare-Net-Objects/wiki/Comparing-Lists-of-Different-Lengths",
                        kvp.Key.Name);
                    throw new Exception(msg);
                }


                if (!TypeHelper.IsClass(kvp.Key) && !TypeHelper.IsInterface(kvp.Key))
                {
                    string msg = string.Format(
                        "Collection matching spec Type {0} should be a class or an interface.  See https://github.com/GregFinzer/Compare-Net-Objects/wiki/Comparing-Lists-of-Different-Lengths",
                        kvp.Key.Name);
                    throw new Exception(msg);
                }

                List<PropertyInfo> propertyInfos = Cache.GetPropertyInfo(config, kvp.Key).ToList();

                foreach (var index in kvp.Value)
                {
                    if (propertyInfos.All(o => o.Name != index))
                    {
                        string msg = string.Format("Collection Matching Spec cannot find property {0} of type {1}",
                            index, kvp.Key.Name);
                        throw new Exception(msg);
                    }
                        
                }
            }
        }
    }
}
