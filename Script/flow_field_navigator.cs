using Godot;
using System;

public partial class flow_field_navigator : Node3D
{
	FlowField field;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		field = new(GetNode<HeightMap>("../HeightMap"), new Vector2i(10, 10), 1.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 direction = field.Sample(new Vector2i((int) GlobalPosition.x, (int) GlobalPosition.z));
		if (direction != new Vector2()) 
		{
			LookAt(new Vector3(Position.x + direction.x, 0, Position.y + direction.y));
		}

		Vector2 move = Input.GetVector("ui_left", "ui_right", "ui_down", "ui_up");
		Position += new Vector3(move.x, 0, move.y) * (float) delta;
		Position += new Vector3(direction.x, 0, direction.y) * (float) delta;
	}
}
