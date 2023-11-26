using CP_SDK.UI.Components;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit.Widgets
{
    internal class XUIListSelectable<t_DataType> : EMBaseButton
    {
        protected XUIListSelectable(string p_Label, int p_Width, int p_Height, string p_Name = "EnvironmentManagerButton", Action p_OnClick = null) : base(p_Label, p_Width, p_Height, p_Name, p_OnClick)
        {
        }

        protected XUIListSelectable(string p_Channel) : this(string.Empty, 40, 5)
        {
            m_ChannelName = p_Channel;
            OnSelectableSelectedEvent += OnSelected;
            OnClick(OnClick);
            OnReady(OnCreation);
        }

        public static XUIListSelectable<t_DataType> Make(string p_ChannelName)
        {
            return new XUIListSelectable<t_DataType>(p_ChannelName);
        }

        protected override Color GetBackgroundColor()
        {
            return Color.clear;
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        protected static event Action<string, t_DataType> OnSelectableSelectedEvent;

        protected Action<t_DataType> m_OnSelectedEvent;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        protected string m_ChannelName;
        protected t_DataType m_Data;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        private void OnCreation(CSecondaryButton p_Button)
        {
            SetWidth(40);
            SetHeight(5);
        }

        private void OnSelected(string p_Channel, t_DataType p_Data)
        {
            if (m_ChannelName != p_Channel) return;

            Element.SetColor(EMUIDefaults.GetSecondaryElemsColor());
        }

        private void OnClick()
        {
            OnSelectableSelectedEvent?.Invoke(m_ChannelName, m_Data);

            Element.SetColor(EMUIDefaults.GetPrimaryElemsColor());
            if (m_OnSelectedEvent == null) return;
            m_OnSelectedEvent.Invoke(m_Data);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public XUIListSelectable<t_DataType> OnSelected(Action<t_DataType> p_Action)
        {
            m_OnSelectedEvent = p_Action;
            return this;
        }

        public XUIListSelectable<t_DataType> SetData(t_DataType p_Data, string p_Text)
        {
            m_Data = p_Data;
            SetText(p_Text);
            return this;
        }
    }
}
