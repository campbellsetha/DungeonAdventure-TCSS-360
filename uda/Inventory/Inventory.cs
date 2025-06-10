using Godot;
<<<<<<< Updated upstream
=======
using System.Collections.Generic;
>>>>>>> Stashed changes
using MonoCustomResourceRegistry;

namespace UDA.inventory;

[RegisteredType(nameof(Inventory), "", nameof(Resource))]
public partial class Inventory : Resource
{
<<<<<<< Updated upstream
    private int _inventoryCount;
    [Export] private Inventory_item[] _items = new Inventory_item[12];

    public void add_To_Inventory(Inventory_item theItem)
=======
    private List<InventoryItem> _generalItems = new();
    private List<InventoryItem> _keyItems = new();
    
    public void AddToInventory(InventoryItem theItem)
>>>>>>> Stashed changes
    {
        var exists = _generalItems.Find(x => x.Id == theItem.Id);
        //the name of the pillars must contain a number to position them in the key item array.
        if (theItem.GetName().ToLower().Contains("pillar"))
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

<<<<<<< Updated upstream
    private bool item_In_Inventory(Inventory_item theItem)
    {
        for (var i = 0; i < _items.Length - 1; i++)
            if (_items[i] == theItem)
                return true;

        return false;
    }

    private int location_of_Item(Inventory_item theItem)
    {
        var i = 0;
        while (i < _items.Length)
        {
            if (_items[i] == theItem) return i;
            i++;
        }

        return -1;
    }

    private bool is_Item_Valid(Inventory_item theItem)
    {
        return true;
    }
}
=======
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
>>>>>>> Stashed changes
