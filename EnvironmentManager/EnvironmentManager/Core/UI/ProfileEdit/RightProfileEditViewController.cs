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
        LightEditView m_LightEditView;

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                (m_ObjectEditView = ObjectEditView.Make()).SetActive(false),
                (m_LightEditView = LightEditView.Make()).SetActive(false)
            ).BuildUI(transform);

            Instance = this;
        }

        public void SetObject(EMConfig.EMEditedElement p_Config)
        {
            m_LightEditView.SetActive(false);
            m_ObjectEditView.SetActive(true);
            m_ObjectEditView.SetObject(p_Config);
        }

        public void SetObject(EMConfig.EMEditedLight p_Light)
        {
            m_LightEditView.SetActive(true);
            m_ObjectEditView.SetActive(false);
            m_LightEditView.SetLight(p_Light);
        }
    }
}
