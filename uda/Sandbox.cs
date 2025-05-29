namespace UDA;

using System;
using Model.Map;

// class to test code
public class Sandbox
{
    public static void Main()
    {
        //Room room = new Room(3, 3);
        Room room = RoomFactory.CreateRoomTwoDoors((Direction.South, Direction.North));
        Room room1 = RoomFactory.CreateRoomThreeDoors((Direction.East, Direction.West, Direction.South));
        Room room2 = RoomFactory.CreateRoomFourDoors();
        Room room3 = RoomFactory.CreateRoomOneDoor(Direction.North);
        //Console.WriteLine(room);
        //Console.WriteLine(room1);
        //Console.WriteLine(room2);
        //Console.WriteLine(room3);
        //room = new Room(0, 0);
        //Console.WriteLine(room.ToString());

        //Console.WriteLine(new Dungeon());
        Dungeon test = new Dungeon();
        Console.WriteLine(test.ToString());
        //new Dungeon();
    }
    
}