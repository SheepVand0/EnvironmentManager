using HarmonyLib;
using IPA;
using IPALogger = IPA.Logging.Logger;

namespace EnvironmentManager
{
    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        /// <summary>
        /// Use to send log messages through BSIPA.
        /// </summary>
        internal static IPALogger Log { get; private set; }

        public static Harmony s_Harmony = new Harmony("sheepvand.environmentmanager");

        [Init]
        public Plugin(IPALogger logger)
        {
            Instance = this;
            Log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            s_Harmony.PatchAll();
        }

        [OnExit]
        public void OnApplicationQuit()
        { 
            s_Harmony.UnpatchSelf();
        }

    }
}
