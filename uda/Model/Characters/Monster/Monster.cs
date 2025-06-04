namespace UDA.Model.Characters.Monster;

public /*partial*/ class Monster : DungeonCharacter
{
    private readonly double _myStunThreshold;

    public Monster(
        string theName,
        int theHitPoints,
        int theAttackSpeed,
        double theHitChance,
        (int, int) theDamageRange,
        double theHealChance,
        (int, int) theHealRange,
        double theStunThreshold
    )
        : base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
    {
        HealRange = theHealRange;
        HealChance = theHealChance;
        IsStunned = false;
        _myStunThreshold = theStunThreshold;
    }

    public (int Min, int Max) HealRange { get; }

    public double HealChance { get; }

    public bool IsStunned { get; set; }

    public void Heal()
    {
        var rand = RandomSingleton.GetInstance();
        if (rand.NextDouble() > 1 - HealChance && !IsStunned)
        {
            var healAmount = rand.Next(HealRange.Min, HealRange.Max + 1);

            if (healAmount + HitPoints > MaxHitPoints) healAmount = MaxHitPoints - HitPoints;

            HitPoints += healAmount;
        }
    }

    public override void TakeDamage(int theDamage)
    {
        if (HitPoints / (double)theDamage >= _myStunThreshold) IsStunned = true;

        HitPoints -= theDamage;
        Heal();
    }

    public override string ToString()
    {
        return base.ToString()
               + $" StunThreshold:{_myStunThreshold} HealChance:{HealChance} " +
               $"HealRange:{HealRange} ";
    }
}