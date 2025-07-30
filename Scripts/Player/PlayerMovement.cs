using Godot;
using System;
using System.Reflection.Metadata.Ecma335;

public partial class PlayerMovement : CharacterBody3D
{
	[Export] 
	public int speed = 1;
	[Export]
	public float sprint = 1.3f;

	[Export] public float gravity = -8.97f;

	[Export] private Node3D cameraPivot;
	[Export] public InteractPrompt prompt;
	[Export] public AnimatedSprite3D sprite;

	public delegate void PlayerInteract();
	public event PlayerInteract onPlayerInteract;
	
	public override void _EnterTree(){
		InventoryManager.onItemDropped += DropItem;
	}
	
	public override void _ExitTree(){
		InventoryManager.onItemDropped -= DropItem;
	}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = ProcessMoveInput(delta);
		if(!IsOnFloor()) Velocity += new Vector3(0, gravity, 0);
		if(Input.IsActionJustPressed("player_interact")) Interact();
		if(Input.IsActionJustPressed("inv_toggle")) InventoryManager.Instance.ToggleInventory();
		MoveAndSlide();
	}

	private Vector3 ProcessMoveInput(double delta)
	{
		
		//Returns the result of player movement inputs
		
		Vector3 result = Vector3.Zero;

		if (Input.IsActionPressed("move_f")) result.X -= (float)(speed * delta);
		if (Input.IsActionPressed("move_b")) result.X += (float)(speed * delta);
		if (Input.IsActionPressed("move_l")) result.Z += (float)(speed * delta);
		if (Input.IsActionPressed("move_r")) result.Z -= (float)(speed * delta);
		
		//result = (Quaternion.FromEuler(new Vector3(0, ((cameraPivot.RotationDegrees.Y % 360) - 45), 0)) * new Vector3(result.X, 0f, result.Z)).Normalized();
		//GD.Print(result);
		if(Input.IsActionPressed("run")){
			if(result.Z != 0 && result.X == 0)
			{
				if(result.Z > 0){
					sprite.Play("RunL");
				}
				else{
					sprite.Play("RunR");
				}
			}
			
			if(result.X != 0)
			{
				if(result.X > 0)
				{
					sprite.Play("RunD");
				}
				else
				{
					sprite.Play("RunU");
				}
			}
			
			result = new Vector3(result.X * sprint, result.Y * sprint, result.Z * sprint);
		} else
		{
			if(result.Z != 0 && result.X == 0)
			{
				if(result.Z > 0){
					sprite.Play("WalkL");
				}
				else{
					sprite.Play("WalkR");
				}
			}
			
			if(result.X != 0)
			{
				if(result.X > 0)
				{
					sprite.Play("WalkD");
				}
				else
				{
					sprite.Play("WalkU");
				}
			}
		}
		
		
		if(result.X == 0 && result.Z == 0){
			sprite.Pause();
			sprite.SetFrameAndProgress(0, 0f);
		}

		result = result.Rotated(Vector3.Up, Mathf.DegToRad((cameraPivot.RotationDegrees.Y - 45) % 360));
		//GD.Print(result);
		
		return result;
	}

	private void Interact()
	{
		onPlayerInteract?.Invoke();
	}
	
	private void DropItem(GroundItem item){
		item.Position = this.Position;
		GetParent().AddChild(item);
	}

	public override void _Input(InputEvent @event)
	{
		base._Input(@event);
	}
}
