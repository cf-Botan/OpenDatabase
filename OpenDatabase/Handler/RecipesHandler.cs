using OpenDatabase.Utilities;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace OpenDatabase.Handler
{
    public class RecipesHandler
    {
        static Dictionary<string, CraftingStation> craftingStations;

        public static ItemDrop getItemById(string id)
        {
            GameObject gameObject = ObjectDB.instance.GetItemPrefab(id);

            if (gameObject == null) return null;
            return gameObject.GetComponent<ItemDrop>();
        }

        public static void ReloadRecipes()
        {
            JSONHandler.LoadRecipes();
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

                Logger.Log($"Found {craftingStations.Count} Crafting/Repair-Stations. [{string.Join(", ", craftingStations.Select(x => x.Key).ToArray())}]");
            }

            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;

                JRecipe jRecipe = JSONHandler.GetJRecipeById(recipe.name);
                if (jRecipe != null)
                {
                    Logger.Log($"Loaded {jRecipe.result_item_id} with {jRecipe.ingredients.Length} ingredients");

                    recipe.m_amount = jRecipe.result_amount;
                    recipe.m_resources = new Piece.Requirement[jRecipe.ingredients.Length];
                    recipe.m_minStationLevel = jRecipe.minStationLevel;

                    if (jRecipe.CraftingStation != null && jRecipe.CraftingStation != "")
                    {
                        CraftingStation station = craftingStations[jRecipe.CraftingStation];
                        if (station != null)
                            recipe.m_craftingStation = station;
                        else
                            Logger.LogError($"Couldn't find CraftingStation {jRecipe.CraftingStation}");

                    } 
                    else
                    {
                        recipe.m_craftingStation = null;
                    }

                    if (jRecipe.RepairStation != null && jRecipe.RepairStation != "")
                    {
                        CraftingStation station = craftingStations[jRecipe.RepairStation];
                        if (station != null)
                            recipe.m_repairStation = station;
                        else
                            Logger.LogError($"Couldn't find RepairStation {jRecipe.RepairStation}");
                    }
                    else
                    {
                        recipe.m_repairStation = null;
                    }

                    for (int i = 0; i < jRecipe.ingredients.Length; i++)
                    {
                        ItemDrop _drop = getItemById(jRecipe.ingredients[i].id);
                        recipe.m_resources[i] = new Piece.Requirement();
                        recipe.m_resources[i].m_amount = jRecipe.ingredients[i].amount;
                        recipe.m_resources[i].m_resItem = _drop;
                        recipe.m_resources[i].m_amountPerLevel = jRecipe.ingredients[i].amountPerLevel;
                    }
                }
            }
        }
    }
}
