using Godot;
using System;
namespace UDA.inventory;

public partial class Inventory_item : Resource
{
    [Export] private string _name = "";
    [Export] private Texture2D _texture;
    private int _itemCount;
    
    public int amount_Of_Items => _itemCount;
    public void set_name(string theName) =>  _name = theName;
    public void set_texture(Texture2D theTexture) =>  _texture = theTexture;
    
    public void add_Amount_Of_Item(int theAmount)
    {
        _itemCount += theAmount;
    }

    public string get_name()
    {
        return _name;
    }

    public Texture2D get_texture()
    {
        return _texture;
    }
}
