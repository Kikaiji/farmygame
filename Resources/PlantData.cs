using Godot;
using System;
using System.Data;
using LitJson;

public partial class PlantData : Resource, ISavable
{
	[Export] public int growTime;
	[Export] public string plantName;
	[Export] public Texture2D plantSprite;
	[Export] public string seedId;
	[Export] public string produceId;
	
	public JsonData ObjectData { get; set; }
	
	public void ConstructObject()
	{
		growTime = (int)ObjectData["growTime"];
		plantName = (string) ObjectData["plantName"];
		plantSprite = GD.Load<Texture2D>("res://Art/Sprites/Plants/turnip test.png");
		GD.Print("Created plant " + plantName);
		//something something convert whatever to sprite
	}

	public PlantData()
	{
		growTime = -1;
		plantName = null;
		plantSprite = null;
		seedId = null;
		produceId = null;
	}

	public PlantData(int GrowTime, string PlantName, Texture2D PlantSprite, string SeedId, string ProduceId)
	{
		growTime = GrowTime;
		plantName = PlantName;
		plantSprite = PlantSprite;
		seedId = SeedId;
		produceId = ProduceId;
	}
}
