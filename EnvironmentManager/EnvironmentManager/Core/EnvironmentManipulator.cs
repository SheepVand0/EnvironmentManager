using EnvironmentManager.Config;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core
{
    internal class EnvironmentManipulator
    {
        public static bool EnvironmentLoaded { private set; get; }

        public static LightWithIdManager LightManager { private set; get; }

        public static PlayerData PlayerConf { private set; get; }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public static async Task<bool> LoadEnvironment()
        {


            try
            {
                var l_GameScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().First();
                var l_MenuEnvironmentManager = Resources.FindObjectsOfTypeAll<MenuEnvironmentManager>().First();
                l_MenuEnvironmentManager.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.None);

                l_GameScenesManager.MarkSceneAsPersistent("MenuCore");

                var l_MenuTransitionHelper = Resources.FindObjectsOfTypeAll<MenuTransitionsHelper>().First();
                var l_SceneTransitionSetupData = l_MenuTransitionHelper.GetField<StandardLevelScenesTransitionSetupDataSO, MenuTransitionsHelper>("_standardLevelScenesTransitionSetupData");

                var l_BeatmapLevelsModel = Resources.FindObjectsOfTypeAll<BeatmapLevelsModel>().First();
                var l_FirstMap = SongCore.Loader.CustomLevels.Values.First();
                var l_CustomLevelLoader = Resources.FindObjectsOfTypeAll<CustomLevelLoader>().First();
                var l_BeatmapResult = await l_CustomLevelLoader.LoadCustomBeatmapLevelAsync(l_FirstMap, new System.Threading.CancellationToken());

                var l_PlayerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;

                IBeatmapLevel l_FinalBossChanLevel = l_BeatmapResult;
                IDifficultyBeatmap l_DifficultyBeatmap = l_FinalBossChanLevel.beatmapLevelData.difficultyBeatmapSets.First().difficultyBeatmaps.Last();
                l_SceneTransitionSetupData.Init("Standard", l_DifficultyBeatmap,
                l_FinalBossChanLevel, l_PlayerData.overrideEnvironmentSettings,
                l_PlayerData.colorSchemesSettings.GetSelectedColorScheme(),
                l_PlayerData.gameplayModifiers, l_PlayerData.playerSpecificSettings,
                null, string.Empty);

                l_GameScenesManager.PushScenes(l_SceneTransitionSetupData, 0.25f, null, (x) =>
                {
                    var l_StandardGameplayTransform = GameObject.Find("StandardGameplay").transform;

                    l_MenuEnvironmentManager.transform.root.gameObject.SetActive(true);
                    Resources.FindObjectsOfTypeAll<MenuShockwave>().First().gameObject.SetActive(false);
                    Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().First().CrossfadeToDefault();

                    foreach (Transform l_Child in l_StandardGameplayTransform)
                        l_Child.gameObject.SetActive(false);

                    if (!Environment.GetCommandLineArgs().Any(y => y.ToLowerInvariant() == "fpfc"))
                    {
                        GameObject.Find("EventSystem").gameObject.SetActive(true);
                        GameObject.Find("ControllerLeft").gameObject.SetActive(true);
                        GameObject.Find("ControllerRight").gameObject.SetActive(true);
                    }

                    var l_FadeControlller = Resources.FindObjectsOfTypeAll<FadeInOutController>().First();
                    l_FadeControlller.FadeOut();

                    EnvironmentLoaded = true;
                });
            }
            catch (Exception ex)
            {
                Plugin.Log.Error(ex);
                return false;
            }
            return true;
        }

        public static void UnloadEnvironment()
        {
            GameScenesManager l_ScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault();
            l_ScenesManager.PopScenes(0.25f, null, (_) =>
            {
                //Resources.FindObjectsOfTypeAll<BeatSaberPlus.UI.MainViewFlowCoordinator>().FirstOrDefault().SetField("showBackButton", true);
                HashSet<string> l_Scenes = l_ScenesManager.GetField<HashSet<string>, GameScenesManager>("_neverUnloadScenes");
                l_Scenes.Remove("MenuCore");
                l_ScenesManager.SetField("_neverUnloadScenes", l_Scenes);
                Resources.FindObjectsOfTypeAll<FadeInOutController>().FirstOrDefault().FadeIn();
                Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault().CrossfadeToDefault();
                Resources.FindObjectsOfTypeAll<MenuEnvironmentManager>().First().ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);

                var l_MenuEnvironmentManager = Resources.FindObjectsOfTypeAll<MenuEnvironmentManager>().First();
                l_MenuEnvironmentManager.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);

                EnvironmentLoaded = false;
            });
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public static void ApplyObjectConfig(EMConfig.EMEditedElement p_Object)
        {
            GameObject l_Object = GameObject.Find("Environment").transform.Find(p_Object.Name).gameObject;
            if (l_Object == null) return;

            if (p_Object.Hide)
            {
                l_Object.transform.gameObject.SetActive(false);
            }
            else
            {
                if (p_Object.ForceShow)
                    l_Object.transform.gameObject.SetActive(true);

                if (p_Object.Move)
                {
                    l_Object.transform.localPosition = p_Object.CustomPosition;
                    l_Object.transform.localRotation = Quaternion.Euler(p_Object.CustomRotationEulers);
                    l_Object.transform.localScale = p_Object.CustomScale;
                }
            }
        }

        public static void ApplyDefaultLightConfig(EMConfig.EMEditedLight p_Light)
        {
            if (LightManager == null)
            {
                LightManager = Resources.FindObjectsOfTypeAll<LightWithIdManager>().First();
            }

            LightManager.SetColorForId(p_Light.LightIndex, p_Light.LeftColor);
        }

        public static void HandleLightChanged(int p_LightId, ref Color p_Color)
        {
            if (PlayerConf == null)
            {
                PlayerConf = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;
            }

            EMConfig.EMEditedLight l_Light = EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights.Where(x => x.LightIndex == p_LightId).First();
            if (l_Light == null) return;

            if (PlayerConf.colorSchemesSettings.GetSelectedColorScheme().environmentColor0 == p_Color)
            {
                p_Color = l_Light.LeftColor;
            }
            else if (PlayerConf.colorSchemesSettings.GetSelectedColorScheme().environmentColor1 == p_Color)
            {
                p_Color = l_Light.RightColor;
            }
        }

        public static void ApplyProfile(EMConfig.EMProfile p_Profile)
        {
            foreach (var l_Item in p_Profile.EditedElements)
            {
                ApplyObjectConfig(l_Item);
            }

            foreach (var l_Item in p_Profile.EditedLights)
            {
                ApplyDefaultLightConfig(l_Item);
            }
        }

    }
}
