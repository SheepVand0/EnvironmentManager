using EnvironmentManager.Config;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.Harmony
{
    /*[HarmonyPatch(typeof(LightWithIdManager), nameof(LightWithIdManager.SetColorForId))]
    internal class LightChangedPatch
    {
        static PlayerData playerData;

        public static void Prefix(ref int lightId, ref Color color)
        {
            if (!EMConfig.Instance.IsEnabled) return;

            var l_Config = EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex];

            if (l_Config.OnlyInStaticLights) return;

            if (playerData == null)
                playerData = Resources.FindObjectsOfTypeAll<PlayerDataModel>().First().playerData;

            foreach (var l_Item in l_Config.EditedLights)
            {
                if (l_Item.LightIndex == lightId)
                {
                    if (color.ColorWithAlpha(1) == playerData.colorSchemesSettings.GetSelectedColorScheme().environmentColor0.ColorWithAlpha(1))
                    {
                        color = l_Item.LeftColor.ColorWithAlpha(color.a);
                        return;
                    }

                    if (color.ColorWithAlpha(1) == playerData.colorSchemesSettings.GetSelectedColorScheme().environmentColor1.ColorWithAlpha(1))
                    {
                        color = l_Item.RightColor.ColorWithAlpha(color.a);
                        return;
                    }
                }
            }
        }

    }*/
}
