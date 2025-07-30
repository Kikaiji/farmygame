using Godot;
using System;

public partial class BuildingPlot : Area3D
{

	public BuildingPlot(){}
	public BuildingPlot(float x, float y)
	{
		gridPosition = new Vector2(x, y);
	}
	
	public bool occupied = false;
	[Export]
	public Vector2 gridPosition;

	private CompressedTexture2D baseSprite;
	private CompressedTexture2D highlightSprite;
	private Sprite3D sprite;
	
	public override void _Ready()
	{
		sprite = (Sprite3D)GetChild(1);
		highlightSprite = (CompressedTexture2D)GD.Load("res://Art/Sprites/Temp/dirtCenter.png");
		baseSprite = (CompressedTexture2D)GD.Load("res://Art/Sprites/Temp/grassCenter.png");
	}
	
	public void Occupy()
	{
		occupied = true;
		//SetSurfaceOverrideMaterial(0,(Material)GD.Load("res://Materials/Test/occupied.tres"));
	}

	public void DeOccupy()
	{
		occupied = false;
	}

	public void HighlightPlot()
	{
		sprite.Texture = highlightSprite;
		//SetSurfaceOverrideMaterial(0, (Material)GD.Load("res://Materials/Test/Highlight.tres"));
	}

	public void RemoveHighlight()
	{
		sprite.Texture = baseSprite;
		//SetSurfaceOverrideMaterial(0, (Material)GD.Load("res://Materials/Test/Base.tres"));
	}
	
	public override void _MouseEnter()
	{
		//mesh.SetSurfaceOverrideMaterial(0, highlight);
		BuildManager.Instance.Select(this);
	}

	public override void _MouseExit()
	{
		//mesh.SetSurfaceOverrideMaterial(0, baseMaterial);
		BuildManager.Instance.Deselect(this);
	}

	public override void _InputEvent(Camera3D camera, InputEvent @event, Vector3 position, Vector3 normal, int shapeIdx)
	{
		if (@event is InputEventMouseButton eventMouseButton &&
		    eventMouseButton.Pressed & eventMouseButton.ButtonIndex == MouseButton.Left)
		{
			BuildManager.Instance.ClickPlot(this);
		}
		base._InputEvent(camera, @event, position, normal, shapeIdx);
	}
}
