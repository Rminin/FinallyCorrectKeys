namespace FinallyCorrectKeys.Util;

internal class BindingReplacer
{
    internal static string Replace(string text, string toReplace, Bindings binding)
    {
        return text.Replace(toReplace, "[" + ActionBindings.GetInputBindingString(binding) + "\u200B]");
    }
}
