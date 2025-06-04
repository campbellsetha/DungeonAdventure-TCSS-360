using Godot;
using UDA.Model.Characters.Monster;
using Monster = UDA.Model.Characters.Monster.Monster;

namespace UDA.Game.Enemies;

// I moved the Godot script stuff out of the Monster.cs class and moved it to here. 
public partial class MonsterClass : Node2D
{
    private Monster _myClass;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _myClass = MonsterFactory.CreateGremlin();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double theDelta)
    {
    }
}