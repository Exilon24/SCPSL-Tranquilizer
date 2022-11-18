namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    using MEC;
    using Mirror;
    using PlayerStatsSystem;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;

    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 3, 2);

        public bool canEnrage = true;
        public string[] ?disabledPlayers;

        public override void OnDisabled()
        {
            ;
        }

        public override void OnEnabled()
        {
            Exiled.Events.Handlers.Player.Shot += Player_Shot;
            Exiled.Events.Handlers.Player.ItemAdded += Player_ItemAdded;
            Exiled.Events.Handlers.Player.UsingItem += Player_UsingItem;
            Exiled.Events.Handlers.Player.InteractingDoor += Player_InteractingDoor;
            Exiled.Events.Handlers.Player.DroppingItem += Player_DroppingItem;
            Exiled.Events.Handlers.Player.DroppingAmmo += Player_DroppingAmmo;
            Exiled.Events.Handlers.Player.PickingUpItem += Player_PickingUpItem;
            Exiled.Events.Handlers.Scp173.Blinking += Scp173_Blinking;
            Exiled.Events.Handlers.Scp106.Teleporting += Scp106_Teleporting;
            Exiled.Events.Handlers.Scp096.Enraging += Scp096_Enraging;
        }

        private void Scp106_Teleporting(Exiled.Events.EventArgs.TeleportingEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Scp173_Blinking(Exiled.Events.EventArgs.BlinkingEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Player_PickingUpItem(Exiled.Events.EventArgs.PickingUpItemEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Player_DroppingAmmo(Exiled.Events.EventArgs.DroppingAmmoEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Player_DroppingItem(Exiled.Events.EventArgs.DroppingItemEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Player_InteractingDoor(Exiled.Events.EventArgs.InteractingDoorEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
        }

        private void Player_UsingItem(Exiled.Events.EventArgs.UsingItemEventArgs ev)
        {
            if (disabledPlayers != null)
            {
                if (disabledPlayers.Contains(ev.Player.UserId))
                {
                    ev.IsAllowed = false;
                }
                else
                {
                    ev.IsAllowed = true;
                }
            }
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
            Log.Debug(disabledPlayers);
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
            ev.Shooter.CurrentItem.Destroy();
            ev.Target.CanSendInputs = false;
            ev.Target.IsInvisible = true;
            disabledPlayers.Append(ev.Target.UserId);
            canEnrage = false;
            Ragdoll playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Unknown), ev.Target.Role.Type, ev.Target.Position + (Vector3.up * 1f), default, "Sleeping victim", NetworkTime.time), true);
            playerRagdoll.Spawn();

            if (isSCP)
            {
                if (is096)
                {
                    playerRagdoll.Delete();
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    disabledPlayers = disabledPlayers.Where(e => e != ev.Target.UserId).ToArray();
                    canEnrage = true;
                }
                else
                {
                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Blinded, (float) Config.SCPKnockoutTime);
                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Deafened, (float)Config.SCPKnockoutTime);
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);
                    disabledPlayers = disabledPlayers.Where(e => e != ev.Target.UserId).ToArray();
                    ev.Target.CanSendInputs = true;
                    ev.Target.IsInvisible = false;
                    playerRagdoll.Delete();
                }
            }
            else 
            {
                ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Blinded, (float)Config.SCPKnockoutTime);
                ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Deafened, (float)Config.SCPKnockoutTime);
                yield return Timing.WaitForSeconds((float)Config.HumanKnockoutTime);
                disabledPlayers = disabledPlayers.Where(e => e != ev.Target.UserId).ToArray();
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;
                playerRagdoll.Delete();
            }
        }
    }
}