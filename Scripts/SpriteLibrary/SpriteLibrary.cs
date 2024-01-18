using Godot;
using System;
using System.Collections.Generic;

public partial class SpriteLibrary
{
	//something to grab sprites without having to constantly refer to exact files.
	private static SpriteLibrary _instance;
	public static SpriteLibrary Instance
	{
		get { return _instance ??= new SpriteLibrary(); }
		private set => _instance = value;
	}
	
	
	
	private Dictionary<string, SpriteSet> _library = new Dictionary<string, SpriteSet>();

	public Dictionary<string, SpriteSet> Library
	{
		get => _library;
		private set => _library = value;
	}

	public SpriteSet GetSpriteSet(string key)
	{
		return Library[key];
	}
}

public class SpriteSet
{
	public List<CompressedTexture2D> Sprites = new List<CompressedTexture2D>();
}
