namespace UDA.Model.Characters;

public class Warrior(in string theName, in int theHitPoints, in int theAttackSpeed, in double theHitChance, 
    in (int, int) theDamageRange, in double theBlockChance, in string theSkill) 
    : Hero(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange, theBlockChance, theSkill)
{
    public override int PerformSkill(in DungeonCharacter theTarget)
    {
        var damageDealt = base.PerformSkill(theTarget);
        var rand = RandomSingleton.GetInstance();
        const double successChance = 0.4;
        if (!(rand.NextDouble() > 1 - successChance))
        {
            Console.WriteLine($"{MySkill} failed!");
        } else {
            Console.WriteLine($"{MySkill} is successful!");
            var percentages = (0.5, 0.9);
            var minDamage = theTarget.MyMaxHitPoints * percentages.Item1;
            var maxDamage = theTarget.MyMaxHitPoints * percentages.Item2;
            damageDealt = rand.Next((int) minDamage, (int) maxDamage);
        }
        return damageDealt;
    }
}