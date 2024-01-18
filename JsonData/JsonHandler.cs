using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LitJson;

public partial class JsonHandler
{
	public List<PlantData> plantList = new List<PlantData>();
	
	private static JsonHandler _instance;
	public static JsonHandler Instance
	{
		get { return _instance ??= new JsonHandler(); }
		private set => _instance = value;
	}
	
	public void PullJson()
	{
		plantList = ParseJson<PlantData>(JsonMapper.ToObject(FileAccess.GetFileAsString("res://JsonData/Plants.json")));
	}

	public List<T> ParseJson<T>(JsonData data) where T : ISavable, new()
	{
		List<T> result = new List<T>();
		for (int i = 0; i < data.Count; i++)
		{
			var NewObject = new T {ObjectData = data[i]};
			NewObject.ConstructObject();
			result.Add(NewObject);
		}

		GD.Print(data);
		GD.Print(result);
		return result;
	}
}

public interface ISavable
{
	JsonData ObjectData { get; set; }
	public abstract void ConstructObject();

}
