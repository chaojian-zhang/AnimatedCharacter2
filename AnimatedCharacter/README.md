# Animated Character

For obvious reasons, Open AI API Key is not included in the source, and since we must include the key in the executable, we must add the key at compile time (it's ok if the user can see the key by decompiling the source code, we just don't want to check in the key in the source tree).

Provide a file (OpenAIKey.cs) like below during compilation (in the project folder):

```c#
namespace AnimatedCharacter
{
    internal static class OpenAIKey
    {
        public static string Prelude = string.Empty;
        public static string AdditionalContext = @"<Your Application Specific Introduction Paragraph, Can be Empty, Don't Leave it as Null>";
        public static string Key = @"<Your Open AI Key String>";
    }
}
```