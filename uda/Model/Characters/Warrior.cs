namespace UDA.Model.Characters;

public class Warrior(in string theName, in int theHitPoints, in int theAttackSpeed, in double theHitChance, 
    in (int, int) theDamageRange, in double theBlockChance, in string theSkill) 
    : Hero(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange, theBlockChance, theSkill)
{
    protected override int PerformSkill(in DungeonCharacter theTarget)
    {
        var damageDealt = base.PerformSkill(theTarget);
        var rand = RandomSingleton.GetInstance();
        const double successChance = 0.4;
        if (!(rand.NextDouble() > 1 - successChance))
        {
            Console.WriteLine($"{MySkill} failed!");
        } else {
            Console.WriteLine($"{MySkill} is successful!");
            const int minDamage = 75;
            const int maxDamage = 175;
            damageDealt = rand.Next(minDamage, maxDamage);
        }
        return damageDealt;
    }
}