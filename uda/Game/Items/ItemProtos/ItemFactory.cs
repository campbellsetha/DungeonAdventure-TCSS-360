using Godot;
using System;
using System.Collections.Generic;
using UDA.inventory;
using UDA.Model.Map;

namespace UDA.Model.Items;
public class ItemFactory
{
    private static Dictionary<string, InventoryItem> _itemTemplates = new();

    public static void RegisterItem(string theId, string theName, string theTexturePath)
    {
        var texture = GD.Load<Texture2D>(theTexturePath);
        var item = new InventoryItem();
        item.Initialize(theId, theName, texture);
        _itemTemplates[theId] = item;
    }

    public static InventoryItem CreateNewItem(string theId)
    {
        if (!_itemTemplates.ContainsKey(theId)) return null;
        var template = _itemTemplates[theId];
        var item = new InventoryItem();
        item.Initialize(template.Id, template.Name, template.Texture);
        return item;
    }

    public static List<InventoryItem> GetItemsFromRoom(UDA.Model.Map.Room theRoom)
    {
        var items = new List<InventoryItem>();

        if (theRoom.ContainsHealingPotion)
        {
            var item = CreateNewItem("heal_potion");
            if (item != null) items.Add(item);
        }
        
        if (theRoom.ContainsVisionPotion)
        {
            var item = CreateNewItem("vision_potion");
            if (item != null) items.Add(item);
        }

        if (theRoom.MyRoomType.ToString().Contains("Pillar"))
        {
            InventoryItem item;
            switch (theRoom.MyRoomType)
            {
                case(RoomType.PillarA):
                    item = CreateNewItem("abstraction_pillar");
                    if (item != null) items.Add(item);
                    break;
                case(RoomType.PillarE):
                    item = CreateNewItem("encapsulation_pillar");
                    if (item != null) items.Add(item);
                    break;
                case(RoomType.PillarI):
                    item = CreateNewItem("inheritance_pillar");
                    if (item != null) items.Add(item);
                    break;
                case(RoomType.PillarP):
                    item = CreateNewItem("polymorphism_pillar");
                    if (item != null) items.Add(item);
                    break;
            }
        }
        return items;
    }
}
