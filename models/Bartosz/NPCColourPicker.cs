using Godot;

public partial class NPCColourPicker : MeshInstance3D
{
	public override void _Ready()
	{
		Mesh = (Mesh)Mesh.Duplicate();

		// Apply random colors to all materials
		for (int i = 0; i < Mesh.GetSurfaceCount(); i++)
		{
			var material = Mesh.SurfaceGetMaterial(i);

			if (material is StandardMaterial3D standardMaterial)
			{
				// Duplicate the material to avoid shared references
				var uniqueMaterial = (StandardMaterial3D)standardMaterial.Duplicate();

				// Generate a random color
				Color randomColor = new Color(GD.Randf(), GD.Randf(), GD.Randf());

				// Assign the random color to the duplicated material
				uniqueMaterial.AlbedoColor = randomColor;

				// Apply the unique material back to the surface
				Mesh.SurfaceSetMaterial(i, uniqueMaterial);
			}
			else
			{
				GD.PrintErr($"Surface {i} does not have a StandardMaterial3D.");
			}
		}
	}
}
