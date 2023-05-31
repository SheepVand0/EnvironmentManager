using CP_SDK;
using EnvironmentManager.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvironmentManager
{
    internal class EMModule : CP_SDK.ModuleBase<EMModule>
    {
        public override EIModuleBaseType Type => EIModuleBaseType.External;

        public override string Name => "Environment Manager";

        public override string Description => "Allows you to edit your environment";

        public override bool UseChatFeatures => false;

        public override bool IsEnabled { get => EMConfig.Instance.IsEnabled; set => EMConfig.Instance.IsEnabled = value; }

        public override EIModuleBaseActivationType ActivationType => EIModuleBaseActivationType.Never;

        protected override void OnDisable()
        {
        }

        protected override void OnEnable()
        {
        }
    }
}
