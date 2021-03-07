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
