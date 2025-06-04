using Godot;

public partial class StartMenu : Control
{
    public override void _Ready()
    {
        var startButton = GetNode<Button>("VBoxContainer/NewGameButton");
        var loadButton = GetNode<Button>("VBoxContainer/LoadGameButton");
        var optionButton = GetNode<Button>("VBoxContainer/OptionsButton");
        startButton.ButtonDown += OnNewGameButtonPressed;
        loadButton.ButtonDown += OnLoadButtonPressed;
        optionButton.ButtonDown += OnOptionButtonPressed;
    }

    //TODO: Configure these to load and run these specific scenes/containers
    private void OnNewGameButtonPressed()
    {
        GD.Print("Start Button works");
    }

    private void OnLoadButtonPressed()
    {
        GD.Print("Load Button works");
    }

    //Probably just want to put options into an option pane
    //Maybe even just replace this with an info tab about us and the game
    private void OnOptionButtonPressed()
    {
        GD.Print("Option Button works");
    }
}