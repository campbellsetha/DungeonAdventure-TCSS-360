using System.Collections.Generic;
using System.Text;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.RoomType;

namespace UDA.Model.Map;
public class Room
{
    private const double MyChance = 0.1;
    private RoomType MyRoomType { get; init; }
    private  bool ContainsTrap { get; init; }
    private bool ContainsHealingPotion { get; init; }
    private bool ContainsVisionPotion { get; init; }
    
    private readonly Dictionary<Direction, BoundaryType?> _myBoundaries = 
                    new Dictionary<Direction, BoundaryType?> 
                    {
                        { North, null },
                        { South, null },
                        { West, null },
                        { East, null }
                    };
    
    public Room(in RoomType theRoomType = Normal, params Direction[] theDoors)
    {
        MyRoomType = theRoomType;
        
        foreach (Direction dir in theDoors)
        {
            _myBoundaries[dir] = BoundaryType.Door;
        }

        foreach (Direction dir in _myBoundaries.Keys)
        {
            _myBoundaries[dir] ??= BoundaryType.Wall;
        }

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
        return ContainsHealingPotion && (ContainsVisionPotion || ContainsTrap) 
               || ContainsVisionPotion && (ContainsHealingPotion || ContainsTrap) 
               || ContainsTrap && (ContainsVisionPotion || ContainsHealingPotion);
    }

    private char GetLabel() => 
        MyRoomType switch
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

    public string[] GetDetails()
    {
        string[] result = new string[3];
        result[0] = _myBoundaries.GetValueOrDefault(North).Equals(BoundaryType.Door) ? "*_*" : "***";
        result[1] = (_myBoundaries.GetValueOrDefault(West).Equals(BoundaryType.Door) ? "|" : "*") + GetLabel() + 
                    (_myBoundaries.GetValueOrDefault(East).Equals(BoundaryType.Door) ? "|" : "*");
        result[2] = _myBoundaries.GetValueOrDefault(South).Equals(BoundaryType.Door) ? "*_*" : "***";
        return result;
    }
    
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        string[] arr = GetDetails();
        foreach (var t in arr)
        {
            result.Append(t).Append('\n');
        }
        return result.ToString();
    }
    
    private enum BoundaryType
    {
        Door, Wall
    }

}