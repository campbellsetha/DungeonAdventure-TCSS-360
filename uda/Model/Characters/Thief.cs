using Godot;

namespace UDA.Model.Characters;

public class Thief : Hero
{
	private static readonly int MyHitPoints = 75;
	private static readonly int MyAttackSpeed = 6;
	private static readonly double MyHitChance = 0.8;
	private static readonly (int, int) MyDamageRange = (20, 40);
	private static readonly double MyBlockChance = 0.4;
	private const double SuccessChance = 0.4;
	private const double FailureChance = 0.2;
	private static readonly string MySkill = "Surprise Attack";

	private const double SkillCooldown = 10.0; // Cooldown in seconds
	private double _lastSkillTime = -SkillCooldown;

	public Thief(string theName) 
		: base(theName, MyHitPoints, MyAttackSpeed, MyHitChance, MyDamageRange, MyBlockChance, MySkill)
	{ }

	public override void PerformSkill(DungeonCharacter theTarget)
	{
		double currentTime = Time.GetTicksMsec() / 1000.0; // Get current time in seconds
		if (currentTime - _lastSkillTime < SkillCooldown)
		{
			GD.Print($"{Skill} is on cooldown!");
			return; // Skill is still on cooldown
		}

		_lastSkillTime = currentTime;

		double roll = RandomNumberGenerator.NextDouble();
		if (roll <= SuccessChance)
		{
			GD.Print($"{Skill} is successful!");
			Attack(theTarget);
			int damage = RandomNumberGenerator.Next(MyDamageRange.Item1, MyDamageRange.Item2 + 1);
			theTarget.TakeDamage(damage);
			theTarget.EmitSignal(nameof(DungeonCharacter.HealthChanged), theTarget.HitPoints, theTarget.MaxHitPoints);
		}
		else if (roll <= SuccessChance + FailureChance)
		{
			GD.Print($"{Skill} failed to deal extra damage!");
			Attack(theTarget);
		}
		else
		{
			GD.Print($"{Skill} completely failed!");
		}
	}
}
