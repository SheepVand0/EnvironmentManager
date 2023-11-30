using CP_SDK.XUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI
{
    internal class Helper
    {


        public static void DisplayListOnUI<t_ListType, t_ObjectType>(List<t_ListType> p_BaseList, ref List<t_ObjectType> p_UIObjectList,
            Transform p_Location, Func<t_ObjectType> p_MakeElement, Action<t_ListType, t_ObjectType> p_Callback) where t_ObjectType : IXUIElement
        {
            if (p_MakeElement == null) return;

            List<t_ObjectType> l_UIList = p_UIObjectList;

            for (int l_i = 0; l_i < p_BaseList.Count();l_i++)
            {
                
                if (l_i > l_UIList.Count() - 1)
                {
                    t_ObjectType l_NewObject = p_MakeElement.Invoke();
                    l_UIList.Add(l_NewObject);
                    l_UIList[l_i].BuildUI(p_Location);
                }
                p_Callback.Invoke(p_BaseList[l_i], l_UIList[l_i]);
            }

            p_UIObjectList = l_UIList;
        }

    }
}
