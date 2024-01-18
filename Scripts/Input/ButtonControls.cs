using Godot;
using System;

public partial class ButtonControls : Control
{
	public void BuildButton()
	{
		BuildManager.Instance.SetMode(BuildMode.Build);
	}

	public void DestroyButton()
	{
		BuildManager.Instance.SetMode(BuildMode.Destroy);
	}

	public void StopBuildButton()
	{
		BuildManager.Instance.SetMode(BuildMode.None);
	}

	public void SelectBuilding(int id)
	{
		BuildManager.Instance.ChooseBuilding(id);
	}
}
