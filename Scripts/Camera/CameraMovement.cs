using Godot;
using System;
using System.Diagnostics;
using System.Net.Http.Headers;

public partial class CameraMovement : Node3D
{

	[Export] private float _cameraRotation = 0f;
	[Export] private double _cameraZoom = 0f;
	private float _rotationGoal = 0f;

	private double currentLCooldown = 0f;
	private double currentRCooldown = 0f;
	[Export] private double rotationCooldown = 0.15f;

	private Camera3D _currentCamera;

	private bool _rotation = false;

	[Export] public Node3D focus;
	[Export] public float maxZoom;
	[Export] public float minZoom;
	[Export] public float rotationSpeed;
	[Export] public float zoomSpeed;

	private Tween rotationTween;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//_currentCamera = GetChild(0).GetNode<Camera3D>("Camera3D");
		rotationTween = GetTree().CreateTween();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("cameraRotateLeft")) RotateClockWise();
		if(Input.IsActionPressed("cameraRotateRight")) RotateAntiClockWise();

		currentLCooldown -= delta;
		currentRCooldown -= delta;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = focus.GlobalPosition;
	}

	private void TweenCameraToGoal(float addition)
	{
		float difference = 0;
		int sign = -1;
		if (_rotation)
		{
			rotationTween.Kill();

			
			_rotationGoal = ClampDegrees(_rotationGoal);
			
			difference = Mathf.Abs(Mathf.Abs(_rotationGoal) - Mathf.Abs(RotationDegrees.Y));
			if (_rotationGoal > RotationDegrees.Y) sign = 1;
		}
		

		//rotationTween ??= GetTree().CreateTween();

		
		_cameraRotation = addition + (difference * sign);
		_rotationGoal = _cameraRotation + RotationDegrees.Y;

		/*
		GD.Print("Goal " + _rotationGoal);
		GD.Print("Camera Rotation " + _cameraRotation);
		GD.Print("Difference " + difference);
		GD.Print("Current Y " + RotationDegrees.Y);
		*/
		
		_rotation = true;	
		rotationTween = GetTree().CreateTween();
		
		rotationTween.Finished += RotationFinished;
		rotationTween.TweenProperty(this, "rotation_degrees", new Vector3(0, _cameraRotation, 0), rotationSpeed)
			.AsRelative()
			.SetTrans(Tween.TransitionType.Expo)
			.SetEase(Tween.EaseType.Out);
	}

	private void RotateClockWise()
	{
		if (currentLCooldown > 0) return;
		
		//_cameraRotation += 45;
		currentLCooldown = rotationCooldown;
		TweenCameraToGoal(45);
	}

	private void RotateAntiClockWise()
	{
		if (currentRCooldown > 0) return;
		
		//_cameraRotation -= 45;
		currentRCooldown = rotationCooldown;
		TweenCameraToGoal(-45);
	}

	private void RotationFinished()
	{
		_rotation = false;
		_cameraRotation = 0;
		rotationTween.Finished -= RotationFinished;
	}

	private float ClampDegrees(float degrees)
	{
		if (degrees > 180) return (degrees % -180) - 180;
		if (degrees < -180) return (degrees % 180) + 180;

		return degrees;
	}
	
}
