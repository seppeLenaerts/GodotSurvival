using Godot;

namespace Game.Resources.Items;

[GlobalClass]
public partial class CropResource : Resource
{
	[Export] public string Type;
	[Export] public int Radius;
}
