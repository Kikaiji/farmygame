using Godot;
using System;

public partial class InvTest : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemId = GetChild(0) as TextEdit;
		Count = GetChild(1) as TextEdit;
	}

	private TextEdit ItemId;

	private TextEdit Count;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnItemTest()
	{
		InventoryManager.Instance.TryAddItem(ItemId.Text, Count.Text.ToInt());
	}

	public void OnRemoveTest()
	{
		InventoryManager.Instance.TryRemoveItem(ItemId.Text, Count.Text.ToInt());
	}
}

