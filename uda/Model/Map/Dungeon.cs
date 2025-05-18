using System;
using System.Collections.Generic;
using System.Text;

namespace UDA.Model.Map;

public class Dungeon
{
    public const int Rows = 5;
    public const int Cols = 5;
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

    private void GeneratePillarRooms()
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
            MyMap[row, col] = new Room(row, col, pillarType);
        }
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
            
        MyMap[entrance.Item1, entrance.Item2] = new Room(entrance.Item1, entrance.Item2, RoomType.Entrance);
        MyMap[exit.Item1, exit.Item2] = new Room(exit.Item1, exit.Item2, RoomType.Exit);
        
        GeneratePillarRooms();
        
        for (int i = 0; i < Rows; i++)
        {
            for (int j = 0; j < Cols; j++)
            {
                if(MyMap[i, j] == null) MyMap[i, j] = new Room(i, j);
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
               row1.Append(arr[0]);
               row2.Append(arr[1]);
               row3.Append(arr[2]);
           }
           result.Append(row1.ToString()).Append('\n');
           result.Append(row2.ToString()).Append('\n');
           result.Append(row3.ToString()).Append('\n');
           
       }

       return result.ToString();

    }
    
}