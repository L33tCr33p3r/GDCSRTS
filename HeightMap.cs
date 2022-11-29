using Godot;
using System;

public partial class HeightMap : Node
{
	[Export]
	public int size = 1000;

	public float[,]? Ground { get; protected set; } // TODO: actually set something in these variables during terrain generation
	// public float[,] Water { get; protected set; }

	public override void _Ready()
	{
		generateData(0);
		generateMesh();
	}

	private void generateData(int seed, float hscale = 0.5f, float vscale = 5.0f) {
		Ground = new float[size, size];

		FastNoiseLite noise = new();
		noise.Seed = seed;
		noise.FractalOctaves = 10;
		// TODO: proper noise settings

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				Ground[x, y] = MathF.Pow(noise.GetNoise2d(x / hscale, y / hscale), 2.0f) * vscale;
			}
		}
	}

	private void generateMesh(float hscale = 0.5f) 
	{
		if (Ground == null)
		{
			GD.PushError("Expected data when generating mesh");
			throw new Exception();
		}

		// this gets us the vertices connected in the right way
		var plane = new PlaneMesh();
		plane.Size = new Vector2(size * hscale, size * hscale);
		plane.SubdivideDepth = size - 2; // subtract 2 so that the nuber of vertices actually matches the number of sample points
		plane.SubdivideWidth = size - 2;

		// convert the PlaneMesh into an ArrayMesh so that we can edit it
		var plane_mesh = new ArrayMesh();
		plane_mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, plane.SurfaceGetArrays(0));

		// the MeshDataTool makes it easier to manipulate the vertices
		var md = new MeshDataTool();
		md.CreateFromSurface(plane_mesh, 0); // for some reason this function only takes ArrayMesh

		// move each vertex based on the heightmap
		for (int i = 0; i < md.GetVertexCount(); i++)
		{
			Vector3 pos = md.GetVertex(i);
			md.SetVertex(i, new Vector3(pos.x, Ground[i % size, i / size], pos.z));
		}

		var final_mesh = new ArrayMesh();
		md.CommitToSurface(final_mesh);

		// automatically generate the correct normals
		var st = new SurfaceTool();
		st.CreateFrom(final_mesh, 0);
		st.GenerateNormals();
		final_mesh = st.Commit();

		// make the mesh cast shadows
		final_mesh.ShadowMesh = final_mesh;

		// add the mesh to the scene
		GetNode<MeshInstance3D>("MeshInstance3D").Mesh = final_mesh;
		GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D").Shape = final_mesh.CreateTrimeshShape();
	}
}
