using Godot;
using System;
using System.Collections.Generic;

public partial class PlantDatabase
{
	private static PlantDatabase _instance;

	public static PlantDatabase Instance
	{
		get { return _instance ??= new PlantDatabase(); }
		private set => _instance = value;
	}
	
	public Dictionary<string, PlantData> PlantList { get; private set; }

	public void Setup()
	{
		PlantList = ResourceFileLoader.Instance.LoadFolderAsDict<PlantData>("res://Resources/PlantDatas/");
	}

	public PlantData GetPlant(string plantId)
	{
		if(PlantList == null) Setup();
		
		PlantData result;
		if (PlantList.TryGetValue(plantId, out result)) return result;
		return null;
	}

	public PlantData GetPlantBySeed(string seedId)
	{
		if(PlantList == null) Setup();
		
		foreach (var plant in PlantList)
		{
			if (plant.Value.seedId == seedId) return plant.Value;
		}

		return null;
	}
}
