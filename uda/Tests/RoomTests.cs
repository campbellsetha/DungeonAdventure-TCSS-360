using NUnit.Framework.Legacy;
using UDA.Model;
using UDA.Model.Map;

namespace Tests;

/// <summary>
/// A Test class for the Room class. 
/// </summary>
[TestFixture]
internal class RoomTests
{

    // Used to count number of doors in a room.
    private int NumOfDoors(in Room theRoom)
    {
        var roomCount = 0;
        foreach (var kvp in theRoom.MyBoundaries)
        {
            if (kvp.Value == BoundaryType.Door)
                roomCount++;
        }
        return roomCount;
    }
    
    /// <summary>
    ///  Called before each Test is executed.
    /// </summary>
   [SetUp]
    public void Setup()
    {
        RandomSingleton.SetSeed(42);
    }
    
    /// <summary>
    ///  Tests that the RoomFactory methods construct doors correctly.
    /// </summary>
    [Test]
    public void RoomFactory_CreatesRoomWithCorrectNumberOfDoors()
    {
        var oneDoor = RoomFactory.CreateRoomOneDoor(Direction.North);
        Assert.That(NumOfDoors(oneDoor), Is.EqualTo(1));

        var twoDoors = RoomFactory.CreateRoomTwoDoors((Direction.North, Direction.South));
        Assert.That(NumOfDoors(twoDoors), Is.EqualTo(2));

        var threeDoors = RoomFactory.CreateRoomThreeDoors((Direction.North, Direction.South, Direction.East));
        Assert.That(NumOfDoors(threeDoors), Is.EqualTo(3));

        var fourDoors = RoomFactory.CreateRoomFourDoors();
        Assert.That(NumOfDoors(fourDoors), Is.EqualTo(4));
    }

    /// <summary>
    ///  Tests that trying to create an entrance or exit room with more than one door will throw an
    /// ArgumentException.
    /// </summary>
    [Test]
    public void RoomFactory_ThrowsExceptionForInvalidRoomTypes()
    {
        Assert.Throws<ArgumentException>(() => RoomFactory.CreateRoomFourDoors(RoomType.Entrance));
        Assert.Throws<ArgumentException>(() => RoomFactory.CreateRoomThreeDoors((Direction.North, Direction.East, 
            Direction.South), RoomType.Exit));
        Assert.Throws<ArgumentException>(() => RoomFactory.CreateRoomTwoDoors((Direction.East, Direction.West), 
            RoomType.Exit));
    }

    /// <summary>
    ///  Tests that doors are placed in the proper location, based on the order of the parameter.
    /// </summary>
    [Test]
    public void Room_HasCorrectBoundaries()
    {
        var room = RoomFactory.CreateRoomTwoDoors((Direction.North, Direction.South));
        Assert.Multiple(() =>
        {
            Assert.That(room.MyBoundaries[Direction.North], Is.EqualTo(BoundaryType.Door));
            Assert.That(room.MyBoundaries[Direction.South], Is.EqualTo(BoundaryType.Door));
            Assert.That(room.MyBoundaries[Direction.East], Is.EqualTo(BoundaryType.Wall));
            Assert.That(room.MyBoundaries[Direction.West], Is.EqualTo(BoundaryType.Wall));
        });
    }

    /// <summary>
    ///  Tests that entrances and exits do not contain items.
    /// </summary>
    [Test]
    public void Room_ContainsItemsOnlyInNormalRooms()
    {
        var entrance = new Room(RoomType.Entrance, Direction.East);
        var exit = new Room(RoomType.Exit, Direction.West, Direction.South);
        Assert.Multiple(() =>
        {
            Assert.That(entrance.ContainsTrap, Is.False);
            Assert.That(entrance.ContainsHealingPotion, Is.False);
            Assert.That(exit.ContainsVisionPotion, Is.False);
            Assert.That(exit.ContainsTrap, Is.False);
            Assert.That(exit.ContainsHealingPotion, Is.False);
            Assert.That(exit.ContainsVisionPotion, Is.False);
        });

        var normal = new Room(RoomType.Normal, Direction.West);
        Assert.That(normal.ContainsTrap || normal.ContainsHealingPotion || normal.ContainsVisionPotion ||
                    normal is not { ContainsTrap: true, ContainsHealingPotion: true, ContainsVisionPotion: true });
    }

    /// <summary>
    ///  Test for the GetDetails method.
    /// </summary>
    [Test]
    public void Room_GetDetails_ReturnsExpectedVisual()
    {
        var room = new Room(RoomType.PillarA, Direction.North, Direction.South);
        var details = room.GetDetails();

        Assert.Multiple(() =>
        {
            Assert.That(details[0], Is.EqualTo("*_*")); // North door
            Assert.That(details[1], Is.EqualTo("*A*"));
            Assert.That(details[2], Is.EqualTo("*_*")); // South door
        });
    }

    /// <summary>
    ///  Test for the ToString method
    /// </summary>
    [Test]
    public void TestToString()
    {
        var room = RoomFactory.CreateRoomThreeDoors((Direction.East, Direction.West, Direction.North), 
            RoomType.PillarE);
        const string str = "*_*\n|E|\n***\n";
        Assert.That(room.ToString(), Is.EqualTo(str));
    }
}