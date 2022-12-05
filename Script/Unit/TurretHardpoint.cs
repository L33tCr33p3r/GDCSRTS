// Utility class to reposition turrets relative to their unit, and to give turrets a more acurate relative position when aiming
internal partial class TurretHardpoint : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	// Tries to aim the Hardpoint's Turret toward the given position in global space.
	// Returns true if the turret is aimed properly, false otherwise.
	public bool Aim(Vector3 target, double delta)
	{
		var isAimed = true;

		var targetRelativePosition = Basis * target;

		foreach (Node child in GetChildren())
		{
			if (typeof(Turret).IsAssignableFrom(child.GetType()))
			{
				var turret = (Turret) child;
				if (!turret.AimAtTarget(targetRelativePosition, delta)) isAimed = false;
			}
		}

		return isAimed;
	}
}
