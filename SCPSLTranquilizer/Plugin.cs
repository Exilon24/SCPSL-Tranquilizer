// TODO:
// REMOVE DEBUGGING
// Make Config.Pacify106 do something when disabled
// #include <iostream>

namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    using MEC;
    using Mirror;
    using PlayerStatsSystem;
    using System.Collections.Generic;
    using UnityEngine;
    using Item = Exiled.API.Features.Items.Item;
    using Player = Exiled.Events.Handlers.Player;

    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 3, 2);

        public List<string> disabledPlayers = new List<string>();

        public override void OnDisabled()
        {
            ;
        }

        public override void OnEnabled()
        {
            Player.Shot += Player_Shot;
            Player.ItemAdded += Player_ItemAdded;
            Player.UsingItem += Player_UsingItem;
            Player.InteractingDoor += Player_InteractingDoor;
            Player.DroppingItem += Player_DroppingItem;
            Player.DroppingAmmo += Player_DroppingAmmo;
            Player.Shooting += Player_Shooting;
            Player.PickingUpItem += Player_PickingUpItem;
            Player.Hurting += Player_Hurting;
            Exiled.Events.Handlers.Scp173.Blinking += Scp173_Blinking;
            Exiled.Events.Handlers.Scp106.Teleporting += Scp106_Teleporting;
            Exiled.Events.Handlers.Scp096.Enraging += Scp096_Enraging;
        }

        // ________________________________________DISABLING THE PLAYER________________________________________

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Attacker.UserId))
            {
                ev.IsAllowed = false;
                ev.Attacker.ClearBroadcasts();
                ev.Attacker.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_Shooting(Exiled.Events.EventArgs.ShootingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Shooter.UserId))
            {
                ev.Shooter.ClearBroadcasts();
                ev.Shooter.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            };
        }

        private void Scp106_Teleporting(Exiled.Events.EventArgs.TeleportingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Scp173_Blinking(Exiled.Events.EventArgs.BlinkingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_PickingUpItem(Exiled.Events.EventArgs.PickingUpItemEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            };
        }

        private void Player_DroppingAmmo(Exiled.Events.EventArgs.DroppingAmmoEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_DroppingItem(Exiled.Events.EventArgs.DroppingItemEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_InteractingDoor(Exiled.Events.EventArgs.InteractingDoorEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_UsingItem(Exiled.Events.EventArgs.UsingItemEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(new Broadcast("You <color=red>cannot</color> do anything when <color=red>tranquilized</color>", 3, true));
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Scp096_Enraging(Exiled.Events.EventArgs.EnragingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Scp096.ResetEnrage();
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
        // ________________________________________DISABLING THE PLAYER________________________________________


        // When someone is shot by the tranquilizer
        private void Player_Shot(Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            Log.Debug(disabledPlayers.ToArray()); // TODO: Remove debugging

            // Get the current weapon instance
            Item weapon = ev.Shooter.CurrentItem;
            
            // Check if the weapon is a COM15 (Tranquilizer)
            if (weapon.IsWeapon && weapon.Type == ItemType.GunCOM15 && ev.Target != null)
            {
                // Tell the user that they tranquilized someone.
                ev.Shooter.Broadcast(new Broadcast($"You tranquilized <color=red><b>{ev.Target.Nickname}</b></color>", 3, true));
                ev.Target.Broadcast(new Broadcast($"You where tranquilized by <color=red>{ev.Shooter.Nickname}</color>!", 3, true));

                // Knock out the target
                Timing.RunCoroutine(knockout(ev.Target.IsScp, (ev.Target.Role.Type == RoleType.Scp096), ev));
            }
        }

        // Corountine to knock out the victims
        public IEnumerator<float> knockout(bool isSCP, bool is096, Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            // Destroy the tranquilizer (It only has one shot)
            ev.Shooter.CurrentItem.Destroy();

            // Create the ragdoll
            Ragdoll playerRagdoll;

            // Check if the target is an SCP
            if (isSCP)
            {
                if (is096) // Check if the target is 096
                {
                    // Pacify 096
                    disabledPlayers.Add(ev.Target.UserId);

                    // Pacify timing
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);

                    // Return 096
                    disabledPlayers.Remove(ev.Target.UserId);
                }
                else
                {
                    // Create the ragdoll
                    playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Unknown), ev.Target.Role.Type, ev.Target.Position + (Vector3.up * 1f), default, "SCP-343", NetworkTime.time), true);
                    playerRagdoll.Spawn();

                    // Disable the player and turn the player invisible (Used for shootable ragdolls)
                    ev.Target.CanSendInputs = false;
                    ev.Target.IsInvisible = true;
                    disabledPlayers.Add(ev.Target.UserId);

                    // Blind and deafen the target
                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Blinded, (float) Config.SCPKnockoutTime);
                    ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Deafened, (float)Config.SCPKnockoutTime);

                    // Tranquilized time
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);

                    // Enable the player
                    disabledPlayers.Remove(ev.Target.UserId);
                    ev.Target.CanSendInputs = true;
                    ev.Target.IsInvisible = false;

                    // Destroy the ragdoll
                    playerRagdoll.Delete();
                }
            }
            else 
            {
                // Create the ragdoll
                playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Unknown), ev.Target.Role.Type, ev.Target.Position + (Vector3.up * 1f), default, "SCP-343", NetworkTime.time), true);
                playerRagdoll.Spawn();

                // Disable the player and turn the player invisible (Used for shootable ragdolls)
                ev.Target.CanSendInputs = false;
                ev.Target.IsInvisible = true;
                disabledPlayers.Add(ev.Target.UserId);

                // Blind and deafen the target
                ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Blinded, (float)Config.SCPKnockoutTime);
                ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Deafened, (float)Config.SCPKnockoutTime);

                // Tranquilized time
                yield return Timing.WaitForSeconds((float)Config.HumanKnockoutTime);

                // Enable the player
                disabledPlayers.Remove(ev.Target.UserId);
                ev.Target.CanSendInputs = true;
                ev.Target.IsInvisible = false;

                // Destroy the ragdoll
                playerRagdoll.Delete();
            }
        }
    }
}