namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    using MEC;
    using PlayerStatsSystem;
    using System.Collections.Generic;


    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 0, 0);

        public bool canEnrage = true;

        public override void OnDisabled()
        {
            Log.Info($"SCP Knockout time in seconds: {Config.SCPKnockoutTime}");
            Log.Info($"Human Knockout time in seconds: {Config.HumanKnockoutTime}");
            Log.Info($"Will 096 be pacified: {Config.pacify096}");

        }

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Shot += Player_Shot;
            Exiled.Events.Handlers.Player.PickingUpItem += Player_PickingUpItem;
            Exiled.Events.Handlers.Scp096.Enraging += Scp096_Enraging;
            new Broadcast("The mod works!", 20, true);
            
        }

        private void Scp096_Enraging(Exiled.Events.EventArgs.EnragingEventArgs ev)
        {
            if (!canEnrage)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_PickingUpItem(Exiled.Events.EventArgs.PickingUpItemEventArgs ev)
        {
            if (ev.Pickup.Type == ItemType.GunCOM15)
            {
                ev.Player.Broadcast(new Broadcast($"You picked up the <color=red>tranquilizer</color>", 5, true));
            }
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
                    // Check if the SCP is 096
                    if (ev.Target.Role.Type == RoleType.Scp096 && Config.pacify096)
                    {
                        ev.Target.Broadcast(new Broadcast("You have been pacified!", 5, true));
                        knockout(true, true, ev);
                    }

                    knockout(true, false, ev);
                }
                else
                {
                    knockout(false, false, ev);
                }
               
            }
        }

        /// <summary>
        /// Block inputs from user when knocked out
        /// </summary>
        /// <param name="isSCP"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        public IEnumerator<float> knockout(bool isSCP, bool is096, Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            ev.Target.CanSendInputs = false;
            ev.Target.IsInvisible = true;

            // Spawn ragdoll
            Ragdoll playerRagdoll = new Ragdoll(new RagdollInfo(Exiled.API.Features.Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Crushed), ev.Target.Position, default), true);
            playerRagdoll.Spawn();
           
            if (isSCP)
            {
                if (is096)
                {
                    Log.Warn("096 Tranquilized");
                    canEnrage = false;
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    canEnrage = true;
                }
                else
                {
                    Log.Warn("SCP Tranquilized");
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    ev.Target.CanSendInputs = true;
                    ev.Target.IsInvisible = false;
                    playerRagdoll.Delete();
                }
            }
            else 
            {
                Log.Warn("Human Tranquilized");
                yield return Timing.WaitForSeconds((float)Config.HumanKnockoutTime);
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;
                playerRagdoll.Delete();
            }
        }
    }
}