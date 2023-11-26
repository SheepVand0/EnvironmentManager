using BeatSaberPlus.SDK.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileEdit.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ProfileEdit
{
    internal class ProfileEditViewController : ViewController<ProfileEditViewController>
    {
        protected XUIVScrollView m_UIEditedElementsScroll;
        protected EMText m_Title;

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

            _ = EnvironmentManipulator.LoadEnvironment();
        }

        protected override void OnViewDeactivation()
        {
            EnvironmentManipulator.UnloadEnvironment();
        }

        //////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                EMText.Make("Title")
                    .Bind(ref m_Title)
                    .SetFontSize(5),
                XUIVLayout.Make(
                    XUIVScrollView.Make(

                        ).Bind(ref m_UIEditedElementsScroll)
                    )
                    .SetHeight(80)
                    .OnReady(x => x.CSizeFitter.horizontalFit = x.CSizeFitter.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                    .OnReady(x => x.HOrVLayoutGroup.childForceExpandHeight = x.HOrVLayoutGroup.childForceExpandWidth = true)
                ).BuildUI(transform);
        }

        public void SetToEditedElements()
        {
            m_EditMode = EEditMode.Objects;
            HideUIEditedLights();
            HideOldEditedElements();

            Helper.DisplayListOnUI(
                EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedElements, ref m_EditedObjectsList, 
                m_UIEditedElementsScroll.Element.Container, () =>
            {
                XUIEditedObject l_Object = XUIEditedObject.Make();
                return l_Object;
            },
            (x, y) => y.SetObject(x));

            m_Title.SetText("Edit Objects");
        }

        public void SetToEditedLightsList()
        {
            m_EditMode = EEditMode.Lights;
            HideUIEditedObjects();
            HideOldEditedElements();

            Helper.DisplayListOnUI(
                EMConfig.Instance.UserProfiles[EMConfig.Instance.SelectedIndex].EditedLights, ref m_EditedObjectsList,
                m_UIEditedElementsScroll.Element.Container, () =>
                {
                    XUIEditedObject l_Object = XUIEditedObject.Make();
                    return l_Object;
                }, 
                (x, y) => { y.SetObject(x); y.SetText(x.Name); });

            m_Title.SetText("Edit Lights");
        }

        ////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////
        
        private void HideOldEditedElements()
        {
            if (m_EditMode == EEditMode.Lights)
            {
                HideUIEditedObjects();
            }
            else
            {
                HideUIEditedLights();
            }
        }

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
