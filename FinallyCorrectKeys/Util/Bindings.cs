namespace FinallyCorrectKeys.Util;

public class Bindings
{
    public readonly string Name;
    public readonly string StandardKey;

    private Bindings(string name, string standardKey)
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

    public static readonly Bindings DISCARD = new("Discard", "G");
    public static readonly Bindings USE = new("Use", "LMB"); // Don't know where it gets used
    public static readonly Bindings ACTIVATE_ITEM = new("ActivateItem", "LMB");
    public static readonly Bindings SECONDARY_USE = new("ItemSecondaryUse", "Q");
    public static readonly Bindings TERTIARY_USE = new("ItemTertiaryUse", "E");
    public static readonly Bindings INSPECT_ITEM = new("InspectItem", "Z");
    public static readonly Bindings SPRINT = new("Sprint", "Shift");
    public static readonly Bindings SCAN = new("PingScan", "RMB");
    public static readonly Bindings INTERACT = new("Interact", "E");
    public static readonly Bindings BUILD = new("BuildMode", "B");
    public static readonly Bindings ROTATE = new("ReloadBatteries", "R");
    public static readonly Bindings STORE = new("Delete", "X");
}
