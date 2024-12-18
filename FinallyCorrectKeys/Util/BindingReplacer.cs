namespace FinallyCorrectKeys.Util;

internal class BindingReplacer
{
    internal static string Replace(string text, string toReplace, string actionName)
    {
        return text.Replace(toReplace, "[" + ActionBindings.GetInputBindingString(actionName) + "]");
    }
}
