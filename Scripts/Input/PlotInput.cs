using Godot;
using System;

public partial class PlotInput : StaticBody3D
{
	public MeshInstance3D mesh;
	public Material highlight;
	public Material baseMaterial;
	public override void _MouseEnter()
	{
		//mesh.SetSurfaceOverrideMaterial(0, highlight);
		BuildManager.Instance.Select((BuildingPlot)GetParent());
	}

	public override void _MouseExit()
	{
		//mesh.SetSurfaceOverrideMaterial(0, baseMaterial);
		BuildManager.Instance.Deselect((BuildingPlot)GetParent());
	}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		mesh = (MeshInstance3D)GetParent();
		baseMaterial = (Material) GD.Load("res://Materials/Test/Base.tres");
		highlight = (Material) GD.Load("res://Materials/Test/Highlight.tres");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
