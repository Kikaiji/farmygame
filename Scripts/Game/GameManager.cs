using Godot;
using System;

public class GameManager
{
	private static GameManager _instance;

	public static GameManager Instance
	{
		get { return _instance ??= new GameManager(); }
		set => _instance = value;
	}

	public delegate void DayPassed();
	public static event DayPassed onDayPassed;

	public int CurrentDay { get; private set; } = 0;

	public void PassDay()
	{
		onDayPassed?.Invoke();
		CurrentDay++;
	}
}
