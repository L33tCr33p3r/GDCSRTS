using Godot;
using System;

public partial class flow_field_navigator : Node3D
{
	FlowField field;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		field = GetNode<HeightMap>("../HeightMap").field;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction = field.Sample(new Vector2i((int) GlobalPosition.x, (int) GlobalPosition.z));
		if (direction != new Vector2i()) 
		{
			LookAt(new Vector3(GlobalPosition.x + direction.x, GlobalPosition.y, GlobalPosition.z + direction.y));
		}
		else
		{
			LookAt(new Vector3(GlobalPosition.x, 10, GlobalPosition.z));
		}

		Position += new Vector3(direction.x, 0, direction.y) * (float)delta * 2;
		GlobalPosition = new Vector3(Position.x, GetNode<HeightMap>("../HeightMap").Ground[(int) GlobalPosition.x, (int) GlobalPosition.z] + 1.0f, Position.z);
	}
}
