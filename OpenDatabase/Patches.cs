using HarmonyLib;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

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

        static GameObject getGameObjectById(string id)
        {
            GameObject gameObject = ObjectDB.instance.GetItemPrefab(id);

            if (gameObject == null) return null;
            return gameObject;
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
        
        static void CreateRecipesFiles()
        {
            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;
                Logging.Log($"Found Recipe '{recipe.m_item.name}'");

                JRecipe jr = new JRecipe();
                jr.itemData = new JItemData();

                jr.itemData.m_name = recipe.m_item.m_itemData.m_shared.m_name;
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
                    ItemDrop.ItemData data = recipe.m_item.m_itemData;
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
                    jr.itemData = new JItemData();

                    if (data.m_shared.m_armor > 0)
                        jr.itemData.m_armor = data.m_shared.m_armor;

                    if (data.m_shared.m_armorPerLevel > 0)
                        jr.itemData.m_armorPerLevel = data.m_shared.m_armorPerLevel;

                    if (data.m_shared.m_blockPower > 0)
                        jr.itemData.m_blockPower = data.m_shared.m_blockPower;

                    if (data.m_shared.m_blockPowerPerLevel > 0)
                        jr.itemData.m_blockPowerPerLevel = data.m_shared.m_blockPowerPerLevel;

                    if (data.m_shared.m_deflectionForce > 0)
                        jr.itemData.m_deflectionForce = data.m_shared.m_deflectionForce;

                    if (data.m_shared.m_deflectionForcePerLevel > 0)
                        jr.itemData.m_deflectionForcePerLevel = data.m_shared.m_deflectionForcePerLevel;

                    if (data.m_shared.m_description != "")
                        jr.itemData.m_description = data.m_shared.m_description;

                    if (data.m_shared.m_durabilityDrain > 0)
                        jr.itemData.m_durabilityDrain = data.m_shared.m_durabilityDrain;

                    if (data.m_shared.m_durabilityPerLevel > 0)
                        jr.itemData.m_durabilityPerLevel = data.m_shared.m_durabilityPerLevel;

                    if (data.m_shared.m_equipDuration > 0)
                        jr.itemData.m_equipDuration = data.m_shared.m_equipDuration;

                    if (data.m_shared.m_food > 0)
                        jr.itemData.m_food = data.m_shared.m_food;

                    if (data.m_shared.m_foodBurnTime > 0)
                        jr.itemData.m_foodBurnTime = data.m_shared.m_foodBurnTime;

                    if (data.m_shared.m_foodRegen > 0)
                        jr.itemData.m_foodRegen = data.m_shared.m_foodRegen;

                    if (data.m_shared.m_foodStamina > 0)
                        jr.itemData.m_foodStamina = data.m_shared.m_foodStamina;

                    if (data.m_shared.m_holdDurationMin > 0)
                        jr.itemData.m_holdDurationMin = data.m_shared.m_holdDurationMin;

                    if (data.m_shared.m_holdStaminaDrain > 0)
                        jr.itemData.m_holdStaminaDrain = data.m_shared.m_holdStaminaDrain;

                    if (data.m_shared.m_maxDurability > 0)
                        jr.itemData.m_maxDurability = data.m_shared.m_maxDurability;

                    if (data.m_shared.m_maxQuality > 0)
                        jr.itemData.m_maxQuality = data.m_shared.m_maxQuality;

                    if (data.m_shared.m_maxStackSize > 0)
                        jr.itemData.m_maxStackSize = data.m_shared.m_maxStackSize;

                    if (data.m_shared.m_toolTier > 0)
                        jr.itemData.m_toolTier = data.m_shared.m_toolTier;

                    jr.itemData.m_useDurability = data.m_shared.m_useDurability;

                    if (data.m_shared.m_useDurabilityDrain > 0)
                        jr.itemData.m_useDurabilityDrain = data.m_shared.m_useDurabilityDrain;

                    if (data.m_shared.m_value > 0)
                        jr.itemData.m_value = data.m_shared.m_value;

                    if (data.m_shared.m_weight > 0)
                        jr.itemData.m_weight = data.m_shared.m_weight;

                    jr.itemData.m_foodColor = null;
                    jr.itemData.m_destroyBroken = data.m_shared.m_destroyBroken;
                    jr.itemData.m_dodgeable = data.m_shared.m_dodgeable;
                    jr.itemData.m_canBeReparied = data.m_shared.m_canBeReparied;
                    jr.itemData.m_damages = damages;
                    jr.itemData.m_damagesPerLevel = damagesPerLevel;
                    jr.itemData.m_name = data.m_shared.m_name;
                    jr.itemData.m_questItem = data.m_shared.m_questItem;
                    jr.itemData.m_teleportable = data.m_shared.m_teleportable;
                    jr.itemData.m_timedBlockBonus = data.m_shared.m_timedBlockBonus;
                }

                string json = TinyJson.JSONWriter.ToJson(jr);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.recipeFolder + "/" + recipe.name + ".json", json);

            }
            
            JSONHandler.needUpdate = false;
        }

        static void ReloadRecipes()
        {
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

                Logging.Log($"Found {craftingStations.Count} Crafting/Repair-Stations.");
            }

            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;

                JRecipe jRecipe = OpenDatabase.JsonInstance().getById(recipe.m_item.gameObject.name);
                if (jRecipe != null)
                {
                    ItemDrop _result = getItemById(jRecipe.result_item_id);

                    if (jRecipe.itemData.m_damages != null)
                    {
                        HitData.DamageTypes damages = new HitData.DamageTypes();
                        damages.m_blunt = jRecipe.itemData.m_damages.m_blunt;
                        damages.m_chop = jRecipe.itemData.m_damages.m_chop;
                        damages.m_damage = jRecipe.itemData.m_damages.m_damage;
                        damages.m_frost = jRecipe.itemData.m_damages.m_frost;
                        damages.m_lightning = jRecipe.itemData.m_damages.m_lightning;
                        damages.m_pickaxe = jRecipe.itemData.m_damages.m_pickaxe;
                        damages.m_pierce = jRecipe.itemData.m_damages.m_pierce;
                        damages.m_poison = jRecipe.itemData.m_damages.m_poison;
                        damages.m_slash = jRecipe.itemData.m_damages.m_slash;
                        damages.m_spirit = jRecipe.itemData.m_damages.m_spirit;
                        _result.m_itemData.m_shared.m_damages = damages;
                    }

                    if (jRecipe.itemData.m_damagesPerLevel != null)
                    {
                        HitData.DamageTypes damagesPerLevel = new HitData.DamageTypes();
                        damagesPerLevel.m_blunt = jRecipe.itemData.m_damagesPerLevel.m_blunt;
                        damagesPerLevel.m_chop = jRecipe.itemData.m_damagesPerLevel.m_chop;
                        damagesPerLevel.m_damage = jRecipe.itemData.m_damagesPerLevel.m_damage;
                        damagesPerLevel.m_frost = jRecipe.itemData.m_damagesPerLevel.m_frost;
                        damagesPerLevel.m_lightning = jRecipe.itemData.m_damagesPerLevel.m_lightning;
                        damagesPerLevel.m_pickaxe = jRecipe.itemData.m_damagesPerLevel.m_pickaxe;
                        damagesPerLevel.m_pierce = jRecipe.itemData.m_damagesPerLevel.m_pierce;
                        damagesPerLevel.m_poison = jRecipe.itemData.m_damagesPerLevel.m_poison;
                        damagesPerLevel.m_slash = jRecipe.itemData.m_damagesPerLevel.m_slash;
                        damagesPerLevel.m_spirit = jRecipe.itemData.m_damagesPerLevel.m_spirit;
                        _result.m_itemData.m_shared.m_damagesPerLevel = damagesPerLevel;
                    }


                    _result.m_itemData.m_shared.m_name = jRecipe.itemData.m_name;
                    _result.m_itemData.m_shared.m_description = jRecipe.itemData.m_description;
                    _result.m_itemData.m_shared.m_weight = jRecipe.itemData.m_weight;
                    _result.m_itemData.m_shared.m_maxStackSize = jRecipe.itemData.m_maxStackSize;
                    _result.m_itemData.m_shared.m_food = jRecipe.itemData.m_food;
                    _result.m_itemData.m_shared.m_foodStamina = jRecipe.itemData.m_foodStamina;
                    _result.m_itemData.m_shared.m_foodRegen = jRecipe.itemData.m_foodRegen;
                    _result.m_itemData.m_shared.m_foodBurnTime = jRecipe.itemData.m_foodBurnTime;
                    //_result.m_itemData.m_shared.m_foodColor = ;
                    _result.m_itemData.m_shared.m_armor = jRecipe.itemData.m_armor;
                    _result.m_itemData.m_shared.m_armorPerLevel = jRecipe.itemData.m_armorPerLevel;
                    _result.m_itemData.m_shared.m_blockPower = jRecipe.itemData.m_blockPower;
                    _result.m_itemData.m_shared.m_blockPowerPerLevel = jRecipe.itemData.m_blockPowerPerLevel;
                    _result.m_itemData.m_shared.m_canBeReparied = jRecipe.itemData.m_canBeReparied;
                    _result.m_itemData.m_shared.m_timedBlockBonus = jRecipe.itemData.m_timedBlockBonus;
                    _result.m_itemData.m_shared.m_deflectionForce = jRecipe.itemData.m_deflectionForce;
                    _result.m_itemData.m_shared.m_deflectionForcePerLevel = jRecipe.itemData.m_deflectionForcePerLevel;
                    _result.m_itemData.m_shared.m_destroyBroken = jRecipe.itemData.m_destroyBroken;
                    _result.m_itemData.m_shared.m_dodgeable = jRecipe.itemData.m_dodgeable;
                    _result.m_itemData.m_shared.m_maxDurability = jRecipe.itemData.m_maxDurability;
                    _result.m_itemData.m_shared.m_durabilityDrain = jRecipe.itemData.m_durabilityDrain;
                    _result.m_itemData.m_shared.m_durabilityPerLevel = jRecipe.itemData.m_durabilityPerLevel;
                    _result.m_itemData.m_shared.m_equipDuration = jRecipe.itemData.m_equipDuration;
                    _result.m_itemData.m_shared.m_holdDurationMin = jRecipe.itemData.m_holdDurationMin;
                    _result.m_itemData.m_shared.m_holdStaminaDrain = jRecipe.itemData.m_holdStaminaDrain;
                    _result.m_itemData.m_shared.m_maxQuality = jRecipe.itemData.m_maxQuality;
                    _result.m_itemData.m_shared.m_useDurability = jRecipe.itemData.m_useDurability;
                    _result.m_itemData.m_shared.m_useDurabilityDrain = jRecipe.itemData.m_useDurabilityDrain;
                    _result.m_itemData.m_shared.m_questItem = jRecipe.itemData.m_questItem;
                    _result.m_itemData.m_shared.m_teleportable = jRecipe.itemData.m_teleportable;
                    _result.m_itemData.m_shared.m_toolTier = jRecipe.itemData.m_toolTier;
                    _result.m_itemData.m_shared.m_value = jRecipe.itemData.m_value;


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

            //Traverse.Create(ObjectDB.instance).Method("UpdateItemHashes").GetValue();
        }

    }
}
