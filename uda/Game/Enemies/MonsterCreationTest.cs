using Godot;

namespace UDA.Game.Enemies;

public partial class MonsterCreationTest : Node2D
{
    //This can be configured to be placed anywhere using the editor
    [Export] private Vector2 _startPosition;

    // Called when the node enters the scene tree for the first time.
    [Export] public PackedScene MobScene;


    public override void _Ready()
    {
        //Load in a scene based on the path specified 
        MobScene = GD.Load<PackedScene>("res://Enemies/Slime.tscn");
        //Instantiate the scene to access it
        var instance = (Node2D)MobScene.Instantiate();
        instance.GlobalPosition = _startPosition;
        //Have to add the node to the scene before setting its position
        AddChild(instance);
    }
}