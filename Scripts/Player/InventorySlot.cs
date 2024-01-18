using Godot;
using System;

public partial class InventorySlot : Button
{
	private TextureRect sprite;
	public int id;
	public override void _Ready()
	{
		sprite = (TextureRect)GetChild(0);
	}

	public void RefreshSlot(InventoryItem item)
	{
		throw new NotImplementedException();
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
