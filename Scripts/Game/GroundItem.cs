using Godot;
using System;

public partial class GroundItem : Node3D
{
	[Export] private Sprite3D ItemSprite;
	private Area3D MagnetTrigger;
	private Area3D CollectionTrigger;
	
	[Export] private ItemData CurrentItem;
	[Export] private int count;
	
	private bool _detected;
	private bool _homing;
	
	private float speed = 0.08f;
	private double homingDelay = 0;
	
	private Node3D player;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_detected = false;
		_homing = false;
		
		InventoryManager.onInventoryModified += InventoryWatch;
	}
	
	public void Setup(ItemData item, int itemCount, double delay){
		CurrentItem = item;
		count = itemCount;
		ItemSprite.Texture = CurrentItem.itemSprite;
		homingDelay = delay;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(homingDelay > 0){
			homingDelay -= delta;
		}
		if(_homing){
			LookAt(player.GetGlobalPosition(), Vector3.Up);
			SetGlobalPosition(GetGlobalPosition() + Basis.Z * -speed);
		}
	}
	

	
	private void InventoryWatch(){
		if(_detected) 
		{
			_homing = true;
		}
	}
	
	public void MagnetEnter(Node3D collider){
		if(collider is not PlayerMovement && homingDelay <= 0) return;
		player = collider;
		GD.Print("Enter");
		
		_detected = true;
		_homing = true;
	}
	
	public void MagnetExit(Node3D collider){
		if(collider is not PlayerMovement && homingDelay <= 0) return;
		GD.Print("Exit");
		
		_detected = false;
		_homing = false;
	}
	
	public void CollectionTriggered(Node3D collider){
		if(collider is not PlayerMovement && homingDelay <= 0) return;
		
		int result = InventoryManager.Instance.TryAddItemPlayer(CurrentItem.itemId, count);
		
		if(result < -1){
			_homing = false;
			GD.Print("None Added");
		} else if (result == -1)
		{
			GD.Print("All Added");
			InventoryManager.onInventoryModified -= InventoryWatch;
			QueueFree();
				
		} else if (result >= 0)
		{
			count = result;
			_homing = false;
			GD.Print("Some Added");
		}
		
		GD.Print("Collect");
	}
}
