using System;


namespace OpenDatabase.JSONObjects
{
    [Serializable]
    public class JItemDrop
    {
        public string name;
        public JItemData itemData;

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

            public string m_setStatusEffect;
            public string m_equipStatusEffect;

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
    }
}
