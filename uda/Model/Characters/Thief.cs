//using Godot;

namespace UDA.Model.Characters;

public class Thief(in string theName, in int theHitPoints, in int theAttackSpeed, in double theHitChance, 
    in (int, int) theDamageRange, in double theBlockChance, in string theSkill) 
    : Hero(theName, theHitPoints, theAttackSpeed, theHitChance, theDamageRange, theBlockChance, theSkill)
{

    protected override int PerformSkill(in DungeonCharacter theTarget)
    {
        const double successChance = 0.4;
        const double failureChance = 0.2;
        var damageDealt = base.PerformSkill(theTarget);
        var rand = RandomSingleton.GetInstance();
        var roll = rand.NextDouble();
        switch (roll)
        {
            case <= successChance:
            {
                Console.WriteLine($"{base.MySkill} is successful!");
                damageDealt += Attack(theTarget) + rand.Next(MyDamageRange.Item1, MyDamageRange.Item2 + 1);
                break;
            }
            case <= successChance + failureChance:
                Console.WriteLine($"{base.MySkill} failed to deal extra damage!");
                damageDealt += Attack(theTarget);
                break;
            default:
                Console.WriteLine($"{base.MySkill} completely failed!");
                break;
        }
        return damageDealt;
    }
}