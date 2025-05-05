namespace UDA.Model;

public class Priest : Hero
{
    private static readonly int MyHitPoints = 75;
    private static readonly int MyAttackSpeed = 5;
    private static readonly double MyHitChance = 0.7;
    private static readonly (int, int) MyDamageRange = (25, 45);
    private static readonly double MyBlockChance = 0.3;
    private static readonly string MySkill = "Heal";
    
    public Priest(string theName) : base(in theName, in MyHitPoints, in MyAttackSpeed, in MyHitChance,
        in MyDamageRange, in MyBlockChance, in MySkill) { }

    public void Heal()
    {
        
    }
}