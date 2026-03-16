using EnvironmentManager.Config;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core
{
    internal class ModLibrary
    {

        /// <summary>
        /// Only uses this only if you're sure that the object will spawn
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static async Task<GameObject> FindObject(string objectName, int tryCount = 0)
        {
            GameObject l_Object = null;
            await WaitUtils.Wait(() =>
            {
                l_Object = GameObject.Find(objectName);
                if (l_Object == null) return false;

                return true;
            },1 , p_MaxTryCount: tryCount);

            return l_Object;
        }

        public static string ScoreVisualizerGetText(int score, IReadonlyCutScoreBuffer scoreBuffer, EMConfig.EMScoreVisualizerElement config)
        {
            string l_Result = config.ScoreText;
            l_Result = l_Result.Replace("/s", score.ToString());
            l_Result = l_Result.Replace("/a", scoreBuffer.centerDistanceCutScore.ToString());
            l_Result = $"<size={config.FontSize}>" + l_Result;
            if (config.Bold)
                l_Result = "<b>" + l_Result;
            if (config.Italic)
                l_Result = "<i>" + l_Result;
            return l_Result;
        }

    }
}
