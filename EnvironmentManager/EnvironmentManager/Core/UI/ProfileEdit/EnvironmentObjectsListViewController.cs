using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CP_SDK_BS.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileEdit.Widgets;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class EnvironmentObjectsListViewController : ViewController<EnvironmentObjectsListViewController>
    {
        List<XUIListSelectable<string>> m_UIObjects = new List<XUIListSelectable<string>>();

        XUIVScrollView m_ObjectsScrollView;

        List<string> m_ObjectsNames = new List<string>();

        string m_SelectedObject;

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                XUIVLayout.Make(
                    XUIVScrollView.Make(

                    ).Bind(ref m_ObjectsScrollView)
                ).OnReady(x => x.CSizeFitter.verticalFit = x.CSizeFitter.horizontalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                .OnReady(x => x.HOrVLayoutGroup.childForceExpandHeight = x.HOrVLayoutGroup.childForceExpandWidth = true)
                .SetMinHeight(50),
                EMPrimaryButton.Make("Add", 20, 5).OnClick(() =>
                {
                    var l_EditedElement = new EMConfig.EMEditedElement();
                    l_EditedElement.Name = m_SelectedObject;
                    GameObject l_Object = GameObject.Find(l_EditedElement.Name);
                    l_EditedElement.CustomPosition = l_Object.transform.localPosition;
                    l_EditedElement.CustomRotationEulers = l_Object.transform.localRotation.eulerAngles;
                    l_EditedElement.CustomScale = l_Object.transform.localScale;
                    EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements.Add(l_EditedElement);
                    EMConfig.Instance.Save();
                    ProfileEditViewController.Instance.SetToEditedElements();
                    EnvironmentManipulator.ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);
                }),
                XUIHLayout.Make(
                    EMText.Make("Rings count: "),
                    EMSlider.Make().OnValueChanged(x =>
                    {
                        EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].RingsCount = (int)x;
                        EMConfig.Instance.Save();
                        EnvironmentManipulator.ApplyProfile(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex]);
                    })
                    .SetMinValue(0).SetMaxValue(16)
                    .SetIncrements(1)
                    .SetInteger(true)
                    .SetValue(EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].RingsCount, false)
                )
            )
            .SetSpacing(0)
            .BuildUI(transform);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public async void UpdateObjects()
        {
            await WaitUtils.Wait(() => EnvironmentManipulator.EnvironmentLoaded == true, 1);

            m_ObjectsNames.Clear();
            foreach (Transform l_Index in GameObject.Find("Environment").transform)
            {
                m_ObjectsNames.Add(l_Index.name);
            }

            Helper.DisplayListOnUI(m_ObjectsNames, ref m_UIObjects,
                m_ObjectsScrollView.Element.Container, () => {
                    XUIListSelectable<string> l_New;
                    (l_New = XUIListSelectable<string>.Make("EnvironmentObjectList")).SetWidth(80);
                    l_New.OnSelected((xx) => { m_SelectedObject = xx; });
                    return l_New;
                }, 
                (x, y) => {
                    y.SetData(x, x);
                });
        }
    }
}
