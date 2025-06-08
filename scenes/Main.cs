using System.Collections.Generic;
using Game.Resources.Items;
using Game.Scenes.Actors;
using Godot;

namespace Game.Scenes;

public partial class Main : Node2D
{

	private const string MOVE_LEFT = "move_left";
	private const string MOVE_RIGHT = "move_right";
	private const string MOVE_UP = "move_up";
	private const string MOVE_DOWN = "move_down";

	private const int GRID_SIZE = 16;

	[Export]
	private Player player;
	[ExportCategory("Tilemap Layers")]
	[Export]
	private TileMapLayer objectLayer;
	[Export]
	private TileMapLayer groundLayer;
	[Export]
	private TileMapLayer highlightLayer;

	public override void _Process(double delta)
	{
		HighlightHoveredTileIfWalkable();
	}

	public override void _Input(InputEvent inputEvent)
	{
		if (inputEvent is InputEventKey key)
		{
			HandleInputKey(key);
		}

		if (inputEvent is InputEventMouse mouse)
		{
			if (mouse.ButtonMask == MouseButtonMask.Left)
			{
				HandleMoveActionClick(mouse);
			}

			if (mouse.ButtonMask == MouseButtonMask.Right)
			{
				HandleHarvestAction();
			}
		}
	}

	private void HandleHarvestAction()
	{
		var gridPos = GetObjectGridCell(GetGlobalMousePosition());
		var tile = objectLayer.GetCellTileData(gridPos);
		if (tile != null && tile.HasCustomData("harvestable") && (bool)tile.GetCustomData("harvestable"))
		{
			CropResource resource = (CropResource)tile.GetCustomData("crop");
			if (IsPlayerInSurroundingCell(gridPos, resource.Radius))
			{
				objectLayer.SetCell(gridPos);
				player.AddPopup("+1 " + resource.Type);
				player.AddResource(resource);
			}
		}
	}

	private void HandleInputKey(InputEventKey key)
	{
		if (key.IsAction(MOVE_LEFT))
		{
			HandleMoveCommand(GetPlayerGridCell() - new Vector2I(1, 0));
		}
		else if (key.IsAction(MOVE_RIGHT))
		{
			HandleMoveCommand(GetPlayerGridCell() - new Vector2I(-1, 0));
		}
		else if (key.IsAction(MOVE_UP))
		{
			HandleMoveCommand(GetPlayerGridCell() - new Vector2I(0, 1));
		}
		else if (key.IsAction(MOVE_DOWN))
		{
			HandleMoveCommand(GetPlayerGridCell() - new Vector2I(0, -1));
		}
	}

	private void HandleMoveCommand(Vector2I targetPos)
	{
		player.SetVelocityTowards(targetPos * GRID_SIZE);
	}

	private void HandleMoveActionClick(InputEventMouse inputEventMouse)
	{
		var gridPos = GetObjectGridCell(GetGlobalMousePosition());
		if (IsCellWalkable(inputEventMouse.GlobalPosition))
		{
			player.SetVelocityTowards(gridPos * GRID_SIZE);
		}
	}

	private Vector2I GetObjectGridCell(Vector2 pos)
	{
		return objectLayer.LocalToMap(ToLocal(pos));
	}

	private Vector2I GetPlayerGridCell()
	{
		return GetObjectGridCell(player.GlobalPosition);
	}

	private bool IsPlayerInSurroundingCell(Vector2I gridCell, int radius)
	{
		var playerCell = GetPlayerGridCell();
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

	private void HighlightHoveredTileIfWalkable()
	{
		highlightLayer.Clear();
		var mousePos = GetGlobalMousePosition();
		var highlightCell = highlightLayer.LocalToMap(ToLocal(mousePos));

		if (IsCellWalkable(mousePos))
		{
			highlightLayer.SetCell(highlightCell, 0, new Vector2I(5, 5));
		}
	}

	private bool IsCellWalkable(Vector2 pos)
	{
		var groundCell = groundLayer.LocalToMap(ToLocal(pos));
		if (groundLayer.GetCellSourceId(groundCell) == -1)
		{
			return false;
		}
		else
		{
			return true;
		}
	}
}
