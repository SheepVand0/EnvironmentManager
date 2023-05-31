using CP_SDK.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace EnvironmentManager.Config
{
    internal class EMConfig : JsonConfig<EMConfig>
    {

        public override string GetRelativePath()
            => $"{CP_SDK.ChatPlexSDK.ProductName}/EnvironmentManager/Config";

        internal static EMProfile s_DefaultConfig = new EMProfile(false);

        [JsonProperty] internal bool IsEnabled = true;
        
        [JsonProperty] internal List<EMProfile> UserProfiles = new List<EMProfile>();

        internal class EMEditedElement
        {
            [JsonProperty] internal string Name;

            [JsonProperty] internal bool Hide;
            [JsonProperty] internal bool ForceShow;
            [JsonProperty] internal bool Moved;

            [JsonProperty] internal Vector3 CustomPosition;
            [JsonProperty] internal Vector3 CustomRotationEulers;
            [JsonProperty] internal Vector3 CustomScale;
        }

        internal class EMProfile
        {
            [JsonProperty] internal string Name;

            [JsonProperty] internal bool UseOnSpecificEnvironment = false;
            [JsonProperty] internal List<string> SpecificEnvironmentsNames = new List<string>();

            [JsonProperty] internal bool Deletable = true;
            [JsonProperty] internal List<EMEditedElement> EditedElements = new List<EMEditedElement>();

            internal EMProfile()
            {
                // Ignored
            }

            internal EMProfile(bool p_Deletable)
            {
                Deletable = p_Deletable;
            }
        }
    
    }
}
