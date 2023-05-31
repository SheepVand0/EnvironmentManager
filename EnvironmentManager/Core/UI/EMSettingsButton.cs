using CP_SDK.XUI;
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

        public static EMSettingsButton Instance = null;


        internal static EMSettingsButton Create()
        {
            if (Instance != null)
                return Instance;

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
                Plugin.Log.Error("Cannot find environment settings panel");

            XUIVLayout.Make(
                EMSecondaryButton.Make("Environment Manager Settings", 40, 5, OpenSettings)
                .Bind(ref m_SettingsButton),
                XUIHLayout.Make(
                    XUIText.Make("Mod enabled:"),
                    EMToggleSetting.Make()
                    .Bind(ref m_ModEnableToggle)
                )
            )
            .SetWidth(40)
            .SetHeight(20)
            .BuildUI(l_Place.transform);

            Plugin.Log.Notice("Finished environment settings button creation");
        }

        protected void OpenSettings()
        {
            if (ProfileSelectionFlowCoordinator.Instance == null)
                ProfileSelectionFlowCoordinator.Instance = ProfileSelectionFlowCoordinator.Instance();

            ProfileSelectionFlowCoordinator.Instance.Present();
        }

    }
}
