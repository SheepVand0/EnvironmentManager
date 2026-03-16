using CP_SDK.Misc;
using EnvironmentManager.Config;
using HarmonyLib;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.Harmony
{
/*    [HarmonyPatch(typeof(SaveDataExtensions), nameof(SaveDataExtensions.SaveToJSONFile), new Type[] { typeof(ISaveData), typeof(object), typeof(string), typeof(bool) })]
    internal class OnSave
    {

        public static void Prefix(ref object obj)
        {
            try
            {
                object l_Data = (object)obj;
                PlayerSaveData l_CData = (PlayerSaveData)l_Data;
                l_CData.localPlayers[0].overrideEnvironmentSettings.overrideEnvironments = EMConfig.Instance.OverrideEnvironments;
                if (EMConfig.Instance.UserSelectedEnvironment == null)
                {
                    //EMConfig.Instance.UserSelectedEnvironment = UnityEngine.Resources.FindObjectsOfTypeAll<EnvironmentOverrideSettingsPanelController>().First().GetField<>()
                }

            } catch
            {

            }
        }

    }*/
}
