using CP_SDK.Config;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using UnityEngine;

namespace EnvironmentManager.Config
{
    internal class EMConfig : JsonConfig<EMConfig>
    {

        static List<JsonConverter> Converters = new List<JsonConverter>();

        public override string GetRelativePath()
            => $"EnvironmentManager/Config";

        internal static EMProfile s_DefaultConfig = new EMProfile(false);

        [JsonProperty] internal bool IsEnabled = true;
        [JsonProperty] internal int SelectedIndex = 0;

        [JsonProperty] internal List<EMProfile> UserProfiles = new List<EMProfile>();

        [JsonProperty] internal bool OverrideEnvironments = false;
        [JsonProperty] internal EnvironmentInfoSO UserSelectedEnvironment = null;

        [JsonProperty] internal Color WallsOutlineColor = Color.white;
        [JsonProperty] internal bool DisableWallsOutline = false;

        [JsonProperty] internal EMScoreVisualizerProfile ScoreVisualizerProfile = new EMScoreVisualizerProfile();

        public bool ProfileNameAlreadyExists(string p_Name)
        {
            return UserProfiles.Where(x => x.Name == p_Name).Count() != 0;
        }

        protected override void OnInit(bool p_OnCreation)
        {
            Converters = m_JsonConverters;

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

            if (ScoreVisualizerProfile == null)
                ScoreVisualizerProfile = new EMScoreVisualizerProfile();
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
            internal static EMEditedLight Default = new EMEditedLight();

            [JsonProperty] internal int LightIndex = 0;
            [JsonProperty] internal Color LeftColor = new Color(1, 0, 0, 1);
            [JsonProperty] internal Color RightColor = new Color(0, 0, 1, 1);
        }

        internal class EMProfile
        {
            [JsonProperty] internal string Name;

            [JsonProperty] internal bool UseOnSpecificEnvironment = false;
            [JsonProperty] internal List<string> SpecificEnvironmentsNames = new List<string>();

            [JsonProperty] internal bool Deletable = true;
            [JsonProperty] internal List<EMEditedElement> EditedElements = new List<EMEditedElement>();
            [JsonProperty] internal List<EMEditedLight> EditedLights = new List<EMEditedLight>();

            [JsonProperty] internal int RingsCount = 16;

            [JsonProperty] internal bool InvertLights;

            [JsonProperty] internal bool OnlyInStaticLights = false;

            internal EMProfile()
            {
                // Ignored
            }

            internal EMProfile(bool p_Deletable)
            {
                Deletable = p_Deletable;
            }
        }

        internal class EMScoreVisualizerElement
        {
            [JsonProperty] internal int MinRange = 100;
            [JsonProperty] internal int MaxRange = 115;

            [JsonProperty] internal float FontSize = 3.5f;
            [JsonProperty] internal Color Color = Color.white;

            /// <summary>
            /// /s : score
            /// </summary>
            [JsonProperty] internal string ScoreText = "/s";

            [JsonProperty] internal bool Italic = false;
            [JsonProperty] internal bool Bold = false;
        }

        internal class EMScoreVisualizerProfile
        {
            [JsonProperty] internal string ConfigPath = "./UserData/EMScoreVisualizer.json";
            [JsonProperty] internal bool ScoreVisualizerEnabled = false;

            internal static List<EMScoreVisualizerElement> Load(string configPath)
            {
                if (!File.Exists(configPath))
                {
                    Save(configPath, new List<EMScoreVisualizerElement>() { new EMScoreVisualizerElement() });
                }

                
                var l_Res = JsonConvert.DeserializeObject<List<EMScoreVisualizerElement>>(File.ReadAllText(configPath), Converters.ToArray());
                return l_Res;
            }

            internal static void Save(string configPath, List<EMScoreVisualizerElement> config)
            {
                File.WriteAllText(configPath, JsonConvert.SerializeObject(config, Converters.ToArray()));
            }
        }

        internal JsonConverter[] GetConverters()
        {
            return m_JsonConverters.ToArray();
        }

    }
}
