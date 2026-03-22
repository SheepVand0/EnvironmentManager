using CP_SDK.XUI;
using CP_SDK_BS.UI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager.Core.UI.ScoreVisualizer
{
    internal class ScoreConfigElement : EMSecondaryButton
    {
        protected ScoreConfigElement(string p_Label, int p_Width, int p_Height, string p_Name = "EnvironmentManagerButton", Action p_OnClick = null) : base(p_Label, p_Width, p_Height, p_Name, p_OnClick)
        {
            OnClick(() =>
            {
                ScoreVisualizerConfigView.Instance.SelectElement(ScoreElement);
            });
        }

        public static ScoreConfigElement Make(string fileName)
        {
            return new ScoreConfigElement(fileName, 25, 7);
        }

        //////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        int ScoreElement;

        public void SetElement(int elem)
        {
            ScoreElement = elem;
            //SetText($"Element {elem}");
        }

        public void UpdateText(EMConfig.EMScoreVisualizerElement elem)
        {
            SetText($"{elem.MinRange} - {elem.MaxRange}");
        }
    }

    internal class ScoreVisualizerConfigView : ViewController<ScoreVisualizerConfigView>
    {
        internal static new ScoreVisualizerConfigView Instance = null;

        List<EMConfig.EMScoreVisualizerElement> ConfigElem;
        int SelectedIndex = 0;

        protected EMSlider MinRangeSlider;
        protected EMSlider MaxRangeSlider;
        protected EMSlider FontSizeSlider;
        protected EMTextInput ScoreTextInput;
        protected XUIColorInput ScoreColorInput;
        protected EMToggleSetting ItalicToggle;
        protected EMToggleSetting BoldToggle;

        protected XUIVLayout Container;

        protected XUIVScrollView ElementsContainer;

        List<ScoreConfigElement> VisualElements = new List<ScoreConfigElement>();

        protected override void OnViewCreation()
        {
            Instance = this;

            SelectedIndex = 0;
            ConfigElem = EMConfig.EMScoreVisualizerProfile.Load(EMConfig.Instance.ScoreVisualizerProfile.ConfigPath);

            Templates.FullRectLayout(
                XUIHLayout.Make(
                    XUIVLayout.Make(
                        XUIHLayout.Make(
                            XUIVScrollView.Make(
                        
                            ).Bind(ref ElementsContainer)
                        ).OnReady(x => x.CSizeFitter.horizontalFit = x.CSizeFitter.verticalFit = UnityEngine.UI.ContentSizeFitter.FitMode.Unconstrained)
                        .OnReady(x => x.HOrVLayoutGroup.childForceExpandWidth = x.HOrVLayoutGroup.childForceExpandHeight = true),
                        EMSecondaryButton.Make("Add", 40, 5, AddConfigElem)
                    ).SetHeight(90),
                    XUIVLayout.Make(
                        EMText.Make("Min Range:"),
                        EMSlider.Make()
                            .Bind(ref MinRangeSlider)
                            .SetMinValue(0)
                            .SetMaxValue(115)
                            .SetIncrements(1)
                            .SetInteger(true)
                            .OnValueChanged(x => ConfigElem[SelectedIndex].MinRange = (int)x),
                        EMText.Make("Max Range:"),
                        EMSlider.Make()
                            .Bind(ref MaxRangeSlider)
                            .SetMinValue(0)
                            .SetMaxValue(115)
                            .SetIncrements(1)
                            .SetInteger(true)
                            .OnValueChanged(x => ConfigElem[SelectedIndex].MaxRange = (int)x),
                        EMText.Make("Font Size:"),
                        EMSlider.Make()
                            .Bind(ref FontSizeSlider)
                            .SetMinValue(0)
                            .SetMaxValue(5)
                            .SetIncrements(0.1f)
                            .OnValueChanged(x => ConfigElem[SelectedIndex].FontSize = x),
                        EMText.Make("Score Text:"),
                        EMTextInput.Make("Score text")
                            .Bind(ref ScoreTextInput)
                            .OnValueChanged(x => ConfigElem[SelectedIndex].ScoreText = x),
                        EMText.Make("Color:"),
                        XUIColorInput.Make()
                            .Bind(ref ScoreColorInput)
                            .SetAlphaSupport(true)
                            .OnValueChanged(x => ConfigElem[SelectedIndex].Color = x),
                        XUIHLayout.Make(
                            EMText.Make("Italic: "),
                            EMToggleSetting.Make()
                                .Bind(ref ItalicToggle)
                                .OnValueChanged(x => ConfigElem[SelectedIndex].Italic = x),
                            EMText.Make("Bold: "),
                            EMToggleSetting.Make()
                                .Bind(ref BoldToggle)
                                .OnValueChanged(x => ConfigElem[SelectedIndex].Bold = x)
                        )
                    )
                    .Bind(ref Container)
                    .SetActive(false)
                )
            ).BuildUI(transform);

            SetProfile(ConfigElem);
        }

        protected override void OnViewDeactivation()
        {
            EMConfig.EMScoreVisualizerProfile.Save(EMConfig.Instance.ScoreVisualizerProfile.ConfigPath, ConfigElem);
        }

        private void AddConfigElem()
        {
            var l_New = new EMConfig.EMScoreVisualizerElement();
            l_New.MinRange = 0;
            l_New.MaxRange = 115;

            if (ConfigElem.Any())
            {
                l_New.MinRange = 0;
                l_New.MaxRange = ConfigElem.First().MinRange;
            }

            ConfigElem.Add(l_New);
            ConfigElem.Sort(x => x.MinRange);
            SetProfile(ConfigElem);
        }

        ////////////////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        public void SetProfile(List<EMConfig.EMScoreVisualizerElement> conf)
        {
            foreach (var l_Item in VisualElements)
            {
                l_Item.SetActive(false);
            }

            for (int l_i = 0; l_i < conf.Count;l_i++)
            {
                if (l_i >= VisualElements.Count - 1)
                {
                    var l_Item = ScoreConfigElement.Make($"x");
                    VisualElements.Add(l_Item);
                    l_Item.BuildUI(ElementsContainer.Element.Container);
                }

                VisualElements[l_i].SetElement(l_i);
                VisualElements[l_i].UpdateText(conf[l_i]);
            }
        }

        public void SelectElement(int index)
        {
            Container.SetActive(true);
            SelectedIndex = index;

            MinRangeSlider.SetValue(ConfigElem[SelectedIndex].MinRange);
            MaxRangeSlider.SetValue(ConfigElem[SelectedIndex].MaxRange);
            FontSizeSlider.SetValue(ConfigElem[SelectedIndex].FontSize);
            ScoreTextInput.SetValue(ConfigElem[SelectedIndex].ScoreText);
            ScoreColorInput.SetValue(ConfigElem[SelectedIndex].Color);
            ItalicToggle.SetValue(ConfigElem[SelectedIndex].Italic);
            BoldToggle.SetValue(ConfigElem[SelectedIndex].Bold);
        }

    }
}
