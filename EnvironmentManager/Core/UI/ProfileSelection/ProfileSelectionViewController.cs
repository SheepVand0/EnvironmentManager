using CP_SDK.UI;
using CP_SDK.XUI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
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

        internal List<ProfileText> Profiles = new List<ProfileText>();

        protected int m_SelectedProfile = 0;

        protected EMPrimaryButton NewProfileButton = null;
        protected EMPrimaryButton DeleteProfileButton = null;
        protected EMPrimaryButton EditProfileButton = null;
        protected EMSecondaryButton RenameProfileButton = null;
        protected EMSecondaryButton ImportProfileButton = null;
        protected EMSecondaryButton ExportProfileButton = null;

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                XUIVLayout.Make(
                    XUIVScrollView.Make(
                            ProfileText.Make("==Default==", -1)
                        )
                        .Bind(ref ProfileList),
                    XUIHLayout.Make(
                        EMPrimaryButton.Make("New", 10, 5, NewProfile),
                        EMPrimaryButton.Make("Delete", 10, 5, RemoveProfile),
                        EMPrimaryButton.Make("Edit", 10, 5, EditProfile),
                        EMSecondaryButton.Make("Rename", 10, 5, RenameProfile),
                        EMSecondaryButton.Make("Import", 10, 5, ImportProfile),
                        EMSecondaryButton.Make("Export", 10, 5, ExportProfile)
                    )
                ).OnReady(p_X => p_X.CSizeFitter.horizontalFit = p_X.CSizeFitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained).
                  OnReady(p_X => p_X.HOrVLayoutGroup.childForceExpandHeight = true).
                  OnReady(p_X => p_X.HOrVLayoutGroup.childForceExpandWidth = true)
            ).BuildUI(transform);
        }

        internal void RefreshProfileList()
        {

            for (int l_i = 0; l_i < EMConfig.Instance.UserProfiles.Count();l_i++)
            {
                if (l_i > Profiles.Count() - 1)
                {
                    var l_Text = ProfileText.Make(EMConfig.Instance.UserProfiles[l_i].Name, l_i);
                    l_Text.BuildUI(ProfileList.Element.transform);
                    Profiles.Add(l_Text);
                } else
                {
                    Profiles[l_i + 1].SetProfile(EMConfig.Instance.UserProfiles[l_i].Name, l_i);
                }
            }
        }

        protected void NewProfile()
        {
            EMConfig.Instance.UserProfiles.Add(new EMConfig.EMProfile(true));
            RefreshProfileList();
        }

        protected void RemoveProfile()
        {
            EMConfig.Instance.UserProfiles.RemoveAt(m_SelectedProfile);
            RefreshProfileList();
        }

        protected void EditProfile()
        {
            
        }

        protected void RenameProfile()
        {

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

            
        }
    }
}
