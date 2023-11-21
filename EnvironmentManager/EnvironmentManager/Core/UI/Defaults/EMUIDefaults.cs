using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace EnvironmentManager.Core.UI.Defaults
{
    internal class EMUIDefaults
    {
        internal static float CornerRadiusMultiplier = 0.02f;

        public static Sprite GetRoundedBackground(int p_Width, int p_Heigth)
        {
            Texture2D l_BackgroundTexture = new Texture2D(p_Width * 15, p_Heigth * 15);
            for (int l_X = 0; l_X < l_BackgroundTexture.width; l_X++)
            {
                for (int l_Y = 0; l_Y < l_BackgroundTexture.height; l_Y++)
                {
                    l_BackgroundTexture.SetPixel(l_X, l_Y, Color.white.ColorWithAlpha(1));
                }
            }
            l_BackgroundTexture = TextureUtils.CreateRoundedTexture(
                TextureUtils.Gradient(l_BackgroundTexture, Color.white.ColorWithAlpha(0.7f), Color.white.ColorWithAlpha(1), p_UseAlpha: true),
                l_BackgroundTexture.width * CornerRadiusMultiplier);
            Sprite l_NewSprite
                = Sprite.Create(l_BackgroundTexture, new Rect(0, 0, l_BackgroundTexture.width, l_BackgroundTexture.height), new Vector2(0, 0), 1000, 0, SpriteMeshType.FullRect);
            return l_NewSprite;
        }

        public static Color GetSecondaryElemsColor()
        {
            return new Color(0, 0, 0, 0.8f);
        }

        public static Color GetPrimaryElemsColor()
        {
            return new Color(0, .12f, .36f, .9f);
        }

    }
}
