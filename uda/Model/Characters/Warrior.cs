namespace UDA.Model.Characters;

// Just a reminder: need to add a listener that tells the model to call the special attack instead of the
// inherited attack method when a certain key is pressed

public partial class Warrior : Hero
{
    private static readonly int MyHitPoints = 125;
    private static readonly int MyAttackSpeed = 4;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (35, 60);
    private static readonly double MyBlockChance = 0.2;
    private static readonly string MySkill = "Crushing Blow";

    public Warrior(string theName) : base(theName, MyHitPoints, MyAttackSpeed, MyHitChance,
        MyDamageRange, MyBlockChance, MySkill)
    {
    }

    public override void PerformSkill(DungeonCharacter theTarget)
    {
        var rand = RandomSingleton.GetInstance();
        var successChance = 0.4;
        if (rand.NextDouble() > 1 - successChance)
        {
            var minDamage = 75;
            var maxDamage = 175;
            var damage = rand.Next(minDamage, maxDamage);
            theTarget.TakeDamage(damage);
        }
    }
}