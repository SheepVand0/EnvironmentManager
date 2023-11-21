using CP_SDK.UI.Components;
using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMBaseButton : XUISecondaryButton
    {
        protected int m_Width;
        protected int m_Height;

        protected virtual Color GetBackgroundColor() { return Color.white; }

        protected EMBaseButton(string p_Label, int p_Width, int p_Height ,string p_Name = "EnvironmentManagerButton", Action p_OnClick = null) : base(p_Name, p_Label, p_OnClick)
        {
            m_Width = p_Width;
            m_Height = p_Height;
            OnReady(SetupStyle);
        }

        private void SetupStyle(CSecondaryButton p_Button)
        {
            SetWidth(m_Width);
            SetHeight(m_Height);

            p_Button.SetBackgroundColor(GetBackgroundColor().ColorWithAlpha(0.9f));
            p_Button.SetBackgroundSprite(EMUIDefaults.GetRoundedBackground(m_Width, m_Height));
        }

        public EMBaseButton Bind(ref EMBaseButton p_Object)
        {
            p_Object = this;
            return this;
        }
    }
}
