//using Godot;

namespace UDA.Model.Characters;

public /*partial*/ class Thief : Hero
{
    private const double SuccessChance = 0.4;
    private const double FailureChance = 0.2;

    //private const double SkillCooldown = 10.0; // Cooldown in seconds
    private static readonly int MyHitPoints = 75;
    private static readonly int MyAttackSpeed = 6;
    private static readonly double MyHitChance = 0.8;
    private static readonly (int, int) MyDamageRange = (20, 40);
    private static readonly double MyBlockChance = 0.4;
    private static readonly string MySkill = "Surprise Attack";
    //private double _lastSkillTime = -SkillCooldown;

    public Thief(string theName)
        : base(theName, MyHitPoints, MyAttackSpeed, MyHitChance, MyDamageRange, MyBlockChance, MySkill)
    {
    }

    public override void PerformSkill(DungeonCharacter theTarget)
    {
        // I would move this code in with the Game code
        /*var currentTime = Time.GetTicksMsec() / 1000.0; // Get current time in seconds
        if (currentTime - _lastSkillTime < SkillCooldown)
        {
            Console.WriteLine($"{Skill} is on cooldown!");
            return; // Skill is still on cooldown
        }

        _lastSkillTime = currentTime;*/

        var rand = RandomSingleton.GetInstance();
        var roll = rand.NextDouble();
        if (roll <= SuccessChance)
        {
            Console.WriteLine($"{Skill} is successful!");
            Attack(theTarget);
            var damage = rand.Next(MyDamageRange.Item1, MyDamageRange.Item2 + 1);
            theTarget.TakeDamage(damage);
            //theTarget.EmitSignal(nameof(HealthChangedEventHandler), theTarget.HitPoints, theTarget.MaxHitPoints);
        }
        else if (roll <= SuccessChance + FailureChance)
        {
            Console.WriteLine($"{Skill} failed to deal extra damage!");
            Attack(theTarget);
        }
        else
        {
            Console.WriteLine($"{Skill} completely failed!");
        }
    }
}