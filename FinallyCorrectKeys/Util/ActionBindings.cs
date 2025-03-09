using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace FinallyCorrectKeys.Util;

internal class ActionBindings
{
    private static readonly InputActionAsset actions = IngamePlayerSettings.Instance.playerInput.actions;

    internal static Dictionary<Bindings, InputAction> actionMap = [];

    internal static InputAction interactAction = actions.FindAction(Bindings.INTERACT.Name);
    internal static InputBinding interactBind = interactAction.bindings[0];
    internal static string interactDisplayString = interactBind.ToDisplayString();

    internal static InputAction GetInputAction(Bindings binding)
    {
        if (actionMap.ContainsKey(binding)) return actionMap[binding];
        var action = actions.FindAction(binding);
        actionMap.Add(binding, action);
        return action;
    }

    internal static InputBinding GetInputBinding(Bindings binding)
    {
        return GetInputAction(binding).bindings[0];
    }

    /// <summary>
    /// This method should not be executed repeatedly because the Unity method "ToDisplayString()" seems to be heavy on performance.
    /// Using this method repeatedly could therefore lead to preformance drops. Avoid it in the context of Unity's "Update()".
    /// </summary>
    internal static string GetInputBindingString(Bindings binding)
    {
        return GetInputBinding(binding).ToDisplayString();
    }
}
