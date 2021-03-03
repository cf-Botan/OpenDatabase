using HarmonyLib;
using UnityEngine.SceneManagement;
using OpenDatabase.Handler;

namespace OpenDatabase
{
    [HarmonyPatch]
    class Patches
    {

        static bool askedForYesNo = false;
        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Player), "Load")]
        static void UpdateKnown()
        {
            if (SceneManager.GetActiveScene().name != "main") return;

            Player.m_localPlayer.UpdateKnownRecipesList();
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
        static void patchOtherDB()
        {

            if (SceneManager.GetActiveScene().name != "main") return;

            ItemsHandler.ReloadItems();
            RecipesHandler.ReloadRecipes();

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        static void PatchDB()
        {

            if (SceneManager.GetActiveScene().name != "main") return;

            ItemsHandler.ReloadItems();
            RecipesHandler.ReloadRecipes();
            
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Console), "InputText")]
        static void ConsoleTextInput()
        {
            string command = Console.instance.m_input.text.ToLower();
            string _command = command;
            if (askedForYesNo) command = Console.instance.m_lastEntry;
            if (command.StartsWith("help"))
            {
                Console.instance.AddString("");
                Console.instance.AddString("[OpenDatabase]");
                Console.instance.AddString("opendatabase.generate all/items/recipes");
                Console.instance.AddString("opendatabase.reload - Reloads the database");
            }

            if (command.Contains("opendatabase"))
            {
                if (SceneManager.GetActiveScene().name != "main")
                {
                    Console.instance.AddString("You need to be in a world to use this command!");
                    return;
                }
            }

            if (command == "opendatabase.reload")
            {
                Console.instance.AddString("Reloading database...");

                ItemsHandler.ReloadItems();
                RecipesHandler.ReloadRecipes();

                if (Player.m_localPlayer != null)
                    Player.m_localPlayer.UpdateKnownRecipesList();

                Console.instance.AddString("Database has been reloaded!");
            } 
            else if (command.StartsWith("opendatabase.generate"))
            {
                string[] cmd = command.Split(' ');
                if (cmd.Length == 2)
                {
                    if (cmd[1] == "all")
                    {
                        if (!askedForYesNo)
                        {
                            Console.instance.AddString("All files inside plugins/OpenDatabase/ will be removed and regenerated. Are you sure ? yes/no");
                            askedForYesNo = true;
                        } 
                        else
                        {
                            if (_command == "yes")
                            {
                                JSONHandler.ClearFolder(OpenDatabase.recipeFolder);
                                JSONHandler.ClearFolder(OpenDatabase.itemsFolder);
                                JSONHandler.CreateItemFiles();
                                JSONHandler.CreateRecipeFiles();
                            }
                            askedForYesNo = false;
                        }
                    }
                    else if (cmd[1] == "items")
                    {
                        if (!askedForYesNo)
                        {
                            Console.instance.AddString("All files inside plugins/OpenDatabase/Items will be removed and regenerated. Are you sure ? yes/no");
                            askedForYesNo = true;
                        }
                        else
                        {
                            if (_command == "yes")
                            {
                                JSONHandler.ClearFolder(OpenDatabase.itemsFolder);
                                JSONHandler.CreateItemFiles();
                            }
                            askedForYesNo = false;
                        }
                    }
                    else if (cmd[1] == "recipes")
                    {
                        if (!askedForYesNo)
                        {
                            Console.instance.AddString("All files inside plugins/OpenDatabase/Recipes will be removed and regenerated. Are you sure ? yes/no");
                            askedForYesNo = true;
                        }
                        else
                        {
                            if (_command == "yes")
                            {
                                JSONHandler.ClearFolder(OpenDatabase.recipeFolder);
                                JSONHandler.CreateRecipeFiles();
                            }
                            askedForYesNo = false;
                        }
                    }
                }
            }
        }
  

    }
}
