using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnvironmentManager.Config;
using HarmonyLib;

namespace EnvironmentManager.Core.Harmony
{
    [HarmonyPatch(typeof(EnvironmentSceneSetup), nameof(EnvironmentSceneSetup.InstallBindings))]
    internal class OnGameStarted
    {

        public static async void Postfix()
        {
            if (!EMConfig.Instance.IsEnabled) return;

            await Task.Delay(500);

            EnvironmentManipulator.ToActivate.Clear();
            EnvironmentManipulator.sOriginalPositions.Clear();
            EnvironmentManipulator.ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);
        }

    }

    [HarmonyPatch(typeof(AudioTimeSyncController), nameof(AudioTimeSyncController.SeekTo))]
    internal class OnSeek
    {
        public static void Postfix()
        {
            OnGameStarted.Postfix();
        }
    }
}
