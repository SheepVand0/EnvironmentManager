using BeatSaberMarkupLanguage;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileSelection;
using EnvironmentManager.Core.UI.ScoreVisualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI
{
    internal class EMSettingsButton
    {

        protected XUISecondaryButton m_SettingsButton;
        protected XUIToggle m_ModEnableToggle;
        protected XUIVLayout m_Main;
        protected XUISecondaryButton m_ScoreVisualizerButton;
        protected XUIToggle m_ScoreVisualizerToggle;

        public static EMSettingsButton Instance = null;


        internal static EMSettingsButton Create()
        {
            if (Instance != null)
            {
                Instance.Init();
                return Instance;
            }

            Plugin.Log.Notice("Creating environment settings button");
            EMSettingsButton l_Button = new EMSettingsButton();
            l_Button.Init();
            Instance = l_Button;

            return l_Button;
        }

        internal async void Init()
        {
            GameObject l_Place = null;
            await WaitUtils.Wait(() =>
            {
                l_Place = GameObject.Find("EnvironmentOverrideSettings");
                return l_Place != null;
            }, 1);

            if (l_Place == null)
            {
                Plugin.Log.Error("Cannot find environment settings panel");
                return;
            }

            if (m_SettingsButton != null)
            {
                m_Main.Element.transform.SetParent(l_Place.transform, false);
                return;
            }

            XUIVLayout.Make(
                XUIHLayout.Make(
                    EMText.Make("Custom environment enabled:")
                        .SetFontSize(3.5f)
                        .SetStyle(TMPro.FontStyles.Italic),
                    EMToggleSetting.Make()
                        .SetValue(EMConfig.Instance.IsEnabled, true)
                        .OnValueChanged(x => { EMConfig.Instance.IsEnabled = x; EMConfig.Instance.Save(); m_SettingsButton.SetInteractable(x); })
                        .Bind(ref m_ModEnableToggle)
                ),
                EMSecondaryButton.Make("Environment Manager Settings", 60, 5, OpenSettings)
                        .Bind(ref m_SettingsButton),
                XUIHLayout.Make(
                    EMText.Make("Hit score visualizer enabled:")
                        .SetFontSize(3.5f)
                        .SetStyle(TMPro.FontStyles.Italic),
                    EMToggleSetting.Make()
                        .SetValue(EMConfig.Instance.ScoreVisualizerProfile.ScoreVisualizerEnabled)
                        .OnValueChanged(x =>
                        {
                            EMConfig.Instance.ScoreVisualizerProfile.ScoreVisualizerEnabled = x; 
                            EMConfig.Instance.Save();
                            m_ScoreVisualizerButton.SetInteractable(x);
                        })
                        .Bind(ref m_ScoreVisualizerToggle)
                ),
                EMSecondaryButton.Make("ScoreVisualizer settings", 60, 5, OpenSVSettings)
                        .Bind(ref m_ScoreVisualizerButton)
            )
            .SetWidth(120)
            .SetHeight(40)
            .SetPadding(20, 0, 0, 0)
            .Bind(ref m_Main)
            .BuildUI(l_Place.transform);

            Plugin.Log.Notice("Finished environment settings button creation");
        }

        protected void OpenSettings()
        {
            if (ProfileSelectionFlowCoordinator.Instance == null)
                ProfileSelectionFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<ProfileSelectionFlowCoordinator>();

            ProfileSelectionFlowCoordinator.Instance.Present();
        }

        protected void OpenSVSettings()
        {
            if (ScoreVisualizerFlowCoordinator.Instance == null)
                ScoreVisualizerFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<ScoreVisualizerFlowCoordinator>();

            ScoreVisualizerFlowCoordinator.Instance.Present();
        }

    }
}
