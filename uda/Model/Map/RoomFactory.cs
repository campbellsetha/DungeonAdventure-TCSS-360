namespace UDA.Model.Map;

public static class RoomFactory
{

    public static Room CreateRoomFourDoors(RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, Direction.East, Direction.North, Direction.South, Direction.West);
    }

    public static Room CreateRoomThreeDoors((Direction, Direction, Direction) theDoors, RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2, theDoors.Item3);
    }

    public static Room CreateRoomTwoDoors((Direction, Direction) theDoors, RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2);
    }
    
    public static Room CreateRoomOneDoor(Direction theDoor, RoomType theRoomType = RoomType.Normal)
    {
        return new Room(theRoomType, theDoor);
    }
}