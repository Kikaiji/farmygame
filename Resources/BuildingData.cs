using Godot;
using System;

public partial class BuildingData : Resource
{
    [Export]
    public Vector2 size { get; set; }

    [Export] public string buildingName;

    [Export] public PackedScene buildingNode;

    public BuildingData() : this(new Vector2(2, 2), "", null) {}
    
    

    public BuildingData(Vector2 Size, string BuildingName, PackedScene BuildingNode)
    {
        size = Size;
        buildingName = BuildingName;
        buildingNode = BuildingNode;
    }
    
}
