using CP_SDK.UI.Components;
using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMToggleSetting : XUIToggle
    {
        protected EMToggleSetting(string p_Name) : base(p_Name)
        {
            OnReady(SetupStyle);
            OnValueChanged(OnToggleChanged);
        }

        internal static new EMToggleSetting Make()
        {
            return new EMToggleSetting("EnvironmentManagerToggleSetting");
        }

        protected void SetupStyle(CToggle p_Toggle)
        {
            for (int l_i = 0; l_i < p_Toggle.transform.childCount; l_i++)
            {
                Plugin.Log.Info(p_Toggle.transform.GetChild(l_i).name);
            }
            Transform l_BackgroundTransform = p_Toggle.transform.FindByPath("View/BG");
            //Plugin.Log.Info((l_BackgroundTransform == null).ToString());

            Image l_Background = l_BackgroundTransform.GetComponent<Image>();

            Sprite l_RoundedBackground = EMUIDefaults.GetRoundedBackground(15, 5);
            l_Background.sprite = l_RoundedBackground;
            l_Background.color = EMUIDefaults.GetSecondaryElemsColor();
        }

        internal void SetKnobColor(Color p_Color)
        {
            Transform l_KnobTransform = Element.transform.FindByPath("View/BG/Knob/Image");
            Image l_KnobImage = l_KnobTransform.GetComponent<Image>();
            Sprite l_RoundedBackground = EMUIDefaults.GetRoundedBackground(7, 5);
            l_KnobImage.sprite = l_RoundedBackground;
            l_KnobImage.color = p_Color;
        }

        protected async void OnToggleChanged(bool p_Value)
        {
            int l_Index = 0;
            await WaitUtils.Wait(() =>
            {
                if (p_Value)
                {
                    SetKnobColor(EMUIDefaults.GetPrimaryElemsColor());
                }
                else
                {
                    SetKnobColor(EMUIDefaults.GetSecondaryElemsColor());
                }
                l_Index += 1;
                return l_Index < 500;
            }, 1);
        }
    }
}
