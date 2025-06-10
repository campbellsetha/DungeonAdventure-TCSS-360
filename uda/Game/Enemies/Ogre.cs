using Godot;
using UDA.Model.Characters.Monster;

namespace UDA.Game.Enemies;

public partial class Ogre : MonsterBase
{
    public override void _Ready()
    {
        _myMonsterClass = MonsterFactory.CreateOgre();
        base.SetUp();
    }
}