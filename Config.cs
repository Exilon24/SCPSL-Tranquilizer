namespace SCPSLTranquilizer
{
    using Exiled.API.Interfaces;

    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public int SCPKnockoutTime { get; set; } = 10;
        public int HumanKnockoutTime { get; set; } = 20;
        public byte tranquilizerAmmo { get; set; } = 4;
        public bool pacify096 { get; set; } = true;
    }
}
