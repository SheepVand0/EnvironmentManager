using BeatSaberMarkupLanguage;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ScoreVisualizer
{
    internal class ScoreVisualizerFlowCoordinator : CustomFlowCoordinator
    {
        internal static ScoreVisualizerFlowCoordinator Instance;

        protected override void OnCreation()
        {
            Instance = this;
        }

        protected override string Title => "Score Visualizer";

        ScoreVisualizerConfigView View = BeatSaberUI.CreateViewController<ScoreVisualizerConfigView>();

        protected override (ViewController, ViewController, ViewController) GetUIImplementation()
        {
            return (View, null, null);
        }
    }
}
