namespace UDA.Model;


public abstract class Hero : DungeonCharacter
{

    // Should probably have a countdown timer for how often the special skill can be used 
    
    protected Hero(
        ref readonly string theName, 
        ref readonly int theHitPoints, 
        ref readonly int theAttackSpeed,
        ref readonly double theHitChance, 
        ref readonly (int, int) theDamageRange, 
        ref readonly double theBlockChance, 
        ref readonly string theSkill) 
        : base(in theName, in theHitPoints, in theAttackSpeed, in theHitChance, in theDamageRange)
    {
        BlockChance = theBlockChance;
        Skill = theSkill;
    }
    
    public double BlockChance { get; }
    
    public string Skill { get; init; }

    public override void TakeDamage(ref readonly int theDamage)
    {
        if (!(RandomNumberGenerator.NextDouble() > 1 - BlockChance)) HitPoints -= theDamage;
    }
    
}