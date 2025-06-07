using Godot;

namespace UDA.inventory;

[GlobalClass, Tool]
public partial class Inventory_item : Resource
{
    [Export] private string _name = "";
    [Export] private Texture2D _texture;

    public int amount_Of_Items { get; private set; }

    public void set_name(string theName)
    {
        _name = theName;
    }

    public void set_texture(Texture2D theTexture)
    {
        _texture = theTexture;
    }

    public void add_Amount_Of_Item(int theAmount)
    {
        amount_Of_Items += theAmount;
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