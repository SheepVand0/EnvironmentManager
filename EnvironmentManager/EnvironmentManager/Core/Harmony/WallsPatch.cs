using EnvironmentManager.Config;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.Harmony
{
    [HarmonyPatch(typeof(ParametricBoxFrameController), "Refresh")]
    internal class WallsPatch
    {
        public static void Prefix(ParametricBoxFrameController __instance)
        {
            if (EMConfig.Instance.DisableWallsOutline)
                __instance.gameObject.SetActive(false);

            __instance.color = EMConfig.Instance.WallsOutlineColor;
        }

    }
}
