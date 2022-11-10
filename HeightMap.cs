using Godot;
using System;

public partial class HeightMap : Node
{
	[Export]
	public int size = 100;

	public float[,] Ground { get; protected set; }
	public float[,] Water { get; protected set; }

	public override void _Ready()
	{
		generateData(0);
		generateMesh();
	}

	private void generateData(int seed, float scale = 1.0f)
	{
		Ground = new float[size, size];
		Water = new float[size, size];

		FastNoiseLite noise = new();
		noise.Seed = seed;
		
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				Ground[x, y] = noise.GetNoise2d(x * scale, y * scale);
			}
		}
	}

	private void generateMesh() 
	{
		var st = new SurfaceTool();
		st.Begin(Mesh.PrimitiveType.Triangles); // TODO: use tristrips instead
		st.SetColor(new Color(0.8f, 0.8f, 0.8f));
		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				st.AddVertex(new Vector3(x * 0.05f, Ground[x, y], y * 0.05f + 0.05f));
				st.AddVertex(new Vector3(x * 0.05f, Ground[x, y], y * 0.05f));
				st.AddVertex(new Vector3(x * 0.05f + 0.05f, Ground[x, y], y * 0.05f));
			}
		}

		st.GenerateNormals();

		GetNode<MeshInstance3D>("MeshInstance3D").Mesh = st.Commit();
	}
}
