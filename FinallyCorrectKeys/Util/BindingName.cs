namespace FinallyCorrectKeys.Util;

public class BindingName
{
    public readonly string Name;
    public readonly string StandardKey;

    private BindingName(string name, string standardKey)
    {
        Name = name;
        StandardKey = standardKey;
    }

    public string ToHUDFormat()
    {
        return "[" + StandardKey + "]";
    }

    public override string ToString()
    {
        return Name;
    }

    public static readonly BindingName DISCARD = new("Discard", "G");
    public static readonly BindingName USE = new("Use", "LMB"); // Don't know where it gets used
    public static readonly BindingName ACTIVATE_ITEM = new("ActivateItem", "LMB");
    public static readonly BindingName SECONDARY_USE = new("ItemSecondaryUse", "Q");
    public static readonly BindingName TERTIARY_USE = new("ItemTertiaryUse", "E");
    public static readonly BindingName INSPECT_ITEM = new("InspectItem", "Z");
    public static readonly BindingName SPRINT = new("Sprint", "Shift");
    public static readonly BindingName SCAN = new("PingScan", "RMB");
    public static readonly BindingName INTERACT = new("Interact", "E");
    public static readonly BindingName BUILD = new("BuildMode", "B");
    public static readonly BindingName ROTATE = new("ReloadBatteries", "R");
    public static readonly BindingName STORE = new("Delete", "X");
}
