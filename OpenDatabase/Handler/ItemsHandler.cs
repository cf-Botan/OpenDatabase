using OpenDatabase.Utilities;
using UnityEngine;

namespace OpenDatabase.Handler
{
    public static class ItemsHandler
    {

        public static void ReloadItems()
        {

            JSONHandler.LoadItems();

            foreach(GameObject obj in ObjectDB.instance.m_items)
            {
                ItemDrop itemDrop = obj.GetComponent<ItemDrop>();
                if (itemDrop != null)
                {
                    JItemDrop jItemDrop = JSONHandler.GetJItemDropById(itemDrop.name);
                    Logger.Log($"Loaded Item {itemDrop.name}");

                    if (jItemDrop != null)
                    {
                        Helper.SetItemDropDataFromJItemData(ref itemDrop.m_itemData, jItemDrop.itemData);
                    }
                }
            }
        }
    }
}
