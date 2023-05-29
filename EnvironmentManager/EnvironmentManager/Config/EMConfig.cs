using CP_SDK.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Config
{
    internal class EMConfig : JsonConfig<EMConfig>
    {

        public override string GetRelativePath()
            => $"{CP_SDK.ChatPlexSDK.ProductName}/EnvironmentManager/Config";

        [JsonProperty] internal bool IsEnabled = true;
         
    
    }
}
