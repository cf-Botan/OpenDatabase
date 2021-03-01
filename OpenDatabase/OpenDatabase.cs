using BepInEx;
using BepInEx.Configuration;
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
        public static bool deepJsonCreation = true;

        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> showZerosInJSON;

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

            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            showZerosInJSON = Config.Bind<bool>("JSONGenerator", "ShowZerosInJSON", false, "If set to true, all int/float values that are 0 won't be hidden");

            if (!modEnabled.Value) return;

            JsonInstance();
            harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }
    }
}
