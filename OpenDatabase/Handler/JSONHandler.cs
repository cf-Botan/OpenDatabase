using System;
using System.Collections.Generic;
using System.IO;
using TinyJson;
using OpenDatabase.Utilities;
using OpenDatabase.Utilities.Formatter;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                wasBlank = true;
            }
            else
            {
                if (!Directory.Exists(OpenDatabase.recipeFolder))
                    Directory.CreateDirectory(OpenDatabase.recipeFolder);

                if (!Directory.Exists(OpenDatabase.itemsFolder))
                    Directory.CreateDirectory(OpenDatabase.itemsFolder);

            }

            if (wasBlank)
            {
                CreateItemFiles();
                CreateRecipeFiles();
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

                jr.ingredients = new JIngredients[recipe.m_resources.Length];
                for (int i = 0; i < recipe.m_resources.Length; i++)
                {
                    jr.ingredients[i] = new JIngredients();

                    jr.ingredients[i].amount = recipe.m_resources[i].m_amount;
                    jr.ingredients[i].id = recipe.m_resources[i].m_resItem.gameObject.name;
                    jr.ingredients[i].amountPerLevel = recipe.m_resources[i].m_amountPerLevel;

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

    [Serializable]
    public class JRecipe
    {
        public string recipe_id;
        public string result_item_id;
        public int result_amount;
        

        public string CraftingStation;
        public string RepairStation;
        public int minStationLevel;

        public JIngredients[] ingredients;

        public JItemData itemData;
    }

    [Serializable]
    public class JItemDrop
    {
        public string name;
        public JItemData itemData;
    }

    [Serializable]
    public class JIngredients
    {
        public string id;
        public int amount;
        public int amountPerLevel;
    }

    [Serializable]
    public class JItemData
    {
        public string m_name;
        public string m_description;
        public float m_weight;

        public int m_maxStackSize;

        public float m_food;
        public float m_foodStamina;
        public float m_foodRegen;
        public float m_foodBurnTime;
        public string m_foodColor;

        public float m_armor;
        public float m_armorPerLevel;

        public float m_blockPower;
        public float m_blockPowerPerLevel;

        public bool m_canBeReparied;

        public JDamages m_damages;
        public JDamages m_damagesPerLevel;

        public float m_timedBlockBonus;
        public float m_deflectionForce;
        public float m_deflectionForcePerLevel;

        public bool m_destroyBroken;

        public bool m_dodgeable;

        public float m_maxDurability;
        public float m_durabilityDrain;
        public float m_durabilityPerLevel;

        public float m_equipDuration;
        public float m_holdDurationMin;
        public float m_holdStaminaDrain;


        public int m_maxQuality;
        public bool m_useDurability;
        public float m_useDurabilityDrain;

        public bool m_questItem;
        public bool m_teleportable;

        public int m_toolTier;

        public int m_value;
    }

    [Serializable]
    public class JDamages
    {
        public float m_blunt;
        public float m_chop;
        public float m_damage;
        public float m_fire;
        public float m_frost;
        public float m_lightning;
        public float m_pickaxe;
        public float m_pierce;
        public float m_poison;
        public float m_slash;
        public float m_spirit;
    }
}
