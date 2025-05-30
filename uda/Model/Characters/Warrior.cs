namespace UDA.Model.Characters;

// Just a reminder: need to add a listener that tells the model to call the special attack instead of the
// inherited attack method when a certain key is pressed

public partial class Warrior : Characters.Hero
{
	private static readonly int MyHitPoints = 125;
	private static readonly int MyAttackSpeed = 4;
	private static readonly double MyHitChance = 0.8;
	private static readonly (int, int) MyDamageRange = (35, 60);
	private static readonly double MyBlockChance = 0.2;
	private static readonly string MySkill = "Crushing Blow";
	
	public Warrior(string theName) : base(theName, MyHitPoints, MyAttackSpeed, MyHitChance,
		MyDamageRange, MyBlockChance, MySkill) { }

	public override void PerformSkill(DungeonCharacter theTarget)
	{
		double successChance = 0.4;
		if (DungeonCharacter.RandomNumberGenerator.NextDouble() > 1 - successChance)
		{
			int minDamage = 75;
			int maxDamage = 175;
			int damage = DungeonCharacter.RandomNumberGenerator.Next(minDamage, maxDamage);
			theTarget.TakeDamage(damage);
		}
	}
	
}
