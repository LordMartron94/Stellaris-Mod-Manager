namespace MD.Common;

/// <summary>
/// Static extension class for miscellaneous helper functions.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Splits a string formatted as 'key:value' into a KeyValuePair.
    /// </summary>
    /// <param name="input">The string to parse, which must contain exactly one colon character.</param>
    /// <returns>A KeyValuePair constructed from the input string, where the key is the part of the string before the colon, and the value is the part of the string after the colon.</returns>
    /// <exception cref="ArgumentException">Thrown when the input string does not contain exactly one colon character.</exception>
    public static KeyValuePair<string, string> ExtractKeyValueFromColonSeparatedString(this string input)
    {
        return Utilities.ExtractKeyValueFromColonSeparatedString(input);
    }
}