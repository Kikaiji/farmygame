using Godot;
using System;

public partial class FarmPlot : Building
{
	[Export] public PlantData plant;
	[Export] public Sprite3D plantSprite;
	private int dayCount;
	public bool occupied { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void Interact()
	{
		throw new NotImplementedException();
	}

	public override void _Ready()
	{
		Setup();
		plant = JsonHandler.Instance.plantList[0];
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (plant != null)
		{
			plantSprite.Texture = plant.plantSprite;
		}
		plant = JsonHandler.Instance.plantList[0];
	}
}
