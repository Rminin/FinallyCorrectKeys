namespace FinallyCorrectKeys.Util;

public class BindingName
{
    private readonly string _typeKeyWord;

    private BindingName(string typeKeyWord)
    {
        _typeKeyWord = typeKeyWord;
    } 

    public override string ToString()
    {
        return _typeKeyWord;
    }

    public static readonly BindingName DISCARD = new("Discard");
    public static readonly BindingName USE = new("Use"); // Don't know where it gets used
    public static readonly BindingName ACTIVATE_ITEM = new("ActivateItem");
    public static readonly BindingName SECONDARY_USE = new("ItemSecondaryUse");
    public static readonly BindingName TERTIARY_USE = new("ItemTertiaryUse");
    public static readonly BindingName INSPECT_ITEM = new("InspectItem");
    public static readonly BindingName SPRINT = new("Sprint");
    public static readonly BindingName SCAN = new("PingScan");
    public static readonly BindingName INTERACT = new("Interact");
    public static readonly BindingName BUILD = new("BuildMode");
    public static readonly BindingName ROTATE = new("ReloadBatteries");
    public static readonly BindingName STORE = new("Delete");
}
