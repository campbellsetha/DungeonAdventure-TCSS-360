using Godot;

namespace UDA.Game.Resources;

[GlobalClass]
[Tool]
public partial class PlayerClassInfo : Resource
{
    //Putting in default values as the constructor for Hero can fail without these
    public PlayerClassInfo()
    {
        MyPlayerName = "Greeble Jenkins";
        // use nameof() to get class names
        MyPlayerClass = nameof(Model.Characters.Warrior);
    }

    public PlayerClassInfo(string thePlayerName, string thePlayerClass)
    {
        MyPlayerName = thePlayerName;
        MyPlayerClass = thePlayerClass;
    }

    [Export] public string MyPlayerName { get; set; }
    [Export] public string MyPlayerClass { get; set; }
    [Export] public int MyPlayerHp { get; set; }
}