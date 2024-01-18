using Godot;
using System;
using LitJson;

public partial class PlantData : Resource, ISavable
{
	public int growTime;
	public string plantName;
	public CompressedTexture2D plantSprite;
	public JsonData ObjectData { get; set; }
	public void ConstructObject()
	{
		growTime = (int)ObjectData["growTime"];
		plantName = (string) ObjectData["plantName"];
		plantSprite = GD.Load<CompressedTexture2D>("res://Art/Sprites/Plants/turnip test.png");
		GD.Print("Created plant " + plantName);
		//something something convert whatever to sprite
	}
}
