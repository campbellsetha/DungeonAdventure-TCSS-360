using Godot;
using System;

[GlobalClass, Tool]
public partial class PlayerClassInfo : Resource
{
    [Export] public string MyPlayerName;
    [Export] public string MyPlayerClass;

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
