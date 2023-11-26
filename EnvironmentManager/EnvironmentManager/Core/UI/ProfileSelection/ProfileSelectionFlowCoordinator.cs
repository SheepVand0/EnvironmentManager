using BeatSaberMarkupLanguage;
using CP_SDK.UI;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileSelection
{
    internal class ProfileSelectionFlowCoordinator : CustomFlowCoordinator
    {
        internal static ProfileSelectionFlowCoordinator Instance = null;

        

        protected ProfileSelectionViewController m_ViewController = BeatSaberUI.CreateViewController<ProfileSelectionViewController>();

        protected override string Title => "Edit";

        protected override (ViewController, ViewController, ViewController) GetUIImplementation()
        {
            return (m_ViewController, null, null);
        }
    }
}
