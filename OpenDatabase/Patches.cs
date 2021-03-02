using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

        static JItemData GetItemDataFromItemDrop(ItemDrop.ItemData data)
        {
            JItemData itemData;
            
            JDamages damages = new JDamages()
            {
                m_blunt = data.m_shared.m_damages.m_blunt,
                m_chop = data.m_shared.m_damages.m_chop,
                m_damage = data.m_shared.m_damages.m_damage,
                m_fire = data.m_shared.m_damages.m_fire,
                m_frost = data.m_shared.m_damages.m_frost,
                m_lightning = data.m_shared.m_damages.m_frost,
                m_pickaxe = data.m_shared.m_damages.m_pickaxe,
                m_pierce = data.m_shared.m_damages.m_pierce,
                m_poison = data.m_shared.m_damages.m_poison,
                m_slash = data.m_shared.m_damages.m_slash,
                m_spirit = data.m_shared.m_damages.m_spirit
            };
            if (damages.m_blunt == 0 && damages.m_chop == 0 && damages.m_damage == 0 && damages.m_fire == 0 && damages.m_frost == 0 && damages.m_lightning == 0 &&
                damages.m_pickaxe == 0 && damages.m_pierce == 0 && damages.m_poison == 0 && damages.m_slash == 0 && damages.m_spirit == 0
                )
                damages = null;

            JDamages damagesPerLevel = new JDamages()
            {
                m_blunt = data.m_shared.m_damagesPerLevel.m_blunt,
                m_chop = data.m_shared.m_damagesPerLevel.m_chop,
                m_damage = data.m_shared.m_damagesPerLevel.m_damage,
                m_fire = data.m_shared.m_damagesPerLevel.m_fire,
                m_frost = data.m_shared.m_damagesPerLevel.m_frost,
                m_lightning = data.m_shared.m_damagesPerLevel.m_frost,
                m_pickaxe = data.m_shared.m_damagesPerLevel.m_pickaxe,
                m_pierce = data.m_shared.m_damagesPerLevel.m_pierce,
                m_poison = data.m_shared.m_damagesPerLevel.m_poison,
                m_slash = data.m_shared.m_damagesPerLevel.m_slash,
                m_spirit = data.m_shared.m_damagesPerLevel.m_spirit
            };
            if (damagesPerLevel.m_blunt == 0 && damagesPerLevel.m_chop == 0 && damagesPerLevel.m_damage == 0 && damagesPerLevel.m_fire == 0 && damagesPerLevel.m_frost == 0 && damagesPerLevel.m_lightning == 0 &&
                damagesPerLevel.m_pickaxe == 0 && damagesPerLevel.m_pierce == 0 && damagesPerLevel.m_poison == 0 && damagesPerLevel.m_slash == 0 && damagesPerLevel.m_spirit == 0
                )
                damagesPerLevel = null;


            itemData = new JItemData()
            {
                m_armor = data.m_shared.m_armor,
                m_armorPerLevel = data.m_shared.m_armorPerLevel,
                m_blockPower = data.m_shared.m_blockPower,
                m_blockPowerPerLevel = data.m_shared.m_blockPowerPerLevel,
                m_deflectionForce = data.m_shared.m_deflectionForce,
                m_deflectionForcePerLevel = data.m_shared.m_deflectionForcePerLevel,
                m_description = data.m_shared.m_description,
                m_durabilityDrain = data.m_shared.m_durabilityDrain,
                m_durabilityPerLevel = data.m_shared.m_durabilityPerLevel,
                m_equipDuration = data.m_shared.m_equipDuration,
                m_food = data.m_shared.m_food,
                m_foodColor = GetHexFromColor(data.m_shared.m_foodColor),
                m_foodBurnTime = data.m_shared.m_foodBurnTime,
                m_foodRegen = data.m_shared.m_foodRegen,
                m_foodStamina = data.m_shared.m_foodStamina,
                m_holdDurationMin = data.m_shared.m_holdDurationMin,
                m_holdStaminaDrain = data.m_shared.m_holdStaminaDrain,
                m_maxDurability = data.m_shared.m_maxDurability,
                m_maxQuality = data.m_shared.m_maxQuality,
                m_maxStackSize = data.m_shared.m_maxStackSize,
                m_toolTier = data.m_shared.m_toolTier,
                m_useDurability = data.m_shared.m_useDurability,
                m_useDurabilityDrain = data.m_shared.m_useDurabilityDrain,
                m_value = data.m_shared.m_value,
                m_weight = data.m_shared.m_weight,
                m_destroyBroken = data.m_shared.m_destroyBroken,
                m_dodgeable = data.m_shared.m_dodgeable,
                m_canBeReparied = data.m_shared.m_canBeReparied,
                m_damages = damages,
                m_damagesPerLevel = damagesPerLevel,
                m_name = data.m_shared.m_name,
                m_questItem = data.m_shared.m_questItem,
                m_teleportable = data.m_shared.m_teleportable,
                m_timedBlockBonus = data.m_shared.m_timedBlockBonus
            };
            if (itemData.m_food == 0 && itemData.m_foodRegen == 0 && itemData.m_foodStamina == 0)
                itemData.m_foodColor = null;
            return itemData;
        }

        static void SetItemDropDataFromJItemData(ref ItemDrop.ItemData itemData, JItemData data)
        {
            if (data.m_damages != null)
            {
                HitData.DamageTypes damages = new HitData.DamageTypes();
                damages.m_blunt = data.m_damages.m_blunt;
                damages.m_chop = data.m_damages.m_chop;
                damages.m_damage = data.m_damages.m_damage;
                damages.m_frost = data.m_damages.m_frost;
                damages.m_lightning = data.m_damages.m_lightning;
                damages.m_pickaxe = data.m_damages.m_pickaxe;
                damages.m_pierce = data.m_damages.m_pierce;
                damages.m_poison = data.m_damages.m_poison;
                damages.m_slash = data.m_damages.m_slash;
                damages.m_spirit = data.m_damages.m_spirit;
                itemData.m_shared.m_damages = damages;
            }

            if (data.m_damagesPerLevel != null)
            {
                HitData.DamageTypes damagesPerLevel = new HitData.DamageTypes();
                damagesPerLevel.m_blunt = data.m_damagesPerLevel.m_blunt;
                damagesPerLevel.m_chop = data.m_damagesPerLevel.m_chop;
                damagesPerLevel.m_damage = data.m_damagesPerLevel.m_damage;
                damagesPerLevel.m_frost = data.m_damagesPerLevel.m_frost;
                damagesPerLevel.m_lightning = data.m_damagesPerLevel.m_lightning;
                damagesPerLevel.m_pickaxe = data.m_damagesPerLevel.m_pickaxe;
                damagesPerLevel.m_pierce = data.m_damagesPerLevel.m_pierce;
                damagesPerLevel.m_poison = data.m_damagesPerLevel.m_poison;
                damagesPerLevel.m_slash = data.m_damagesPerLevel.m_slash;
                damagesPerLevel.m_spirit = data.m_damagesPerLevel.m_spirit;
                itemData.m_shared.m_damagesPerLevel = damagesPerLevel;
            }


            itemData.m_shared.m_name = data.m_name;
            itemData.m_shared.m_description = data.m_description;
            itemData.m_shared.m_weight = data.m_weight;
            itemData.m_shared.m_maxStackSize = data.m_maxStackSize;
            itemData.m_shared.m_food = data.m_food;
            itemData.m_shared.m_foodStamina = data.m_foodStamina;
            itemData.m_shared.m_foodRegen = data.m_foodRegen;
            itemData.m_shared.m_foodBurnTime = data.m_foodBurnTime;

            if (data.m_foodColor != null && data.m_foodColor != "" && data.m_foodColor.StartsWith("#"))
                itemData.m_shared.m_foodColor = GetColorFromHex(data.m_foodColor);

            itemData.m_shared.m_armor = data.m_armor;
            itemData.m_shared.m_armorPerLevel = data.m_armorPerLevel;
            itemData.m_shared.m_blockPower = data.m_blockPower;
            itemData.m_shared.m_blockPowerPerLevel = data.m_blockPowerPerLevel;
            itemData.m_shared.m_canBeReparied = data.m_canBeReparied;
            itemData.m_shared.m_timedBlockBonus = data.m_timedBlockBonus;
            itemData.m_shared.m_deflectionForce = data.m_deflectionForce;
            itemData.m_shared.m_deflectionForcePerLevel = data.m_deflectionForcePerLevel;
            itemData.m_shared.m_destroyBroken = data.m_destroyBroken;
            itemData.m_shared.m_dodgeable = data.m_dodgeable;
            itemData.m_shared.m_maxDurability = data.m_maxDurability;
            itemData.m_shared.m_durabilityDrain = data.m_durabilityDrain;
            itemData.m_shared.m_durabilityPerLevel = data.m_durabilityPerLevel;
            itemData.m_shared.m_equipDuration = data.m_equipDuration;
            itemData.m_shared.m_holdDurationMin = data.m_holdDurationMin;
            itemData.m_shared.m_holdStaminaDrain = data.m_holdStaminaDrain;
            itemData.m_shared.m_maxQuality = data.m_maxQuality;
            itemData.m_shared.m_useDurability = data.m_useDurability;
            itemData.m_shared.m_useDurabilityDrain = data.m_useDurabilityDrain;
            itemData.m_shared.m_questItem = data.m_questItem;
            itemData.m_shared.m_teleportable = data.m_teleportable;
            itemData.m_shared.m_toolTier = data.m_toolTier;
            itemData.m_shared.m_value = data.m_value;
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
                    jr.itemData = GetItemDataFromItemDrop(recipe.m_item.m_itemData);
                }

                string json = TinyJson.JSONWriter.ToJson(jr);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.recipeFolder + "/" + recipe.name + ".json", json);

            }
            
            JSONHandler.needUpdate = false;
        }

        static Color GetColorFromHex(string hex)
        {
            hex = hex.TrimStart('#');
            Color c = new Color(255,0,0);
            if (hex.Length >= 6)
            {
                c.r = int.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
                c.g = int.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
                c.b = int.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);
                if (hex.Length == 8)
                    c.a = int.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            }
            return c;
        }

        static string GetHexFromColor(Color color)
        {
            return $"#{(int)(color.r * 255):X2}" +
                $"{(int)(color.g * 255):X2}" +
                $"{(int)(color.b * 255):X2}" +
                $"{(int)(color.a * 255):X2}";
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
                    SetItemDropDataFromJItemData(ref _result.m_itemData, jRecipe.itemData);

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
