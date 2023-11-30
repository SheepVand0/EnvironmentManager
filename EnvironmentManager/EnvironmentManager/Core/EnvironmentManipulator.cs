using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.ProfileEdit;
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

        private static PlayerData _playerData { set; get; }

        public static PlayerData PlayerConf { private set { } get { if (_playerData == null)
                {
                    _playerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;
                }

                return _playerData;
            }
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public static async Task<bool> LoadEnvironment()
        {
            if (EnvironmentLoaded == true) return false;
            Plugin.s_Harmony.UnpatchSelf();

            sOriginalPositions.Clear();
            ToActivate.Clear();

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

                l_GameScenesManager.PushScenes(l_SceneTransitionSetupData, 0.25f, null, async (x) =>
                {
                    var l_StandardGameplayTransform = GameObject.Find("StandardGameplay").transform;

                    l_MenuEnvironmentManager.transform.root.gameObject.SetActive(true);
                    Resources.FindObjectsOfTypeAll<MenuShockwave>().First().gameObject.SetActive(false);
                    //Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().First().CrossfadeToDefault();

                    if (!Environment.GetCommandLineArgs().Any(y => y.ToLowerInvariant() == "fpfc"))
                    {
                        GameObject.Find("EventSystem").gameObject.SetActive(true);
                        GameObject.Find("ControllerLeft").gameObject.SetActive(true);
                        GameObject.Find("ControllerRight").gameObject.SetActive(true);
                    }

                    foreach (Transform l_Child in l_StandardGameplayTransform)
                        l_Child.gameObject.SetActive(false);

                    //l_StandardGameplayTransform.Find("GameplayCore").gameObject.SetActive(false);

                    var l_FadeControlller = Resources.FindObjectsOfTypeAll<FadeInOutController>().First();
                    l_FadeControlller.FadeOut();

                    await Task.Delay(1000);

                    //l_StandardGameplayTransform.Find("GameplayCore").gameObject.SetActive(true);

                    Resources.FindObjectsOfTypeAll<EnvironmentObjectsListViewController>().First().UpdateObjects();

                    ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);

                    Plugin.s_Harmony.PatchAll();

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

        public static void UnloadEnvironment(Action p_FinishedCallback = null)
        {
            if (EnvironmentLoaded == false) return;

            GameScenesManager l_ScenesManager = Resources.FindObjectsOfTypeAll<GameScenesManager>().FirstOrDefault();
            l_ScenesManager.PopScenes(0.25f, null, (_) =>
            {
                //Resources.FindObjectsOfTypeAll<BeatSaberPlus.UI.MainViewFlowCoordinator>().FirstOrDefault().SetField("showBackButton", true);
                HashSet<string> l_Scenes = l_ScenesManager.GetField<HashSet<string>, GameScenesManager>("_neverUnloadScenes");
                l_Scenes.Remove("MenuCore");
                l_ScenesManager.SetField("_neverUnloadScenes", l_Scenes);

                Resources.FindObjectsOfTypeAll<SongPreviewPlayer>().FirstOrDefault().CrossfadeToDefault();
                Resources.FindObjectsOfTypeAll<MenuEnvironmentManager>().First().ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);

                var l_MenuEnvironmentManager = Resources.FindObjectsOfTypeAll<MenuEnvironmentManager>().First();
                l_MenuEnvironmentManager.ShowEnvironmentType(MenuEnvironmentManager.MenuEnvironmentType.Default);

                if (p_FinishedCallback != null)
                    try
                    {
                        p_FinishedCallback.Invoke();
                    } catch (Exception ex)
                    {
                        Plugin.Log.Error(ex);
                    }

                Resources.FindObjectsOfTypeAll<FadeInOutController>().FirstOrDefault().FadeIn();

                EnvironmentLoaded = false;
            });
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public static List<GameObject> ToActivate = new List<GameObject>();

        public static async Task<Task> ShowAllOnEnvironment(bool p_Active)
        {
            GameObject l_Environment = null;
            await WaitUtils.Wait(() =>
            {
                if (l_Environment == null)
                {
                    l_Environment = GameObject.Find("Environment");
                    return false;
                }
                return true;
            }, 10);

            l_Environment.SetActive(false);
            await Task.Delay(100);
            l_Environment.SetActive(true);

            bool l_SetHiddenElems = ToActivate.Any() == false;

            if (l_SetHiddenElems)
            {
                foreach (Transform l_Item in l_Environment.transform)
                {

                    if (l_Item.gameObject.activeInHierarchy)
                        ToActivate.Add(l_Item.gameObject);

                }
            }

            foreach (var l_Object in ToActivate)
                l_Object.SetActive(p_Active);

            return Task.CompletedTask;
        }

        public static Dictionary<GameObject, (Vector3, Vector3, Vector3)> sOriginalPositions = new Dictionary<GameObject, (Vector3, Vector3, Vector3)>();

        public static async void ApplyObjectConfig(EMConfig.EMEditedElement p_Object)
        {
            if (p_Object == null) return;

            GameObject l_PreObject = null;
            await WaitUtils.Wait(() =>
            {
                if (l_PreObject == null)
                {
                    l_PreObject = GameObject.Find("Environment");
                    return false;
                }
                return true;
            }, 10);

            GameObject l_Object = l_PreObject.transform.Find(p_Object.Name).gameObject;

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
                    if (sOriginalPositions.Keys.Contains(l_Object) == false)
                    {
                        sOriginalPositions.Add(l_Object, (l_Object.transform.localPosition, l_Object.transform.localRotation.eulerAngles, l_Object.transform.localScale));
                    }
                    l_Object.transform.localPosition = p_Object.CustomPosition;
                    l_Object.transform.localRotation = Quaternion.Euler(p_Object.CustomRotationEulers);
                    l_Object.transform.localScale = p_Object.CustomScale;
                    
                } else
                {
                    if (sOriginalPositions.Keys.Contains(l_Object))
                    {
                        sOriginalPositions.TryGetValue(l_Object, out var l_Value);
                        l_Object.transform.localPosition = l_Value.Item1;
                        l_Object.transform.localRotation = Quaternion.Euler(l_Value.Item2);
                        l_Object.transform.localScale = l_Value.Item3;
                    }
                }
            }
        }

        public static void ApplyLightConfig(EMConfig.EMEditedLight p_Light)
        {
            if (LightManager == null)
            {
                InitLightManager();
            }

            LightManager.SetColorForId(p_Light.LightIndex, p_Light.LeftColor);
        }

        public static void HandleLightChanged(int p_LightId, ref Color p_Color)
        {

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

        public static async void ApplyProfile(EMConfig.EMProfile p_Profile)
        {
            await ShowAllOnEnvironment(true);

            foreach (var l_Item in p_Profile.EditedElements)
            {
                try
                {
                    ApplyObjectConfig(l_Item);
                } catch
                {
                    // ignored
                }
            } 

            ApplyLights(p_Profile.EditedLights);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        static Dictionary<int, Color> m_DefaultEnvironmentColors = new Dictionary<int, Color>();

        static LightWithIdManager m_LightManager;

        protected static async void InitLightManager()
        {
            if (m_LightManager != null) return;
            m_DefaultEnvironmentColors.Clear();

            await WaitUtils.Wait(() => Resources.FindObjectsOfTypeAll<LightWithIdManager>().Length > 1, 10);

            m_LightManager = Resources.FindObjectsOfTypeAll<LightWithIdManager>()[1];
            Plugin.Log.Info((m_LightManager == null).ToString());

            Color?[] l_Colors = m_LightManager.GetField<Color?[], LightWithIdManager>("_colors");

            for (int l_i = 0; l_i < l_Colors.Length;l_i++)
            {
                if (l_Colors[l_i] != null)
                    m_DefaultEnvironmentColors.Add(l_i, (Color)l_Colors[l_i]);
            }
        }

        public static void SetToDefaultLights()
        {
            InitLightManager();

            for (int l_i = 0; l_i < m_LightManager.GetField<List<ILightWithId>[], LightWithIdManager>("_lights").Length;l_i++)
            {
                m_LightManager.SetColorForId(l_i, Color.clear);
            }

            m_LightManager.SetColorForId(5, PlayerConf.colorSchemesSettings.GetSelectedColorScheme().saberBColor);
        }

        public static void ApplyLights(List<EMConfig.EMEditedLight> p_Lights)
        {
            if (m_LightManager == null) InitLightManager();

            SetToDefaultLights();

            foreach (var l_Item in p_Lights)
            {
                m_LightManager.SetColorForId(l_Item.LightIndex, l_Item.LeftColor);
            }
        }
    }

}
