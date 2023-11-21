using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core
{
    internal static class Utils
    {
        public static Transform FindByPath(this Transform p_Transform, string p_Path)
        {
            string[] l_Elems = p_Path.Split('/');
            Transform l_LastGameObject = null;
            for (int l_i = 0; l_i < l_Elems.Length; l_i++)
            {
                if (l_LastGameObject == null)
                {
                    l_LastGameObject = p_Transform.Find(l_Elems[l_i]);
                    Plugin.Log.Info(l_Elems[l_i]);
                    Plugin.Log.Info((l_LastGameObject == null).ToString());
                    continue;
                }

                Transform l_CurrentGameObject = l_LastGameObject.transform.Find(l_Elems[l_i]);
                l_LastGameObject = l_CurrentGameObject;
            }
            return l_LastGameObject;
        }

        public static GameObject FindByPath(string p_Path)
        {
            string[] l_Elems = p_Path.Split('/');
            GameObject l_LastGameObject = null;
            for (int l_i = 0; l_i < l_Elems.Length; l_i++)
            {
                if (l_LastGameObject == null)
                {
                    l_LastGameObject = GameObject.Find(l_Elems[l_i]);
                    continue;
                }

                GameObject l_CurrentGameObject = l_LastGameObject.transform.Find(l_Elems[l_i]).gameObject;
                l_LastGameObject = l_CurrentGameObject;
            }
            return l_LastGameObject;
        }
    }
}
