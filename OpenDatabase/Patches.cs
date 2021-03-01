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
                jr.display_name = recipe.m_item.m_itemData.m_shared.m_name;
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


                string json = TinyJson.JSONWriter.ToJson(jr);
                json = JsonFormatter.Format(json);
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

                    
                    /*
                    _result.m_itemData.m_shared = new ItemDrop.ItemData.SharedData()
                    {
                        m_name = jRecipe.display_name,
                        m_description = _result.m_itemData.m_shared.m_description,
                        m_itemType = _result.m_itemData.m_shared.m_itemType,
                        m_maxStackSize = _result.m_itemData.m_shared.m_maxStackSize,
                        m_food = _result.m_itemData.m_shared.m_food,
                        m_foodStamina = _result.m_itemData.m_shared.m_foodStamina,
                        m_foodRegen = _result.m_itemData.m_shared.m_foodRegen,
                        m_foodBurnTime = _result.m_itemData.m_shared.m_foodBurnTime,
                        m_aiAttackInterval = _result.m_itemData.m_shared.m_aiAttackInterval,
                        m_aiAttackMaxAngle = _result.m_itemData.m_shared.m_aiAttackMaxAngle,
                        m_aiAttackRange = _result.m_itemData.m_shared.m_aiAttackRange,
                        m_aiAttackRangeMin = _result.m_itemData.m_shared.m_aiAttackRangeMin,
                        m_aiPrioritized = _result.m_itemData.m_shared.m_aiPrioritized,
                        m_aiTargetType = _result.m_itemData.m_shared.m_aiTargetType,
                        m_aiWhenFlying = _result.m_itemData.m_shared.m_aiWhenFlying,
                        m_aiWhenSwiming = _result.m_itemData.m_shared.m_aiWhenSwiming,
                        m_aiWhenWalking = _result.m_itemData.m_shared.m_aiWhenWalking,
                        m_ammoType = _result.m_itemData.m_shared.m_ammoType,
                        m_animationState = _result.m_itemData.m_shared.m_animationState,
                        m_armor = _result.m_itemData.m_shared.m_armor,
                        m_armorMaterial = _result.m_itemData.m_shared.m_armorMaterial,
                        m_armorPerLevel = _result.m_itemData.m_shared.m_armorPerLevel,
                        m_attachOverride = _result.m_itemData.m_shared.m_attachOverride,
                        m_attack = _result.m_itemData.m_shared.m_attack,
                        m_attackForce = _result.m_itemData.m_shared.m_attackForce,
                        m_attackStatusEffect = _result.m_itemData.m_shared.m_attackStatusEffect,
                        m_backstabBonus = _result.m_itemData.m_shared.m_backstabBonus,
                        m_blockable = _result.m_itemData.m_shared.m_blockable,
                        m_blockEffect = _result.m_itemData.m_shared.m_blockEffect,
                        m_blockPower = _result.m_itemData.m_shared.m_blockPower,
                        m_blockPowerPerLevel = _result.m_itemData.m_shared.m_blockPowerPerLevel,
                        m_buildPieces = _result.m_itemData.m_shared.m_buildPieces,
                        m_canBeReparied = _result.m_itemData.m_shared.m_canBeReparied,
                        m_centerCamera = _result.m_itemData.m_shared.m_centerCamera,
                        m_consumeStatusEffect = _result.m_itemData.m_shared.m_consumeStatusEffect,
                        m_damageModifiers = _result.m_itemData.m_shared.m_damageModifiers,
                        m_damages = _result.m_itemData.m_shared.m_damages,
                        m_damagesPerLevel = _result.m_itemData.m_shared.m_damagesPerLevel,
                        m_deflectionForce = _result.m_itemData.m_shared.m_deflectionForce,
                        m_deflectionForcePerLevel = _result.m_itemData.m_shared.m_deflectionForcePerLevel,
                        m_destroyBroken = _result.m_itemData.m_shared.m_destroyBroken,
                        m_dlc = _result.m_itemData.m_shared.m_dlc,
                        m_dodgeable = _result.m_itemData.m_shared.m_dodgeable,
                        m_durabilityDrain = _result.m_itemData.m_shared.m_durabilityDrain,
                        m_durabilityPerLevel = _result.m_itemData.m_shared.m_durabilityPerLevel,
                        m_equipDuration = _result.m_itemData.m_shared.m_equipDuration,
                        m_equipStatusEffect = _result.m_itemData.m_shared.m_equipStatusEffect,
                        m_foodColor = _result.m_itemData.m_shared.m_foodColor,
                        m_helmetHideHair = _result.m_itemData.m_shared.m_helmetHideHair,
                        m_hitEffect = _result.m_itemData.m_shared.m_hitEffect,
                        m_hitTerrainEffect = _result.m_itemData.m_shared.m_hitTerrainEffect,
                        m_holdAnimationState = _result.m_itemData.m_shared.m_holdAnimationState,
                        m_holdDurationMin = _result.m_itemData.m_shared.m_holdDurationMin,
                        m_holdStaminaDrain = _result.m_itemData.m_shared.m_holdStaminaDrain,
                        m_holdStartEffect = _result.m_itemData.m_shared.m_holdStartEffect,
                        m_icons = _result.m_itemData.m_shared.m_icons,
                        m_maxDurability = _result.m_itemData.m_shared.m_maxDurability,
                        m_maxQuality = _result.m_itemData.m_shared.m_maxQuality,
                        m_movementModifier = _result.m_itemData.m_shared.m_movementModifier,
                        m_questItem = _result.m_itemData.m_shared.m_questItem,
                        m_secondaryAttack = _result.m_itemData.m_shared.m_secondaryAttack,
                        m_setName = jRecipe.display_name,
                        m_setSize = _result.m_itemData.m_shared.m_setSize,
                        m_setStatusEffect = _result.m_itemData.m_shared.m_setStatusEffect,
                        m_skillType = _result.m_itemData.m_shared.m_skillType,
                        m_spawnOnHit = _result.m_itemData.m_shared.m_spawnOnHit,
                        m_spawnOnHitTerrain = _result.m_itemData.m_shared.m_spawnOnHitTerrain,
                        m_startEffect = _result.m_itemData.m_shared.m_startEffect,
                        m_teleportable = _result.m_itemData.m_shared.m_teleportable,
                        m_timedBlockBonus = _result.m_itemData.m_shared.m_timedBlockBonus,
                        m_toolTier = _result.m_itemData.m_shared.m_toolTier,
                        m_trailStartEffect = _result.m_itemData.m_shared.m_trailStartEffect,
                        m_triggerEffect = _result.m_itemData.m_shared.m_triggerEffect,
                        m_trophyPos = _result.m_itemData.m_shared.m_trophyPos,
                        m_useDurability = _result.m_itemData.m_shared.m_useDurability,
                        m_useDurabilityDrain = _result.m_itemData.m_shared.m_useDurabilityDrain,
                        m_value = _result.m_itemData.m_shared.m_value,
                        m_variants = _result.m_itemData.m_shared.m_variants,
                        m_weight = _result.m_itemData.m_shared.m_weight
                    };*/

                    _result.m_itemData.m_shared.m_name = jRecipe.display_name;

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
