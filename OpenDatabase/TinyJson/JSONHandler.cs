using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TinyJson;


namespace OpenDatabase
{
    public class JSONHandler
    {
        public List<JRecipe> recipes;

        
        public static bool needUpdate = false;

        public JSONHandler()
        {
            CheckIntegrity();
        }

        public static void CheckIntegrity()
        {
            if (!Directory.Exists(OpenDatabase.jsonFolder))
            {
                needUpdate = true;
                Directory.CreateDirectory(OpenDatabase.jsonFolder);
                Directory.CreateDirectory(OpenDatabase.recipeFolder);
            }
            else
            {
                if (Directory.GetFiles(OpenDatabase.recipeFolder).Length <= 0)
                    needUpdate = true;
            }
        }
        public void LoadRecipes()
        {
            CheckIntegrity();

            recipes = new List<JRecipe>();
            string[] files = Directory.GetFiles(OpenDatabase.recipeFolder);
            foreach (string f in files)
            {
                if (!f.EndsWith(".json")) continue;
                string content = File.ReadAllText(f);
                JRecipe jRecipe = content.FromJson<JRecipe>();
                recipes.Add(jRecipe);
                Logging.Log($"Loaded {jRecipe.result_item_id} with {jRecipe.ingredients.Length} ingredients");
            }
        }

        public JRecipe getById(string id)
        {
            foreach (JRecipe j in recipes)
                if (j.result_item_id == id)
                    return j;

            return null;
        }
    }

    [Serializable]
    public class JRecipe
    {
        public string result_item_id;
        public int result_amount;
        

        public string CraftingStation;
        public string RepairStation;
        public int minStationLevel;

        public JIngredients[] ingredients;

        public JItemData itemData;
    }

    [Serializable]
    public class JIngredients
    {
        public string id;
        public int amount;
    }

    [Serializable]
    public class JStation
    {
        public string name;
        public int minStationLevel;
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
