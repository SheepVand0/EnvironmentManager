using CP_SDK.UI.Components;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit.Widgets
{
    internal class ObjectEditView : XUIVLayout
    {
        protected ObjectEditView(string p_Name, params IXUIElement[] p_Childs) : base(p_Name, p_Childs)
        {
            OnReady(OnCreation);
        }

        public static ObjectEditView Make()
        {
            return new ObjectEditView("EMEdit");
        }

        //////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        protected string m_SelectedObjectName;

        //////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        protected EMTextInput m_NameInput;
        protected EMToggleSetting m_HideToggle;
        protected EMToggleSetting m_ForceShowToggle;
        protected EMToggleSetting m_MoveToggle;
        protected EMVector3Parameter m_PosParam;
        protected EMVector3Parameter m_RotParam;
        protected EMVector3Parameter m_ScaleParam;

        protected EMConfig.EMEditedElement m_Object;
        protected EMConfig.EMEditedLight m_Light;

        //////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////

        private void OnCreation(CHOrVLayout p_Layout)
        {
            XUIVLayout.Make(
                XUIVLayout.Make(
                    XUIVScrollView.Make(
                        XUIHLayout.Make(
                            EMText.Make("Name: "),
                            EMTextInput.Make("Enter a name...")
                                .Bind(ref m_NameInput)
                                .OnValueChanged(OnTextInputChanged)
                            ),

                        XUIHLayout.Make(
                            EMText.Make("Hide: "),
                            EMToggleSetting.Make()
                                .Bind(ref m_HideToggle)
                                .OnValueChanged(OnToggleChanged)
                        ),

                        XUIHLayout.Make(
                            EMText.Make("Force show: "),
                            EMToggleSetting.Make()
                                .Bind(ref m_ForceShowToggle)
                                .OnValueChanged(OnToggleChanged)
                        ),

                        XUIHLayout.Make(
                            EMText.Make("Move: "),
                            EMToggleSetting.Make()
                                .Bind(ref m_MoveToggle)
                                .OnValueChanged(OnToggleChanged)
                        ),

                        XUIHLayout.Make(
                            EMText.Make("Position: "),
                            EMVector3Parameter.Make()
                                .Bind(ref m_PosParam)
                                .OnValueChanged(OnSliderChanged)
                        ),

                        XUIHLayout.Make(
                            EMText.Make("Rotation: "),
                            EMVector3Parameter.Make()
                                .Bind(ref m_RotParam)
                                .OnValueChanged(OnSliderChanged)
                        ),

                        XUIHLayout.Make(
                            EMText.Make("Scale: "),
                            EMVector3Parameter.Make()
                                .Bind(ref m_ScaleParam)
                                .OnValueChanged(OnSliderChanged)
                        )
                    )
                ).OnReady(x => x.CSizeFitter.horizontalFit = x.CSizeFitter.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                 .OnReady(x => x.HOrVLayoutGroup.childForceExpandHeight = x.HOrVLayoutGroup.childForceExpandWidth = true)
            )
            .SetWidth(80)
            .SetHeight(80)
            .BuildUI(Element.LElement.transform);
        }

        private void OnTextInputChanged(string p_Value)
        {
            var l_Index = EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements.IndexOf(m_Object);
            EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements[l_Index].Name = p_Value;
            m_Object.Name = p_Value;
            EMConfig.Instance.Save();
        }
        private int GetIndexOfCurrentElement()
        {
            return EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements.IndexOf(m_Object);
        }

        private void OnToggleChanged(bool p_Ignored)
        {
            int l_Index = GetIndexOfCurrentElement();
            m_Object.Hide = m_HideToggle.Element.GetValue();
            m_Object.ForceShow = m_ForceShowToggle.Element.GetValue();
            m_Object.Move = m_MoveToggle.Element.GetValue();
            EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements[l_Index] = m_Object;
            EMConfig.Instance.Save();

            EnvironmentManipulator.ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);
        }

        private void OnSliderChanged(Vector3 p_Ignored)
        {
            int l_Index = GetIndexOfCurrentElement();

            m_Object.CustomPosition = m_PosParam.GetValue();
            m_Object.CustomRotationEulers = m_RotParam.GetValue();
            m_Object.CustomScale = m_ScaleParam.GetValue();
            EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements[l_Index] = m_Object;
            EMConfig.Instance.Save();

            EnvironmentManipulator.ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);
        }

        public void SetObject(EMConfig.EMEditedElement p_Object)
        {
            m_Object = p_Object;
            m_NameInput.SetValue(p_Object.Name, false);
            m_HideToggle.SetValue(p_Object.Hide, false);
            m_ForceShowToggle.SetValue(p_Object.ForceShow, false);
            m_MoveToggle.SetValue(p_Object.Move, false);
            m_PosParam.SetValue(p_Object.CustomPosition);
            m_RotParam.SetValue(p_Object.CustomRotationEulers);
            m_ScaleParam.SetValue(p_Object.CustomScale);
        }
    }
}
