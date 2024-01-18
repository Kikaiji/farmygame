using Godot;
using System;
using System.Net.Mime;
using GodotPlugins.Game;

public partial class InteractPrompt : Label3D
{
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//LookAt(GetViewport().GetCamera3D().GlobalPosition);
		
	}

	public void SetPrompt(string text)
	{
		Text = text;
	}
}
