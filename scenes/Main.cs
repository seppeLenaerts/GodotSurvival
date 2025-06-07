using System.Collections.Generic;
using Game.Actors;
using Godot;

namespace Game;

public partial class Main : Node2D
{

	private const int GRID_SIZE = 16;

	[Export]
	private Player player;

	[Export]
	private TileMapLayer objectLayer;
	[Export]
	private TileMapLayer highlightLayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HighlightHoveredTile();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventMouse mouse)
		{
			if (mouse.ButtonMask == MouseButtonMask.Left)
			{
				var gridPos = GetGridCell(mouse.GlobalPosition);
				player.SetVelocityTowards(gridPos * GRID_SIZE);
			}

			if (mouse.ButtonMask == MouseButtonMask.Right)
			{
				HandleHarvestAction(mouse);
			}
		}
	}

	private void HandleHarvestAction(InputEventMouse inputEventMouse)
	{
		var gridPos = GetGridCell(inputEventMouse.GlobalPosition);
		if (!IsPlayerInSurroundingCell(gridPos, 2))
		{
			return;
		}
		var tile = objectLayer.GetCellTileData(gridPos);
		if (tile != null && tile.HasCustomData("harvestable") && (bool)tile.GetCustomData("harvestable"))
		{
			objectLayer.SetCell(gridPos);
		}
	}

	private Vector2I GetGridCell(Vector2 pos)
	{
		return objectLayer.LocalToMap(ToLocal(pos));
	}

	private bool IsPlayerInSurroundingCell(Vector2I gridCell, int radius)
	{
		var playerCell = GetGridCell(player.GlobalPosition);
		var surroundingCells = new List<Vector2I>();
		for (int x = -1 * radius; x <= 1 * radius; x++)
		{
			for (int y = -1 * radius; y <= 1 * radius; y++)
			{
				surroundingCells.Add(new Vector2I(gridCell.X + x, gridCell.Y + y));
			}
		}
		return surroundingCells.Contains(playerCell);
	}

	private void HighlightHoveredTile()
	{
		highlightLayer.Clear();
		var mousePos = GetGlobalMousePosition();
		var cell = highlightLayer.LocalToMap(ToLocal(mousePos));
		highlightLayer.SetCell(cell, 0, new Vector2I(5, 5));
	}
}
