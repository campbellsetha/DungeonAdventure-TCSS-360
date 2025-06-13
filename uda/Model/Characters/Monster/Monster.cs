namespace UDA.Model.Characters.Monster;

public class Monster : DungeonCharacter
{
    private readonly double _myStunThreshold;
    private (int Min, int Max) MyHealRange { get; }

    private double MyHealChance { get; }

    private bool MyStunStatus { get; set; }

    public Monster(in string theName,
        in int theHitPoints,
        in int theAttackSpeed,
        in double theHitChance,
        in (int, int) theDamageRange,
        in double theHealChance,
        in (int, int) theHealRange,
        in double theStunThreshold) : base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
    {
        if (theStunThreshold < 0) 
            throw new ArgumentOutOfRangeException(nameof(theStunThreshold), theStunThreshold, 
                "The stun threshold is negative");
        if (theHealRange.Item1 < 0 || theHealRange.Item2 < 0)
            throw new ArgumentOutOfRangeException(nameof(theHealRange), theHealRange, 
                "The heal range is negative");
        if (theHealChance < 0)
            throw new ArgumentOutOfRangeException(nameof(theHealChance),  theHealChance, 
                "The heal chance is negative");
        _myStunThreshold = theStunThreshold;
        MyHealRange = theHealRange;
        MyHealChance = theHealChance;
    }

    private void Heal()
    {
        var rand = RandomSingleton.GetInstance();
        if (!(rand.NextDouble() > 1 - MyHealChance) || MyStunStatus) return;
        var healAmount = rand.Next(MyHealRange.Min, MyHealRange.Max + 1);
        if (healAmount + MyHitPoints > MyMaxHitPoints) healAmount = MyMaxHitPoints - MyHitPoints;
        MyHitPoints += healAmount;
    }

    public override void TakeDamage(in int theDamage)
    {
        base.TakeDamage(theDamage);
        if (MyHitPoints / (double) theDamage >= _myStunThreshold) MyStunStatus = true;
        MyHitPoints -= theDamage;
        Heal();
    }

    public override string ToString()
    {
        return base.ToString()
               + $"\nStunThreshold: {_myStunThreshold}\nHealChance: {MyHealChance}\n" +
               $"HealRange: {MyHealRange}";
    }
}