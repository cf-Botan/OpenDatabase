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

            if (!Directory.Exists(OpenDatabase.jsonFolder))
            {
                needUpdate = true;
                Directory.CreateDirectory(OpenDatabase.jsonFolder);
                Directory.CreateDirectory(OpenDatabase.recipeFolder);
            } else
            {
                if (Directory.GetFiles(OpenDatabase.recipeFolder).Length <= 0)
                    needUpdate = true;
            }
        }

        public void LoadRecipes()
        {
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
        public string display_name;
        public string result_item_id;
        public int result_amount;
        

        public string CraftingStation;
        public string RepairStation;
        public int minStationLevel;

        public JIngredients[] ingredients;
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
}
