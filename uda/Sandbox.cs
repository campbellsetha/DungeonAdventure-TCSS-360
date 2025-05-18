namespace UDA;

using System;
using Model.Map;

// class to test code
public class Sandbox
{
    public static void Main()
    {
        Room room = new Room(3, 3);
        Console.WriteLine(room.ToString());
        room = new Room(0, 0);
        Console.WriteLine(room.ToString());

        //Console.WriteLine(new Dungeon());
    }
    
}