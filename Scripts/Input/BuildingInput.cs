using Godot;
using System;

public partial class BuildingInput : StaticBody3D
{
	private MeshInstance3D mesh;
	private Material baseMat;
	public override void _Ready()
	{
		mesh = (MeshInstance3D) GetParent();
		baseMat = mesh.Mesh.SurfaceGetMaterial(0);
	}

	public override void _MouseEnter()
	{
		BuildManager.Instance.SelectBuilding((Building)GetParent());
		
		mesh.SetSurfaceOverrideMaterial(0, (Material)GD.Load("res://Materials/Test/Highlight.tres"));
		GD.Print("Selected new building");
	}

	public override void _MouseExit()
	{
		BuildManager.Instance.DeselectBuilding((Building)GetParent());
		
		mesh.SetSurfaceOverrideMaterial(0, baseMat);
		GD.Print("DelectedBuilding");
	}
}
