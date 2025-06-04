using Godot;

namespace UDA.Game.Resources;

[GlobalClass, Tool]
public partial class PlayerClassInfo : Resource
{
    [Export] public string MyPlayerName { get; set; }
    [Export] public string MyPlayerClass { get; set; }

    public PlayerClassInfo()
    {
        MyPlayerName = "Greeble Jenkins";
        MyPlayerClass = "Warrior";
    }

    public PlayerClassInfo(string thePlayerName, string thePlayerClass)
    {
        MyPlayerName = thePlayerName;
        MyPlayerClass = thePlayerClass;
    }
}