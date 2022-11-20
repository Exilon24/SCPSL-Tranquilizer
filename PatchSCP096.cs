using UnityEngine;
using PlayableScps;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;




namespace Tranquilizer
{
    [HarmonyPatch(typeof(Scp096), nameof(Scp096.UpdateVision))]
    public class UpdateVisionPatch
    {
        public static bool Prefix(Scp096 __instance)
        {    
            Vector3 vector = __instance.Hub.transform.TransformPoint(Scp096._headOffset);
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
                        if (!Tranquilizer.Plugin.disabledPlayers.Contains(Exiled.API.Features.Player.Get(__instance.Hub).UserId))
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
                    }
                }
            }

            return false;
        }
    }
}
