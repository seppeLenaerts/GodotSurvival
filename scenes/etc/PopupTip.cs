using Godot;

namespace Game.Scenes.Etc;

public partial class PopupTip : Control
{

	[Export] private Timer timer;
	[Export] private Label label;

	public override void _Ready()
	{
		timer.Timeout += Destroy;
	}

	public override void _Process(double delta)
	{
		label.GlobalPosition += new Vector2(0, -0.2f);
	}

	public void SetPopupText(string text)
	{
		label.Text = text;
	}

	private void Destroy()
	{
		QueueFree();
	}
}
