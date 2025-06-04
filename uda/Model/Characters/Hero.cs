//using Godot;
using static System.Math;

namespace UDA.Model.Characters;

public abstract /*partial*/ class Hero : DungeonCharacter
{
    // Should probably have a countdown timer for how often the special skill can be used 

    /*[Signal]
    public delegate void HealthChangedEventHandler(int theCurrentHealth, int theMaxHealth);*/

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
        if (!(RandomSingleton.GetInstance().NextDouble() > 1 - BlockChance)) HitPoints -= theDamage;
        {
            HitPoints -= theDamage;

            // Clamp health to avoid it going below 0
            HitPoints = Max(HitPoints, 0);

            // Emit the signal
            //EmitSignal(nameof(HealthChanged), HitPoints, MaxHitPoints);

            /*void EmitSignal(string theHealthChangedName, int theHitPoints, int theMaxHitPoints)
            {
                throw new NotImplementedException();
            }*/
        }
    }

    public void Heal(int theHealAmount)
    {
        HitPoints += theHealAmount;

        // Clamp health to avoid it exceeding MaxHitPoints
        HitPoints = Min(HitPoints, MaxHitPoints);

        // Emit the signal
        // code needs to be moved elsewhere
        /*EmitSignal(nameof(HealthChanged), HitPoints, MaxHitPoints);

        void EmitSignal(string theHealthChangedName, object theHitPoints, int theMaxHitPoints)
        {
            throw new NotImplementedException();
        }*/
        
        
    }

    /*public void Connect(string theHealthchanged, Callable theCallable)
    {
        throw new NotImplementedException();
    }*/

    //TODO: SERIALIZE THIS
}