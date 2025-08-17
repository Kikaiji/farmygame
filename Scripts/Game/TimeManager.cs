using Godot;
using System;

public partial class TimeManager : Node
{
	[Export] private double _TickRate = 20;
	[Export] private double _TimeChunk = 30;
	
	private double _indvTickSpeed;
	private double _indvTimeSpeed;
	
	public int TickCount {get; private set;} = 0;
	
	public override void _Ready(){
		_indvTickSpeed = 1 / _TickRate;
		_indvTimeSpeed = _TimeChunk;
	}
	
	public override void _Process(double delta){
		_indvTickSpeed -= delta;
		_indvTimeSpeed -= delta;
		
		if(_indvTickSpeed <= 0){
			GameManager.Instance.Tick();
			TickCount++;
			_indvTickSpeed = (1 / _TickRate) + _indvTickSpeed;
		}
		if(_indvTimeSpeed <= 0){
			GameManager.Instance.PassTime();
			_indvTimeSpeed = _TimeChunk + _indvTimeSpeed;
		}
	}
}
