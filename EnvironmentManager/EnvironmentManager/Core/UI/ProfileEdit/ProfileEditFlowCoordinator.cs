using BeatSaberMarkupLanguage;
using CP_SDK.UI;
using HMUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class ProfileEditFlowCoordinator : CustomFlowCoordinator
    {
        protected override string Title => "Environment Manager";

        public static ProfileEditFlowCoordinator Instance;

        protected override void OnCreation()
        {
            Instance = this;
            topViewController.__Deactivate(false, true, false);
        }

        public ProfileEditViewController ViewController = BeatSaberUI.CreateViewController<ProfileEditViewController>();

        public RightProfileEditViewController RightView = BeatSaberUI.CreateViewController<RightProfileEditViewController>();
        public EnvironmentObjectsListViewController LeftView = BeatSaberUI.CreateViewController<EnvironmentObjectsListViewController>();

        protected override (HMUI.ViewController, HMUI.ViewController, HMUI.ViewController) GetUIImplementation()
        {
            return (ViewController, LeftView, RightView);
        }

        protected override void OnShow()
        {
            _ = EnvironmentManipulator.LoadEnvironment();
        }

    }
}
