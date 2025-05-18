using System;

namespace UDA.Model;

public abstract class HeroFactory
{
    private HeroFactory() : base() { }

    public static Warrior CreateWarrior(String theName)
    {
        return new Warrior(theName);
    }

    public static Priest CreatePriest(String theName)
    {
        return new Priest(theName);
    }

    public static Thief CreateThief(String theName)
    {
        return new Thief(theName);
    }
    
}