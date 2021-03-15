using System;
using System.Collections.Generic;
using System.IO;
using TinyJson;
using OpenDatabase.Utilities;
using OpenDatabase.Utilities.Formatter;
using UnityEngine;
using UnityEngine.SceneManagement;
using OpenDatabase.JSONObjects;
using System.Linq;

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
                Directory.CreateDirectory(OpenDatabase.piecesFolder);
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

                if (!Directory.Exists(OpenDatabase.piecesFolder))
                    Directory.CreateDirectory(OpenDatabase.piecesFolder);

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

                    jr.ingredients[i].amountPerLevel = recipe.m_resources[i].m_amountPerLevel;
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
                Piece piece = obj.GetComponent<Piece>();

                if (itemDrop != null)
                {
                    
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
                //jStatusEffect.m_activationAnimation = effect.m_activationAnimation;

                if (effect.m_attributes.HasFlag(StatusEffect.StatusAttribute.ColdResistance))
                {
                    if (jStatusEffect.m_attributes == null)
                        jStatusEffect.m_attributes = new JStatusEffect.StatusAttribute();
                    jStatusEffect.m_attributes.ColdResistance = true;
                }
                if (effect.m_attributes.HasFlag(StatusEffect.StatusAttribute.DoubleImpactDamage))
                {
                    if (jStatusEffect.m_attributes == null)
                        jStatusEffect.m_attributes = new JStatusEffect.StatusAttribute();
                    jStatusEffect.m_attributes.DoubleImpactDamage = true;
                }
                if (effect.m_attributes.HasFlag(StatusEffect.StatusAttribute.SailingPower))
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
                else if (effect is SE_Burning)
                {
                    SE_Burning burning = effect as SE_Burning;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Burning = new JStatusEffect.JStatusInstance.JSE_Burning()
                    {
                        m_damageInterval = burning.m_damageInterval
                    };
                }
                else if (effect is SE_Cozy)
                {
                    SE_Cozy cozy = effect as SE_Cozy;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Cozy = new JStatusEffect.JStatusInstance.JSE_Cozy()
                    {
                        m_delay = cozy.m_delay,
                        m_statusEffect = cozy.m_statusEffect
                    };
                }
                else if (effect is SE_Finder)
                {
                    SE_Finder finder = effect as SE_Finder;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Finder = new JStatusEffect.JStatusInstance.JSE_Finder()
                    {
                        m_closeFrequency = finder.m_closeFrequency,
                        m_closerTriggerDistance = finder.m_closerTriggerDistance,
                        m_distantFrequency = finder.m_distantFrequency,
                        m_furtherTriggerDistance = finder.m_furtherTriggerDistance
                    };
                }
                else if (effect is SE_Frost)
                {
                    SE_Frost frost = effect as SE_Frost;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Frost = new JStatusEffect.JStatusInstance.JSE_Frost()
                    {
                        m_freezeTimeEnemy = frost.m_freezeTimeEnemy,
                        m_freezeTimePlayer = frost.m_freezeTimePlayer,
                        m_minSpeedFactor = frost.m_minSpeedFactor
                    };
                }
                else if (effect is SE_Harpooned)
                {
                    SE_Harpooned harpooned = effect as SE_Harpooned;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Harpooned = new JStatusEffect.JStatusInstance.JSE_Harpooned()
                    {
                        m_maxDistance = harpooned.m_maxDistance,
                        m_maxForce = harpooned.m_maxForce,
                        m_maxMass = harpooned.m_maxMass,
                        m_minDistance = harpooned.m_minDistance,
                        m_minForce = harpooned.m_minForce,
                        m_staminaDrain = harpooned.m_staminaDrain,
                        m_staminaDrainInterval = harpooned.m_staminaDrainInterval
                    };
                }
                else if (effect is SE_HealthUpgrade)
                {
                    SE_HealthUpgrade health = effect as SE_HealthUpgrade;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_HealthUpgrade = new JStatusEffect.JStatusInstance.JSE_HealthUpgrade()
                    {
                        m_moreHealth = health.m_moreHealth,
                        m_moreStamina = health.m_moreStamina
                    };
                }
                else if (effect is SE_Poison)
                {
                    SE_Poison poison = effect as SE_Poison;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Poison = new JStatusEffect.JStatusInstance.JSE_Poison()
                    {
                        m_baseTTL = poison.m_baseTTL,
                        m_damageInterval = poison.m_damageInterval,
                        m_TTLPerDamage = poison.m_TTLPerDamage,
                        m_TTLPerDamagePlayer = poison.m_TTLPerDamagePlayer,
                        m_TTLPower = poison.m_TTLPower
                    };
                }
                else if (effect is SE_Rested)
                {
                    SE_Rested rested = effect as SE_Rested;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Rested = new JStatusEffect.JStatusInstance.JSE_Rested()
                    {
                        m_baseTTL = rested.m_baseTTL,
                        m_TTLPerComfortLevel = rested.m_TTLPerComfortLevel
                    };
                }
                else if (effect is SE_Shield)
                {
                    SE_Shield shield = effect as SE_Shield;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Shield = new JStatusEffect.JStatusInstance.JSE_Shield()
                    {
                        m_absorbDamage = shield.m_absorbDamage
                    };
                }
                else if (effect is SE_Smoke)
                {
                    SE_Smoke smoke = effect as SE_Smoke;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Smoke = new JStatusEffect.JStatusInstance.JSE_Smoke()
                    {
                        m_damageInterval = smoke.m_damageInterval
                    };
                }
                else if (effect is SE_Spawn)
                {
                    SE_Spawn spawn = effect as SE_Spawn;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Spawn = new JStatusEffect.JStatusInstance.JSE_Spawn()
                    {
                        m_delay = spawn.m_delay
                    };
                }
                else if (effect is SE_Wet)
                {
                    SE_Wet wet = effect as SE_Wet;
                    jStatusEffect.instance_of = new JStatusEffect.JStatusInstance();
                    jStatusEffect.instance_of.SE_Wet = new JStatusEffect.JStatusInstance.JSE_Wet()
                    {
                        m_damageInterval = wet.m_damageInterval,
                        m_waterDamage = wet.m_waterDamage
                    };
                }

                string json = TinyJson.JSONWriter.ToJson(jStatusEffect);
                json = JsonFormatter.Format(json, !OpenDatabase.showZerosInJSON.Value);
                File.WriteAllText(OpenDatabase.statusEffectsFolder + "/" + effect.name + ".json", json);
            }
        }

        public static void CreatePieceFiles()
        {
            Piece[] pieces = Resources.FindObjectsOfTypeAll(typeof(Piece)) as Piece[];
            foreach(Piece piece in pieces)
            {
                Array c_list = Enum.GetValues(typeof(Piece.PieceCategory))
                    .Cast<Piece.PieceCategory>()
                    .Where(m => piece.m_category.HasFlag(m)).ToArray();

                JPiece jPiece = new JPiece()
                {
                    m_allowAltGroundPlacement = piece.m_allowAltGroundPlacement,
                    m_allowedInDungeons = piece.m_allowedInDungeons,
                    m_canBeRemoved = piece.m_canBeRemoved,
                    m_category = string.Join(",", c_list)
                };
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
