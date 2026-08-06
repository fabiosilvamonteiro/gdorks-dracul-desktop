using System;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace gdracul_project
{
    /// <summary>Carrega imagens/icones embutidos no assembly (self-contained).</summary>
    internal static class Assets
    {
        public static Image Image(string name)
        {
            using (Stream s = Stream(name))
                return s == null ? null : new Bitmap(s);
        }

        public static Icon Icon(string name)
        {
            using (Stream s = Stream(name))
                return s == null ? null : new Icon(s);
        }

        private static Stream Stream(string name)
        {
            return Assembly.GetExecutingAssembly().GetManifestResourceStream(name);
        }
    }
}
