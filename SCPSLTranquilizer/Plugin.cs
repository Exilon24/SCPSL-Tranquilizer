namespace SCPSLTranquilizer
{
    using Exiled.Events.Handlers;
    using Exiled.API.Features;
    using MEC;

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
            Exiled.Events.Handlers.Player.Shot += Player_Shot;
            new Broadcast("The mod works!", 20, true);
            
        }

        private void Player_Shot(Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            Exiled.API.Features.Items.Item weapon = ev.Shooter.CurrentItem;
            if (weapon.IsWeapon && weapon.Type == ItemType.GunCOM15 && ev.Target != null)
            {
                ev.Shooter.Broadcast(new Broadcast($"You tranquilized <color=red>{ev.Target.Nickname}</color>", 3, true));
                ev.Target.Broadcast(new Broadcast($"You where tranquilized by <color=red>{ev.Target.Nickname}</color>!", 3, true));

                if (ev.Target.IsScp)
                {
                    knockout(true, ev);
                }
                else
                {
                    knockout(false, ev);
                }
               
            }
        }

        /// <summary>
        /// Block inputs from user when knocked out
        /// </summary>
        /// <param name="isSCP"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        public IEnumerator<float> knockout(bool isSCP, Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            ev.Target.CanSendInputs = false;
            ev.Target.IsInvisible = true;
            

            if (isSCP)
            {
                Log.Warn("SCP Tranquilized");
                yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;
            }
            else 
            {
                Log.Warn("Human Tranquilized");
                yield return Timing.WaitForSeconds((float)Config.HumanKnockoutTime);
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;
            }
        }
    }
}