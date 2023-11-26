using CP_SDK.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnvironmentManager.Config
{
    internal class EMConfig : JsonConfig<EMConfig>
    {

        public override string GetRelativePath()
            => $"{CP_SDK.ChatPlexSDK.ProductName}/EnvironmentManager/Config";

        internal static EMProfile s_DefaultConfig = new EMProfile(false);

        [JsonProperty] internal bool IsEnabled = true;
        [JsonProperty] internal int SelectedIndex = 0;

        [JsonProperty] internal List<EMProfile> UserProfiles = new List<EMProfile>();

        public bool ProfileNameAlreadyExists(string p_Name)
        {
            return UserProfiles.Where(x => x.Name == p_Name).Count() != 0;
        }

        protected override void OnInit(bool p_OnCreation)
        {
            if (p_OnCreation) return;

            for (int l_i = 0; l_i < UserProfiles.Count;l_i++)
            {
                if (UserProfiles[l_i].Name == string.Empty)
                {
                    string l_Name = $"EmptyName{l_i}";
                    while (ProfileNameAlreadyExists(l_Name))
                        l_Name += "_";
                    UserProfiles[l_i].Name = l_Name;
                    Save();
                }
            }
        }

        internal class EMEditedElement
        {
            [JsonProperty] internal string Name;

            [JsonProperty] internal bool Hide;
            [JsonProperty] internal bool ForceShow;
            [JsonProperty] internal bool Move;

            [JsonProperty] internal Vector3 CustomPosition;
            [JsonProperty] internal Vector3 CustomRotationEulers;
            [JsonProperty] internal Vector3 CustomScale;
        }

        internal class EMEditedLight
        {
            [JsonProperty] internal string Name;
            [JsonProperty] internal int LightIndex;
            [JsonProperty] internal Color LeftColor;
            [JsonProperty] internal Color RightColor;
        }

        internal class EMProfile
        {
            [JsonProperty] internal string Name;

            [JsonProperty] internal bool UseOnSpecificEnvironment = false;
            [JsonProperty] internal List<string> SpecificEnvironmentsNames = new List<string>();

            [JsonProperty] internal bool Deletable = true;
            [JsonProperty] internal List<EMEditedElement> EditedElements = new List<EMEditedElement>();
            [JsonProperty] internal List<EMEditedLight> EditedLights = new List<EMEditedLight>();

            [JsonProperty] internal bool InvertLights;
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
