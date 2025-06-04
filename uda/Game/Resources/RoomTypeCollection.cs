using Godot;

[GlobalClass]
public partial class RoomTypeCollection : Resource
{
    public enum RoomType
    {
        OneDoorEast,
        OneDoorNorth,
        OneDoorWest,
        OneDoorSouth,
        TwoDoorNorthSouth,
        TwoDoorNorthWest,
        TwoDoorNorthEast,
        TwoDoorSouthWest,
        TwoDoorSouthEast,
        TwoDoorEastWest,
        ThreeDoorNorthEastWest,
        ThreeDoorNorthSouthEast,
        ThreeDoorNorthSouthWest,
        ThreeDoorSouthEastWest,
        FourDoor
    }

    [Export]
    public Godot.Collections.Dictionary<RoomType, PackedScene> RoomDictionary { get; set; } = new()
    {
        { RoomType.OneDoorEast, ResourceLoader.Load<PackedScene>("res://Rooms/OneDoorEast.tscn") },
        { RoomType.OneDoorNorth, ResourceLoader.Load<PackedScene>("res://Rooms/OneDoorNorth.tscn") },
        { RoomType.OneDoorSouth, ResourceLoader.Load<PackedScene>("res://Rooms/OneDoorSouth.tscn") },
        { RoomType.OneDoorWest, ResourceLoader.Load<PackedScene>("res://Rooms/OneDoorWest.tscn") },
        { RoomType.TwoDoorEastWest, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorEastWest.tscn") },
        { RoomType.TwoDoorNorthEast, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorNorthEast.tscn") },
        { RoomType.TwoDoorNorthSouth, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorNorthSouth.tscn") },
        { RoomType.TwoDoorNorthWest, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorNorthWest.tscn") },
        { RoomType.TwoDoorSouthEast, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorSouthEast.tscn") },
        { RoomType.TwoDoorSouthWest, ResourceLoader.Load<PackedScene>("res://Rooms/TwoDoorSouthWest.tscn") },
        {
            RoomType.ThreeDoorNorthEastWest, ResourceLoader.Load<PackedScene>("res://Rooms/ThreeDoorNorthEastWest.tscn")
        },
        {
            RoomType.ThreeDoorNorthSouthEast,
            ResourceLoader.Load<PackedScene>("res://Rooms/ThreeDoorNorthSouthEast.tscn")
        },
        {
            RoomType.ThreeDoorNorthSouthWest,
            ResourceLoader.Load<PackedScene>("res://Rooms/ThreeDoorNorthSouthWest.tscn")
        },
        {
            RoomType.ThreeDoorSouthEastWest, ResourceLoader.Load<PackedScene>("res://Rooms/ThreeDoorSouthEastWest.tscn")
        },
        { RoomType.FourDoor, ResourceLoader.Load<PackedScene>("res://Rooms/FourDoor.tscn") }
    };
}