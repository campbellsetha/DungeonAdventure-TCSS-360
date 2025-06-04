namespace UDA.Model.Characters;

// Move priest, thief, and warrior classes as private inner classes here?
public abstract class HeroFactory
{
    private const string Warrior = "warrior";
    private const string Priest = "priest";
    private const string Thief = "thief";

    private HeroFactory()
    {
    }

    public static Hero CreateHero(string theClassType, string theName)
    {
        //Do we input check here too? Maybe, its worth considering 
        return theClassType.ToLower() switch
        {
            Warrior => new Warrior(theName),
            Priest => new Priest(theName),
            Thief => new Thief(theName),
            _ => throw new ArgumentException("The class type must be Warrior, Priest, or Thief")
        };
    }
}