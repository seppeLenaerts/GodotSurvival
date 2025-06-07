using Godot;
using System;

namespace Game.Actors;

public partial class Player : CharacterBody2D
{
	public const float Speed = 100.0f;
	private Vector2 targetPos;

	public override void _PhysicsProcess(double delta)
	{
		if (GlobalPosition.DistanceTo(targetPos) < 1f)
		{
			Velocity = Vector2.Zero;
		}
		MoveAndSlide();
	}

	public void SetVelocityTowards(Vector2 pos)
	{
		Velocity = GlobalPosition.DirectionTo(pos) * Speed;
		targetPos = pos;
	}
}
