using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using OpenDatabase.Handler;
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
        public static string itemsFolder = Path.Combine(jsonFolder, "Items");


        public static ConfigEntry<bool> modEnabled;
        public static ConfigEntry<bool> showZerosInJSON;
        private static Harmony harmony;
        private static BaseUnityPlugin baseUnityPlugin;

        public static BaseUnityPlugin instance => baseUnityPlugin;

        
        public void Awake()
        {
            baseUnityPlugin = this;

            modEnabled = Config.Bind<bool>("General", "Enabled", true, "Enable this mod");
            showZerosInJSON = Config.Bind<bool>("JSONGenerator", "ShowZerosInJSON", false, "If set to true, all int/float values that are 0 won't be hidden");
            
            if (!modEnabled.Value) return;

            JSONHandler.CheckIntegrity();
            harmony = new Harmony(PluginInfo.Guid);
            harmony.PatchAll();
        }
    }
}
