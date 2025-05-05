namespace UDA.Model;

public class Ogre : Monster
{
    private static readonly string MyName =  "Ogre";
    private static readonly int MyHitPoints = 200;
    private static readonly int MyAttackSpeed = 2;
    private static readonly double MyHitChance = 0.6;
    private static readonly (int, int) MyDamageRange = (30, 60);
    private static readonly double MyHealChance = 0.1;
    private static readonly (int, int) MyHealRange =  (30, 60);
    private static readonly double MyStunThreshold = 0.8;
    
    public Ogre() : base(in MyName, in MyHitPoints, in MyAttackSpeed, in MyHitChance, in MyDamageRange, 
        in MyHealChance, in MyHealRange, in MyStunThreshold) { }
}