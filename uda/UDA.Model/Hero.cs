using System;
using Godot;

namespace UDA.Model;

public abstract partial class Hero : DungeonCharacter
{
    [Signal]
    public delegate void HealthChangedEventHandler(int theCurrentHealth, int theMaxHealth);

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
        MaxHitPoints = theHitPoints;
        
        // Emit initial health signal
        EmitSignal(nameof(global::UDA.Model.Hero.HealthChanged), HitPoints, MaxHitPoints);
    }
    
    public double BlockChance { get; }
    
    public string Skill { get; }
    
    public new int MaxHitPoints { get; }

    public virtual void PerformSkill(DungeonCharacter theTarget)
    {
        
    }
    
    public override void TakeDamage(int theDamage)
    {
        bool blocked = RandomNumberGenerator.NextDouble() > 1 - BlockChance;
        if (!blocked) 
        {
            int prevHealth = HitPoints;
            HitPoints -= theDamage;
            // Clamp health to avoid it going below 0
            HitPoints = Math.Max(0, HitPoints);
            
            if (prevHealth != HitPoints)
            {
                // Only emit the signal if health actually changed
                EmitSignal(nameof(global::UDA.Model.Hero.HealthChanged), HitPoints, MaxHitPoints);
            }
        }
    }
    
    public void Heal(int theHealAmount)
    {
        int prevHealth = HitPoints;
        HitPoints += theHealAmount;
        
        // Clamp health to avoid it exceeding MaxHitPoints
        HitPoints = Math.Min(HitPoints, MaxHitPoints);
        
        if (prevHealth != HitPoints)
        {
            // Only emit the signal if health actually changed
            EmitSignal(nameof(global::UDA.Model.Hero.HealthChanged), HitPoints, MaxHitPoints);
        }
    }

    //TODO: SERIALIZE THIS
}
