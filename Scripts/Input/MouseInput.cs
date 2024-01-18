using Godot;
using System;

public partial class MouseInput : Camera3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	
	
	private void SendRay()
	{
		RayCast3D ray = new RayCast3D();
		
	}
}
