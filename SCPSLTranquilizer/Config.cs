namespace SCPSLTranquilizer
{
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int SCPKnockoutTime { get; set; } = 30;
        public int HumanKnockoutTime { get; set; } = 60;
        public bool pacify096 { get; set; } = true;
    }
}
