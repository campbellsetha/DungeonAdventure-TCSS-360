using Godot;
using UDA.Model.Characters.Monster;

namespace UDA.Game.Enemies;

public partial class BossMonster : MonsterBase
{
    public override void _Ready()
    {
        _myMonsterClass = MonsterFactory.CreateRandoMonster();
        base.SetUp(); 
    }
}