// TODO:
// #include <iostream>

namespace SCPSLTranquilizer
{
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using MEC;
    using Mirror;
    using PlayerStatsSystem;
    using System.Collections.Generic;
    using Item = Exiled.API.Features.Items.Item;
    using Player = Exiled.Events.Handlers.Player;
    using Firearm = Exiled.API.Features.Items.Firearm;
    using System;

    public class Plugin : Plugin<Config>
    {
        // Shit I have to override for the plugin to work
        public override string Name => "SCP:SL Tranquilizer";
        public override string Author => "(S)Exilon";
        public override System.Version Version => new System.Version(1, 3, 2);

        public List<string> disabledPlayers = new List<string>();

        // Create the ragdoll
        Ragdoll playerRagdoll;

        public override void OnDisabled()
        {
            // Unsubscribe to events
            Player.Shot -= Player_Shot;
            Player.ItemAdded -= Player_ItemAdded;
            Player.UsingItem -= Player_UsingItem;
            Player.InteractingDoor -= Player_InteractingDoor;
            Player.DroppingItem -= Player_DroppingItem;
            Player.DroppingAmmo -= Player_DroppingAmmo;
            Player.Shooting -= Player_Shooting;
            Player.PickingUpItem -= Player_PickingUpItem;
            Player.Hurting -= Player_Hurting;
            Player.Died -= Player_Died;
            Player.ThrowingItem -= Player_ThrowingItem;
            Exiled.Events.Handlers.Scp173.Blinking -= Scp173_Blinking;
            Exiled.Events.Handlers.Scp106.Teleporting -= Scp106_Teleporting;
            Exiled.Events.Handlers.Scp096.Enraging -= Scp096_Enraging;
        }

        public override void OnEnabled()
        {
            // Subscribe to events
            Player.Shot += Player_Shot;
            Player.ChangingItem += Player_ChangingItem;
            Player.ItemAdded += Player_ItemAdded;
            Player.UsingItem += Player_UsingItem;
            Player.InteractingDoor += Player_InteractingDoor;
            Player.DroppingItem += Player_DroppingItem;
            Player.DroppingAmmo += Player_DroppingAmmo;
            Player.Shooting += Player_Shooting;
            Player.PickingUpItem += Player_PickingUpItem;
            Player.Hurting += Player_Hurting;
            Player.Died += Player_Died;
            Player.ThrowingItem += Player_ThrowingItem;
            Player.ReloadingWeapon += Player_ReloadingWeapon;
            Player.EnteringPocketDimension += Player_EnteringPocketDimension;
            Exiled.Events.Handlers.Scp173.Blinking += Scp173_Blinking;
            Exiled.Events.Handlers.Scp106.Teleporting += Scp106_Teleporting;
            Exiled.Events.Handlers.Scp096.Enraging += Scp096_Enraging;
        }



        // ________________________________________DISABLING THE PLAYER________________________________________

        private void Player_ChangingItem(Exiled.Events.EventArgs.ChangingItemEventArgs ev)
        {
            if (ev.NewItem.Type == ItemType.GunCOM15)
            {
                Firearm gun = ev.NewItem as Firearm;

                if (gun.Ammo > Config.tranquilizerAmmo)
                {
                    gun.Ammo = Config.tranquilizerAmmo;
                }

                ev.Player.ShowHint($"<color=red>{gun.Ammo} / {Config.tranquilizerAmmo}</color>", 10);
            }
        }

        private void Player_ReloadingWeapon(Exiled.Events.EventArgs.ReloadingWeaponEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId) || ev.Firearm.Type == ItemType.GunCOM15)
            {
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_EnteringPocketDimension(Exiled.Events.EventArgs.EnteringPocketDimensionEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Scp106.UserId))
            {
                ev.IsAllowed = false;
                ev.Player.ClearBroadcasts();
                ev.Scp106.ShowHint($"<color=red>Tranquilized</color>", 2);
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_ThrowingItem(Exiled.Events.EventArgs.ThrowingItemEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.IsAllowed = false;
                ev.Player.ClearBroadcasts();
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
            }
            else
            {
                ev.IsAllowed = true;
            }
        }

        private void Player_Died(Exiled.Events.EventArgs.DiedEventArgs ev)
        {
            if (playerRagdoll != null)
            {
                playerRagdoll.Delete();
            }
        }

        private void Player_Hurting(Exiled.Events.EventArgs.HurtingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Attacker.UserId))
            {
                ev.IsAllowed = false;
                ev.Attacker.ClearBroadcasts();
                ev.Attacker.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Shooter.ShowHint($"<color=red>Tranquilized</color>", 2);
                ev.IsAllowed = false;
            }
            else
            {
                ev.IsAllowed = true;

                if (ev.Shooter.CurrentItem.Type == ItemType.GunCOM15)
                {
                    Firearm gun = ev.Shooter.CurrentItem as Firearm;
                    ev.Shooter.ShowHint($"<color=red>{gun.Ammo} / {Config.tranquilizerAmmo}</color>", 10);

                    if (gun.Ammo < 1)
                    {
                        ev.Shooter.CurrentItem.Destroy();
                        ev.Shooter.ShowHint($"<color=red>OUT OF AMMO</color>", 10);
                    }
                }
            };
        }

        private void Scp106_Teleporting(Exiled.Events.EventArgs.TeleportingEventArgs ev)
        {
            if (disabledPlayers.Contains(ev.Player.UserId))
            {
                ev.Player.ClearBroadcasts();
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                ev.Player.ShowHint($"<color=red>Tranquilized</color>", 2);
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
                Firearm gun = ev.Item as Firearm;
                ev.Player.Broadcast(new Broadcast($"You picked up the <color=red><b>Tranquilizer</b></color>", 5, true));
            }
        }
        // ________________________________________DISABLING THE PLAYER________________________________________


        // When someone is shot by the tranquilizer
        private void Player_Shot(Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            // Get the current weapon instance
            Item weapon = ev.Shooter.CurrentItem;

            // Check if the weapon is a COM15 (Tranquilizer)
            if (weapon.IsWeapon && weapon.Type == ItemType.GunCOM15 && ev.Target != null)
            {
                // Tell the user that they tranquilized someone.
                ev.Shooter.Broadcast(new Broadcast($"You tranquilized <color=red><b>{ev.Target.Nickname}</b></color>", 3, true));
                ev.Target.Broadcast(new Broadcast($"You where tranquilized by <color=red>{ev.Shooter.Nickname}</color>!", 3, true));

                // Knock out the target
                if (!disabledPlayers.Contains(ev.Target.UserId))
                {
                    Timing.RunCoroutine(knockout(ev.Target.IsScp, (ev.Target.Role.Type == RoleType.Scp096), ev));
                }
            }
        }

        // Here to make the effect a little neater
        public IEnumerator<float> knockoutEffect(Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            playerRagdoll = new Ragdoll(new RagdollInfo(Server.Host.ReferenceHub, new UniversalDamageHandler(200, DeathTranslations.Unknown), ev.Target.Role.Type, ev.Target.Position, default, ev.Target.DisplayNickname, NetworkTime.time), true);
            playerRagdoll.Spawn();

            // Disable the player and turn the player invisible (Used for shootable ragdolls)
            ev.Target.CanSendInputs = false;
            ev.Target.IsInvisible = true;

            disabledPlayers.Add(ev.Target.UserId);

            // Blind and deafen the target
            ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Blinded, ev.Target.IsScp ? Config.SCPKnockoutTime : Config.HumanKnockoutTime);
            ev.Target.EnableEffect(Exiled.API.Enums.EffectType.Deafened, ev.Target.IsScp ? Config.SCPKnockoutTime : Config.HumanKnockoutTime);

            // Tranquilized time
            yield return Timing.WaitForSeconds(ev.Target.IsScp ? Config.SCPKnockoutTime : Config.HumanKnockoutTime);

            // Enable the player
            disabledPlayers.Remove(ev.Target.UserId);
            ev.Target.CanSendInputs = true;
            ev.Target.IsInvisible = false;

            // Destroy the ragdoll
            playerRagdoll.Delete();
        }

        // Corountine to knock out the victims
        // TODO: Dear god
        public IEnumerator<float> knockout(bool isSCP, bool is096, Exiled.Events.EventArgs.ShotEventArgs ev)
        {
            // Check if the target is an SCP
            if (isSCP)
            {
                if (is096) // Check if the target is 096
                {
                    // calming the timid little fella down
                    Scp096Role role = ev.Target.Role as Scp096Role;
                    if (role != null)
                    {
                        if (role.IsEnraged && Config.pacify096)
                        {
                            role.Script.EnrageTimeLeft = 0f;
                            role.Script.EndEnrage();

                        }
                        // treat him normally
                        else
                        {
                            disabledPlayers.Add(ev.Target.UserId);
                            Timing.RunCoroutine(knockoutEffect(ev));
                        }
                    }
                }
                else
                {
                    Timing.RunCoroutine(knockoutEffect(ev));
                }
            }
            else
            {
                Timing.RunCoroutine(knockoutEffect(ev));
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }
}