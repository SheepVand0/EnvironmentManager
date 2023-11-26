using CP_SDK.UI.Components;
using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMSlider : XUISlider
    {
        protected EMSlider(string p_Name) : base(p_Name)
        {
            OnReady(OnCreation);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        public static new EMSlider Make()
        {
            return new EMSlider("GuildSaberSlider");
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        private void OnCreation(CSlider p_Slider)
        {
            SetColor(Color.black.ColorWithAlpha(0.7f));
            EMText.PatchText(Element.transform.Find("SlidingArea").GetComponentInChildren<TextMeshProUGUI>());
        }
        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        public EMSlider Bind(ref EMSlider p_Slider)
        {
            p_Slider = this;
            return this;
        }

    }
}
