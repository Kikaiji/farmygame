using Godot;
using System;

public partial class InventorySlot : Button
{
	private TextureRect sprite;
	private Label count;
	public int id;
	public override void _Ready()
	{
		sprite = (TextureRect)GetChild(0);
		count = (Label) GetChild(1);
	}

	public void RefreshSlot(InventoryItem item)
	{
		if(item != null)
		{
			sprite.Texture = item.item.itemSprite;
			count.Text = item.Count.ToString();
		}
		else
		{
			sprite.Texture = null;
			count.Text = "";
		}
	}
	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
