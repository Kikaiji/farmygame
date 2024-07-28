using Godot;
using System;
using System.Collections.Generic;

public partial class Building : MeshInstance3D
{
	//dont need building functionality yet
	public Vector2 size = new Vector2(2,2);
	public List<BuildingPlot> tiles = new List<BuildingPlot>();
	public BuildingData data;

	public virtual void Interact()
	{
		throw new NotImplementedException();
	}	

	protected virtual void Setup()
	{
		size = data.size;
		Name = data.buildingName;

		//Scale = new Vector3(size.X * 2, 2, size.Y * 2);
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Setup();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}


