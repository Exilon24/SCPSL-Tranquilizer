using UnityEngine;
using PlayableScps;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Exiled.API.Features;

namespace Tranquilizer
{
    [HarmonyPatch(typeof(PlayableScps.Scp096), nameof(PlayableScps.Scp096.UpdateVision))]
    public static class UpdateVisionPatch
    {
        public static bool Prefix(PlayableScps.Scp096 __instance)
        {
            Vector3 vector = __instance.Hub.transform.TransformPoint(PlayableScps.Scp096._headOffset);
            foreach (KeyValuePair<GameObject, global::ReferenceHub> keyValuePair in global::ReferenceHub.GetAllHubs())
            {
                global::ReferenceHub value = keyValuePair.Value;
                global::CharacterClassManager characterClassManager = value.characterClassManager;
                if (characterClassManager.CurClass != global::RoleType.Spectator && !(value == __instance.Hub) && !characterClassManager.IsAnyScp() && Vector3.Dot((value.PlayerCameraReference.position - vector).normalized, __instance.Hub.PlayerCameraReference.forward) >= 0.1f)
                {
                    VisionInformation visionInformation = VisionInformation.GetVisionInformation(value, vector, -0.1f, 60f, true, true, __instance.Hub.localCurrentRoomEffects, 0);
                    if (visionInformation.IsLooking)
                    {
                        float delay = visionInformation.LookingAmount / 0.25f * (visionInformation.Distance * 0.1f);
                        if (!(Plugin.disabledPlayers.Contains(Player.Get(__instance.Hub).UserId)))
                        {
                            if (!__instance.Calming)
                            {
                                __instance.AddTarget(value.gameObject);
                            }
                            if (__instance.CanEnrage && value.gameObject != null)
                            {
                                __instance.PreWindup(delay);
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }

            return false;
        }
    }
}

