using Godot;

namespace UDA.Game.UI.SignalGuide;

public partial class SignalTest : Node2D
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double theDelta)
    {
    }

    private void OnButtonPressed()
    {
        GD.Print("BUTTON PRESSED!");
    }
}