using System;
using System.Collections.Generic;
using System.Text;

namespace UDA.Model.Map;

public sealed class Dungeon
{
    private const int Rows = 5;
    private const int Cols = 5;
    private static readonly Random MyRand = RandomSingleton.GetInstance();
    private static readonly Room[,] MyMap = new Room[Rows, Cols];
    private readonly Dictionary<RoomType, (int, int)> _myPillars = new Dictionary<RoomType, (int, int)>();
    

    public Dungeon()
    {
        FillMap();
    }

    private (int, int) GenerateEntranceExitCoordinates()
    {
        int row = MyRand.Next(0, Rows);
        int col;
        if (row != 0)
        {
            col = (MyRand.Next(0, 2) == 0) ? 0 : 4;
        } else col = MyRand.Next(0, Cols);
        return (row, col);
    }

    private void CreatePillarRooms()
    {
        for (int i = 0; i < 4; i++)
        {
            int row = MyRand.Next(0, Rows);
            int col = MyRand.Next(0, Cols);
            RoomType pillarType = (RoomType) MyRand.Next(3, 7);
            while (MyMap[row, col] != null || _myPillars.ContainsKey(pillarType))
            {
                row = MyRand.Next(0, Rows);
                col = MyRand.Next(0, Cols);
                pillarType = (RoomType) MyRand.Next(3, 7);
            }
            _myPillars.Add(pillarType, (row, col));
            MyMap[row, col] = CreateRoom(row, col, pillarType);
        }
    }

    // Ask if this method should be broken up
    private Room CreateRoom(in int theX, in int theY, in RoomType theRoomType = RoomType.Normal)
    {
        List<Direction> directions = [Direction.North, Direction.South, Direction.East, Direction.West];
        Direction? dir = null;
        
        switch (theX)
        {
            case 0 when theRoomType is RoomType.Entrance or RoomType.Exit:
                dir = Direction.North;
                break;
            case 0:
                directions.Remove(Direction.North);
                break;
            case Rows - 1 when theRoomType is RoomType.Entrance or RoomType.Exit:
                dir = Direction.South;
                break;
            case Rows - 1:
                directions.Remove(Direction.South);
                break;
        }

        switch (theY)
        {
            case 0 when (theRoomType is RoomType.Entrance or RoomType.Exit) && dir == null:
                dir = Direction.West;
                break;
            case 0:
                directions.Remove(Direction.West);
                break;
            case Cols - 1 when (theRoomType is RoomType.Entrance or RoomType.Exit) && dir == null:
                dir = Direction.East;
                break;
            case Cols - 1:
                directions.Remove(Direction.East);
                break;
        }

        int numOfDoors = MyRand.Next(1, directions.Count + 1);
        Room room;
        dir ??= directions[MyRand.Next(directions.Count)];
        directions.Remove(dir.GetValueOrDefault());
        
        switch (numOfDoors)
        {
            case 1:
                room = RoomFactory.CreateRoomOneDoor(dir.GetValueOrDefault(), theRoomType);
                break; 
            case 2:
                room = RoomFactory.CreateRoomTwoDoors((dir.GetValueOrDefault(), 
                                    directions[MyRand.Next(directions.Count)]), theRoomType);
                break;
            case 3:
                Direction dir1 = directions[MyRand.Next(directions.Count)];
                directions.Remove(dir1);
                Direction dir2 = directions[MyRand.Next(directions.Count)];
                room = RoomFactory.CreateRoomThreeDoors((dir.GetValueOrDefault(), dir1, dir2), theRoomType);
                break;
            default:
                room = RoomFactory.CreateRoomFourDoors(theRoomType);
                break;
        }
        return room;
    }
    
    private void FillMap()
    {
        // Generate entrance and exit rooms
        (int, int) entrance = GenerateEntranceExitCoordinates();
        (int, int) exit = GenerateEntranceExitCoordinates();
        while (exit == entrance)
        {
            exit = GenerateEntranceExitCoordinates();
        }
        MyMap[entrance.Item1, entrance.Item2] = 
            CreateRoom(entrance.Item1, entrance.Item2, RoomType.Entrance);
        MyMap[exit.Item1, exit.Item2] = CreateRoom(exit.Item1, exit.Item2, RoomType.Exit);
        
        // Fill Dungeon with pillar rooms
        CreatePillarRooms();
        
        // Fill the rest of the Dungeon with random rooms
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                MyMap[i, j] ??= CreateRoom(i, j);
            }
        }
    }
    
    public override string ToString()
    {
       StringBuilder result = new StringBuilder();
       for (int i = 0; i < Rows; i++)
       {
           StringBuilder row1 = new StringBuilder();
           StringBuilder row2 = new StringBuilder();
           StringBuilder row3 = new StringBuilder();
           for (int j = 0; j < Cols; j++)
           {
               string[] arr = MyMap[i, j].GetDetails();
               row1.Append(arr[0]).Append("  ");
               row2.Append(arr[1]).Append("  ");
               row3.Append(arr[2]).Append("  ");
           }
           result.Append(row1.ToString()).Append('\n');
           result.Append(row2.ToString()).Append('\n');
           result.Append(row3.ToString()).Append("\n\n");
           
       }
       return result.ToString();
    }
}