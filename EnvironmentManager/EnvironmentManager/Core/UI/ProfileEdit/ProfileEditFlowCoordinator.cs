using CP_SDK.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class ProfileEditFlowCoordinator : CP_SDK.UI.FlowCoordinator<ProfileEditFlowCoordinator>
    {
        public override string Title => "Environment Manager";



        protected override (IViewController, IViewController, IViewController) GetInitialViewsController()
        {
            throw new NotImplementedException();
        }
    }
}
