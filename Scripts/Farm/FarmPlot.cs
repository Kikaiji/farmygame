using Godot;
using System;

public partial class FarmPlot : Building
{
	[Export] public PlantData plant;
	[Export] public Sprite3D plantSprite;

	public PlantSlot[] plants; 
	private int dayCount;
	public bool occupied { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void Interact()
	{
		throw new NotImplementedException();
		//Get Currently held item through event
		ItemData holding = null;

		if (InventoryManager.Instance.GetHeldItem() != null)
			holding = ItemDatabase.Instance.GetItem(InventoryManager.Instance.GetHeldItem());
		
		if (GetFirstAvailableSlot() == -1 || holding == null)
		{
			int grown = -1;
			if ((grown = GetFirstGrown()) >= 0)
			{
				if (InventoryManager.Instance.TryAddItem(plants[grown].plant.produceId) == -1)
				{
					plants[grown].RemovePlant();
				}
			}
		}

		if (holding.tags.Contains(ItemTags.Seed))
		{
			//plant
		}
		
		//Check tags of held item to determine what to do.
		//there are slots and holding seeds, plant the seeds in the first available slot.
		//there are no slots and holding seeds, try and harvest plants if any are done.
		//not holding seeds, try and harvest plants if any are done.
	}

	public override void _Ready()
	{
		Setup();
		plants = new PlantSlot[9];

		var nodes = GetChild(1).GetChildren();

		for (int i = 0; i < nodes.Count; i++)
		{
			plants[i] = new PlantSlot(nodes[i]);
		}
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (plant != null)
		{
			plantSprite.Texture = plant.plantSprite;
		}
		plant = JsonHandler.Instance.plantList[0];
	}

	private int GetFirstAvailableSlot()
	{
		for (int i = 0; i < plants.Length; i++)
		{
			if (plants[i].plant == null) return i;
		}

		return -1;
	}

	private int GetFirstGrown()
	{
		for (int i = 0; i < plants.Length; i++)
		{
			if (plants[i].dayCount >= plants[i].plant.growTime) return i;
		}

		return -1;
	}
}

public class PlantSlot
{
	public PlantData plant;
	public Sprite3D spriteSlot;
	public int dayCount;

	public PlantSlot(){ }

	public PlantSlot(Node sprite)
	{
		spriteSlot = sprite as Sprite3D;
		dayCount = 0;
		
	}

	public void AddPlant(PlantData data)
	{
		plant = data;
		spriteSlot.Texture = data.plantSprite;
		dayCount = 0;
		GameManager.onDayPassed += CountDay;
	}

	public void RemovePlant()
	{
		plant = null;
		spriteSlot.Texture = null;
		GameManager.onDayPassed -= CountDay;
	}
	
	public void CountDay()
	{
		dayCount++;
	}
}
