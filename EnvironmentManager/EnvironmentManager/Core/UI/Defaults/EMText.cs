using CP_SDK.UI.Components;
using CP_SDK.XUI;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

namespace EnvironmentManager.Core.UI.Defaults
{
    public class EMText : XUIText
    {
        public const string FONT_FILE_NAME = "EnvironmentManagerFont.ttf";

        protected EMText(string p_Name, string p_Text) : base(p_Name, p_Text)
        {
            OnReady(PatchText);
        }

        public static new EMText Make(string p_Text)
        {
            return new EMText("GuildSaberText", p_Text);
        }

        public EMText Bind(ref EMText p_Value)
        {
            p_Value = this;
            return this;
        }

        public static TMP_FontAsset TextFont = null;

        public static void PatchText(CText p_Text)
        {
            PatchText(p_Text.GetComponentInChildren<TextMeshProUGUI>());
        }

        public static void PatchText(TextMeshProUGUI p_Text)
        {
            if (TextFont == null)
            {
                TextFont = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>().Where(x => x.font.name.Contains("Curved")).ElementAt(0).font;
            }

            p_Text.font = TextFont;
        }

        public static void DeleteFontFolder()
        {
            if (Directory.Exists("./Font"))
            {
                try
                {
                    File.Delete($"./Font/{FONT_FILE_NAME}");
                } catch { /*Ignored*/ }
                Directory.Delete("./Font");
            }
        }

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        public new EMText SetMargins(float p_Left, float p_Top, float p_Right, float p_Bottom)
        {
            base.SetMargins(p_Left, p_Top, p_Right, p_Bottom);
            return this;
        }
    }
}
