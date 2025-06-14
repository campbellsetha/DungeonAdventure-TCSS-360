using UDA.Model.Map;
namespace Tests;

/// <summary>
/// A Test class for the Dungeon class singleton. 
/// </summary>
[TestFixture]
internal class DungeonTests
{
    private Dungeon _dungeon;

    /// <summary>
    ///  Called before each Test is executed.
    /// </summary>
    [SetUp]
    public void Setup()
    {
        _dungeon = Dungeon.MyInstance;
    }

    /// <summary>
    ///  Called after each Test is executed.
    /// </summary>
    [Test]
    public void Singleton_ShouldReturnSameInstance()
    {
        var otherInstance = Dungeon.MyInstance;
        Assert.That(otherInstance, Is.SameAs(_dungeon));
    }

    /// <summary>
    ///  Test to make sure the Dungeon doesn't have any empty rooms.
    /// </summary>
    [Test]
    public void DungeonMap_ShouldBeCompletelyFilled()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                Assert.That(_dungeon.MyMap[i, j], Is.Not.Null, $"Room at ({i},{j}) should not be null");
            }
        }
    }

    /// <summary>
    ///  Test to make sure that each room has at least one door.
    /// </summary>
    [Test]
    public void EachRoom_ShouldHaveAtLeastOneDoor()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var room = _dungeon.MyMap[i, j];
                var roomCount = 0;
                foreach (var kvp in room.MyBoundaries)
                {
                    if (kvp.Value == BoundaryType.Door)
                        roomCount++;
                }
                Assert.That(roomCount, Is.GreaterThanOrEqualTo(1), $"Room at ({i},{j}) should have at least one door");
            }
        }
    }

    /// <summary>
    ///  Test to make sure that rooms that share edges have the same boundary type. For example, if a room has a door
    ///  on its eastern boundary, its left neighbor has a door on its western boundary.
    /// </summary>
    [Test]
    public void Doors_ShouldBeSymmetricalBetweenAdjacentRooms()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                var room = _dungeon.MyMap[i, j];
                foreach (Direction dir in Enum.GetValues(typeof(Direction)))
                {
                    int adjX = i, adjY = j;
                    switch (dir)
                    {
                        case Direction.North:
                            adjX = i - 1;
                            break;
                        case Direction.South:
                            adjX = i + 1;
                            break;
                        case Direction.East:
                            adjY = j + 1;
                            break;
                        case Direction.West:
                            adjY = j - 1;
                            break;
                    }

                    if (adjX is < 0 or >= 10 || adjY is < 0 or >= 10) continue;
                    var adjacent = _dungeon.MyMap[adjX, adjY];
                    var method = typeof(Dungeon).GetMethod("GetOppositeDir", 
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, 
                        [typeof(Direction)]);
                        
#pragma warning disable
                    var opposite = (Direction) method.Invoke(_dungeon, [dir]);
#pragma warning enable
                        
                    Assert.That(adjacent.MyBoundaries[opposite], Is.EqualTo(room.MyBoundaries[dir]),
                        $"Mismatch at ({i},{j}) and ({adjX},{adjY}) for direction {dir}");
                }
            }
        }
    }

    /// <summary>
    ///  Test to make sure that the Dungeon can be completed (i.e., all pillars and the exit can be accessed from the
    ///  entrance room).
    /// </summary>
    [Test]
    public void Dungeon_ShouldBeTraversable()
    {
        var field = typeof(Dungeon).GetMethod("IsTraversable", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.That((bool) field.Invoke(_dungeon, null), Is.True, 
            "Dungeon should be traversable from entrance to exit and all pillars");
    }

    [Test]
    public void Dungeon_ShouldHaveMoreThanTwoDeadEnds()
    {
        var method = typeof(Dungeon).GetMethod("HasDeadEnds", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        #pragma warning disable
        Assert.That((bool) method.Invoke(_dungeon, null), Is.True, 
            "Dungeon should have more than two dead-end rooms");
        #pragma warning enable
    }

    /// <summary>
    ///  Test to make sure that the Dungeon contains all pillars and that they are unique.
    /// </summary>    
    [Test]
    public void Dungeon_HasAllPillars()
    {
        var containsPillarA = false;
        var containsPillarI = false;
        var containsPillarE = false;
        var containsPillarP = false;
        var count = 0;
        var numOfPillars = 4;
        for (var i = 0; i < _dungeon.MyMap.GetLength(0); i++)
        {
            for (var j = 0; j < _dungeon.MyMap.GetLength(1); j++)
            {
                switch (_dungeon.MyMap[i, j].MyRoomType)
                {
                    case RoomType.PillarA:
                        containsPillarA = true;
                        count++;
                        break;
                    case RoomType.PillarI:
                        containsPillarI = true;
                        count++;
                        break;
                    case RoomType.PillarE:
                        containsPillarE = true;
                        count++;
                        break;
                    case RoomType.PillarP:
                        containsPillarP = true;
                        count++;
                        break;
                }
            }
        }
        Assert.That(count == numOfPillars, Is.True);
        Assert.That(containsPillarA && containsPillarI && containsPillarE && containsPillarP, Is.True);
    }
}
