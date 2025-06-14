namespace UDA.Model.Characters;
public class Priest(in string theName, in int theHitPoints, in int theAttackSpeed, in double theHitChance, 
    in (int, int) theDamageRange, in double theBlockChance, in string theSkill) 
    : Hero(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange, theBlockChance, theSkill)
{
    
    /// <summary>
    /// Implements the PerformSkill method.
    /// </summary>
    public override int PerformSkill(in DungeonCharacter theTarget)
    {
        const int maxHeal = 20;
        var hpToSteal = base.PerformSkill(theTarget) + Math.Min(maxHeal, theTarget.MyHitPoints);
        MyHitPoints = Math.Min(MyMaxHitPoints, MyHitPoints + hpToSteal);
        Console.WriteLine($"{MySkill} stole {hpToSteal} hit point(s) from enemy!");
        return hpToSteal;
    }
}