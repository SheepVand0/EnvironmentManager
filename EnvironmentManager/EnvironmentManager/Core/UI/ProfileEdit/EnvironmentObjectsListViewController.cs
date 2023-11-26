using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeatSaberPlus.SDK.UI;
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
                    EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements.Add(l_EditedElement);
                    EMConfig.Instance.Save();
                    ProfileEditViewController.Instance.SetToEditedElements();
                })
            )
            .SetSpacing(0)
            .BuildUI(transform);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        protected override void OnViewActivation()
        {
            UpdateObjects();
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
                    var l_New = XUIListSelectable<string>.Make("EnvironmentObjectList");
                    l_New.OnSelected((xx) => m_SelectedObject = xx);
                    return l_New;
                }, 
                (x, y) => {
                    y.SetData(x, x);
                });

        }
    }
}
