// Base class for all Turret's Muzzle Nodes
internal partial class Muzzle : Node3D
{

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Tries to elevate the Muzzle towards target. 'delta' is the elapsed time since the previous frame.
	public void ElevateToTarget(Vector3 target, double delta)
	{
		var directionToTarget = GlobalPosition.DirectionTo(target);
		var horizontalDistanceToTarget = new Vector2(directionToTarget.x, directionToTarget.z).Length();
		var verticalAngleToTarget = Math.Atan2(directionToTarget.y,horizontalDistanceToTarget );

		var targetRotation = verticalAngleToTarget - Rotation.x;
	}
}
