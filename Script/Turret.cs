// Base class for all Unit's Turret Nodes
internal partial class Turret : Node3D
{
	// Fields (ALL OF THESE ARE PRIVATE)
	private double _rotationSpeed;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Tries to rotate the turret toward the Vector3 given. 'delta' is the elapsed time since the previous frame.
	public void AimAtTarget(Vector3 target, double delta)
	{
		var directionToTarget = GlobalPosition.DirectionTo(target);
		var HorizontalAngleToTarget = Math.Atan2(directionToTarget.x, -directionToTarget.z);

		var targetRotation = HorizontalAngleToTarget - Rotation.y;

		double amountToRotate;
		if (Math.Abs(targetRotation) > _rotationSpeed)
		{
			if (targetRotation < 0) amountToRotate = _rotationSpeed;
			else amountToRotate = -_rotationSpeed;
		}
		else amountToRotate = targetRotation;

		amountToRotate *= delta;

		RotateObjectLocal(new Vector3(0, 1, 0), (float) amountToRotate);

		foreach (Node3D child in GetChildren())
		{
			if (child.GetType() == typeof(Muzzle))
			{
				var muzzle = (Muzzle) child;
				muzzle = 
			}
		}
	}
}
