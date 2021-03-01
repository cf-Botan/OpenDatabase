using BepInEx;

using HarmonyLib;
using System.IO;
using System.Reflection;

namespace OpenDatabase
{
    [BepInPlugin(PluginInfo.Guid, PluginInfo.Name, PluginInfo.Version)]
    public class OpenDatabase : BaseUnityPlugin
    {

        public static string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        public static string jsonFolder = Path.Combine(assemblyFolder, "OpenDatabase");
        public static string recipeFolder = Path.Combine(jsonFolder, "Recipes");

        private static JSONHandler jSONHandler;
        private static Harmony harmony;
        public static JSONHandler JsonInstance(bool doLoad = true)
        {
            if (jSONHandler == null)
                jSONHandler = new JSONHandler();

            return jSONHandler;

        }
        
        public void Awake()
        {
            JsonInstance();
            harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }
    }
}
