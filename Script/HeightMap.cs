// Does HeightMap things
internal partial class HeightMap : Node
{
	[Export]
	public int size = 400;

	public float[,]? Ground { get; protected set; } // TODO: actually set something in these variables during terrain generation
	// public float[,] Water { get; protected set; }
	public FlowField field; // DEBUG

	public override void _EnterTree()
	{
		GenerateData(0);
		field = new(this, new Vector2i(size / 2, size / 2), Vector2.Zero, 0.2f); // DEBUG
		GenerateMesh();
	}

	private void GenerateData(int seed, float hscale = 0.5f, float vscale = 15.0f) {
		Ground = new float[size, size];

		var noise = new FastNoiseLite
		{
			Seed = seed,
			FractalOctaves = 6
		};
		// TODO: proper noise settings

		for (int x = 0; x < size; x++)
		{
			for (int y = 0; y < size; y++)
			{
				Ground[x, y] = MathF.Pow(noise.GetNoise2d(x / hscale, y / hscale), 2.0f) * vscale;
			}
		}
	}

	private void GenerateMesh() 
	{
		if (Ground == null)
		{
			GD.PushError("Expected data when generating mesh");
			throw new Exception();
		}

		// this gets us the vertices connected in the right way
		var plane = new PlaneMesh
		{
			Size = new Vector2(1, 1),
			SubdivideDepth = size - 2, // subtract 2 so that the nuber of vertices actually matches the number of sample points
			SubdivideWidth = size - 2
		};

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
			md.SetVertex(i, new Vector3(i % size, Ground[i % size, i / size], i / size));

			// DEBUG: flowfield based coloring
			Color color = new Color(0, 0, 1);

			// var distance = field.DistanceField[i % size, i / size];
			// if (distance == null)
			// {
			// 	color = new(1, 0.2f, 0.2f);
			// }
			// else
			// {
			// 	color = new((float)distance * 0.1f % 1, (float)distance * 0.1f % 1, (float)distance * 0.1f % 1);
			// }

			Vector2 direction = field.FlowPathSample(this, new Vector2i(i % size, i / size), 0.2f);
			if (direction != new Vector2(0, 0)) 
			{
				color = new Color((direction.x * 0.5f) + 0.5f, (direction.y * 0.5f) + 0.5f, 0);
			}

			md.SetVertexColor(i, color);
			// END DEBUG
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
