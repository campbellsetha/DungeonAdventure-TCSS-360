using System;
using System.Collections.Generic;
using System.Text;
using static UDA.Model.Map.Direction;
using static UDA.Model.Map.RoomType;

namespace UDA.Model.Map;

public class Room
{
    private const double MyChance = 0.1;
    public RoomType RoomType { get; init; }
    // can probably combine these into one tuple
    public bool ContainsTrap { get; init; }
    public bool ContainsHealingPotion { get; init; }
    public bool ContainsVisionPotion { get; init; }

    private readonly  (int myX, int myY) _myCoordinates;
    private static Dictionary<Direction, string> _myBoundaries = 
                    new Dictionary<Direction, string> 
                    {
                        { North, null },
                        { South, null },
                        { West, null },
                        { East, null }
                    };
    
    public Room(int theX, int theY, RoomType theRoomType = Normal)
    {
        RoomType = theRoomType;
        _myCoordinates = (theX, theY);
        _myBoundaries = GenerateBoundaries();

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

    private Dictionary<Direction, string> GenerateBoundaries()
    {

        if (_myCoordinates.myX == 0) _myBoundaries[North] = "wall";
        if (_myCoordinates.myX == Dungeon.Rows - 1) _myBoundaries[South] = "wall";
        if (_myCoordinates.myY == 0) _myBoundaries[West] = "wall";
        if (_myCoordinates.myY == Dungeon.Cols - 1) _myBoundaries[East] = "wall";

        foreach (var kvp in _myBoundaries)
        {
            if (RandomSingleton.GetInstance().Next(0, 2) == 0 && _myBoundaries[kvp.Key] == null) _myBoundaries[kvp.Key] = "door";
            else if (_myBoundaries[kvp.Key] == null) _myBoundaries[kvp.Key] = "wall";
        }

        return _myBoundaries;
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
        result[0] = _myBoundaries.GetValueOrDefault(North).Equals("door") ? "*_*" : "***";
        result[1] = (_myBoundaries.GetValueOrDefault(West).Equals("door") ? "|" : "*") + GetLabel() + 
                    (_myBoundaries.GetValueOrDefault(East).Equals("door") ? "|" : "*");
        result[2] = _myBoundaries.GetValueOrDefault(South).Equals("door") ? "*_*" : "***";
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