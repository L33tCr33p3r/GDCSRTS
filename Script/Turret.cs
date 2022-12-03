// Base class for all Unit's Turret Nodes
internal partial class Turret : Node3D
{
	// Fields (ALL OF THESE ARE PRIVATE)
	private double _rotationSpeed;
	private double? _rotationLimit;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Tries to aim the turret toward the Vector3 given. 'delta' is the elapsed time since the previous frame.
	// Returns true if it and all of it's muzzles can sucessfully aim at the target.
	public bool AimAtTarget(Vector3 target, double delta)
	{
		var isAimed = true;

		var directionToTarget = GlobalPosition.DirectionTo(target);
		var HorizontalAngleToTarget = Math.Atan2(directionToTarget.x, -directionToTarget.z);
		
		var rotationLimit = _rotationSpeed * delta;
		
		double targetRotation;
		if (_rotationLimit != null)
		{
			if (HorizontalAngleToTarget > _rotationLimit)
			{
				targetRotation = (double) _rotationLimit - Rotation.y;
				isAimed = false;
			}
			else if (HorizontalAngleToTarget < -_rotationLimit)
			{
				targetRotation = (double) -_rotationLimit - Rotation.y;
				isAimed = false;
			}
			else targetRotation = HorizontalAngleToTarget - Rotation.y;
		}
		else targetRotation = HorizontalAngleToTarget - Rotation.y;

		double amountToRotate;
		if (Math.Abs(targetRotation) > rotationLimit)
		{
			if (targetRotation < 0) amountToRotate = rotationLimit;
			else amountToRotate = -rotationLimit;
			isAimed = false;
		}
		else amountToRotate = targetRotation;

		RotateObjectLocal(Vector3.Up, (float) amountToRotate);

		foreach (Node child in GetChildren())
		{
			if (typeof(Muzzle).IsAssignableFrom(child.GetType()))
			{
				var muzzle = (Muzzle) child;
				if (!muzzle.ElevateToTarget(target, delta)) isAimed = false;
			}
		}

		return isAimed;
	}
}
