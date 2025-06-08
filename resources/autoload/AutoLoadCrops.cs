using Game.Resources.Items;
using Godot;
using Godot.Collections;


namespace Game.Resources.Autoload;

[GlobalClass]
public partial class AutoLoadCrops : Node
{
	public static AutoLoadCrops Instance;

	[Export]
	public Array<CropResource> CropResources;

	public Array<CropResource> GetCropResources()
	{
		return CropResources;
	}

	public override void _Ready()
	{
		base._Ready();
		Instance ??= this;
	}

}
