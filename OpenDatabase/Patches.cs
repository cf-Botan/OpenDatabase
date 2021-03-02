using HarmonyLib;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using OpenDatabase.Utilities;
using OpenDatabase.Utilities.Formatter;

namespace OpenDatabase
{
    [HarmonyPatch]
    class Patches
    {
        static Dictionary<string, CraftingStation> craftingStations;

        static ItemDrop getItemById(string id)
        {
            GameObject gameObject = ObjectDB.instance.GetItemPrefab(id);

            if (gameObject == null) return null;
            return gameObject.GetComponent<ItemDrop>();
        }

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
            ReloadRecipes();

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        static void PatchDB()
        {

            if (SceneManager.GetActiveScene().name != "main") return;
            ReloadRecipes();
            
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Console), "InputText")]
        static void ConsoleTextInput()
        {
            string command = Console.instance.m_input.text.ToLower();
            if (command.StartsWith("help"))
            {
                Console.instance.AddString("");
                Console.instance.AddString("[OpenDatabase]");
                Console.instance.AddString("opendatabase.reload - Reloads the database");
            }
            if (command == "opendatabase.reload")
            {
                if (SceneManager.GetActiveScene().name != "main")
                {
                    Console.instance.AddString("You need to be in a world to use this command!");
                    return;
                }
                Console.instance.AddString("Reloading database...");
                ReloadRecipes();
                Console.instance.AddString("Database has been reloaded!");
            }
        }

        static void CreateRecipesFiles()
        {
            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;
                Logging.Log($"Found Recipe '{recipe.m_item.name}'");

                JRecipe jr = new JRecipe();
                jr.itemData = new JItemData();

                jr.result_item_id = recipe.m_item.gameObject.name;
                jr.result_amount = recipe.m_amount;

                if (recipe.m_craftingStation != null)
                    jr.CraftingStation = recipe.m_craftingStation.m_name;
                if (recipe.m_repairStation != null)
                    jr.RepairStation = recipe.m_repairStation.m_name;

                jr.minStationLevel = recipe.m_minStationLevel;

                jr.ingredients = new JIngredients[recipe.m_resources.Length];
                for (int i = 0; i < recipe.m_resources.Length; i++)
                {
                    jr.ingredients[i] = new JIngredients();

                    jr.ingredients[i].amount = recipe.m_resources[i].m_amount;
                    jr.ingredients[i].id = recipe.m_resources[i].m_resItem.gameObject.name;
                }

                if (OpenDatabase.deepJsonCreation)
                {
                    jr.itemData = Helper.GetItemDataFromItemDrop(recipe.m_item.m_itemData);
                }

                string json = TinyJson.JSONWriter.ToJson(jr);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.recipeFolder + "/" + recipe.name + ".json", json);

            }
            
            JSONHandler.needUpdate = false;
        }


        static void ReloadRecipes()
        {
            JSONHandler.CheckIntegrity();
            if (JSONHandler.needUpdate)
            {
                CreateRecipesFiles();
            }

            OpenDatabase.JsonInstance().LoadRecipes();
            if (craftingStations == null)
            {
                craftingStations = new Dictionary<string, CraftingStation>();
                foreach (Recipe recipe in ObjectDB.instance.m_recipes)
                {
                    if (recipe.m_craftingStation != null && !craftingStations.ContainsKey(recipe.m_craftingStation.m_name))
                        craftingStations.Add(recipe.m_craftingStation.m_name, recipe.m_craftingStation);

                    if (recipe.m_repairStation != null && !craftingStations.ContainsKey(recipe.m_repairStation.m_name))
                        craftingStations.Add(recipe.m_repairStation.m_name, recipe.m_repairStation);
                }

                Logging.Log($"Found {craftingStations.Count} Crafting/Repair-Stations. [{string.Join(", ", craftingStations.Select(x => x.Key).ToArray())}]");
            }

            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;

                JRecipe jRecipe = OpenDatabase.JsonInstance().getById(recipe.m_item.gameObject.name);
                if (jRecipe != null)
                {
                    ItemDrop _result = getItemById(jRecipe.result_item_id);
                    Helper.SetItemDropDataFromJItemData(ref _result.m_itemData, jRecipe.itemData);

                    recipe.m_amount = jRecipe.result_amount;
                    recipe.m_item = _result;
                    recipe.m_resources = new Piece.Requirement[jRecipe.ingredients.Length];
                    recipe.m_minStationLevel = jRecipe.minStationLevel;

                    if (jRecipe.CraftingStation != null && jRecipe.CraftingStation != "")
                    {
                        CraftingStation station = craftingStations[jRecipe.CraftingStation];
                        if (station != null)
                            recipe.m_craftingStation = station;
                        else
                            Logging.LogError($"Couldn't find CraftingStation {jRecipe.CraftingStation}");
                    }

                    if (jRecipe.RepairStation != null && jRecipe.RepairStation != "")
                    {
                        CraftingStation station = craftingStations[jRecipe.RepairStation];
                        if (station != null)
                            recipe.m_repairStation = station;
                        else
                            Logging.LogError($"Couldn't find RepairStation {jRecipe.RepairStation}");
                    }

                    for (int i = 0; i < jRecipe.ingredients.Length; i++)
                    {
                        ItemDrop _drop = getItemById(jRecipe.ingredients[i].id);
                        recipe.m_resources[i] = new Piece.Requirement();
                        recipe.m_resources[i].m_amount = jRecipe.ingredients[i].amount;
                        recipe.m_resources[i].m_resItem = _drop;
                    }
                }
            }
        }

    }
}
