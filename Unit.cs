using Godot;
using System;

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

	public abstract record Order
	{
		public record Move(Vector2 To) : Order;
		public record Attack(Unit Target) : Order;
	}

	public Guid Id { get; } = Guid.NewGuid(); // To be used for syncing with clients
	public int Health { get; set; }
	public Queue<Order> Orders { get; set; } = new();
	public FireStance Stance { get; set; }
	public Unit? Target { get; set; }
	protected Vector2 _targetLastSeen { get; set; }
	protected float _viewRange;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
