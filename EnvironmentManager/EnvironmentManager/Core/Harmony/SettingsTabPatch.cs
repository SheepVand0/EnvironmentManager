using HarmonyLib;
using IPA.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.Harmony
{
    //[HarmonyPatch(typeof(GameplaySetupViewController), nameof(GameplaySetupViewController.Setup))]
    internal class SettingsTabPatch
    {
        public async static void Postfix(GameplaySetupViewController __instance)
        {
            ///var l_ColorSettings = Resources.FindObjectsOfTypeAll<ColorsOverrideSettingsPanelController>().First();
            ///var l_EnvironmentSettingsPanel = Resources.FindObjectsOfTypeAll<EnvironmentOverrideSettingsPanelController>().First();

            //var l_EnvironmentSettingsGameObject = await ModLibrary.FindObject("EnvironmentOverrideSettings");
            //var l_ColorSettingsGameObjectParent = await ModLibrary.FindObject("ColorsOverrideSettings");
            //l_ColorSettingsGameObjectParent.SetActive(true);
            //var l_ColorSettingsGameObject = l_ColorSettingsGameObjectParent.transform.Find("Settings");
            //l_ColorSettingsGameObject.transform.SetParent(l_EnvironmentSettingsGameObject.transform, false);
            //l_ColorSettingsGameObject.transform.localPosition = new Vector3(0, -45, 0);
            //l_ColorSettingsGameObject.gameObject.SetActive(true);
        }

    }
}
