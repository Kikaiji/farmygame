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
	
	public delegate void TickPassed();
	public static event TickPassed onTick;
	
	public delegate void TimePassed();
	public static event TimePassed onTimePass;

	public int CurrentDay { get; private set; } = 0;

	public void PassDay()
	{
		onDayPassed?.Invoke();
		CurrentDay++;
	}
	
	public void Tick()
	{
		onTick?.Invoke();
	}
	
	public void PassTime()
	{
		onTimePass?.Invoke();
	}
}
