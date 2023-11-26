using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;
using CP_SDK.UI.Components;
using System.Security.Cryptography;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMVector3Parameter : XUIVLayout
    {
        protected EMVector3Parameter(string p_Name, params IXUIElement[] p_Childs) : base(p_Name, p_Childs)
        {
            OnReady(OnCreation);
        }


        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        protected Vector3 m_Value;

        protected List<Action<Vector3>> m_Events = new List<Action<Vector3>>();

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        protected EMSlider m_PosXSlider;
        protected EMSlider m_PosYSlider;
        protected EMSlider m_PosZSlider;

        protected EMSlider[] m_Sliders;

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        private void OnCreation(CHOrVLayout p_Layout)
        {

            XUIHLayout.Make(
                EMText.Make("X: "),
                EMSlider.Make()
                .Bind(ref m_PosXSlider)
                .OnValueChanged(OnSliderValueChanged)
                )
                .BuildUI(Element.LElement.transform);
            XUIHLayout.Make(
                EMText.Make("Y: "),
            EMSlider.Make()
                .Bind(ref m_PosYSlider)
                .OnValueChanged(OnSliderValueChanged)
                )
                .BuildUI(Element.LElement.transform);
            XUIHLayout.Make(
                EMText.Make("Z: "),
                EMSlider.Make()
                    .Bind(ref m_PosZSlider)
                    .OnValueChanged(OnSliderValueChanged)
            )
            .BuildUI(Element.LElement.transform);

            m_Sliders = new EMSlider[] { m_PosXSlider, m_PosYSlider, m_PosZSlider };

            ForEachSlider(x => {
                x.SetMinValue(-10).SetMaxValue(10);
                x.OnValueChanged(OnSliderValueChanged);
            });

            SetSpacing(0);
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        public Vector3 GetValue() => m_Value;

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        public EMVector3Parameter ForEachSlider(Action<EMSlider> p_Slider)
        {
            OnReady(x =>
            {
                foreach (var l_Slider in m_Sliders)
                {
                    p_Slider(l_Slider);
                }
            });

            return this;
        }

        private void OnSliderValueChanged(float p_Ignored)
        {
            m_Value.x = m_PosXSlider.Element.GetValue();
            m_Value.y = m_PosYSlider.Element.GetValue();
            m_Value.z = m_PosZSlider.Element.GetValue();
            foreach (var l_Item in m_Events)
            {
                l_Item.Invoke(m_Value);
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////

        public static EMVector3Parameter Make()
        {
            return new EMVector3Parameter("EMVector3");
        }
        
        public EMVector3Parameter SetValue(Vector3 p_Value, bool p_FireEvents = false)
        {
            m_Value = p_Value;
            m_PosXSlider.SetValue(p_Value.x);
            m_PosYSlider.SetValue(p_Value.y);
            m_PosZSlider.SetValue(p_Value.z);
            return this;
        }

        public EMVector3Parameter OnValueChanged(Action<Vector3> p_Value) {
            m_Events.Add(p_Value);
            return this;
        }

        public EMVector3Parameter Bind(ref EMVector3Parameter p_Object)
        {
            p_Object = this;
            return this;
        }

    }
}
