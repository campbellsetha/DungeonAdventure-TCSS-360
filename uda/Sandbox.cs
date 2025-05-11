namespace UDA;

using System;
using Model.Map;

// class to test code
public class Sandbox
{
    public static void Main()
    {
        //Room room = new Room(RoomType.Normal, BorderType.TopLeft);
        //Console.WriteLine(room.ToString());

        Dungeon dungeon = new Dungeon();
        Console.WriteLine(dungeon.ToString());
    }
    
}