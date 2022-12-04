using Godot;
using System;

internal partial class PlayerView : Node3D
{
	[Export]
	float Speed = 10;

	Vector2 _mouse_motion = new();
	Node3D? _pivot;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_pivot = GetNode<Node3D>("Pivot");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 input_dir = Input.GetVector("left", "right", "forward", "backward"); // get movement input
		var input_vec = new Vector3(input_dir.x, 0, input_dir.y); // turn it into a Vector3
		Vector3 move_dir = Basis * input_vec; // rotate it to the player's forward direction
		move_dir.y = 0; // make it horizontal only
		move_dir = move_dir.Normalized() * Speed; // set its length to the player speed

		Position += move_dir * (float) delta; // apply the movement

		_pivot!.GlobalPosition = new Vector3(GlobalPosition.x, GetNode<RayCast3D>("RayCast3D").GetCollisionPoint().y, GlobalPosition.z);

		RotateY(_mouse_motion.x * (float) delta); // TODO: make this rotate on global y instead of local
		_pivot!.RotateX(_mouse_motion.y * (float) delta);
		// RotateObjectLocal(new Vector3(1, 0, 0), _mouse_motion.y * (float) delta); // this should rotate the camera, but that has a weird problem
		// var camera = GetNode<Camera3D>("Camera"); // TODO: see if this is a bug that should be reported
		// var rotation_x = _mouse_motion.y * (float) delta;
		// var rotation_basis = new Basis(new Vector3(1, 0, 0), rotation_x);
		// camera.Basis *= rotation_basis;
		
		_mouse_motion = new Vector2(0, 0);
	}

	public override void _UnhandledInput(InputEvent e)
	{
		if (Input.IsActionPressed("pan")) 
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
			if (e is InputEventMouseMotion motion)
			{
				_mouse_motion = -motion.Relative;
			}
		}
		else
		{
			Input.MouseMode = Input.MouseModeEnum.Visible;
		}
	}
}
