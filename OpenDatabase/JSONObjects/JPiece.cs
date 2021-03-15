

namespace OpenDatabase.JSONObjects
{
    public class JPiece
    {
        public string m_name;
        public string m_description;
        public bool m_enabled;
        public string m_category;
        public bool m_isUpgrade;
        public int m_comfort;
        public string m_comfortGroup;
        public bool m_groundPiece;
        public bool m_allowAltGroundPlacement;
        public bool m_groundOnly;
        public bool m_cultivatedGroundOnly;
        public bool m_waterPiece;
        public bool m_clipGround;
        public bool m_clipEverything;
        public bool m_noInWater;
        public bool m_notOnWood;
        public bool m_notOnTiltingSurface;
        public bool m_inCeilingOnly;
        public bool m_notOnFloor;
        public bool m_noClipping;
        public bool m_onlyInTeleportArea;
        public bool m_allowedInDungeons;
        public float m_spaceRequirement;
        public bool m_repairPiece;
        public bool m_canBeRemoved;
        public string m_onlyInBiome;
        public string m_craftingStation;

        public JRecipe.JIngredients ingredients;
    }
}
