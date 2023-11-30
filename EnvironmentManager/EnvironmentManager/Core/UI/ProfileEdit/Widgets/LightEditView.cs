using CP_SDK.UI.Components;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit.Widgets
{
    internal class LightEditView : XUIVLayout
    {
        protected LightEditView(string p_Name, params IXUIElement[] p_Childs) : base(p_Name, p_Childs)
        {
            OnReady(OnCration);
        }

        public static LightEditView Make()
        {
            return new LightEditView("LightEditView");
        }

        ///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        EMConfig.EMEditedLight m_Light;

        ///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        protected EMSlider m_Index;
        protected XUIColorInput m_LeftColor;
        protected XUIColorInput m_RightColor;
        protected int m_ElementIndex;

        ///////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////

        private void OnCration(CVLayout p_Layout)
        {
            XUIVLayout.Make(
                XUIHLayout.Make(
                    EMText.Make("Light Index: "),
                    EMSlider.Make()
                        .Bind(ref m_Index)
                        .SetMinValue(0)
                        .SetMaxValue(10)
                        .SetInteger(true)
                        .SetIncrements(1)
                        .OnValueChanged(OnSliderChanged)
                ),
                XUIHLayout.Make(
                    XUIVLayout.Make(
                        XUIColorInput.Make().Bind(ref m_LeftColor).OnValueChanged(OnColorChanged).SetAlphaSupport(true)
                    ),
                    XUIVLayout.Make(
                        XUIColorInput.Make().Bind(ref m_RightColor).OnValueChanged(OnColorChanged).SetAlphaSupport(true)
                    )
                )
            ).BuildUI(Element.LElement.transform);
        }

        public void SetLight(EMConfig.EMEditedLight p_Light)
        {
            m_Light = p_Light;
            m_ElementIndex = GetIndexOfCurrentElement();
            m_LeftColor.SetValue(m_Light.LeftColor);
            m_RightColor.SetValue(m_Light.RightColor);
            m_Index.SetValue(m_Light.LightIndex);
        }

        private int GetIndexOfCurrentElement()
        {
            return EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights.IndexOf(m_Light);
        }

        private void OnSliderChanged(float p_Value)
        {
            m_Light.LightIndex = (int)p_Value;
            UpdateOnConfig();
        }

        private void OnColorChanged(Color p_Ignored)
        {
            m_Light.LeftColor = m_LeftColor.Element.GetValue();
            m_Light.RightColor = m_RightColor.Element.GetValue();
            UpdateOnConfig();
        }

        private void UpdateOnConfig()
        {
            EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights[m_ElementIndex] = m_Light;
            EnvironmentManipulator.ApplyLights(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights);
            EMConfig.Instance.Save();
        }
    }
}
