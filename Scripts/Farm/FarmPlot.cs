using Godot;
using System;

public partial class FarmPlot : Building
{

	public PlantSlot[] plants; 
	private int dayCount;
	public bool occupied { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void Interact()
	{
		//Get Currently held item through event
		ItemData holding = null;

		HotbarSlot slot = InventoryManager.Instance.GetHeldItem();
		holding = ItemDatabase.Instance.GetItem(InventoryManager.Instance.GetInventoryItem(slot.referenceSlot.id));
		
		if (GetFirstAvailableSlot() == -1 || holding == null)
		{
			int grown = -1;
			if ((grown = GetFirstGrown()) >= 0)
			{
				if (InventoryManager.Instance.TryAddItem(plants[grown].plant.produceId) == -1)
				{
					HarvestPlant(grown);
					
				}
			}
			return;
		}

		if (holding.tags.Contains(ItemTags.Seed))
		{
			var plant = PlantDatabase.Instance.GetPlantBySeed(holding.itemId);
			if(plant != null) PlantSeed(GetFirstAvailableSlot(), plant, slot.referenceSlot.id);
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
		/*if (plant != null)
		{
			plantSprite.Texture = plant.plantSprite;
		}
		plant = JsonHandler.Instance.plantList[0];*/
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

	private void PlantSeed(int slot, PlantData plant, int invSlot)
	{
		plants[slot].AddPlant(plant);
		InventoryManager.Instance.TryRemoveItem(plant.seedId, 1, invSlot);
	}

	private void HarvestPlant(int slot)
	{
		if (InventoryManager.Instance.TryAddItem(plants[slot].plant.produceId, 1) == -1)
		{
			plants[slot].RemovePlant();
		}
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
		dayCount = 0;
		GameManager.onDayPassed += CountDay;
		Refresh();
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
		Refresh();
	}

	public void Refresh()
	{
		spriteSlot.Texture = plant.plantSprite;
	}
}
