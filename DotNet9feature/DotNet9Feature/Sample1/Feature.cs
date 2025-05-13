using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet9Feature
{
    public  class Feature
    {
        [FeatureSwitchDefinition("Feature.IsEnabled")]
        internal static bool IsFeatureEnabled => !AppContext.TryGetSwitch("Feature.IsEnabled", out var isEnabled) || isEnabled;
        internal static void DoSomething()
        {
            Console.WriteLine("I called something");
        }
    }
}