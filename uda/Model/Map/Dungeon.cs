using System.Text;
using static UDA.Model.Map.RoomType;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.BoundaryType;

namespace UDA.Model.Map;

public sealed class Dungeon
{
    public static readonly Dungeon MyInstance;
    public readonly Room[,] MyMap;
    private const int MyRows = 10;
    private const int MyCols = 10;
    private static readonly Random MyRand = RandomSingleton.GetInstance();
    private (int, int) _myEntrance;
    private (int, int) _myExit;
    private readonly Dictionary<RoomType, (int, int)> _myPillars = new();
    

    private Dungeon()
    {
        MyMap = new Room[MyRows, MyCols];
        FillMap();
    }

    static Dungeon()
    {
        MyInstance = new Dungeon();
        while (!MyInstance.IsTraversable() && !MyInstance.HasDeadEnds())
        {
            MyInstance = new Dungeon();
        }
        
    }

    private static (int, int) GenerateEntranceExitCoordinates()
    {
        var row = MyRand.Next(0, MyRows);
        int col;
        if (row != 0)
        {
            col = (MyRand.Next(0, 2) == 0) ? 0 : MyCols - 1;
        } else col = MyRand.Next(0, MyCols);
        return (row, col);
    }

    private void CreatePillarRooms()
    {
        for (int i = 0; i < 4; i++)
        {
            int row = MyRand.Next(0, MyRows);
            int col = MyRand.Next(0, MyCols);
            RoomType pillarType = (RoomType) MyRand.Next(3, 7);
            while (MyMap[row, col] != null || _myPillars.ContainsKey(pillarType))
            {
                row = MyRand.Next(0, MyRows);
                col = MyRand.Next(0, MyCols);
                pillarType = (RoomType) MyRand.Next(3, 7);
            }
            _myPillars.Add(pillarType, (row, col));
            MyMap[row, col] = CreateRoom(row, col, pillarType);
        }
    }

    private List<Direction> GetValidDirections(in int theX, in int theY)
    {
        List<Direction> directions = new List<Direction>();
        if (theX != 0) directions.Add(North);
        if (theX != MyRows - 1) directions.Add(South);
        if (theY != 0) directions.Add(West);
        if (theY != MyCols - 1) directions.Add(East);
        return directions;
    }
    
    private Room CreateRoom(in int theX, in int theY, in RoomType theRoomType = Normal)
    {
        List<Direction> directions = GetValidDirections(theX, theY);
        int numOfDoors = MyRand.Next(1, directions.Count + 1);
        Room room;
        Direction dir = directions[MyRand.Next(directions.Count)];
        directions.Remove(dir);
        
        switch (numOfDoors)
        {
            case 1:
                room = RoomFactory.CreateRoomOneDoor(dir, theRoomType);
                break; 
            case 2:
                Direction temp = directions[MyRand.Next(directions.Count)];
                room = RoomFactory.CreateRoomTwoDoors((dir, temp), theRoomType);
                break;
            case 3:
                Direction temp1 = directions[MyRand.Next(directions.Count)];
                directions.Remove(temp1);
                Direction temp2 = directions[MyRand.Next(directions.Count)];
                room = RoomFactory.CreateRoomThreeDoors((dir, temp1, temp2), theRoomType);
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
        _myEntrance = GenerateEntranceExitCoordinates();
        _myExit = GenerateEntranceExitCoordinates();
        while (_myExit == _myEntrance)
        {
            _myExit = GenerateEntranceExitCoordinates();
        }
        MyMap[_myEntrance.Item1, _myEntrance.Item2] = 
            CreateRoom(_myEntrance.Item1, _myEntrance.Item2, Entrance);
        MyMap[_myExit.Item1, _myExit.Item2] = CreateRoom(_myExit.Item1, _myExit.Item2, Exit);
        
        // Fill Dungeon with pillar rooms
        CreatePillarRooms();
        
        // Fill the rest of the Dungeon with random rooms
        for (int i = 0; i < MyRows; i++)
        {
            for (int j = 0; j < MyCols; j++)
            {
                MyMap[i, j] ??= CreateRoom(i, j);
            }
        }
        FillMapHelper();
    }
    
    private Room GetAdjRoom(Direction theDir, int theX, int theY)
    {
        //These can throw OOB exceptions
        return theDir switch
        {
            North => MyMap[theX - 1, theY],
            South => MyMap[theX + 1, theY],
            //This can throw an OOB sometimes
            West => MyMap[theX, theY - 1],
            East => MyMap[theX, theY + 1],
            _ => throw new Exception("Unknown direction")
        };
    }
    
    private void FillMapHelper()
    {
        for (int i = 0; i < MyRows; i++)
        {
            for (int j = 0; j < MyCols; j++)
            {
                Room current = MyMap[i, j];
                foreach (Direction key in new[] { South, East, West, North })
                {
                    if ((key == North && i == 0) || (key == West && j == 0) ||
                        (key == South && i == MyRows - 1) || (key == East && j == MyCols - 1)) 
                        continue;
                    Direction opposite = GetOppositeDir(key);
                    Room adjacent = GetAdjRoom(key, i, j);
                    if (current.MyBoundaries[key] == adjacent.MyBoundaries[opposite]) 
                        continue;
                    current.MyBoundaries[key] = Door;
                    adjacent.MyBoundaries[opposite] = Door;
                }
            }
        }
    }
    
    public override string ToString()
    {
       StringBuilder result = new StringBuilder();
       for (int i = 0; i < MyRows; i++)
       {
           StringBuilder row1 = new StringBuilder();
           StringBuilder row2 = new StringBuilder();
           StringBuilder row3 = new StringBuilder();
           for (int j = 0; j < MyCols; j++)
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
    
    private bool IsReachable((int, int) theCoordinates)
    {
        bool result = false;
        bool[,] visited = new bool[MyRows, MyCols];
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

                if (newX is >= 0 and < MyRows && newY is >= 0 and < MyCols &&
                    !visited[newX, newY] &&
                    MyMap[currentX, currentY].MyBoundaries[dir] == Door &&
                    MyMap[newX, newY].MyBoundaries[GetOppositeDir(dir)] == Door)
                {
                    visited[newX, newY] = true;
                    stack.Push((newX, newY));
                }
            }
        }
        return result;
    }
    
    private static Direction GetOppositeDir(Direction theDir)
    {
        return theDir switch
        {
            North => South,
            South => North,
            East => West,
            West => East,
            _ => throw new ArgumentException("Invalid direction")
        };
    }

    private bool HasDeadEnds()
    { 
        int numOfDeadEnds = 0;
        for (int i = 0; i < MyRows; i++)
        {
            for (int j = 0; j < MyCols; j++)
            {
                if (MyMap[i, j].GetNumberOfDoors() == 1) numOfDeadEnds++;
            }
        }
        return numOfDeadEnds > 2;
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