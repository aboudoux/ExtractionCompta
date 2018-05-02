using System.IO;
using System.Reflection;

namespace ExtractionCompta.Tests
{
    public static class TestDirectory 
    {
        private static readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static string RessourcesDirectory { get; } = Path.Combine(CurrentDirectory.Replace(@"\bin\Debug", "").Replace(@"\bin\Release", ""), "Resources");

      
    }
}