using Godot;

namespace Sitter.Scripts;

public partial class Seat : Node2D
{
	[Export()] public bool Occupied;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Occupied = false;
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
