using Godot;
using System.Collections.Generic;
using MonoCustomResourceRegistry;
using UDA.Game.GameManager;

namespace UDA.inventory;

[GlobalClass]
public partial class Inventory : Resource
{
    private int _inventoryCount;
    [Export] private InventoryItem[] _items = new InventoryItem[12];
    private List<InventoryItem> _generalItems = new();
    private List<InventoryItem> _keyItems = new();
    
    public void AddToInventory(InventoryItem theItem)
    {
        var exists = _generalItems.Find(x => x.Id == theItem.Id);
        //the name of the pillars must contain a number to position them in the key item array.
        if (theItem.Name.Contains("pillar"))
        {
            _keyItems.Add(theItem);
            return;
        }
        
        if (exists == null)
        {
            _generalItems.Add(theItem);
        }
        else
        {
            exists.IncreaseItemCount();
        }
    }

    public void UsePotion(InventoryItem theItem)
    {
        var thePotion = _generalItems.Find(itemsInInventory => itemsInInventory.Id == theItem.Id);
        if (thePotion == null || thePotion.ItemCount == 0)
        {
            GD.Print("You dont have any!");
        }
        else
        {
            thePotion.DecreaseItemCount();
        }
    }

    
    
    private bool item_In_Inventory(InventoryItem theItem)
    {
        for (var i = 0; i < _items.Length - 1; i++)
            if (_items[i] == theItem)
                return true;

        return false;
    }

    private int location_of_Item(InventoryItem theItem)
    {
        var i = 0;
        while (i < _items.Length)
        {
            if (_items[i] == theItem) return i;
            i++;
        }

        return -1;
    }

    private bool is_Item_Valid(InventoryItem theItem)
    {
        return true;
    }


    private bool IsItemValid(InventoryItem theItem)
    {
        return true;
    }

    public List<InventoryItem> GetKeyItems()
    {
        return _keyItems;
    }

    public List<InventoryItem> GetGeneralItems()
    {
        return _generalItems;
    }
}

