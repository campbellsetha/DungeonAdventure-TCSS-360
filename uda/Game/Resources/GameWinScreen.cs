using Godot;
using System;

public partial class GameWinScreen : Control
{
    private Button thebutton;
    public override void _Ready()
    {
        thebutton = GetNode<Button>("Panel/Button");
        thebutton.ButtonDown += OnButtonDown;
        
    }

    private void OnButtonDown()
    {
        GetTree().ChangeSceneToFile("res://Game/UI/MainMenu.tscn");
    }
    
}
