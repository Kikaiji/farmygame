using Godot;
using System;
using System.Diagnostics;

public partial class Bed : Area3D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void InteractBehaviour()
	{
		GD.Print("Interacted with " + Name);
		GameManager.Instance.PassDay();
	}

	public void OnBodyEnter(Node3D body)
	{
		if (body is not PlayerMovement other) return;

		other.onPlayerInteract += InteractBehaviour;
		other.prompt.SetPrompt(Name);
	}

	public void OnBodyExit(Node3D body)
	{
		if (body is not PlayerMovement other) return;

		other.onPlayerInteract -= InteractBehaviour;
		other.prompt.SetPrompt("");
	}
}

