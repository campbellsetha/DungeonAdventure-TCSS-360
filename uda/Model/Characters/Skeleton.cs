namespace UDA.Model;

public class Skeleton : Monster
{
    private static readonly string MyName =  "Spooky Scary Skeleton";
    private static readonly int MyHitPoints = 100;
    private static readonly int MyAttackSpeed = 3;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (30, 50);
    private static readonly double MyHealChance = 0.3;
    private static readonly (int, int) MyHealRange =  (30, 50);
    private static readonly double MyStunThreshold = 0.45;
    
    public Skeleton() : base(in MyName, in MyHitPoints, in MyAttackSpeed, in MyHitChance, in MyDamageRange, 
        in MyHealChance, in MyHealRange, in MyStunThreshold) { }
}