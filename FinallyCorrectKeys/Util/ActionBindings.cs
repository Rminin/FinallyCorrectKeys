using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Util;

internal class ActionBindings
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    internal static readonly string discardBinding = "Discard";
    internal static readonly string useBinding = "Use"; // Don't know where it gets used
    internal static readonly string activateItemBinding = "ActivateItem";
    internal static readonly string secondaryUseBinding = "ItemSecondaryUse";
    internal static readonly string tertiaryUseBinding = "ItemTertiaryUse";
    internal static readonly string inspectItemBinding = "InspectItem";
    internal static readonly string sprintBinding = "Sprint";
    internal static readonly string scanBinding = "PingScan";

    internal static readonly string interactBinding = "Interact";

    internal static readonly string buildBinding = "BuildMode";
    internal static readonly string rotateBinding = "ReloadBatteries";
    internal static readonly string storeBinding = "Delete";

    internal static InputBinding GetInputBinding(string actionName)
    {
        return actions.FindAction(actionName).bindings[0];
    }

    internal static string GetInputBindingString(string actionName)
    {
        return GetInputBinding(actionName).ToDisplayString();
    }
}
