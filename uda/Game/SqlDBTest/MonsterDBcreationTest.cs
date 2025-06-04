using Godot;
using UDA.Model.Characters.Monster;

namespace UDA.Game.SqlDBTest;

public partial class MonsterDBcreationTest : Control
{
    //Testing if we can create a monster from a db using the factory,
    //and if its methods will work
    public override void _Ready()
    {
        base._Ready();
        /*Monster testGremlin = MonsterFactory.CreateGremlin();
    GD.Print(testGremlin.ToString());
    GD.Print(testGremlin.HitPoints);
    GD.Print("\n");
    testGremlin.TakeDamage(42);
    GD.Print("\n");
    GD.Print(testGremlin.ToString());
    GD.Print(testGremlin.HitPoints);
    GD.Print("\n");
    Monster testOgre = MonsterFactory.CreateOgre();
    GD.Print(testOgre.ToString());
    GD.Print(testOgre.HitPoints);
    GD.Print("\n");*/
        var randomMonster = MonsterFactory.CreateRandoMonster();
        GD.Print(randomMonster.ToString());
        GD.Print("Do ten damage to it" + "\n");
        randomMonster.TakeDamage(10);
        GD.Print(randomMonster.HitPoints);
    }
}