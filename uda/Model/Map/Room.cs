using System.Text;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.RoomType;
using static UDA.Model.Map.BoundaryType;

namespace UDA.Model.Map;

public class Room
{
    private const string VerticalDoor = "|";
    private const string VerticalWall = "*";
    private const string HorizontalDoor = "*_*";
    private const string HorizontalWall = "***";
    private const double MyChance = 0.1;
    public RoomType MyRoomType { get; }
    public bool ContainsTrap { get; protected set; }
    public bool ContainsHealingPotion { get; protected set; }
    public bool ContainsVisionPotion { get; protected set; }
    public Dictionary<Direction, BoundaryType?> MyBoundaries { get; } = new()
    {
        { North, null },
        { South, null },
        { West, null },
        { East, null }
    };
    
    public Room(in RoomType theRoomType = Normal, params Direction[] theDoors)
    {
        if (!Enum.IsDefined(typeof(RoomType), theRoomType))
            throw new ArgumentException("Room type not defined");
        const int maxDoors = 4;
        if (theDoors.Length > maxDoors)
            throw new ArgumentException("Rooms can only have four doors");
        if (theDoors.Any(door => !Enum.IsDefined(typeof(Direction), door)))
        {
            throw new ArgumentException("Door not defined");
        }
        MyRoomType = theRoomType;
        foreach (var dir in theDoors) MyBoundaries[dir] = Door;
        foreach (var dir in MyBoundaries.Keys) MyBoundaries[dir] ??= Wall;
        if (theRoomType != Entrance && theRoomType != Exit)
        {
            ContainsTrap = RandomSingleton.GetInstance().NextDouble() > 1 - MyChance;
            ContainsHealingPotion = RandomSingleton.GetInstance().NextDouble() > 1 - MyChance;
            ContainsVisionPotion = RandomSingleton.GetInstance().NextDouble() > 1 - MyChance;
        }
        else
        {
            ContainsTrap = false;
            ContainsHealingPotion = false;
            ContainsVisionPotion = false;
        }
    }
    
    private bool ContainsMultipleItems()
    {
        return (ContainsHealingPotion && (ContainsVisionPotion || ContainsTrap))
               || (ContainsVisionPotion && (ContainsHealingPotion || ContainsTrap))
               || (ContainsTrap && (ContainsVisionPotion || ContainsHealingPotion));
    }

    private char GetLabel()
    {
        return MyRoomType switch
        {
            Entrance => 'i',
            Exit => 'O',
            _ when ContainsMultipleItems() => 'M',
            PillarA => 'A',
            PillarE => 'E',
            PillarI => 'I',
            PillarP => 'P',
            Normal when ContainsHealingPotion => 'H',
            Normal when ContainsVisionPotion => 'V',
            Normal when ContainsTrap => 'X',
            _ => ' '
        };
    }

    internal int GetNumberOfDoors()
    {
        var numOfDoors = 0;
        foreach (var kvp in MyBoundaries)
        {
            if (kvp.Value == Door)
            {
                numOfDoors++;
            }
        }

        return numOfDoors;
    }

    public string[] GetDetails()
    {
        const int numOfLines = 3;
        var result = new string[numOfLines];
        
        result[0] = MyBoundaries[North] == Door ? HorizontalDoor : HorizontalWall;
        result[1] = (MyBoundaries[West] == Door ? VerticalDoor : VerticalWall) + GetLabel() +
                    (MyBoundaries[East] == Door ? VerticalDoor : VerticalWall);
        result[2] = MyBoundaries[South] == Door ? HorizontalDoor : HorizontalWall;
        return result;
    }

    public override string ToString()
    {
        var result = new StringBuilder();
        var arr = GetDetails();
        foreach (var t in arr) result.Append(t).Append('\n');
        return result.ToString();
    }
}