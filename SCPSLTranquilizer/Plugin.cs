namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    using MEC;
    using Mirror;
    using PlayerStatsSystem;
    using System.Collections.Generic;
    using UnityEngine;

    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 3, 2);

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
            Exiled.Events.Handlers.Player.ItemAdded += Player_ItemAdded;
            Exiled.Events.Handlers.Scp096.Enraging += Scp096_Enraging;
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

        private void Player_ItemAdded(Exiled.Events.EventArgs.ItemAddedEventArgs ev)
        {
            if (ev.Item.Type == ItemType.GunCOM15)
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
                ev.Target.Broadcast(new Broadcast($"You where tranquilized by <color=red>{ev.Shooter.Nickname}</color>!", 3, true));

                if (ev.Target.IsScp)
                {
                    // Check if the SCP is 096
                    if (ev.Target.Role.Type == RoleType.Scp096 && Config.pacify096)
                    {
                        ev.Target.Broadcast(new Broadcast("You have been pacified!", 5, true));
                        Timing.RunCoroutine(knockout(true, true, ev));
                    }
                    else
                    {
                        Timing.RunCoroutine(knockout(true, false, ev));
                    }
                }
                else
                {
                    Timing.RunCoroutine(knockout(false, false, ev));
                }
               
            }
        }

        public IEnumerator<float> knockout(bool isSCP, bool is096, Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            ev.Target.CanSendInputs = false;
            ev.Target.IsInvisible = true;

            // Spawn ragdoll
            Ragdoll playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Crushed), ev.Target.Role.Type, ev.Target.Position + (Vector3.up * 3f), default, "SCP-343", NetworkTime.time), true);
            playerRagdoll.Spawn();
           
            if (isSCP)
            {
                if (is096)
                {
                    canEnrage = false;
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    canEnrage = true;
                }
                else
                {
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    ev.Target.CanSendInputs = true;
                    ev.Target.IsInvisible = false;
                    playerRagdoll.Delete();
                }
            }
            else 
            {
                yield return Timing.WaitForSeconds((float)Config.HumanKnockoutTime);
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;
                playerRagdoll.Delete();
            }
        }
    }
}