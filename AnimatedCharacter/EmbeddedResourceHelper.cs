using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AnimatedCharacter
{
    public static class EmbeddedResourceHelper
    {
        public static Stream ReadBinaryResource(string path)
        {
            Assembly sourceAssembly = Assembly.GetCallingAssembly();

            if (!sourceAssembly.GetManifestResourceNames().Any(str => str == path))
                throw new ArgumentException($"Specified resource doesn't exist in assembly {sourceAssembly.GetName()}.");

            Stream stream = sourceAssembly.GetManifestResourceStream(path);
            return stream;
        }
        public static string ReadTextResource(string path)
        {
            Assembly sourceAssembly = Assembly.GetCallingAssembly();

            if (!sourceAssembly.GetManifestResourceNames().Any(str => str == path))
                throw new ArgumentException($"Specified resource doesn't exist in assembly {sourceAssembly.GetName()}.");

            using (Stream stream = sourceAssembly.GetManifestResourceStream(path))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
