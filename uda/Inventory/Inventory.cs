using Godot;
using System;
using MonoCustomResourceRegistry;

namespace UDA.inventory;

[RegisteredType(nameof(Inventory), "", nameof(Resource))]
public partial class Inventory : Resource
{
    [Export] private Inventory_item[] _items = new Inventory_item[12];
    private int _inventoryCount;
    
    public void add_To_Inventory(Inventory_item theItem)
    {
        if (!item_In_Inventory(theItem))
        {
            _items[_inventoryCount] = theItem;
            _inventoryCount++;
        }
        else
        {
            _items[location_of_Item(theItem)].add_Amount_Of_Item(1);
        }
    }

    private bool item_In_Inventory(Inventory_item theItem)
    {
        for (var i = 0; i < _items.Length - 1; i++)
        {
            if (_items[i] == theItem)
            {
                return true;
            }
        }
        return false;
    }

    private int location_of_Item(Inventory_item theItem)
    {
        var i = 0;
        while (i < _items.Length)
        {
            if (_items[i] == theItem) 
            {
                return i;
            }
            i++;
        }
        return -1;
    }

    private bool is_Item_Valid(Inventory_item theItem)
    {
        return true;
    }
}
