namespace SCPSLTranquilizer
{
    using Exiled.API.Interfaces;
    using System.ComponentModel;

    public class Config : IConfig
    {
        [Description("Whether the plugin is enabled")]
        public bool IsEnabled { get; set; }

        [Description("How long should an SCP be knocked out (s)")]
        public int SCPKnockoutTime { get; set; } = 30;

        [Description("How long should a human be knocked out (s)")]
        public int HumanKnockoutTime { get; set; } = 60;

        [Description("Should 096 be affected by the tranquilizer (if true, he will not be enraged")]
        public bool pacify096 { get; set; } = true;
    }
}
