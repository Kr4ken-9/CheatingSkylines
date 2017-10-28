using System.Reflection;

namespace CheatingSkylines
{
    public class BFlags
    {
        public static BindingFlags NonPublicInstance = BindingFlags.NonPublic | BindingFlags.Instance;
        
        public static BindingFlags PublicInstance = BindingFlags.Public | BindingFlags.Instance;

        public static BindingFlags NonPublicStatic = BindingFlags.NonPublic | BindingFlags.Static;

        public static BindingFlags PublicStatic = BindingFlags.Public | BindingFlags.Static;
    }
}