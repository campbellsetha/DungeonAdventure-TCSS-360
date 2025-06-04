namespace UDA.Model.Map;

// Move room class inside this one?
public static class RoomFactory
{
    public static Room CreateRoomFourDoors(in RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, Direction.East, Direction.North, Direction.South, Direction.West);
    }

    public static Room CreateRoomThreeDoors(in (Direction, Direction, Direction) theDoors,
        in RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2, theDoors.Item3);
    }

    public static Room CreateRoomTwoDoors(in (Direction, Direction) theDoors, in RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2);
    }

    public static Room CreateRoomOneDoor(in Direction theDoor, in RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoor);
    }
}