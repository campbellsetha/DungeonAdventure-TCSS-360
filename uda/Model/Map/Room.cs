using System.Collections.Generic;
using System.Text;
using static UDA.Model.Map.BorderType;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.RoomType;

namespace UDA.Model.Map;

public class Room
{
    private static readonly double MyChance = 0.1;
    public RoomType RoomType { get; init; }
    public Dictionary<Direction, string> Boundaries { get; init; }
    // can probably combine these into one tuple
    public bool ContainsTrap { get; init; }
    public bool ContainsHealingPotion { get; init; }
    public bool ContainsVisionPotion { get; init; }
    
    public Room(RoomType theRoomType, BorderType theBorderType)
    {
        RoomType = theRoomType;
        Boundaries = GenerateBoundaries(theBorderType);

        if (theRoomType == Normal)
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

    private Dictionary<Direction, string> GenerateBoundaries(BorderType theBorderType)
    {
        Dictionary<Direction, string> result = new Dictionary<Direction, string>
        {
            { North, null },
            { South, null },
            { West, null },
            { East, null }
        };
        
        if (theBorderType is TopLeft or TopRight or Top) result[North] = "wall";
        if (theBorderType is BottomLeft or BottomRight or Bottom) result[South] = "wall";
        if (theBorderType is BottomLeft or TopLeft or Left) result[West] = "wall";
        if (theBorderType is BottomRight or TopRight or Right) result[East] = "wall";

        foreach (var kvp in result)
        {
            if (RandomSingleton.GetInstance().Next(0, 2) == 0 && result[kvp.Key] == null) result[kvp.Key] = "door";
            else if (result[kvp.Key] == null) result[kvp.Key] = "wall";
        }

        return result;
    }

    private bool ContainsMultipleItems()
    {
        return ContainsHealingPotion && (ContainsVisionPotion || ContainsTrap) 
               || ContainsVisionPotion && (ContainsHealingPotion || ContainsTrap) 
               || ContainsTrap && (ContainsVisionPotion || ContainsHealingPotion);
    }

    private char GetLabel() => 
        RoomType switch
        {
            Entrance => 'i',
            Exit => 'O',
            PillarA => 'A',
            PillarE => 'E',
            PillarI => 'I',
            PillarP => 'P',
            Normal when ContainsMultipleItems() => 'M',
            Normal when ContainsHealingPotion => 'H',
            Normal when ContainsVisionPotion => 'V',
            Normal when ContainsTrap => 'X',
            _ => ' '
        };

    public string[] GetDetails()
    {
        string[] result = new string[3];
        result[0] = Boundaries.GetValueOrDefault(North).Equals("door") ? "*_*" : "***";
        result[1] = (Boundaries.GetValueOrDefault(West).Equals("door") ? "|" : "*") + GetLabel() + 
                    (Boundaries.GetValueOrDefault(East).Equals("door") ? "|" : "*");
        result[2] = Boundaries.GetValueOrDefault(South).Equals("door") ? "*_*" : "***";
        return result;
    }
    
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        string[] arr = GetDetails();
        for (int i = 0; i < arr.Length; i++)
        {
            result.Append(arr[i]).Append('\n');
        }
        return result.ToString();
    }
    
}