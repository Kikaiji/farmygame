using Godot;
using System;

public partial class DayCounter : Label
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Text = GameManager.Instance.CurrentDay.ToString();
	}

	public override void _EnterTree()
	{
		GameManager.onDayPassed += UpdateDayCounter;
	}

	public override void _ExitTree()
	{
		GameManager.onDayPassed -= UpdateDayCounter;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void UpdateDayCounter()
	{
		Text = GameManager.Instance.CurrentDay.ToString();
	}
}
