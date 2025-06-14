using Godot;

namespace UDA.Game.UI.MainMenu;

[GlobalClass]
public partial class CharacterClasses : Resource
{
    [Export] public string ClassName { get; set; } = "Unnamed";
    [Export] public SpriteFrames SpriteFrames { get; set; }
}
