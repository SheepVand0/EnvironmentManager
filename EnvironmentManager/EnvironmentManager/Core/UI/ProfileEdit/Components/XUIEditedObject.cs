using CP_SDK.UI.Components;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.ProfileEdit.Components
{
    internal class XUIEditedObject : EMBaseButton
    {
        protected XUIEditedObject(string p_Label, int p_Width, int p_Height, string p_Name = "EnvironmentManagerButton", Action p_OnClick = null) : base(p_Label, p_Width, p_Height, p_Name, p_OnClick)
        {
            OnClick(OnClick);
            OnReady(OnCreation);
        }

        public static XUIEditedObject Make()
        {
            return new XUIEditedObject(string.Empty, 40, 5);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        private void OnCreation(CSecondaryButton p_Button)
        {
            SetWidth(40);
            SetHeight(5);
        }

        protected override Color GetBackgroundColor()
        {
            return Color.clear;
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public enum EObjectType
        {
            Object,
            Light
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        protected EObjectType m_ObjectType;
        EMConfig.EMEditedElement m_ObjectData;
        EMConfig.EMEditedLight m_LightData;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public void SetObject(EMConfig.EMEditedElement p_Object)
        {
            m_ObjectType = EObjectType.Object;
            SetText(p_Object.Name);

            m_ObjectData = p_Object;
        }

        public void SetObject(EMConfig.EMEditedLight p_Light)
        {
            m_ObjectType = EObjectType.Light;
            SetText(p_Light.Name);

            m_LightData = p_Light;
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        public EObjectType GetObjectType() => m_ObjectType;

        public EMConfig.EMEditedElement GetObjectData() => m_ObjectData;

        public EMConfig.EMEditedLight GetLightData() => m_LightData;

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

        private void OnClick()
        {
            if (m_ObjectType == EObjectType.Object)
            {
                RightProfileEditViewController.Instance.SetObject(m_ObjectData);
            } else
            {
                RightProfileEditViewController.Instance.SetObject(m_LightData);
            }
        }

    }
}
