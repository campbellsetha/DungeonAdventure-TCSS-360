using System;

namespace UDA.Model;

public abstract class DungeonCharacter(
    ref readonly string theName,
    ref readonly int theHitPoints,
    ref readonly int theAttackSpeed,
    ref readonly double theHitChance,
    ref readonly (int, int) theDamageRange)
{
    protected static readonly Random RandomNumberGenerator = new Random();
    
    /* Setters can be added to these properties as needed. If setters won't be needed, add init keyword
     to enforce immutability. */
    
    public string Name { get; } = theName;
    
    public int MaxHitPoints { get; } = theHitPoints;

    public int HitPoints { get; set; } = theHitPoints;

    public (int Min, int Max) DamageRange { get; } = theDamageRange;

    public int AttackSpeed { get; } = theAttackSpeed;
    
    public double HitChance { get; } =  theHitChance;

    public bool IsDead => HitPoints == 0;
    
    public virtual void TakeDamage(ref readonly int theDamage)
    {
        HitPoints -= theDamage;
    }
    
    public void Attack(ref readonly DungeonCharacter theTarget)
    {

        if (IsDead || !(RandomNumberGenerator.NextDouble() > 1 - HitChance))
        {
            Console.WriteLine("Character cannot attack!");
            return;
        }
        
        Console.WriteLine("Character can attack!");
        
        for (int i = -1; i < AttackSpeed / theTarget.AttackSpeed; i++)
        {
            int damage = RandomNumberGenerator.Next(DamageRange.Min, DamageRange.Max + 1);
            TakeDamage(in damage);
        }
        
    }
}