using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Vector2 = Godot.Vector2;

public partial class BuildingGrid : Node3D
{
	[Export]
	public PackedScene PlotNode;
	public BuildingPlot[,] Grid;
	

	[Export] public int gridSizeX, gridSizeY;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Grid = new BuildingPlot[gridSizeX, gridSizeY];
		//var b = PlotNode.Instantiate() as BuildingPlot;
		BuildManager.Instance.Setup();
		CreateGrid();;
		
	}

	public override void _EnterTree()
	{
		BuildManager.onPlotSelected += Highlight;
		BuildManager.onPlotDeselected += UnHighlight;
		BuildManager.onModeChanged += HandleModeChange;
		BuildManager.onPlotClicked += HandlePlotClick;
		BuildManager.onBuildingClicked += HandleBuildingClick;
	}

	public override void _ExitTree()
	{
		BuildManager.onPlotSelected -= Highlight;
		BuildManager.onPlotDeselected -= UnHighlight;
		BuildManager.onModeChanged -= HandleModeChange;
		BuildManager.onPlotClicked -= HandlePlotClick;
		BuildManager.onBuildingClicked += HandleBuildingClick;
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionPressed("debugSwitchMode")){BuildManager.Instance.DebugSwitchMode();}
	}

	private void CreateGrid()
	{
		GD.Print("size " + gridSizeX + ", " + gridSizeY);
		
		for (int ix = 0; ix < gridSizeX; ix++)
		{
			for (int iy = 0; iy < gridSizeY; iy++)
			{
				//var b = new BuildingPlot(ix, iy);
				//GD.Print("x" + ix + ", y" + iy);
				var b = PlotNode.Instantiate() as BuildingPlot;
				Grid[ix, iy] = b;
				
				GetChild(0).AddChild(b);
				b.Name = ("GridTile " + ix + ", " + iy);
				//b.Mesh = new PlaneMesh();
				
				b.Position += new Vector3(ix * 2, 0, iy * 2);
				b.gridPosition = new Vector2(ix, iy);
			}
		}
		
		HandleModeChange();
	}
	private bool CheckArea(List<BuildingPlot> plotlist)
	{

		foreach (var p in plotlist)
		{
			if (p == null || p.occupied)
			{
				return false;
			}
		}

		return true;
	}
	private List<BuildingPlot> GetArea(Vector2 start, Vector2 size)
	{
		List<BuildingPlot> result = new List<BuildingPlot>();
		
		for (int ix = 0; ix < size.X; ix++)
		{
			for (int iy = 0; iy < size.Y; iy++)
			{
				int xpos = (int) (start.X + ix);
				int ypos = (int) (start.Y + iy);
				
				//GD.Print(xpos + ", " + ypos);
				if (xpos >= gridSizeX || ypos >= gridSizeY)
				{
					result.Add(null);
					continue;
				}
				result.Add(Grid[xpos, ypos]);
				
			}
		}

		return result;
	}
	private void CreateBuilding(BuildingData data, BuildingPlot p)
	{
		GD.Print(data);
		GD.Print(p);
		var plotList = GetArea(p.gridPosition, data.size);
		var check = CheckArea(plotList);

		GD.Print(p.gridPosition);
		if (!check) return;

		var b = BuildManager.Instance._currentBuildingNode.Instantiate() as Building;
		b.data = data;

		foreach (var plot in plotList)
		{
			plot.Occupy();
		}

		GetChild(1).AddChild(b);
		
		b.Position = p.Position + new Vector3(b.size.X -1, 0, b.size.Y -1);
		b.tiles = plotList;
	}

	private void DestroyBuilding(Building b)
	{
		foreach (var p in b.tiles)
		{
			p.DeOccupy();
		}
		BuildManager.Instance.DeselectBuilding(b);
		b.QueueFree();
	}

	public void ShowGrid()
	{
		foreach (var b in Grid)
		{
			b.Visible = true;
			b.InputRayPickable = true;
		}
	}

	public void HideGrid()
	{
		foreach (var b in Grid)
		{
			b.Visible = false;
			b.InputRayPickable = false;
		}
	}

	public void Highlight()
	{
		var area = GetArea(BuildManager.Instance.selected.gridPosition, BuildManager.Instance._currentBuilding.size);
		foreach (var p in area)
		{
			p?.HighlightPlot();
		}
	}

	public void UnHighlight()
	{
		var area = GetArea(BuildManager.Instance.selected.gridPosition, BuildManager.Instance._currentBuilding.size);;
		foreach (var p in area)
		{
			p?.RemoveHighlight();
		}
	}
	
	private void HandlePlotClick(BuildingPlot plot)
	{
		if (BuildManager.Instance.currentMode == BuildMode.Build && 
			BuildManager.Instance.selected != null && 
			BuildManager.Instance._currentBuilding != null)
		{
			CreateBuilding(BuildManager.Instance._currentBuilding,
				BuildManager.Instance.selected);
		}
	}

	private void HandleBuildingClick(Building b)
	{
		if (BuildManager.Instance.currentMode == BuildMode.Destroy)
		{
			DestroyBuilding(b);
		}
	}

	public void HandleModeChange()
	{
		if (BuildManager.Instance.currentMode != BuildMode.None)
		{
			ShowGrid();
		}
		else
		{
			HideGrid();
		}
	}
}
