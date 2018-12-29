using System;
using UnityEngine;

[Serializable]
public class SerializableShopItemData {

    //Abstract Shop Item
    public AbstractShopItem.Type type;
    public int id;
    public string itemName;
    public int price;
    public int[] requiredItemIds;

    //New Effect / Double Effect
    public EffectData.EffectName effectName;
    //Increase Time
    public int newTotalTime;

    public SerializableShopItemData OnSave(AbstractShopItem item) {
        type = item.type;
        id = item.id;
        itemName = item.itemName;
        price = item.price;
        requiredItemIds = item.requiredItemIds;
        if (item.GetType() == typeof(NewEffectItem)) {
            effectName = ((NewEffectItem)item).effectName;
        } else if (item.GetType() == typeof(DoubleEffectItem)) {
            effectName = ((DoubleEffectItem)item).effectName;
        } else if (item.GetType() == typeof(IncreaseTimeItem)) {
            newTotalTime = ((IncreaseTimeItem)item).newTotalTime;
        }
        return this;
    }

}
