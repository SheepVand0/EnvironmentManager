using BeatSaberPlus.SDK.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.ProfileEdit.Widgets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class RightProfileEditViewController : ViewController<RightProfileEditViewController>
    {
        public static RightProfileEditViewController Instance;

        ObjectEditView m_ObjectEditView;

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                m_ObjectEditView = ObjectEditView.Make()

            ).BuildUI(transform);

            Instance = this;
        }

        public void SetObject(EMConfig.EMEditedElement p_Config)
        {
            m_ObjectEditView.SetObject(p_Config);
        }

        public void SetObject(EMConfig.EMEditedLight p_Light)
        {

        }
    }
}
