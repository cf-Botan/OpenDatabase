using System;

namespace OpenDatabase.JSONObjects
{
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

        [Serializable]
        public class JIngredients
        {
            public string id;
            public int amount;
            public int amountPerLevel;
            public bool recover;
        }
    }
}
