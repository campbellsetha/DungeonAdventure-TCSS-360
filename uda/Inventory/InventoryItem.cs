using Godot;
using MonoCustomResourceRegistry;
namespace UDA.inventory;

[RegisteredType(nameof(InventoryItem), "", nameof(Resource))]
public partial class InventoryItem : Resource
{
    [Export] public string Name { get; set; }

    [Export] public string Id { get; set; }

    [Export] public Texture2D Texture { get; set; }

    public int ItemCount { get; private set; } = 1;
    
    public InventoryItem() {}
    public void Initialize(string theId, string theName, Texture2D theTexture)
    {
        Id = theId.ToLower();
        Name = theName.ToLower();
        Texture = theTexture;
    }

    public void IncreaseItemCount() => ItemCount++;
}
