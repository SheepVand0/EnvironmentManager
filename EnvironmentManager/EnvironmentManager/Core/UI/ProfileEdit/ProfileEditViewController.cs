using CP_SDK_BS.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileEdit.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class ProfileEditViewController : ViewController<ProfileEditViewController>
    {
        protected XUIVScrollView m_UIEditedElementsScroll;
        protected EMText m_Title;
        protected EMBaseButton m_AddLightButton;

        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        public enum EEditMode
        {
            Objects,
            Lights
        }

        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        protected EEditMode m_EditMode;

        protected List<XUIEditedObject> m_EditedObjectsList = new List<XUIEditedObject>();
        protected List<XUIEditedObject> m_EditedObjectsLights = new List<XUIEditedObject>();

        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        protected override async void OnViewActivation()
        {
            await WaitUtils.Wait(() => m_UIEditedElementsScroll != null, 1);

            SetToEditedElements();
        }

        protected override void OnViewDeactivation()
        {
        }

        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                EMText.Make("Title")
                    .Bind(ref m_Title)
                    .SetFontSize(10),
                EMSecondaryButton.Make("Preview", 20, 5).OnClick(async () => await EnvironmentManipulator.ShowAllOnEnvironment(true)),
                XUIVLayout.Make(
                    XUIVScrollView.Make(

                        ).Bind(ref m_UIEditedElementsScroll)
                    )
                    .SetHeight(120)
                    .OnReady(x => x.CSizeFitter.horizontalFit = x.CSizeFitter.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                    .OnReady(x => x.HOrVLayoutGroup.childForceExpandHeight = x.HOrVLayoutGroup.childForceExpandWidth = true),
                XUIHLayout.Make(
                    EMPrimaryButton.Make("Switch", 20, 5)
                    .OnClick(() =>
                    {
                        if (m_EditMode == EEditMode.Objects)
                        {
                            SetToEditedLightsList();
                        }
                        else
                        {
                            SetToEditedElements();
                        }
                    }),
                    EMPrimaryButton.Make("Add", 20, 5)
                        .Bind(ref m_AddLightButton)
                        .OnClick(AddLight)
                ),
                EMSecondaryButton.Make("Exit", 40, 5).OnClick(() =>
                {
                    EnvironmentManipulator.UnloadEnvironment(async () =>
                    {
                        await Task.Delay(500);

                        ProfileEditFlowCoordinator.Instance.Dismiss();
                    });
                })
                ).BuildUI(transform);
        }

        public void SetToEditedElements()
        {
            m_AddLightButton.SetActive(false);
            m_EditMode = EEditMode.Objects;
            HideUIEditedLights();
            HideUIEditedObjects();

            Helper.DisplayListOnUI(
                EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements, ref m_EditedObjectsList, 
                m_UIEditedElementsScroll.Element.Container, () =>
            {
                XUIEditedObject l_Object = XUIEditedObject.Make();
                return l_Object;
            },
            (x, y) => { y.SetObject(x); y.SetActive(true); });

            m_Title.SetText("Edit Objects");
        }

        public void SetToEditedLightsList()
        {
            m_AddLightButton.SetActive(true);
            m_EditMode = EEditMode.Lights;
            HideUIEditedObjects();
            HideUIEditedLights();

            var l_Lights = EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights;

            Helper.DisplayListOnUI(
                l_Lights, ref m_EditedObjectsLights,
                m_UIEditedElementsScroll.Element.Container, () =>
                {
                    XUIEditedObject l_Object = XUIEditedObject.Make();
                    return l_Object;
                }, 
                (x, y) => { y.SetObject(x); y.SetText($"Light{l_Lights.IndexOf(x)}"); y.SetActive(true); });

            m_Title.SetText("Edit Lights");
        }

        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        private void AddLight()
        {
            EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights.Add(new EMConfig.EMEditedLight());
            EMConfig.Instance.Save();
            SetToEditedLightsList();
        }

        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////

        private void HideUIEditedLights()
        {
            m_EditedObjectsLights.ForEach(x => x.SetActive(false));
        }

        private void HideUIEditedObjects()
        {
            m_EditedObjectsList.ForEach(x => x.SetActive(false));
        }

    }
}
