namespace UDA.Model.Map;
public static class RoomFactory
{
    public static Room CreateRoomFourDoors(in RoomType theRoomType = RoomType.Normal)
    {
        if (theRoomType is RoomType.Entrance or RoomType.Exit) 
            throw new ArgumentException("Entrance and exit rooms can only have one door");
        if (!Enum.IsDefined(typeof(RoomType), theRoomType)) 
            throw new ArgumentException("Invalid room type");
        return new Room(theRoomType, Direction.East, Direction.North, Direction.South, Direction.West);
    }

    public static Room CreateRoomThreeDoors(in (Direction, Direction, Direction) theDoors,
        in RoomType theRoomType = RoomType.Normal)
    {
        if (theRoomType is RoomType.Entrance or RoomType.Exit) 
            throw new ArgumentException("Entrance and exit rooms can only have one door");
        if (!Enum.IsDefined(typeof(RoomType), theRoomType)) 
            throw new ArgumentException("Invalid room type");
        if  (!Enum.IsDefined(typeof(Direction), theDoors.Item1) ||  !Enum.IsDefined(typeof(Direction), theDoors.Item2)
             || !Enum.IsDefined(typeof(Direction), theDoors.Item3))
            throw new ArgumentException("Invalid direction");
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2, theDoors.Item3);
    }

    public static Room CreateRoomTwoDoors(in (Direction, Direction) theDoors, in RoomType theRoomType = RoomType.Normal)
    {
        if (theRoomType is RoomType.Entrance or RoomType.Exit) 
            throw new ArgumentException("Entrance and exit rooms can only have one door");
        if (!Enum.IsDefined(typeof(RoomType), theRoomType)) 
            throw new ArgumentException("Invalid room type");
        if  (!Enum.IsDefined(typeof(Direction), theDoors.Item1) ||  !Enum.IsDefined(typeof(Direction), theDoors.Item2))
            throw new ArgumentException("Invalid direction");
        return new Room(theRoomType, theDoors.Item1, theDoors.Item2);
    }

    public static Room CreateRoomOneDoor(in Direction theDoor, in RoomType theRoomType = RoomType.Normal)
    {
        if (!Enum.IsDefined(typeof(RoomType), theRoomType)) 
            throw new ArgumentException("Invalid room type");
        if  (!Enum.IsDefined(typeof(Direction), theDoor))
            throw new ArgumentException("Invalid direction");
        return new Room(theRoomType, theDoor);
    }
}