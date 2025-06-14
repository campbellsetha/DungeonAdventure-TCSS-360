using UDA.Model;
using UDA.Model.Characters;
using UDA.Model.Characters.Monster;

namespace Tests;

/// <summary>
/// A Test class for the DungeonCharacter class and its children. 
/// </summary>

[TestFixture]
internal class DungeonCharacterTests
{
    private StringWriter _writer;
    private static DungeonCharacter _warrior;
    private static DungeonCharacter _thief;
    private static DungeonCharacter _priest;
    private static DungeonCharacter _ogre;
    private static DungeonCharacter _skeleton;
    private static DungeonCharacter _gremlin;
    
    // Sets up a StringWriter to read from the console when a method prints to the console. 
    private void FlushConsole()
    {
        Console.Out.Flush();
        _writer = new StringWriter();
        Console.SetOut(_writer);
    }

    /// <summary>
    ///  Called before each Test is executed.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _writer = new StringWriter();
        _warrior = HeroFactory.CreateWarrior("Warrior");
        _thief = HeroFactory.CreateThief("Thief");
        _priest = HeroFactory.CreatePriest("Priest");
        _gremlin = MonsterFactory.CreateGremlin();
        _ogre = MonsterFactory.CreateOgre();
        _skeleton = MonsterFactory.CreateSkeleton();
        const int seed = 42;
        RandomSingleton.SetSeed(seed);
        // Capture printed output from tests
        _writer = new StringWriter();
        Console.SetOut(_writer);
    }

    /// <summary>
    ///  Called after each Test is executed.
    /// </summary>
    [TearDown]
    public void CleanUp()
    { 
        // Flush console
        Console.Out.Flush();
        _writer.Dispose();
        // Return the seed value back to its default state.
        RandomSingleton.UnsetSeed();
    }
    
    /// <summary>
    ///  Tests that constructors throw ArgumentExceptions when the parameters are null/empty (in the case of strings)
    /// or less than equal to 0 (in the case of value types).
    /// </summary>
    [Test]
    public void TestConstructorExceptions()
    {
        Assert.Throws<ArgumentException>(() =>
            new Warrior(null, 1, 1, 0.1, (1, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster(null, 1, 1, 0.1, (1, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() =>
            new Thief("", 1, 1, 0.1, (1, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("", 1, 1, 0.1, (1, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() => 
            new Thief("Name", 0, 1, 0.1, (1, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("Name", 0, 1, 0.1, (1, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() => 
            new Priest("Name", 1, 0, 0.1, (1, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("Name", 1, 0, 0.1, (1, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() => 
            new Warrior("Name", 1, 1, 0.0, (1, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("Name", 1, 1, 0.0, (1, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() => 
            new Thief("Name", 1, 1, 0.1, (0, 1), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("Name", 1, 1, 0.1, (0, 1), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() => 
            new Priest("Name", 1, 1, 0.1, (1, 0), 0.1, "Skill"));
        Assert.Throws<ArgumentException>(() =>
            new Monster("Name", 1, 1, 0.1, (1, 0), 0.1, (1, 1), 0.1));
        Assert.Throws<ArgumentException>(() =>
            new Warrior("Name", 1, 1, 0.1, (1, 1), 0.1, null));
        Assert.Throws<ArgumentException>(() =>
                new Priest("Name", 1, 1, 0.1, (1, 1), 0.1, ""));
        Assert.Throws<ArgumentException>(() =>
                new Warrior("Name", 1, 1, 0.1, (1, 1), 0.0, "Skill"));
    }

    /// <summary>
    ///  Tests that the Attack method inherited from the DungeonCharacter class throws an ArgumentNullException
    ///  when the target DungeonCharacter is null.
    /// </summary>
    [Test]
    public void TestAttackExceptions()
    {
        Assert.Throws<ArgumentNullException>(() => _warrior.Attack(null));
        Assert.Throws<ArgumentNullException>(() => _thief.Attack(null));
        Assert.Throws<ArgumentNullException>(() => _priest.Attack(null));
        Assert.Throws<ArgumentNullException>(() => _ogre.Attack(null));
        Assert.Throws<ArgumentNullException>(() => _skeleton.Attack(null));
        Assert.Throws<ArgumentNullException>(() => _gremlin.Attack(null));
    }

    /// <summary>
    ///  Tests that the various implementations of the Attack method function properly.
    /// </summary>
    [Test]
    public void TestAttacks()
    {
        const string successfulOutput = "Attack successful!";
        const string unsuccessfulOutput = "Attack unsuccessful!";
        /* A Thief's attack speed is two times as great as a Skeleton's, so they get three turns. Therefore, the
         amount of damage they deal will be between three times the minimum damage they can deal and three times 
         the max damage they can deal. */
        const int thiefTurns = 3;
        var thiefDamageToSkeleton = (thiefTurns * _thief.MyDamageRange.Min, 
            thiefTurns * _thief.MyDamageRange.Max);
        Assert.Multiple(() =>
        {
            var thiefActualDamage = _thief.Attack(_skeleton);
            Assert.That(thiefActualDamage, Is.LessThanOrEqualTo(thiefDamageToSkeleton.Item2));
            Assert.That(thiefActualDamage, Is.GreaterThanOrEqualTo(thiefDamageToSkeleton.Item1));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(successfulOutput));
            FlushConsole();
        });
        
        /* The randomly generated value was greater than the threshold for the Warrior's hit chance, so they
         cannot attack. */
        Assert.Multiple(() =>
        {
            Assert.That(_warrior.Attack(_gremlin), Is.EqualTo(0));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(unsuccessfulOutput));
            FlushConsole();
        });
        
        // This attack fails for the same reason as above.
        Assert.Multiple(() =>
        {
            Assert.That(_ogre.Attack(_priest),  Is.EqualTo(0));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(unsuccessfulOutput));
            FlushConsole();
        });
        
        /* An Ogre's attack speed is less than a Priest's attack speed, so they can only attack once. Therefore,
         the amount of damage their attack deals to a Priest is a randomly generated value between the minimum and 
         maximum amount of damage they can deal. */
        Assert.Multiple(() =>
        {
            var ogreActualDamage = _ogre.Attack(_priest);
            Assert.That(ogreActualDamage,  Is.LessThanOrEqualTo(_ogre.MyDamageRange.Max));
            Assert.That(ogreActualDamage, Is.GreaterThanOrEqualTo(_ogre.MyDamageRange.Min));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(successfulOutput));
        });
    }

    /// <summary>
    ///  Tests the implementations of the ToString methods for each type of DungeonCharacter.
    /// </summary>
    [Test]
    public void TestToString()
    {
        const string warriorStr = "Name: Warrior\nMaxHP: 125\nCurrentHP: 125\nDamageRange: (35, 60)\n" + 
                                  "AttackSpeed: 4\nHitChance: 0.8\nBlockChance: 0.2\nSkill: Crushing Blow";
        const string thiefStr = "Name: Thief\nMaxHP: 75\nCurrentHP: 75\nDamageRange: (20, 40)" +
                                "\nAttackSpeed: 6\nHitChance: 0.8\nBlockChance: 0.4\nSkill: Surprise Attack";
        const string priestStr = "Name: Priest\nMaxHP: 75\nCurrentHP: 75\nDamageRange: (25, 45)" +
                                 "\nAttackSpeed: 5\nHitChance: 0.7\nBlockChance: 0.3\nSkill: Heal";
        const string ogreStr = "Name: Ogre\nMaxHP: 200\nCurrentHP: 200\nDamageRange: (30, 60)\nAttackSpeed: 2\n" +
                               "HitChance: 0.6\nStunThreshold: 0.8\nHealChance: 0.1\nHealRange: (30, 60)";
        const string gremlinStr = "Name: Gremlin\nMaxHP: 70\nCurrentHP: 70\nDamageRange: (15, 30)\nAttackSpeed: 5\n" +
                                  "HitChance: 0.8\nStunThreshold: 0.6\nHealChance: 0.4\nHealRange: (20, 40)";
        const string skelStr = "Name: Skeleton\nMaxHP: 100\nCurrentHP: 100\nDamageRange: (30, 50)\nAttackSpeed: 3\n" +
                                   "HitChance: 0.8\nStunThreshold: 0.45\nHealChance: 0.3\nHealRange: (30, 50)";
        Assert.Multiple(() =>
        {
            Assert.That(_ogre.ToString(),  Is.EqualTo(ogreStr));
            Assert.That(_skeleton.ToString(),  Is.EqualTo(skelStr));
            Assert.That(_gremlin.ToString(),  Is.EqualTo(gremlinStr));
            Assert.That(_warrior.ToString(),  Is.EqualTo(warriorStr));
            Assert.That(_priest.ToString(),  Is.EqualTo(priestStr));
            Assert.That(_thief.ToString(),  Is.EqualTo(thiefStr));
        });

    }

    /// <summary>
    ///  Tests that the TakeDamage method inherited from the DungeonCharacter class throw ArgumentOutOfRangeExceptions
    ///  when passed a negative value.
    /// </summary>
    [Test]
    public void TestTakeDamageException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _warrior.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => _priest.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => _thief.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => _ogre.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => _skeleton.TakeDamage(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => _gremlin.TakeDamage(-1));
    }

    /// <summary>
    ///  Tests that the various implementations of the Attack method function properly.
    /// </summary>
    [Test]
    public void TestTakeDamage()
    {
        /* The randomly generated value was less than the threshold for the Priest's block chance, so
         they lost all their health points. */
        var damageAmount = _priest.MyMaxHitPoints + 1;
        _priest.TakeDamage(damageAmount);
        Assert.That(_priest.MyHitPoints, Is.EqualTo(0));

        // The Thief was unable to block the attack for 5 turns. 
       var turns = 5;
       damageAmount = 5;
       var totalDamage = damageAmount;
       for (var i = 0; i < turns; i++)
       {
           _thief.TakeDamage(damageAmount);
           Assert.That(_thief.MyHitPoints, Is.EqualTo(_thief.MyMaxHitPoints - totalDamage));
           totalDamage += damageAmount;
       }
        
       // The Thief was finally able to block the damage.
        _thief.TakeDamage(damageAmount);
        Assert.That(_thief.MyHitPoints, Is.EqualTo(_thief.MyMaxHitPoints - (totalDamage -  damageAmount)));

        /* The skeleton is unable to heal for two rounds. */
        turns = 2;
        damageAmount = 15;
        totalDamage = damageAmount;
        for (var i = 0; i < turns; i++)
        {
            _skeleton.TakeDamage(damageAmount);
            Assert.That(_skeleton.MyHitPoints, Is.EqualTo(_skeleton.MyMaxHitPoints - totalDamage));
            totalDamage += damageAmount;
        }
        
        // The skeleton was able to heal, so a randomly generated number of hit points within the range of the
        // minimum and maximum amount the skeleton can heal is added to their total hit points. 
        var oldHp = _skeleton.MyHitPoints;
        _skeleton.TakeDamage(damageAmount);
        Assert.That(_skeleton.MyHitPoints, Is.LessThanOrEqualTo(_skeleton.MyMaxHitPoints));
        Assert.That(_skeleton.MyHitPoints, Is.GreaterThan(oldHp));
        
        const string stunned = "Monster is stunned and cannot heal";
        
        // The Skeleton is stunned and can no longer heal. 
        damageAmount = 50;
        _skeleton.TakeDamage(damageAmount);
        Assert.That(_writer.ToString().Trim(), Is.EqualTo(stunned));
    }

    /// <summary>
    ///  Tests that Heal method in the Hero class throws an ArgumentOutOfRangeException when passed a negative value
    ///  or when it is called on a Hero that has 0 hit points remaining.
    /// </summary>
    [Test]
    public void TestHeroHealException()
    {
        var warrior = (Hero) _warrior;
        var priest = (Hero) _priest;
        var thief = (Hero) _thief;
        Assert.Throws<ArgumentOutOfRangeException>(() => warrior.Heal(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => warrior.Heal(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => priest.Heal(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => priest.Heal(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => thief.Heal(-1));
        Assert.Throws<ArgumentOutOfRangeException>(() => thief.Heal(0));
        
        // If the character has 0 hit points, they cannot heal.
        _priest.TakeDamage(_priest.MyMaxHitPoints);
        _warrior.TakeDamage(_warrior.MyMaxHitPoints);
        _thief.TakeDamage(_thief.MyMaxHitPoints);
        Assert.Throws<ArgumentException>(() => thief.Heal(_thief.MyMaxHitPoints));
        Assert.Throws<ArgumentException>(() => warrior.Heal(_warrior.MyMaxHitPoints));
        Assert.Throws<ArgumentException>(() => priest.Heal(_priest.MyMaxHitPoints));
        
    }

    /// <summary>
    ///  Tests the functionality of the Heal method in the Hero class.
    /// </summary>
    [Test]
    public void TestHeroHeal()
    {
        var warrior = (Hero) _warrior;
        var priest = (Hero) _priest;
        var thief = (Hero) _thief;
        
        // If the character already has full health, adding health does nothing.
        priest.Heal(1);
        Assert.That(_priest.MyHitPoints, Is.EqualTo(_priest.MyMaxHitPoints));
        warrior.Heal(1);
        Assert.That(_warrior.MyHitPoints, Is.EqualTo(_warrior.MyMaxHitPoints));
        thief.Heal(1);
        Assert.That(_thief.MyHitPoints, Is.EqualTo(_thief.MyMaxHitPoints));
        
        // Healing after taking damage.
        priest.TakeDamage(_priest.MyMaxHitPoints - 1);
        warrior.TakeDamage(_warrior.MyMaxHitPoints - 1);
        thief.TakeDamage(_thief.MyMaxHitPoints - 1);
        var oldHealth = _priest.MyHitPoints;
        priest.Heal(1);
        Assert.That(_priest.MyHitPoints, Is.EqualTo(oldHealth + 1));
        oldHealth = _warrior.MyHitPoints;
        warrior.Heal(1);
        Assert.That(_warrior.MyHitPoints, Is.EqualTo(oldHealth + 1));
        oldHealth = _thief.MyHitPoints;
        thief.Heal(1);
        Assert.That(_thief.MyHitPoints, Is.EqualTo(oldHealth + 1));
        
    }

    /// <summary>
    ///  Tests that Heal method in the Hero class throws an ArgumentNullException when a null DungeonCharacter is
    /// passed as the target.
    /// </summary>
    [Test]
    public void TestHeroSkillException()
    {
        var warrior = (Hero) _warrior;
        var priest = (Hero) _priest;
        var thief = (Hero) _thief;
        Assert.Throws<ArgumentNullException>(() => warrior.PerformSkill(null));
        Assert.Throws<ArgumentNullException>(() => thief.PerformSkill(null));
        Assert.Throws<ArgumentNullException>(() => priest.PerformSkill(null));
        
    }

    /// <summary>
    ///  Tests the implementation of Hero special skills. 
    /// </summary>
    [Test]
    public void TestHeroSkills()
    {
        var warrior = (Hero) _warrior;
        var priest = (Hero) _priest;
        var thief = (Hero) _thief;
        Assert.Multiple(() =>
        {
            const string successfulStr = "Crushing Blow is successful!";
            var unsuccessfulStr = "Crushing Blow failed!";
            var target = MonsterFactory.CreateOgre();
            var damagePossible = (0.5 * target.MyMaxHitPoints, 0.9 * target.MyMaxHitPoints);
            var actualDamage = warrior.PerformSkill(target);
            // The randomly generated value was greater than the skill's success chance, so it was successful.
            Assert.That(actualDamage,  Is.LessThanOrEqualTo(damagePossible.Item2));
            Assert.That(actualDamage, Is.GreaterThanOrEqualTo(damagePossible.Item1));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(successfulStr));
            FlushConsole();
            actualDamage = warrior.PerformSkill(target);
            // The randomly generated value was less than the skill's success chance, so it failed.
            Assert.That(actualDamage, Is.EqualTo(0));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(unsuccessfulStr));
           FlushConsole();
        });

        Assert.Multiple(() =>
        {
            const string successfulStr = "Heal stole 20 hit point(s) from enemy!";
            const string successfulStr1 = "Heal stole 1 hit point(s) from enemy!";
            var target = MonsterFactory.CreateSkeleton();
            var max = 20;
            // The Priest's heal ability steals 20 hit points from the enemy if the enemy has that many hit points.
            priest.TakeDamage(max);
            Assert.That(priest.MyHitPoints,  Is.EqualTo(priest.MyMaxHitPoints - max));
            var hpStolen = priest.PerformSkill(target);
            Assert.That(hpStolen, Is.EqualTo(max));
            Assert.That(priest.MyHitPoints, Is.EqualTo(priest.MyMaxHitPoints));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(successfulStr));
            FlushConsole();
            target.TakeDamage(target.MyMaxHitPoints - 1);
            FlushConsole();
            // If the enemy doesn't have 20 hit points of health left, the Priest takes their remaining health.
            priest.TakeDamage(1);
            Assert.That(priest.MyHitPoints,  Is.EqualTo(priest.MyMaxHitPoints - 1));
            hpStolen = priest.PerformSkill(target);
            Assert.That(hpStolen, Is.EqualTo(1));
            Assert.That(priest.MyHitPoints, Is.EqualTo(priest.MyMaxHitPoints));
            Assert.That(_writer.ToString().Trim(), Is.EqualTo(successfulStr1));
            FlushConsole();
        });

        Assert.Multiple(() =>
        {
            const string successfulStr = "Surprise Attack is successful!";
            const string regularStr = "Surprise Attack failed to deal extra damage!";
            const string unsuccessfulStr1 = "Surprise Attack completely failed!";
            var target = MonsterFactory.CreateGremlin();
            var damage = thief.PerformSkill(target);
            var baseDamage = thief.Attack(target);
            // Attack was successful and the Thief got an extra turn.
            Assert.That(damage, Is.LessThanOrEqualTo(baseDamage + thief.MyDamageRange.Max));
            Assert.That(damage, Is.GreaterThanOrEqualTo(baseDamage + thief.MyDamageRange.Min));
            Assert.That(_writer.ToString().Trim().Contains(successfulStr));
            thief.PerformSkill(target);
            FlushConsole();
            // The skill completely failed, and the Thief deals no damage.
            damage = thief.PerformSkill(target);
            Assert.That(damage, Is.EqualTo(0));
            Assert.That(_writer.ToString().Trim().Contains(unsuccessfulStr1));
            FlushConsole();
            // The skill wasn't successful, so the Thief just performs a regular attack.
            thief.PerformSkill(target);
            Assert.That(_writer.ToString().Trim().Contains(regularStr));
        });
    }
    
}