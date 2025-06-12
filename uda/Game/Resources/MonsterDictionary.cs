using Godot;
using System;

[GlobalClass]
public partial class MonsterDictionary : Resource
{
	[Export]
	public Godot.Collections.Dictionary<string, PackedScene> RoomDictionary { get; set; } = new()
	{
		{"Gremlin", ResourceLoader.Load<PackedScene>("res://Game/Enemies/Gremlin.tscn") },
		{"Ogre", ResourceLoader.Load<PackedScene>("res://Game/Enemies/Ogre.tscn")},
		{"Skeleton", ResourceLoader.Load<PackedScene>("res://Game/Enemies/Skeleton.tscn")},
		{"Boss", ResourceLoader.Load<PackedScene>("res://Game/Enemies/BossMonster.tscn")}
	};
	
	
}
