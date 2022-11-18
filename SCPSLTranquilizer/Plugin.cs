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
    using System.Linq;
    using UnityEngine;
    using Item = Exiled.API.Features.Items.Item;

    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 3, 2);

        public bool canEnrage = true;

        public List<string> disabledPlayers = new List<string>();

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

        // ________________________________________DISABLING THE PLAYER________________________________________
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
            if (disabledPlayers.Contains(ev.Player.UserId))
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
        // ________________________________________DISABLING THE PLAYER________________________________________


        // When someone is shot by the tranquilizer
        private void Player_Shot(Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            Log.Debug(disabledPlayers); // TODO: Remove debugging

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

            // Pacify 096
            canEnrage = false;

            // Create the ragdoll
            Ragdoll playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Unknown), ev.Target.Role.Type, ev.Target.Position + (Vector3.up * 1f), default, "SCP-343", NetworkTime.time), true);
            playerRagdoll.Spawn();

            // Check if the target is an SCP
            if (isSCP)
            {
                if (is096) // Check if the target is 096
                {
                    // Remove 096's ragdoll [TESTING]
                    playerRagdoll.Delete();

                    // Pacify timing
                    yield return Timing.WaitForSeconds((float)Config.SCPKnockoutTime);

                    // Return 096
                    canEnrage = true;
                }
                else
                {
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
                // Disable the player and turn the player invisible (Used for shootable ragdolls)
                ev.Target.CanSendInputs = false;
                ev.Target.IsInvisible = true;
                disabledPlayers.Append(ev.Target.UserId);

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