namespace UDA.Model;
using Godot;

public abstract partial class Hero : DungeonCharacter
{

    // Should probably have a countdown timer for how often the special skill can be used 
    
    protected Hero(
        string theName, 
        int theHitPoints, 
        int theAttackSpeed,
        double theHitChance, 
        (int, int) theDamageRange, 
        double theBlockChance, 
        string theSkill) 
        : base(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange)
    {
        BlockChance = theBlockChance;
        Skill = theSkill;
    }
    
    public double BlockChance { get; }
    
    public string Skill { get; }

    public virtual void PerformSkill(DungeonCharacter theTarget)
    {
        
    }
    public override void TakeDamage(int theDamage)
    {
        if (!(RandomNumberGenerator.NextDouble() > 1 - BlockChance)) HitPoints -= theDamage;
    }


}