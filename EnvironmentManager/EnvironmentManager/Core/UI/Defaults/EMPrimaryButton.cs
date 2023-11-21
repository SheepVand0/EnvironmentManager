using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMPrimaryButton : EMBaseButton
    {
        protected EMPrimaryButton(string p_Label, int p_Width, int p_Height, string p_Name = "EnvironmentManagerButton", Action p_OnClick = null) : base(p_Label, p_Width, p_Height, p_Name, p_OnClick)
        {
        }

        public static EMPrimaryButton Make(string p_Label, int p_Width, int p_Heigth, Action p_OnClick = null)
        {
            return new EMPrimaryButton(p_Label, p_Width, p_Heigth, p_OnClick: p_OnClick);
        }

        protected override Color GetBackgroundColor()
        {
            return EMUIDefaults.GetPrimaryElemsColor();
        }

    }
}
