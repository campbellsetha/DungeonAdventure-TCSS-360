namespace UDA.Model;

public class Priest : Hero
{
    private static int myHitPoints = 75;
    private static readonly int MyAttackSpeed = 5;
    private static readonly double MyHitChance = 0.7;
    private static readonly (int, int) MyDamageRange = (25, 45);
    private static readonly double MyBlockChance = 0.3;
    private static readonly string MySkill = "Heal";
    
    public Priest(string theName) : base(theName, myHitPoints,MyAttackSpeed, MyHitChance,
        MyDamageRange, MyBlockChance, MySkill) { }

    public void Heal()
    {
        myHitPoints += 20;
    }
}