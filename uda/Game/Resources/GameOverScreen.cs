using Godot;
using System;

public partial class GameOverScreen : Control
{
    private AnimationPlayer theEpicTale;
    private Button TheGetMeOutButton;
    private const int ButtonLimit = 10;
    private int TimesButtonPressed = 0;
    public override void _Ready()
    {
        TheGetMeOutButton = GetNode<Button>("Panel/Button");
        TheGetMeOutButton.ButtonDown += OnButtonDown;
        theEpicTale = GetNode<AnimationPlayer>("AnimationPlayer");
        LightTheBeacons();
        SingTheEpic();
    }

    public void LightTheBeacons()
    {
        Panel theBigOne = GetNode<Panel>("Panel");
        var theFlames = theBigOne.GetChildren();
        foreach (Node sprites in theFlames)
        {
            if (sprites is AnimatedSprite2D fireball)
            {
                fireball.Play("default");
            }
        }
    }
    
    public void SingTheEpic()
    {
        theEpicTale.Play("TextScroll");   
    }

    public void OnButtonDown()
    {
        TimesButtonPressed++;
        TheGetMeOutButton.Size += new Vector2(10,10);
        if (TimesButtonPressed == ButtonLimit)
        {
            GetMeOuttaHere();
        }
    }
    public void GetMeOuttaHere()
    {
        GetTree().ChangeSceneToFile("res://Game/UI/MainMenu.tscn");
    }
}
