using Godot;
using System;

public partial class Level : Node
{
	public HeightMap Map { get; init; } = new(100, 0);
	public List<Unit> Units { get; set; } = new();

	public override void _Ready()
	{

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
