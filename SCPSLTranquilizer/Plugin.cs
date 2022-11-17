namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override Version Version => new Version(1, 0, 0);

        public override void OnDisabled()
        {
            Log.Info($"SCP Knockout time in seconds: {Config.SCPKnockoutTime}");
            Log.Info($"Human Knockout time in seconds: {Config.HumanKnockoutTime}");
            Log.Info($"Will 096 be pacified: {Config.pacify096}");

        }

        public override void OnEnabled()
        {
            ;
        }
    }
}