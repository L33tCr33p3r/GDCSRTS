// Base class for all Turret's Muzzle Nodes
internal partial class Muzzle : Node3D
{
	// Fields (ALL OF THESE ARE PRIVATE)
	private double _rotationSpeed;
	private double _elevationLimitUp;
	private double _elevationLimitDown;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Tries to elevate the Muzzle towards target. 'delta' is the elapsed time since the previous frame.
	// Returns true if it can sucessfully aim at the target.
	public bool ElevateToTarget(Vector3 target, double delta)
	{
		var isAimed = true;

		var directionToTarget = GlobalPosition.DirectionTo(target);
		var horizontalDistanceToTarget = new Vector2(directionToTarget.x, directionToTarget.z).Length();
		var verticalAngleToTarget = Math.Atan2(directionToTarget.y, horizontalDistanceToTarget);

		var rotationLimit = _rotationSpeed * delta;

		double targetRotation;
		if (verticalAngleToTarget > _elevationLimitUp)
		{
			targetRotation = _elevationLimitUp - Rotation.y;
			isAimed = false;
		}
		else if (verticalAngleToTarget < _elevationLimitDown)
		{
			targetRotation = _elevationLimitDown - Rotation.y;
			isAimed = false;
		}
		else targetRotation = verticalAngleToTarget - Rotation.y;

		double amountToRotate;
		if (Math.Abs(targetRotation) > rotationLimit)
		{
			if (targetRotation < 0) amountToRotate = rotationLimit;
			else amountToRotate = -rotationLimit;
			isAimed = false;
		}
		else amountToRotate = targetRotation;

		RotateObjectLocal(Vector3.Right, (float)amountToRotate);

		return isAimed;
	}
}
