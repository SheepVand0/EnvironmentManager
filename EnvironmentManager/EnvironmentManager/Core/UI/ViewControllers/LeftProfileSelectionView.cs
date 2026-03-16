using CP_SDK.XUI;
using CP_SDK_BS.UI;
using EnvironmentManager.Config;
using EnvironmentManager.Core.UI.Defaults;

namespace EnvironmentManager.Core.UI.ViewControllers
{
    internal class LeftProfileSelectionView : ViewController<LeftProfileSelectionView>
    {

        protected override void OnViewCreation()
        {
            Templates.FullRectLayout(
                XUIHLayout.Make(
                    EMText.Make("Walls outline color:"),
                    XUIColorInput.Make()
                        .SetValue(EMConfig.Instance.WallsOutlineColor)
                        .OnValueChanged(x =>
                        {
                            EMConfig.Instance.WallsOutlineColor = x;
                            EMConfig.Instance.Save();
                        })
                ),
                XUIHLayout.Make(
                    EMText.Make("Disable walls outline:"),
                    EMToggleSetting.Make()
                        .SetValue(EMConfig.Instance.DisableWallsOutline)
                        .OnValueChanged(x =>
                        {
                            EMConfig.Instance.DisableWallsOutline = x;
                            EMConfig.Instance.Save();
                        })
                )
            ).BuildUI(transform);
        }

        //////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////

    }
}
