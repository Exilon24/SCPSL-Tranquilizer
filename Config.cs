namespace SCPSLTranquilizer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Should the plugin be enabled")]
        public bool IsEnabled { get; set; }

        [Description("How long should humans stay knocked out")]
        public bool HumanKnockoutTime { get; set; }

        [Description("How long should SCP's be knocked out")]
        public bool SCPKnockoutTime { get; set; }
    }
}
