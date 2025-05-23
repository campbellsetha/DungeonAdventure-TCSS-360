namespace UDA.Model;

// Just a reminder: need to add a listener that tells the model to call the special attack instead of the
// inherited attack method when a certain key is pressed

public class Thief : Hero
{
    private static readonly int MyHitPoints = 75;
    private static readonly int MyAttackSpeed = 6;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (20, 40);
    private static readonly double MyBlockChance = 0.4;
    private static readonly string MySkill = "Surprise Attack";
    
    public Thief(string theName) : base(in theName, in MyHitPoints, in MyAttackSpeed, in MyHitChance,
        in MyDamageRange, in MyBlockChance, in MySkill) { }

    public void SurpriseAttack(ref readonly DungeonCharacter theTarget)
    {
        double successChance = 0.4;
        double failureChance = 0.2;
        if (RandomNumberGenerator.NextDouble() > 1 - successChance)
        {
            Attack(in theTarget);
            int damage = RandomNumberGenerator.Next(DamageRange.Min, DamageRange.Max + 1);
            theTarget.TakeDamage(in damage);
        }
        else if (RandomNumberGenerator.NextDouble() > 1 - failureChance)
        {
            Attack(in theTarget);
        }
    }
}