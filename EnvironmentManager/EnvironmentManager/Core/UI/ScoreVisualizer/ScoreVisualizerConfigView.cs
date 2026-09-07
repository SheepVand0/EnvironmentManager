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
        protected ScoreConfigElement(string label, int width, int height, string name = "EnvironmentManagerButton", Action onClick = null) 
            : base(label, width, height, name, onClick)
        {
            OnClick(() =>
            {
                ScoreVisualizerConfigView.Instance.SelectElement(_scoreElement);
            });
        }

        public static ScoreConfigElement Make(string fileName)
        {
            return new ScoreConfigElement(fileName, 25, 7);
        }

        //////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////

        private int _scoreElement;

        public void SetElement(int elem)
        {
            _scoreElement = elem;
        }

        public void UpdateText(EMConfig.EMScoreVisualizerElement elem)
        {
            SetText($"{elem.MinRange} - {elem.MaxRange}");
        }
    }

    internal class ScoreVisualizerConfigView : ViewController<ScoreVisualizerConfigView>
    {
        internal static new ScoreVisualizerConfigView Instance = null;

        List<EMConfig.EMScoreVisualizerElement> ConfigElems;
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
            ConfigElems = EMConfig.EMScoreVisualizerProfile.Load(EMConfig.Instance.ScoreVisualizerProfile.ConfigPath);

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
                            .OnValueChanged(x => ConfigElems[SelectedIndex].MinRange = (int)x),
                        EMText.Make("Max Range:"),
                        EMSlider.Make()
                            .Bind(ref MaxRangeSlider)
                            .SetMinValue(0)
                            .SetMaxValue(115)
                            .SetIncrements(1)
                            .SetInteger(true)
                            .OnValueChanged(x => ConfigElems[SelectedIndex].MaxRange = (int)x),
                        EMText.Make("Font Size:"),
                        EMSlider.Make()
                            .Bind(ref FontSizeSlider)
                            .SetMinValue(0)
                            .SetMaxValue(5)
                            .SetIncrements(0.1f)
                            .OnValueChanged(x => ConfigElems[SelectedIndex].FontSize = x),
                        EMText.Make("Score Text:"),
                        EMTextInput.Make("Score text")
                            .Bind(ref ScoreTextInput)
                            .OnValueChanged(x => ConfigElems[SelectedIndex].ScoreText = x),
                        EMText.Make("Color:"),
                        XUIColorInput.Make()
                            .Bind(ref ScoreColorInput)
                            .SetAlphaSupport(true)
                            .OnValueChanged(x => ConfigElems[SelectedIndex].Color = x),
                        XUIHLayout.Make(
                            EMText.Make("Italic: "),
                            EMToggleSetting.Make()
                                .Bind(ref ItalicToggle)
                                .OnValueChanged(x => ConfigElems[SelectedIndex].Italic = x),
                            EMText.Make("Bold: "),
                            EMToggleSetting.Make()
                                .Bind(ref BoldToggle)
                                .OnValueChanged(x => ConfigElems[SelectedIndex].Bold = x)
                        )
                    )
                    .Bind(ref Container)
                    .SetActive(false)
                )
            ).BuildUI(transform);
            
            EnableEditComponents(false);
            
            SetProfile(ConfigElems);
        }

        protected override void OnViewDeactivation()
        {
            EMConfig.EMScoreVisualizerProfile.Save(EMConfig.Instance.ScoreVisualizerProfile.ConfigPath, ConfigElems);
        }

        private void AddConfigElem()
        {
            var l_New = new EMConfig.EMScoreVisualizerElement();
            l_New.MinRange = 0;
            l_New.MaxRange = 115;

            if (ConfigElems.Any())
            {
                l_New.MinRange = 0;
                l_New.MaxRange = ConfigElems.First().MinRange;
            }

            ConfigElems.Add(l_New);
            //ConfigElem.Sort(x => x.MinRange);
            SetProfile(ConfigElems);
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

            EnableEditComponents(true);
            
            MinRangeSlider.SetValue(ConfigElems[SelectedIndex].MinRange);
            MaxRangeSlider.SetValue(ConfigElems[SelectedIndex].MaxRange);
            FontSizeSlider.SetValue(ConfigElems[SelectedIndex].FontSize);
            ScoreTextInput.SetValue(ConfigElems[SelectedIndex].ScoreText);
            ScoreColorInput.SetValue(ConfigElems[SelectedIndex].Color);
            ItalicToggle.SetValue(ConfigElems[SelectedIndex].Italic);
            BoldToggle.SetValue(ConfigElems[SelectedIndex].Bold);
        }

        private void EnableEditComponents(bool enable)
        {
            Container.ForEachDirect<EMText>(x => x.SetActive(enable));
            Container.ForEachDirect<EMSlider>(x => x.SetActive(enable));
            Container.ForEachDirect<EMTextInput>(x => x.SetActive(enable));
        }
        
    }
}
