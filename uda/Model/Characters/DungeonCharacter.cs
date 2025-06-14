namespace UDA.Model.Characters;

public abstract class DungeonCharacter
{
    /// <summary>
    /// Constructor for this class.
    /// </summary>
    /// <param name="theName">Character's name</param>
    /// <param name="theHitPoints">The max number of hit points this character has</param>
    /// <param name="theAttackSpeed">How fast the character can attack</param>
    /// <param name="theHitChance">How likely it is the character lands an attack</param>
    /// <param name="theDamageRange">The amount of damage the character can deal</param>
    /// <exception cref="ArgumentException">Thrown when one of the numeric values is less than or equal to 0,
    /// or when the character's name is null or empty.</exception>
    protected DungeonCharacter(in string theName,
        in int theHitPoints,
        in int theAttackSpeed,
        in double theHitChance,
        in (int, int) theDamageRange)
    {
        if (theHitPoints <= 0) 
            throw new ArgumentException("Hit points must be positive");
        if (theAttackSpeed <= 0) 
            throw new ArgumentException("Attack speed must be positive");
        if (theHitChance <= 0)
            throw new ArgumentException("Hit chance must be positive");
        if (theDamageRange.Item1 <= 0 || theDamageRange.Item2 <= 0)
            throw new ArgumentException("Damage range must be positive");
        if (string.IsNullOrEmpty(theName))
            throw new ArgumentException("Name is null or empty");
        MyName = theName;
        MyMaxHitPoints = theHitPoints;
        MyHitPoints = theHitPoints;
        MyDamageRange = theDamageRange;
        MyAttackSpeed = theAttackSpeed;
        MyHitChance = theHitChance;
    }

    private string MyName { get; }

    public int MyMaxHitPoints { get; }

    public int MyHitPoints { get; protected set; }

    public (int Min, int Max) MyDamageRange { get; }

    private int MyAttackSpeed { get; }

    private double MyHitChance { get; }

    /// <summary>
    /// Provides definition but not implementation of how damage is to be taken. Child classes override this class.
    /// </summary>
    /// <param name="theDamage">The amount of damage the character is dealt</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when theDamage is less than 0.</exception>
    public virtual void TakeDamage(in int theDamage)
    {
        if (theDamage < 0) 
            throw new ArgumentOutOfRangeException(nameof(theDamage), theDamage, "The damage is negative");
    }

    /// <summary>
    /// Method that controls the character's attack behavior.
    /// </summary>
    /// <param name="theTarget">The character to be attacked</param>
    /// <exception cref="ArgumentNullException">Thrown when the target is null.</exception>
    public int Attack(in DungeonCharacter theTarget)
    {
        if (theTarget == null) throw new ArgumentNullException(nameof(theTarget), "Target is null");
        var rand = RandomSingleton.GetInstance();
        var damageDealt = 0;
        if (MyHitPoints <= 0 || !(rand.NextDouble() > 1 - MyHitChance))
        {
            Console.WriteLine("Attack unsuccessful!");
        }
        else
        {
            Console.WriteLine("Attack successful!");
            for (var i = -1; i < MyAttackSpeed / theTarget.MyAttackSpeed; i++)
            {
                damageDealt += rand.Next(MyDamageRange.Min, MyDamageRange.Max + 1);
            }
        }
        return damageDealt;
    }

    /// <summary>
    /// Returns a string representation of this class.
    /// </summary>
    public override string ToString()
    {
        return $"Name: {MyName}\nMaxHP: {MyMaxHitPoints}\nCurrentHP: {MyHitPoints}\n" +
               $"DamageRange: {MyDamageRange}\nAttackSpeed: {MyAttackSpeed}\nHitChance: {MyHitChance}";
    }
}