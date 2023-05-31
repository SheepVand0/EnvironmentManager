using CP_SDK.UI.Components;
using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileSelection.Components
{
    internal class ProfileText : XUISecondaryButton
    {
        protected ProfileText(string p_Name, string p_Label, Action p_OnClick = null) : base(p_Name, p_Label, p_OnClick)
        {
            OnClick(OnSelect);
            OnReady(SetupStyle);
        }

        internal int ProfileIndex = 0;
        internal event Action<int> OnProfileSelected;

        internal static ProfileText Make(string p_Label, int p_ProfileIndex)
        {
            var l_Object = new ProfileText("ProfileText", p_Label);
            l_Object.ProfileIndex = p_ProfileIndex;
            return l_Object;
        }

        internal void SetProfile(string p_Name, int p_Index)
        {
            SetText(p_Name);
            ProfileIndex = p_Index;
        }

        private void SetupStyle(CSecondaryButton p_Button)
        {
            p_Button.SetBackgroundColor(Color.white.ColorWithAlpha(0));
        }

        private void OnSelect()
        {
            OnProfileSelected?.Invoke(ProfileIndex);
        }
    }
}
