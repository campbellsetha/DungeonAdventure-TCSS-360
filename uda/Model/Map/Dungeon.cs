using System;
using System.Collections.Generic;
using System.Text;
using UDA.inventory;
using UDA.Model.Items;
using static UDA.Model.Map.RoomType;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.BoundaryType;

namespace UDA.Model.Map;

public sealed class Dungeon
{
    public static readonly Dungeon MyInstance;
    private const int Rows = 10;
    private const int Cols = 10;
    private static readonly Random MyRand = RandomSingleton.GetInstance();
    public readonly Room[,] _myMap = new Room[Rows, Cols];
    private (int, int) _myEntrance;
    private (int, int) _myExit;
    private readonly Dictionary<RoomType, (int, int)> _myPillars = new();
    

    private Dungeon()
    {
        FillMap();
    }

    static Dungeon()
    {
        MyInstance = new Dungeon();
        while (!MyInstance.IsTraversable())
        {
            MyInstance = new Dungeon();
        }
        
    }

    private (int, int) GenerateEntranceExitCoordinates()
    {
        int row = MyRand.Next(0, Rows);
        int col;
        if (row != 0)
        {
            col = (MyRand.Next(0, 2) == 0) ? 0 : Cols - 1;
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
            while (_myMap[row, col] != null || _myPillars.ContainsKey(pillarType))
            {
                row = MyRand.Next(0, Rows);
                col = MyRand.Next(0, Cols);
                pillarType = (RoomType) MyRand.Next(3, 7);
            }
            _myPillars.Add(pillarType, (row, col));
            _myMap[row, col] = CreateRoom(row, col, pillarType);
        }
    }

    private List<Direction> GetValidDirections(in int theX, in int theY)
    {
        List<Direction> directions = new List<Direction>();
        if (theX != 0) directions.Add(North);
        if (theX != Rows - 1) directions.Add(South);
        if (theY != 0) directions.Add(West);
        if (theY != Cols - 1) directions.Add(East);
        return directions;
    }
    
    private Room CreateRoom(in int theX, in int theY, in RoomType theRoomType = Normal)
    {
        Direction? dir = (theX, theY, theRoomType is Entrance or Exit) switch
                        {
                            (0, _, true) => North,
                            (Rows - 1, _, true) => South,
                            (_, 0, true) => West,
                            (_, Cols - 1, true) => East,
                            (_, _, _) => null
                        };
        List<Direction> directions = GetValidDirections(theX, theY);
        int numOfDoors = 
            theRoomType is Entrance or Exit ? MyRand.Next(2, directions.Count + 1) : MyRand.Next(1, directions.Count + 1);
        Room room;
        dir ??= directions[MyRand.Next(directions.Count)];
        directions.Remove(dir.GetValueOrDefault());
        
        switch (numOfDoors)
        {
            case 1:
                room = RoomFactory.CreateRoomOneDoor(dir.GetValueOrDefault(), theRoomType);
                break; 
            case 2:
                Direction temp = directions[MyRand.Next(directions.Count)];
                room = RoomFactory.CreateRoomTwoDoors((dir.GetValueOrDefault(), temp), theRoomType);
                break;
            case 3:
                Direction temp1 = directions[MyRand.Next(directions.Count)];
                directions.Remove(temp1);
                Direction temp2 = directions[MyRand.Next(directions.Count)];
                room = RoomFactory.CreateRoomThreeDoors((dir.GetValueOrDefault(), temp1, temp2), theRoomType);
                break;
            default:
                room = RoomFactory.CreateRoomFourDoors(theRoomType);
                break;
        }

        List<InventoryItem> ItemsToPlaceInRoom = ItemFactory.GetItemsFromRoom(room);
        
        return room;
    }
    
    private void FillMap()
    {
        // Generate entrance and exit rooms
        _myEntrance = GenerateEntranceExitCoordinates();
        _myExit = GenerateEntranceExitCoordinates();
        while (_myExit == _myEntrance)
        {
            _myExit = GenerateEntranceExitCoordinates();
        }
        _myMap[_myEntrance.Item1, _myEntrance.Item2] = 
            CreateRoom(_myEntrance.Item1, _myEntrance.Item2, Entrance);
        _myMap[_myExit.Item1, _myExit.Item2] = CreateRoom(_myExit.Item1, _myExit.Item2, Exit);
        
        // Fill Dungeon with pillar rooms
        CreatePillarRooms();
        
        // Fill the rest of the Dungeon with random rooms
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                _myMap[i, j] ??= CreateRoom(i, j);
            }
        }
        FillMapHelper();
    }

    private Room OppositeRoom(Direction theDir, int theX, int theY)
    {
        //These can throw OOB exceptions
        return theDir switch
        {
            North => _myMap[theX - 1, theY],
            South => _myMap[theX + 1, theY],
            West => _myMap[theX, theY - 1],
            East => _myMap[theX, theY + 1],
            _ => throw new Exception("Unknown direction")
        };
    }
    
    private void FillMapHelper()
    {
        for (int i = 0; i < Rows - 1; i++)
        {
            for (int j = 0; j < Cols - 1; j++)
            {
                int numOfDoors = 0;
                Direction dir = 0;
                // probably should move this to its own method in the Room class
                foreach (KeyValuePair<Direction, BoundaryType?> kvp in _myMap[i, j].MyBoundaries)
                {
                    if (kvp.Value == Door)
                    {
                        numOfDoors++;
                        dir = kvp.Key;
                    }
                }

                if (numOfDoors == 1 && OppositeRoom(dir, i, j).MyBoundaries[OppositeDirection(dir)] !=
                    _myMap[i, j].MyBoundaries[dir])
                    OppositeRoom(dir, i, j).MyBoundaries[OppositeDirection(dir)] = _myMap[i, j].MyBoundaries[dir];
                else
                {
                    foreach (Direction key in new[] { South, East })
                    {
                        (int x1, int y1) = (i, j);
                        int x2 = key == South ? i + 1 : i;
                        int y2 = key == East ? j + 1 : j;
                        Direction opposite = key == South ? North : West;

                        var b1 = _myMap[x1, y1].MyBoundaries[key];
                        var b2 = _myMap[x2, y2].MyBoundaries[opposite];

                        if (b1 != b2)
                        {
                            switch (MyRand.Next(0, 2))
                            {
                                case 0:
                                    _myMap[x1, y1].MyBoundaries[key] = b2;
                                    break;
                                case 1:
                                    _myMap[x2, y2].MyBoundaries[opposite] = b1;
                                    break;
                            }
                        }
                    }
                }
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
               string[] arr = _myMap[i, j].GetDetails();
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
    
    private bool IsReachable((int, int) theCoordinates)
    {
        bool result = false;
        bool[,] visited = new bool[Rows, Cols];
        Stack<(int, int)> stack = new Stack<(int, int)>();
        stack.Push(_myEntrance);
        visited[_myEntrance.Item1, _myEntrance.Item2] = true;
        while (stack.Count != 0)
        {
            (int currentX, int currentY) = stack.Pop();

            if ((currentX, currentY) == theCoordinates) result = true;

            foreach (Direction dir in Enum.GetValues(typeof(Direction)))
            {
                int newX = currentX, newY = currentY;

                switch (dir)
                {
                    case North:
                        newX = currentX - 1;
                        break;
                    case South:
                        newX = currentX + 1;
                        break;
                    case East:
                        newY = currentY + 1;
                        break;
                    case West:
                        newY = currentY - 1;
                        break;
                }

                if (newX is >= 0 and < Rows && newY is >= 0 and < Cols &&
                    !visited[newX, newY] &&
                    _myMap[currentX, currentY].MyBoundaries[dir] == Door &&
                    _myMap[newX, newY].MyBoundaries[OppositeDirection(dir)] == Door)
                {
                    visited[newX, newY] = true;
                    stack.Push((newX, newY));
                }
            }
        }
        return result;
    }
    
    private static Direction OppositeDirection(Direction theDir)
    {
        return theDir switch
        {
            North => South,
            South => North,
            East => West,
            West => East,
            _ => throw new ArgumentException("Invalid direction"),
        };
    }
    
    private bool IsTraversable()
    {
        return IsReachable(_myExit) 
               && IsReachable(_myPillars[PillarA]) 
               && IsReachable(_myPillars[PillarE]) 
               && IsReachable(_myPillars[PillarI])
               && IsReachable(_myPillars[PillarP]);
    }
}