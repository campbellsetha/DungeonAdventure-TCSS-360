using System;

namespace UDA.Model;

// Just a reminder: need to add a listener that tells the model to call the special attack instead of the
// inherited attack method when a certain key is pressed

public class Warrior : Hero
{
    private static readonly int MyHitPoints = 125;
    private static readonly int MyAttackSpeed = 4;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (35, 60);
    private static readonly double MyBlockChance = 0.2;
    private static readonly string MySkill = "Crushing Blow";
    
    public Warrior(string theName) : base(in theName, in MyHitPoints, in MyAttackSpeed, in MyHitChance,
        in MyDamageRange, in MyBlockChance, in MySkill) { }

    public void CrushingBlow(ref readonly DungeonCharacter theTarget)
    {
        double successChance = 0.4;
        if (RandomNumberGenerator.NextDouble() > 1 - successChance)
        {
            int minDamage = 75;
            int maxDamage = 175;
            int damage = RandomNumberGenerator.Next(minDamage, maxDamage);
            theTarget.TakeDamage(in damage);
        }
    }

    //TODO: Customize ToString as needed for Warrior
    public override String ToString()
    {
        return base.ToString();
    }
    
}