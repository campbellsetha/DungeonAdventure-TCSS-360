using UDA.Model.Characters;
using UDA.Model.Characters.Monster;

namespace UDA;

// class to test code
public class Sandbox
{
    public static void Main()
    {
        //Room room = new Room(3, 3);
        //Room room = RoomFactory.CreateRoomTwoDoors((Direction.South, Direction.North));
        //Room room1 = RoomFactory.CreateRoomThreeDoors((Direction.East, Direction.West, Direction.South));
        //Room room2 = RoomFactory.CreateRoomFourDoors();
        //Room room3 = RoomFactory.CreateRoomOneDoor(Direction.North);
        //Console.WriteLine(room);
        //Console.WriteLine(room1);
        //Console.WriteLine(room2);
        //Console.WriteLine(room3);
        //room = new Room(0, 0);
        //Console.WriteLine(room.ToString());
        //Console.WriteLine(new Dungeon());
        //Dungeon test = new Dungeon();
        //Console.WriteLine(Dungeon.MyInstance.ToString());
        //Console.WriteLine(test.IsTraversable());
        //new Dungeon();
        //var randomMonster = MonsterFactory.CreateRandoMonster();
        //Console.WriteLine(randomMonster.ToString());
        //Console.WriteLine("Do ten damage to it" + "\n");
        //randomMonster.TakeDamage(10);
        //Console.WriteLine(randomMonster.HitPoints);
        //Console.WriteLine("yo");
        DungeonCharacter monst = new Monster("monster", 20, 25, 0.1, (2, 10), 0.1, (0, 10), 0.1);
        Console.WriteLine(monst);
        //DungeonCharacter hero = new Priest("priest");
    }
}