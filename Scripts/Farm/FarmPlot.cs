using Godot;
using System;
using System.Linq;

public partial class FarmPlot : Building
{

	public PlantSlot[] plants; 
	private int dayCount;
	public bool occupied { get; private set; }
	// Called when the node enters the scene tree for the first time.
	public override void Interact()
	{
		GD.Print("interact");
		//Get Currently held item through event
		ItemData holding = null;

		int slot = InventoryManager.Instance.GetHeldItem();
		if (slot != -1)
		{
			var item = InventoryManager.Instance.GetInventoryItemPlayer(slot);
			if(item != null) holding = ItemDatabase.Instance.GetItem(item);
			GD.Print(slot);
		}
		
		if (GetFirstAvailableSlot() == -1 || holding == null || !holding.tags.Contains(ItemTags.Seed))
		{
			int grown = -1;
			if ((grown = GetFirstGrown()) >= 0)
			{
				HarvestPlant(grown);
			}
			return;
		}

		if (holding.tags.Contains(ItemTags.Seed))
		{
			var plant = PlantDatabase.Instance.GetPlantBySeed(holding.itemId);
			if (plant != null)
			{
				PlantSeed(GetFirstAvailableSlot(), plant, slot);
				GD.Print("Planted " + plant);
			}
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
			plants[i].Refresh();
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
			if (plants[i].plant != null && plants[i].dayCount >= plants[i].plant.growTime) return i;
		}

		return -1;
	}

	private void PlantSeed(int slot, PlantData plant, int invSlot)
	{
		plants[slot].AddPlant(plant);
		InventoryManager.Instance.TryRemoveItemPlayer(plant.seedId, 1, invSlot);
	}

	private void HarvestPlant(int slot)
	{
		if (InventoryManager.Instance.TryAddItemPlayer(plants[slot].plant.produceId, 1) == -1)
		{
			plants[slot].RemovePlant();
			GD.Print("Harvested " + slot);
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
		if(plant != null)
		{
			spriteSlot.Texture = GetNextSprite();
			return;
		}
		spriteSlot.Texture = null;
	}

	private Texture2D GetNextSprite()
	{
		if (dayCount > plant.growTime) return plant.plantSprites.Last();

		return plant.plantSprites[dayCount];
	}
}
