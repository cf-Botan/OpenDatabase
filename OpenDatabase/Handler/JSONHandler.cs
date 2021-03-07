using System;
using System.Collections.Generic;
using System.IO;
using TinyJson;
using OpenDatabase.Utilities;
using OpenDatabase.Utilities.Formatter;
using UnityEngine;
using UnityEngine.SceneManagement;
using OpenDatabase.JSONObjects;

namespace OpenDatabase.Handler
{
    public class JSONHandler
    {
        public static List<JRecipe> recipes;
        public static List<JItemDrop> items;
        public static bool wasBlank = false;

        public static void CheckIntegrity()
        {
            if (!Directory.Exists(OpenDatabase.jsonFolder))
            {

                Directory.CreateDirectory(OpenDatabase.jsonFolder);
                Directory.CreateDirectory(OpenDatabase.recipeFolder);
                Directory.CreateDirectory(OpenDatabase.itemsFolder);
                Directory.CreateDirectory(OpenDatabase.statusEffectsFolder);
                wasBlank = true;
            }
            else
            {
                if (!Directory.Exists(OpenDatabase.recipeFolder))
                    Directory.CreateDirectory(OpenDatabase.recipeFolder);

                if (!Directory.Exists(OpenDatabase.itemsFolder))
                    Directory.CreateDirectory(OpenDatabase.itemsFolder);

                if (!Directory.Exists(OpenDatabase.statusEffectsFolder))
                    Directory.CreateDirectory(OpenDatabase.statusEffectsFolder);

            }

            if (wasBlank)
            {
                CreateItemFiles();
                CreateRecipeFiles();
                CreateStatusEffectsFiles();
            }

        }

        public static void ClearFolder(string path)
        {
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
                File.Delete(file);
        }

        public static void CreateRecipeFiles()
        {
            if (SceneManager.GetActiveScene().name != "main") return;
            wasBlank = false;
            foreach (Recipe recipe in ObjectDB.instance.m_recipes)
            {
                if (recipe.m_item == null) continue;
                Logger.Log($"Generated Recipe '{recipe.m_item.name}'");

                JRecipe jr = new JRecipe();

                jr.recipe_id = recipe.name;
                jr.result_item_id = recipe.m_item.gameObject.name;
                jr.result_amount = recipe.m_amount;

                if (recipe.m_craftingStation != null)
                    jr.CraftingStation = recipe.m_craftingStation.m_name;
                if (recipe.m_repairStation != null)
                    jr.RepairStation = recipe.m_repairStation.m_name;

                jr.minStationLevel = recipe.m_minStationLevel;

                jr.ingredients = new JRecipe.JIngredients[recipe.m_resources.Length];
                for (int i = 0; i < recipe.m_resources.Length; i++)
                {
                    jr.ingredients[i] = new JRecipe.JIngredients();

                    jr.ingredients[i].amount = recipe.m_resources[i].m_amount;
                    jr.ingredients[i].id = recipe.m_resources[i].m_resItem.gameObject.name;
                }

                
                string json = TinyJson.JSONWriter.ToJson(jr);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.recipeFolder + "/" + recipe.name + ".json", json);

            }
        }

        public static void CreateItemFiles()
        {
            if (SceneManager.GetActiveScene().name != "main") return;
            wasBlank = false;
            foreach (GameObject obj in ObjectDB.instance.m_items)
            {
                ItemDrop itemDrop = obj.GetComponent<ItemDrop>();
                if (itemDrop != null)
                {
                    JItemDrop jItemDrop = new JItemDrop();

                    Logger.Log($"Generated Item '{itemDrop.name}'");
                    JItemDrop jItemData = Helper.GetItemDataFromItemDrop(itemDrop);
                    string json = TinyJson.JSONWriter.ToJson(jItemData);
                    json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                    File.WriteAllText(OpenDatabase.itemsFolder + "/" + itemDrop.name + ".json", json);
                }
            }
        }

        public static void CreateStatusEffectsFiles()
        {
            if (SceneManager.GetActiveScene().name != "main") return;
            wasBlank = false;

            foreach(var effect in ObjectDB.instance.m_StatusEffects)
            {
                JStatusEffect jStatusEffect = new JStatusEffect();
                jStatusEffect.m_name = effect.m_name;
                jStatusEffect.m_category = effect.m_category;
                //jStatusEffect.m_flashIcon = effect.m_flashIcon;
                //jStatusEffect.m_cooldownIcon = effect.m_cooldownIcon;
                jStatusEffect.m_tooltip = effect.m_tooltip;
                jStatusEffect.m_startMessageType = (effect.m_startMessageType == MessageHud.MessageType.TopLeft) ? "TopLeft" : "Center";
                jStatusEffect.m_stopMessageType = (effect.m_stopMessageType == MessageHud.MessageType.TopLeft) ? "TopLeft" : "Center";
                jStatusEffect.m_repeatMessageType = (effect.m_repeatMessageType == MessageHud.MessageType.TopLeft) ? "TopLeft" : "Center";
                jStatusEffect.m_startMessage = effect.m_startMessage;
                jStatusEffect.m_stopMessage = effect.m_stopMessage;
                jStatusEffect.m_repeatMessage = effect.m_repeatMessage;
                jStatusEffect.m_repeatInterval = effect.m_repeatInterval;
                jStatusEffect.m_duration = effect.m_ttl;

                if (effect.m_startEffects.m_effectPrefabs.Length > 0)
                {
                    jStatusEffect.m_startEffects = new JStatusEffect.EffectData[effect.m_startEffects.m_effectPrefabs.Length];
                    for (int i = 0; i < jStatusEffect.m_startEffects.Length; i++)
                    {
                        jStatusEffect.m_startEffects[i] = new JStatusEffect.EffectData();
                        jStatusEffect.m_startEffects[i].m_name = effect.m_startEffects.m_effectPrefabs[i].m_prefab.name;
                        jStatusEffect.m_startEffects[i].m_attach = effect.m_startEffects.m_effectPrefabs[i].m_attach;
                        jStatusEffect.m_startEffects[i].m_enabled = effect.m_startEffects.m_effectPrefabs[i].m_enabled;
                        jStatusEffect.m_startEffects[i].m_inheritParentRotation = effect.m_startEffects.m_effectPrefabs[i].m_inheritParentRotation;
                        jStatusEffect.m_startEffects[i].m_inheritParentScale = effect.m_startEffects.m_effectPrefabs[i].m_inheritParentScale;
                        jStatusEffect.m_startEffects[i].m_randomRotation = effect.m_startEffects.m_effectPrefabs[i].m_randomRotation;
                        jStatusEffect.m_startEffects[i].m_scale = effect.m_startEffects.m_effectPrefabs[i].m_scale;
                    }
                }

                if (effect.m_stopEffects.m_effectPrefabs.Length > 0)
                {
                    jStatusEffect.m_stopEffects = new JStatusEffect.EffectData[effect.m_stopEffects.m_effectPrefabs.Length];
                    for (int i = 0; i < jStatusEffect.m_stopEffects.Length; i++)
                    {
                        jStatusEffect.m_stopEffects[i] = new JStatusEffect.EffectData();
                        jStatusEffect.m_stopEffects[i].m_name = effect.m_stopEffects.m_effectPrefabs[i].m_prefab.name;
                        jStatusEffect.m_stopEffects[i].m_attach = effect.m_stopEffects.m_effectPrefabs[i].m_attach;
                        jStatusEffect.m_stopEffects[i].m_enabled = effect.m_stopEffects.m_effectPrefabs[i].m_enabled;
                        jStatusEffect.m_stopEffects[i].m_inheritParentRotation = effect.m_stopEffects.m_effectPrefabs[i].m_inheritParentRotation;
                        jStatusEffect.m_stopEffects[i].m_inheritParentScale = effect.m_stopEffects.m_effectPrefabs[i].m_inheritParentScale;
                        jStatusEffect.m_stopEffects[i].m_randomRotation = effect.m_stopEffects.m_effectPrefabs[i].m_randomRotation;
                        jStatusEffect.m_stopEffects[i].m_scale = effect.m_stopEffects.m_effectPrefabs[i].m_scale;
                    }
                }

                jStatusEffect.m_cooldown = effect.m_cooldown;
                jStatusEffect.m_activationAnimation = effect.m_activationAnimation;

                if ((effect.m_attributes & StatusEffect.StatusAttribute.ColdResistance) == StatusEffect.StatusAttribute.ColdResistance)
                {
                    if (jStatusEffect.m_attributes == null)
                        jStatusEffect.m_attributes = new JStatusEffect.StatusAttribute();
                    jStatusEffect.m_attributes.ColdResistance = true;
                }
                if ((effect.m_attributes & StatusEffect.StatusAttribute.DoubleImpactDamage) == StatusEffect.StatusAttribute.DoubleImpactDamage)
                {
                    if (jStatusEffect.m_attributes == null)
                        jStatusEffect.m_attributes = new JStatusEffect.StatusAttribute();
                    jStatusEffect.m_attributes.DoubleImpactDamage = true;
                }
                if ((effect.m_attributes & StatusEffect.StatusAttribute.SailingPower) == StatusEffect.StatusAttribute.SailingPower)
                {
                    if (jStatusEffect.m_attributes == null)
                        jStatusEffect.m_attributes = new JStatusEffect.StatusAttribute();
                    jStatusEffect.m_attributes.SailingPower = true;
                }

                
                if (effect is SE_Stats)
                {
                    SE_Stats stats = effect as SE_Stats;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Stats = new JStatusEffect.JStatusInstance.JSE_Stats()
                    {
                        m_addMaxCarryWeight = stats.m_addMaxCarryWeight,
                        m_damageModifier = stats.m_damageModifier,
                        m_healthOverTime = stats.m_healthOverTime,
                        m_healthOverTimeDuration = stats.m_healthOverTimeDuration,
                        m_healthOverTimeInterval = stats.m_healthOverTimeInterval,
                        m_healthPerTick = stats.m_healthPerTick,
                        m_healthPerTickMinHealthPercentage = stats.m_healthPerTickMinHealthPercentage,
                        m_healthRegenMultiplier = stats.m_healthRegenMultiplier,
                        m_jumpStaminaUseModifier = stats.m_jumpStaminaUseModifier,
                        m_noiseModifier = stats.m_noiseModifier,
                        m_raiseSkillModifier = stats.m_raiseSkillModifier,
                        m_runStaminaDrainModifier = stats.m_runStaminaDrainModifier,
                        m_staminaDrainPerSec = stats.m_staminaDrainPerSec,
                        m_staminaOverTime = stats.m_staminaOverTime,
                        m_staminaOverTimeDuration = stats.m_staminaOverTimeDuration,
                        m_staminaRegenMultiplier = stats.m_staminaRegenMultiplier,
                        m_stealthModifier = stats.m_stealthModifier,
                        m_tickInterval = stats.m_tickInterval
                    };
                }

                string json = TinyJson.JSONWriter.ToJson(jStatusEffect);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.statusEffectsFolder + "/" + effect.name + ".json", json);
            }
        }


        public static void LoadRecipes()
        {
            CheckIntegrity();

            recipes = new List<JRecipe>();
            string[] files = Directory.GetFiles(OpenDatabase.recipeFolder);
            foreach (string f in files)
            {
                if (!f.EndsWith(".json")) continue;
                string content = File.ReadAllText(f);
                JRecipe jRecipe = content.FromJson<JRecipe>();

                jRecipe.recipe_id = Path.GetFileNameWithoutExtension(f);
                recipes.Add(jRecipe);
                
            }
        }

        public static void LoadItems()
        {
            CheckIntegrity();

            items = new List<JItemDrop>();
            string[] files = Directory.GetFiles(OpenDatabase.itemsFolder);
            foreach (string f in files)
            {
                if (!f.EndsWith(".json")) continue;
                string content = File.ReadAllText(f);
                JItemDrop jItemData = content.FromJson<JItemDrop>();
                items.Add(jItemData);
                
            }
        }

        public static JRecipe GetJRecipeById(string id)
        {
            foreach (JRecipe j in recipes)
                if (j.recipe_id == id)
                    return j;

            return null;
        }

        public static JItemDrop GetJItemDropById(string id)
        {
            foreach (JItemDrop drop in items)
                if (drop.name == id)
                    return drop;

            return null;
        }
    }
}
