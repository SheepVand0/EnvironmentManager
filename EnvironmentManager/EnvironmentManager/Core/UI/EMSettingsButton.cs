﻿using BeatSaberMarkupLanguage;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileSelection;
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
            //m_SettingsButton.BuildUI(p_Transform);
            
            
            GameObject l_Place = null;
            await WaitUtils.Wait(() =>
            {
                l_Place = GameObject.Find("EnvironmentOverrideSettings"/*.Settings.Elements"*/);
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
                EMSecondaryButton.Make("Environment Manager Settings", 60, 5, OpenSettings)
                .Bind(ref m_SettingsButton),
                XUIHLayout.Make(
                    XUIText.Make("Mod enabled:"),
                    EMToggleSetting.Make()
                    .SetValue(EMConfig.Instance.IsEnabled, true)
                    .OnValueChanged(x => { EMConfig.Instance.IsEnabled = x; EMConfig.Instance.Save(); m_SettingsButton.SetInteractable(x); })
                    .Bind(ref m_ModEnableToggle)
                )
            )
            .SetWidth(60)
            .SetHeight(20)
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

    }
}
