using System;
using Godot;

namespace UDA.Model.Characters;

public abstract partial class DungeonCharacter(
    string theName,
    int theHitPoints,
    int theAttackSpeed,
    double theHitChance,
    (int, int) theDamageRange) : CharacterBody2D
{
    
    /* Setters can be added to these properties as needed. If setters won't be needed, add init keyword
     to enforce immutability. */
    
    public string NewName { get; } = theName;
    
    public int MaxHitPoints { get; } = theHitPoints;

    public int HitPoints { get; protected set; } = theHitPoints;

    public (int Min, int Max) DamageRange { get; } = theDamageRange;

    public int AttackSpeed { get; } = theAttackSpeed;
    
    public double HitChance { get; } =  theHitChance;

    public bool IsDead => HitPoints == 0;
    
    public virtual void TakeDamage(int theDamage)
    {
        HitPoints -= theDamage;
    }
    
    public void Attack(DungeonCharacter  theTarget)
    {

        Random rand = RandomSingleton.GetInstance();
        if (IsDead || !(rand.NextDouble() > 1 - HitChance))
        {
            Console.WriteLine("Character cannot attack!");
            return;
        }
        
        Console.WriteLine("Character can attack!");
        
        for (int i = -1; i < AttackSpeed / theTarget.AttackSpeed; i++)
        {
            int damage = rand.Next(DamageRange.Min, DamageRange.Max + 1);
            TakeDamage(damage);
        }
        
    }

    public override String ToString()
    {
        return $"Name:{Name} MaxHP:{MaxHitPoints} CurrentHP:{HitPoints} " +
               $"DamageRange:{DamageRange} AttackSpeed:{AttackSpeed} HitChance:{HitChance}";
    }
}