using System;

namespace UDA.Model;

public abstract class HeroFactory
{
    private HeroFactory()
    {
    }
    public static Hero CreateHero(string theClassType, string theName)
    {
        //Do we input check here too? Maybe, its worth considering 
        return theClassType switch
        {
            "Warrior" => new Warrior(theName),
            "Priest" => new Priest(theName),
            "Thief" => new Thief(theName),
            _ => throw new ArgumentException("The class type must be Warrior, Priest, or Thief")
        };
    }
    
}