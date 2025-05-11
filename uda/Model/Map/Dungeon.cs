using System.Text;

namespace UDA.Model.Map;

public class Dungeon
{
    private static readonly int MyCols = 5;
    private static readonly int MyRows = 5;
    public static readonly Room[,] MyMap = new Room[MyCols, MyRows];

    public Dungeon()
    {
        FillMap();
    }

    private void FillMap()
    {
        /* Find random spot for entrance and exit - right now entrance and exit can only be on left
         and right borders, respectively. Also, they can't be in corners but this can be changed. */
        int size = MyMap.GetLength(0) - 1;
        MyMap[0, RandomSingleton.GetInstance().Next(0, size)] = new Room(RoomType.Entrance, BorderType.Left);
        MyMap[size, RandomSingleton.GetInstance().Next(0, size)] = new Room(RoomType.Exit, BorderType.Left);
        
        MyMap[0, 0] = new Room(RoomType.Normal, BorderType.TopLeft);
        MyMap[size, 0] = new Room(RoomType.Normal, BorderType.BottomLeft);
        MyMap[0, size] = new Room(RoomType.Normal, BorderType.TopRight);
        MyMap[size, size] = new Room(RoomType.Normal, BorderType.BottomRight);
        
        // Fill border rooms - need a way to make these capable of being pillar rooms later
        for (int i = 1; i < MyMap.GetLength(0); i++)
        {
            MyMap[0, i] = new Room(RoomType.Normal, BorderType.Top);
        }
        
        for (int i = 1; i < MyMap.GetLength(0); i++)
        {
            MyMap[MyMap.GetLength(1) - 1, i] = new Room(RoomType.Normal, BorderType.Bottom);
        }
        
    }

    // currently not working due to line breaks in Room ToString method
    public override string ToString()
    {
       StringBuilder result = new StringBuilder();
       for (int i = 0; i < MyMap.GetLength(0); i++)
       {
           StringBuilder row1 = new StringBuilder();
           StringBuilder row2 = new StringBuilder();
           StringBuilder row3 = new StringBuilder();
           for (int j = 0; j < MyMap.GetLength(1); j++)
           {
               // if statement only here for testing
               if (MyMap[i, j] != null)
               {
                   string[] arr = MyMap[i, j].GetDetails();
                   row1.Append(arr[0]);
                   row2.Append(arr[1]);
                   row3.Append(arr[2]);
               }
           }
           result.Append(row1.ToString()).Append('\n');
           result.Append(row2.ToString()).Append('\n');
           result.Append(row3.ToString()).Append('\n');
       }
       return result.ToString();



    }
    
}