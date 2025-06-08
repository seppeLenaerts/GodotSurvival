using Game.Resources.Autoload;
using Game.Resources.Items;
using Game.Scenes.Etc;
using Godot;
using Godot.Collections;

namespace Game.Scenes.Actors;

public partial class Player : CharacterBody2D
{

	[Export] private PackedScene popupTip;
	[Export] private AnimatedSprite2D animatedSprite2D;

	private Dictionary<CropResource, int> inventory = [];

	public const float Speed = 80.0f;
	private Vector2 targetPos;

	public override void _Ready()
	{
		PlayAnimation("idle");
		InitInventory();
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GlobalPosition.DistanceTo(targetPos) < 1f)
		{
			Velocity = Vector2.Zero;
			GlobalPosition = targetPos;
			PlayAnimation("idle");
		}
		MoveAndSlide();
	}

	public void SetVelocityTowards(Vector2 pos)
	{
		Velocity = GlobalPosition.DirectionTo(pos) * Speed;
		targetPos = pos;
		PlayAnimation("move");
	}

	public void AddPopup(string text)
	{
		var popupRef = popupTip.Instantiate();
		((PopupTip)popupRef).SetPopupText(text);
		AddChild(popupRef);
	}

	public void AddResource(CropResource resource)
	{
		inventory[resource] += 1;
		GD.Print(inventory[resource]);
	}

	private void PlayAnimation(string name)
	{
		animatedSprite2D.Play(name);
	}

	private void InitInventory()
	{
		foreach (var resource in AutoLoadCrops.Instance.CropResources)
		{
			inventory.Add(resource, 0);
		}
	}
}
