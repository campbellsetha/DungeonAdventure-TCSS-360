using Godot;
using Godot.Collections;
namespace UDA.Game.Resources;

[GlobalClass]
public partial class RoomConverter : Resource
{
    [Export]
    // Possible room types with doors at specific locations
    public Godot.Collections.Dictionary<string, RoomTypeCollection.RoomType> baseRooms = new ()
    {
        { "*********", RoomTypeCollection.RoomType.NoDoor},
        { "*****|***", RoomTypeCollection.RoomType.OneDoorEast },
        { "*_*******", RoomTypeCollection.RoomType.OneDoorNorth },
        { "***|*****", RoomTypeCollection.RoomType.OneDoorWest },
        { "*******_*", RoomTypeCollection.RoomType.OneDoorSouth },
        { "*_*****_*", RoomTypeCollection.RoomType.TwoDoorNorthSouth },
        { "*_*|*****", RoomTypeCollection.RoomType.TwoDoorNorthWest },
        { "*_***|***", RoomTypeCollection.RoomType.TwoDoorNorthEast },
        { "***|***_*", RoomTypeCollection.RoomType.TwoDoorSouthWest },
        { "*****|*_*", RoomTypeCollection.RoomType.TwoDoorSouthEast },
        { "***|*|***", RoomTypeCollection.RoomType.TwoDoorEastWest },
        { "*_*|*|***", RoomTypeCollection.RoomType.ThreeDoorNorthEastWest },
        { "*_***|*_*", RoomTypeCollection.RoomType.ThreeDoorNorthSouthEast },
        { "*_*|***_*", RoomTypeCollection.RoomType.ThreeDoorNorthSouthWest },
        { "***|*|*_*", RoomTypeCollection.RoomType.ThreeDoorSouthEastWest },
        { "*_*|*|*_*", RoomTypeCollection.RoomType.FourDoor }
    };

    
    
}