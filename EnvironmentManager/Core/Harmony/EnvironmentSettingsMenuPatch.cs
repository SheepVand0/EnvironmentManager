using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvironmentManager.Core.UI;
using HarmonyLib;

namespace EnvironmentManager.Core.Harmony
{
    [HarmonyPatch(typeof(EnvironmentOverrideSettingsPanelController), "SetData")]
    internal class EnvironmentSettingsMenuPatch
    {
        public static void Postfix(EnvironmentOverrideSettingsPanelController __instance)
        {
            EMSettingsButton.Create();
        }

    }
}
