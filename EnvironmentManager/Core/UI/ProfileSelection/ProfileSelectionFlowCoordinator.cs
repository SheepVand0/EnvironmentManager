using CP_SDK.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileSelection
{
    internal class ProfileSelectionFlowCoordinator : CP_SDK.UI.FlowCoordinator<ProfileSelectionFlowCoordinator>
    {
        internal static ProfileSelectionFlowCoordinator Instance = null;

        public override string Title => "Your Profiles";

        protected ProfileSelectionViewController m_ViewController = UISystem.CreateViewController<ProfileSelectionViewController>();

        protected override (IViewController, IViewController, IViewController) GetInitialViewsController()
        {
            return (m_ViewController, null, null);
        }
    }
}
