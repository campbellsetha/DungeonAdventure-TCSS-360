using System;

namespace UDA.Model;

public class HeroFactory
{
    //TODO: Replace with an actual exception
    private HeroFactory()
    {
        throw new Exception();
    }

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