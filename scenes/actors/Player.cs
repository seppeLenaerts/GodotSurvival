using Game.Scenes.Etc;
using Godot;

namespace Game.Scenes.Actors;

public partial class Player : CharacterBody2D
{

	[Export] private PackedScene popupTip;

	[Export] private AnimatedSprite2D animatedSprite2D;

	public const float Speed = 80.0f;
	private Vector2 targetPos;

	public override void _Ready()
	{
		PlayAnimation("idle");
	}

	public override void _PhysicsProcess(double delta)
	{
		if (GlobalPosition.DistanceTo(targetPos) < 0.5f)
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
		((PopupTip) popupRef).SetPopupText(text);
		AddChild(popupRef);
	}

	private void PlayAnimation(string name)
	{
		animatedSprite2D.Play(name);
	}
}
