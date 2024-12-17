using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Patches;

[HarmonyPatch(typeof(PlayerControllerB))]
public class PlayerControllerBPatches
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    private static readonly string interactBinding = "Interact";

    [HarmonyPatch(nameof(PlayerControllerB.SetHoverTipAndCurrentInteractTrigger))]
    [HarmonyPostfix]
    private static void SetHoverTipAndCurrentInteractTriggerPatch(PlayerControllerB __instance)
    {
        var cursorTip = __instance.cursorTip;
        if (cursorTip.text.Contains("[E]"))
        {
            cursorTip.text = cursorTip.text.Replace("[E]", "[" + GetInputBinding(interactBinding).ToDisplayString() + "]");
            FinallyCorrectKeys.Logger.LogDebug("Replaced the " + interactBinding + " binding.");
        }
    }
    private static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    }
}
