using System.Text;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.RoomType;

namespace UDA.Model.Map;

public class Room
{
    private const double MyChance = 0.1;
<<<<<<< Updated upstream

=======
    public RoomType MyRoomType { get; init; }
    private bool ContainsTrap { get; init; }
    public bool ContainsHealingPotion { get; init; }
    public bool ContainsVisionPotion { get; init; }
    internal Dictionary<Direction, BoundaryType?> MyBoundaries { get => _myBoundaries; set => _myBoundaries = value; }
    private Dictionary<Direction, BoundaryType?> _myBoundaries = 
                    new () 
                    {
                        { North, null },
                        { South, null },
                        { West, null },
                        { East, null }
                    };
    
>>>>>>> Stashed changes
    public Room(in RoomType theRoomType = Normal, params Direction[] theDoors)
    {
        MyRoomType = theRoomType;

        foreach (var dir in theDoors) MyBoundaries[dir] = BoundaryType.Door;

        foreach (var dir in MyBoundaries.Keys) MyBoundaries[dir] ??= BoundaryType.Wall;

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

        MyBoundaries = MyBoundaries;
    }

    private RoomType MyRoomType { get; }
    private bool ContainsTrap { get; }
    private bool ContainsHealingPotion { get; }
    private bool ContainsVisionPotion { get; }

    internal Dictionary<Direction, BoundaryType?> MyBoundaries { get; set; } = new()
    {
        { North, null },
        { South, null },
        { West, null },
        { East, null }
    };

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

    public string[] GetDetails()
    {
        var result = new string[3];
        result[0] = MyBoundaries.GetValueOrDefault(North).Equals(BoundaryType.Door) ? "*_*" : "***";
        result[1] = (MyBoundaries.GetValueOrDefault(West).Equals(BoundaryType.Door) ? "|" : "*") + GetLabel() +
                    (MyBoundaries.GetValueOrDefault(East).Equals(BoundaryType.Door) ? "|" : "*");
        result[2] = MyBoundaries.GetValueOrDefault(South).Equals(BoundaryType.Door) ? "*_*" : "***";
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