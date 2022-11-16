using Godot;
using System;

public partial class HeightMap : Node
{
	[Export]
	public int size = 200;

	// public float[,] Ground { get; protected set; } // TODO: actually set something in these variables during terrain generation
	// public float[,] Water { get; protected set; }

	public override void _Ready()
	{
		generateMesh(0);
	}

	private void generateMesh(int seed, float hscale = 20.0f, float vscale = 1.0f) 
	{
		var plane = new PlaneMesh();
		plane.Size = new Vector2(6, 6);
		plane.SubdivideDepth = size;
		plane.SubdivideWidth = size;

		var plane_mesh = new ArrayMesh();
		plane_mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, plane.SurfaceGetArrays(0));

		var md = new MeshDataTool();
		// st.Begin(Mesh.PrimitiveType.Triangles);
		md.CreateFromSurface(plane_mesh, 0); // for some reason this function only takes ArrayMesh

		FastNoiseLite noise = new();
		noise.Seed = seed;
		noise.FractalOctaves = 10;

		for (int i = 0; i < md.GetVertexCount(); i++)
		{
			Vector3 pos = md.GetVertex(i);
			float height = noise.GetNoise2d(pos.x * hscale, pos.z * hscale);
			md.SetVertex(i, new Vector3(pos.x, height * vscale, pos.z));
		}

		var final_mesh = new ArrayMesh();
		md.CommitToSurface(final_mesh);

		var st = new SurfaceTool();
		st.CreateFrom(final_mesh, 0);
		st.GenerateNormals();
		final_mesh = st.Commit();

		final_mesh.ShadowMesh = final_mesh;

		GetNode<MeshInstance3D>("MeshInstance3D").Mesh = final_mesh;
		GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D").Shape = final_mesh.CreateTrimeshShape();
	}
}
