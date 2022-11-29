using Godot;

public partial class Unit : Node3D
{
	public enum FireStance
	{
		/// <summary> Run away from enemies </summary>
		Flee,
		/// <summary> Do not attack enemies </summary>
		Hold,
		/// <summary> Attack enemies that attacked first </summary>
		Return,
		/// <summary> Attack any spotted enemies </summary>
		AtWill
	}

	// Fields (ALL OF THESE ARE PRIVATE)
	private readonly Guid _id = Guid.NewGuid(); // To be used for syncing with clients
	private int _health;
	private Queue<Order> _orders = new();
	private Order? _goal;
	private FireStance _stance;
	private Unit? _target;
	private Vector2? _targetLastSeen;
	private float _viewRange;

	// Properties (ALL OF THESE ARE PUBLIC)
	public Guid Id { get { return _id; } }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RunAI();
	}

	// Called when on the right client.
	private void RunAI()
	{
		EvaluateGoal();
		PerformGoal();
	}

	// Evaluates Order queue to decide what the unit's current goal is.
	private void EvaluateGoal()
	{
		var currentOrder = _orders.Peek();
	}

	// Tries to complete whatever the unit's current goal is
	private void PerformGoal()
	{
		if (_goal != null) // If the unit has a goal, tries to execute it.
		{
			
		}
		else // If the unit has no goal, decides what else to do
		{
			
		}
	}
}
