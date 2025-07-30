using Godot;
using System;

public partial class ItemSpawnTest : Node3D
{
	[Export] private ItemData testItem;
	[Export] private int testCount;
	
	private GroundItem groundItem;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		groundItem = GroundItemRequest.Instance.NewGroundItem(testItem, testCount);
		groundItem.Position = this.Position;
		AddChild(groundItem);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
