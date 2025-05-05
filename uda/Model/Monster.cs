namespace UDA.Model;

// Honestly, the child classes of Monster could probably be collapsed into this class. Unfortunately, 
// C# does not allow enums to be constructed, which is how I would have handled it with Java. I need to look
// into how to do it appropriately in C#.
public abstract class Monster : DungeonCharacter
{
    private readonly double _myStunThreshold;
    
    protected Monster(
        ref readonly string theName, 
        ref readonly int theHitPoints, 
        ref readonly int theAttackSpeed,
        ref readonly double theHitChance, 
        ref readonly (int, int) theDamageRange,
        ref readonly double theHealChance,
        ref readonly (int, int) theHealRange,
        ref readonly double theStunThreshold
        )
        : base(in theName, in theHitPoints, in theAttackSpeed, in theHitChance, 
            in theDamageRange)
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
       if (RandomNumberGenerator.NextDouble() > 1 - HealChance && !IsStunned)
       {
           int healAmount = RandomNumberGenerator.Next(HealRange.Min, HealRange.Max + 1);

           if (healAmount + HitPoints > MaxHitPoints)
           {
               healAmount = MaxHitPoints - HitPoints;
           }
           
           HitPoints += healAmount;
       }
   }

   public override void TakeDamage(ref readonly int theDamage)
   {
       if (HitPoints / (double) theDamage >= _myStunThreshold)
       {
           IsStunned = true;
       }
       
       HitPoints -= theDamage;
       Heal();
   }

}