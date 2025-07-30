using Godot;
using System;
using System.Collections.Generic;

public partial class BuildManager
{
    private static BuildManager _instance;
    public static BuildManager Instance
    {
        get { return _instance ??= new BuildManager(); }
        private set => _instance = value;
    }

    public delegate void PlotSelected();
    public static event PlotSelected onPlotSelected;

    public delegate void PlotDeselected();
    public static event PlotDeselected onPlotDeselected;

    public delegate void ModeChanged();
    public static event ModeChanged onModeChanged;

    public delegate void PlotClicked(BuildingPlot plot);
    public static event PlotClicked onPlotClicked;
    
    public delegate void BuildingClicked(Building b);
    public static event BuildingClicked onBuildingClicked;

    public List<BuildingData> buildingList { get; private set; }
    //public List<PackedScene> buildingNodeList { get; private set; }

    public BuildMode currentMode { get; private set; } = BuildMode.None;
    public BuildingPlot selected{ get; private set; }
    public Building selectedBuilding { get; private set; }
    public BuildingData _currentBuilding{ get; private set; }
    public PackedScene _currentBuildingNode { get; private set; }
    public PackedScene _buildingNode { get; private set; }

    public void Setup()
    {
        buildingList = ResourceFileLoader.Instance.LoadFolder<BuildingData>("res://Resources/BuildingDatas/");
        //buildingNodeList = ResourceFileLoader.Instance.LoadFolder<PackedScene>("res://Nodes/Farm/Buildings/");
        _buildingNode = (PackedScene)GD.Load("res://Nodes/Farm/Buildings/test_building.tscn");
        _currentBuilding = buildingList[0];
        _currentBuildingNode = _currentBuilding.buildingNode;
        GD.Print(_buildingNode);
    }
    public void Select(BuildingPlot b)
    {
        if(selected != null) onPlotDeselected?.Invoke();
        selected = b;
        onPlotSelected?.Invoke();
        GD.Print(b.gridPosition);
    }

    public void Deselect(BuildingPlot b)
    {
        onPlotDeselected?.Invoke();
        if (selected == b) selected = null;
    }

    public void SelectBuilding(Building b)
    {
        selectedBuilding = b;
    }
    
    public void DeselectBuilding(Building b)
    {
        if (selectedBuilding == b) selectedBuilding = null;
    }

    public void ChooseBuilding(int id)
    {
        _currentBuilding = buildingList[id];
        _currentBuildingNode = buildingList[id].buildingNode;
    }

    public void SetMode(BuildMode mode)
    {
        currentMode = mode;
        onModeChanged?.Invoke();
        GD.Print(mode);
    }

    public void DebugSwitchMode()
    {
        switch (currentMode)
        {
            case BuildMode.Build:
                currentMode = BuildMode.Destroy;
                break;
            default:
                currentMode = BuildMode.Build;
                break;
        }
        
        GD.Print(currentMode);
    }

    public void ClickPlot(BuildingPlot plot)
    {
        onPlotClicked?.Invoke(plot);
    }

    public void ClickBuilding(Building b)
    {
        onBuildingClicked?.Invoke(b);
    }
}

public enum BuildMode
{
    Build,
    Destroy,
    None
}
