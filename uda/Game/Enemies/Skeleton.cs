using UDA.Model.Characters.Monster;

namespace UDA.Game.Enemies;

public partial class Skeleton : MonsterBase
{
    public override void _Ready()
    {
        _myMonsterClass = MonsterFactory.CreateSkeleton();
        base.SetUp();
    }
}