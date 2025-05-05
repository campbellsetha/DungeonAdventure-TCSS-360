namespace UDA.Model;

public class Gremlin : Monster
{
    private static readonly string MyName =  "Gremlin";
    private static readonly int MyHitPoints = 70;
    private static readonly int MyAttackSpeed = 5;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (15, 30);
    private static readonly double MyHealChance = 0.4;
    private static readonly (int, int) MyHealRange =  (20, 40);
    private static readonly double MyStunThreshold = 0.6;
    
    public Gremlin() : base(in MyName, in MyHitPoints, in MyAttackSpeed, in MyHitChance, in MyDamageRange, 
        in MyHealChance, in MyHealRange, in MyStunThreshold) { }
}