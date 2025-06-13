namespace UDA.Model.Characters;

public abstract class DungeonCharacter
{
    protected DungeonCharacter(in string theName,
        in int theHitPoints,
        in int theAttackSpeed,
        in double theHitChance,
        in (int, int) theDamageRange)
    {
        if (theHitPoints <= 0) 
            throw new ArgumentOutOfRangeException(nameof(theHitPoints), theHitPoints,
                "Hit points must be positive");
        if (theAttackSpeed <= 0) 
            throw new ArgumentOutOfRangeException(nameof(theAttackSpeed), theAttackSpeed,
                "Attack speed must be positive");
        if (theHitChance <= 0)
            throw new ArgumentOutOfRangeException(nameof(theHitChance), theHitChance,
                "Hit chance must be positive");
        if (theDamageRange.Item1 <= 0 || theDamageRange.Item2 <= 0)
            throw new ArgumentOutOfRangeException(nameof(theDamageRange), theDamageRange,
                "Damage range must be positive");
        MyName = theName ?? throw new ArgumentNullException(nameof(theName), "Name is null");
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

    public virtual void TakeDamage(in int theDamage)
    {
        if (theDamage < 0) 
            throw new ArgumentOutOfRangeException(nameof(theDamage), theDamage, "The damage is negative");
    }

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

    public override string ToString()
    {
        return $"Name: {MyName}\nMaxHP: {MyMaxHitPoints}\nCurrentHP: {MyHitPoints}\n" +
               $"DamageRange: {MyDamageRange}\nAttackSpeed: {MyAttackSpeed}\nHitChance: {MyHitChance}";
    }
}