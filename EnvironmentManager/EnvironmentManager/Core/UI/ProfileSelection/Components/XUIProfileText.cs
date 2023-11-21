using CP_SDK.UI.Components;
using CP_SDK.XUI;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileSelection.Components
{
    internal class XUIProfileText : XUISecondaryButton
    {
        protected XUIProfileText(string p_Name, string p_Label, Action p_OnClick = null) : base(p_Name, p_Label, p_OnClick)
        {
            OnReady(OnCreation);
            OnClick(OnClick);
        }

        public static XUIProfileText Make()
        {
            return new XUIProfileText("", string.Empty);
        }

        internal static event Action<int> eOnProfileSelected;

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////

        private void OnCreation(CSecondaryButton p_Button)
        {
            Element.SetBackgroundColor(Color.black.ColorWithAlpha(0));
            eOnProfileSelected += OnSelect;
            SetHeight(5);
        }

        ////////////////////////////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////////////////////////////

        protected int m_Index;

        internal XUIProfileText SetProfile(string p_Name, int p_Index)
        {
            if (p_Name == string.Empty)
            {
                SetActive(false);
                return this;
            }
            m_Index = p_Index;
            SetText(p_Name);
            return this;
        }

        private void OnClick()
        {
            eOnProfileSelected?.Invoke(m_Index);
        }

        private void OnSelect(int p_Index)
        {
            if (p_Index == m_Index)
            {
                Element.GetComponentInChildren<TMPro.TextMeshProUGUI>().color = new Color(0, .12f, .36f, .9f);
            }
            else
            {
                Element.GetComponentInChildren<TextMeshProUGUI>().color = Color.white;
            }
        }
    }
}
