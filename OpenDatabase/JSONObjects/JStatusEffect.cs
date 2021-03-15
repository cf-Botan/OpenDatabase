using System;

namespace OpenDatabase.JSONObjects
{
    [Serializable]
    public class JStatusEffect
    {
        public string m_name;
        public string m_category;

        //public bool m_flashIcon;
        //public bool m_cooldownIcon;

        public string m_tooltip;

        public string m_startMessageType; // 1=TopLeft, 2=Center
        public string m_stopMessageType; // 1=TopLeft, 2=Center
        public string m_repeatMessageType; // 1=TopLeft, 2=Center

        public string m_startMessage;
        public string m_stopMessage;
        public string m_repeatMessage;

        public float m_repeatInterval;
        public float m_duration;

        public float m_cooldown;
        public string m_activationAnimation;

        public JStatusInstance instance_of;

        public StatusAttribute m_attributes;
        public EffectData[] m_startEffects;
        public EffectData[] m_stopEffects;


        [Serializable]
        public class JSprite
        {

        }

        [Serializable]
        public class JStatusInstance
        {
            public JSE_Stats SE_Stats;
            public JSE_Burning SE_Burning;
            public JSE_Cozy SE_Cozy;
            public JSE_Finder SE_Finder;
            public JSE_Frost SE_Frost;
            public JSE_Harpooned SE_Harpooned;
            public JSE_HealthUpgrade SE_HealthUpgrade; 
            public JSE_Poison SE_Poison;
            public JSE_Rested SE_Rested;
            public JSE_Shield SE_Shield;
            public JSE_Smoke SE_Smoke;
            public JSE_Spawn SE_Spawn;
            public JSE_Wet SE_Wet;

            [Serializable]
            public class JSE_Stats
            {
                public float m_tickInterval;
                public float m_healthPerTickMinHealthPercentage;
                public float m_healthPerTick;
                public float m_healthOverTime;
                public float m_healthOverTimeDuration;
                public float m_healthOverTimeInterval;
                public float m_staminaOverTime;
                public float m_staminaOverTimeDuration;
                public float m_staminaDrainPerSec;
                public float m_runStaminaDrainModifier;
                public float m_jumpStaminaUseModifier;
                public float m_healthRegenMultiplier;
                public float m_staminaRegenMultiplier;
                public float m_raiseSkillModifier;
                public float m_damageModifier;
                public float m_noiseModifier;
                public float m_stealthModifier;
                public float m_addMaxCarryWeight;
            }

            [Serializable]
            public class JSE_Burning
            {
                public float m_damageInterval;
            }

            [Serializable]
            public class JSE_Cozy
            {
                public float m_delay;
                public string m_statusEffect;
            }

            [Serializable]
            public class JSE_Finder
            {
                public float m_closerTriggerDistance;
                public float m_furtherTriggerDistance;
                public float m_closeFrequency;
                public float m_distantFrequency;
            }

            [Serializable]
            public class JSE_Frost
            {
                public float m_freezeTimeEnemy;
                public float m_freezeTimePlayer;
                public float m_minSpeedFactor;
            }

            [Serializable]
            public class JSE_Harpooned
            {
                public float m_minForce;
                public float m_maxForce;
                public float m_minDistance;
                public float m_maxDistance;
                public float m_staminaDrain;
                public float m_staminaDrainInterval;
                public float m_maxMass;
            }

            [Serializable]
            public class JSE_HealthUpgrade
            {
                public float m_moreHealth;
                public float m_moreStamina;
            }

            [Serializable]
            public class JSE_Poison
            {
                public float m_damageInterval;
                public float m_baseTTL;
                public float m_TTLPerDamagePlayer;
                public float m_TTLPerDamage;
                public float m_TTLPower;
            }

            [Serializable]
            public class JSE_Rested
            {
                public float m_baseTTL;
                public float m_TTLPerComfortLevel;
            }

            [Serializable]
            public class JSE_Shield
            {
                public float m_absorbDamage;
            }

            [Serializable]
            public class JSE_Smoke
            {
                public float m_damageInterval;
            }

            [Serializable]
            public class JSE_Spawn
            {
                public float m_delay;
            }

            [Serializable]
            public class JSE_Wet
            {
                public float m_waterDamage;
                public float m_damageInterval;
            }
        }

        [Serializable]
        public class StatusAttribute
        {
            public bool ColdResistance; //-> 1 
            public bool DoubleImpactDamage; //-> 2 
            public bool SailingPower; //-> 4 
        }

        [Serializable]
        public class EffectData
        {
            public string m_name;
            public bool m_enabled;
            public bool m_attach;
            public bool m_inheritParentRotation;
            public bool m_inheritParentScale;
            public bool m_randomRotation;
            public bool m_scale;
        }
    }
}
