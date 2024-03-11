using BeatSaberMarkupLanguage;
using CP_SDK_BS.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using EnvironmentManager.Core.UI.ProfileEdit;
using EnvironmentManager.Core.UI.ProfileSelection.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace EnvironmentManager.Core.UI.ProfileSelection
{
    internal class ProfileSelectionViewController : ViewController<ProfileSelectionViewController>
    {
        XUIVScrollView ProfileList = null;

        protected List<XUIProfileText> m_UIProfiles = new List<XUIProfileText>();

        protected int m_SelectedProfile = 0;

        protected EMBaseButton NewProfileButton = null;
        protected EMBaseButton DeleteProfileButton = null;
        protected EMBaseButton EditProfileButton = null;
        protected EMBaseButton RenameProfileButton = null;
        protected EMBaseButton ImportProfileButton = null;
        protected EMBaseButton ExportProfileButton = null;

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                XUIVLayout.Make(
                    XUIVScrollView.Make(
                            XUIProfileText.Make().SetProfile("==Default==", -1)
                        )
                        .Bind(ref ProfileList),
                    XUIHLayout.Make(
                        EMPrimaryButton.Make("New", 15, 5, NewProfile).Bind(ref NewProfileButton),
                        EMPrimaryButton.Make("Delete", 15, 5, RemoveProfile).Bind(ref DeleteProfileButton),
                        EMPrimaryButton.Make("Edit", 15, 5, EditProfile).Bind(ref EditProfileButton),
                        EMSecondaryButton.Make("Rename", 15, 5, RenameProfile).Bind(ref RenameProfileButton),
                        EMSecondaryButton.Make("Import", 15, 5, ImportProfile).Bind(ref ImportProfileButton),
                        EMSecondaryButton.Make("Export", 15, 5, ExportProfile).Bind(ref ExportProfileButton)
                    )
                ).OnReady(p_X => p_X.CSizeFitter.horizontalFit = p_X.CSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained).
                  OnReady(p_X => p_X.HOrVLayoutGroup.childForceExpandHeight = true).
                  OnReady(p_X => p_X.HOrVLayoutGroup.childForceExpandWidth = true)
            ).BuildUI(transform);

            XUIProfileText.eOnProfileSelected += OnProfileSelected;

            RefreshProfileList();
        }

        internal void RefreshProfileList()
        {
            foreach (var l_Index in m_UIProfiles)
                l_Index.SetActive(false);

            for (int l_i = 0; l_i < EMConfig.Instance.UserProfiles.Count();l_i++)
            {
                if (l_i > m_UIProfiles.Count - 1)
                {
                    var l_Text = XUIProfileText.Make();
                    l_Text.BuildUI(ProfileList.Element.Container);
                    m_UIProfiles.Add(l_Text);
                }
                m_UIProfiles[l_i].SetProfile(EMConfig.Instance.UserProfiles[l_i].Name, l_i);
                m_UIProfiles[l_i].SetActive(true);
            }
        }

        protected void NewProfile()
        {

            ShowKeyboardModal("New Profile", (p_Value) =>
            {
                if (p_Value == string.Empty)
                {
                    ShowMessageModal("<color=#ff0000>Enter valid name please");
                    return;
                }

                if (EMConfig.Instance.ProfileNameAlreadyExists(p_Value))
                {
                    ShowMessageModal("<color=#ff0000>Profile with this name already exists!");
                    return;
                }
                var l_Profile = new EMConfig.EMProfile(true);
                l_Profile.Name = p_Value;
                

                EMConfig.Instance.UserProfiles.Add(l_Profile);
                EMConfig.Instance.Save();
                RefreshProfileList();
            });
        }

        protected void RemoveProfile()
        {
            if (m_SelectedProfile == -1)
            {
                ShowMessageModal("<color=#ff0000>You cannot remove the default profile!");
                return;
            }


            EMConfig.Instance.UserProfiles.RemoveAt(m_SelectedProfile);
            EMConfig.Instance.Save();
            RefreshProfileList();
        }

        protected void EditProfile()
        {
            if (ProfileEditFlowCoordinator.Instance == null)
                ProfileEditFlowCoordinator.Instance = BeatSaberUI.CreateFlowCoordinator<ProfileEditFlowCoordinator>();

            ProfileEditFlowCoordinator.Instance.Present();
        }

        protected void RenameProfile()
        {
            if (m_SelectedProfile == -1)
            {
                ShowMessageModal("<color=#ff0000>You cannot rename default profile!");
                return;
            }

            ShowKeyboardModal(EMConfig.Instance.UserProfiles[m_SelectedProfile].Name, (p_Val) =>
            {
                if (p_Val == string.Empty)
                {
                    ShowMessageModal("<color=#ff0000>Enter valid name please");
                    return;
                }
                if (EMConfig.Instance.ProfileNameAlreadyExists(p_Val))
                {
                    ShowMessageModal("<color=#ff0000>Profile name already exists!");
                    return;
                }

                EMConfig.Instance.UserProfiles[m_SelectedProfile].Name = p_Val;
                EMConfig.Instance.Save();
                RefreshProfileList();
            });
        }

        protected void ImportProfile()
        {
            
        }

        protected void ExportProfile()
        {

        }

        protected void OnProfileSelected(int p_ProfileIndex)
        {
            DeleteProfileButton.SetInteractable(p_ProfileIndex >= -1);
            RenameProfileButton.SetInteractable(p_ProfileIndex >= -1);
            ExportProfileButton.SetInteractable(p_ProfileIndex >= -1);

            m_SelectedProfile = p_ProfileIndex;
            EMConfig.Instance.SelectedIndex = m_SelectedProfile;
            EMConfig.Instance.Save();
        }
    }
}
