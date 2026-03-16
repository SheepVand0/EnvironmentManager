using EnvironmentManager.Config;
using HarmonyLib;
using IPA.Utilities;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace EnvironmentManager.Core.Harmony
{
    [HarmonyPatch(typeof(FlyingScoreEffect), "RefreshScore")]
    internal class FlyingScorePatch
    {
        internal static List<EMConfig.EMScoreVisualizerElement> LoadedConfig = new List<EMConfig.EMScoreVisualizerElement>();

        public static void Postfix(FlyingScoreEffect __instance, TextMeshPro ____text, ref int score)
        {
            if (!EMConfig.Instance.ScoreVisualizerProfile.ScoreVisualizerEnabled) return;

            for (int l_i = 0; l_i < LoadedConfig.Count;l_i++)
            {
                var l_Current = LoadedConfig[l_i];
                if (score <= l_Current.MaxRange && score >= l_Current.MinRange)
                {
                    ____text.richText = true;
                    ____text.SetText(ModLibrary.ScoreVisualizerGetText(score, __instance.GetField<IReadonlyCutScoreBuffer, FlyingScoreEffect>("_cutScoreBuffer") ,l_Current));
                    ____text.fontStyle = FontStyles.Normal;
                    __instance.GetField<SpriteRenderer, FlyingScoreEffect>("_maxCutDistanceScoreIndicator").enabled = false;
                    __instance.SetField("_color", l_Current.Color);
                    __instance.SetField("_colorAMultiplier", l_Current.Color.a);
                    //____text.tintAllSprites
                    return;
                }

            }
        }
    }
}
