using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class NPCColourPicker : MeshInstance3D
{
	[Export] private ColorPalette _colorPalette;
	[Export] private Array<int> _skinColorsWhitelist;

	private List<Color> _usableColorsBuffer = new();

	public override void _Ready()
	{
		Mesh = (Mesh)Mesh.Duplicate();
		
		SetMaterialColor(0, _skinColorsWhitelist, true);
		SetMaterialColor(1, _skinColorsWhitelist, false);
		SetMaterialColor(2, _skinColorsWhitelist, false);
		SetMaterialColor(3, _skinColorsWhitelist, false);
	}

	private void SetMaterialColor(int materialIndex, Array<int> colorArray, bool useItAsAWhitelist)
	{
		Material material = Mesh.SurfaceGetMaterial(materialIndex);

		if (material is not StandardMaterial3D standardMaterial)
		{
			GD.PrintErr($"Surface {materialIndex} does not have a StandardMaterial3D.");
			return;
		}
		
		_usableColorsBuffer.Clear();

		for (int i = 0; i < _colorPalette.Colors.Length; i++)
		{
			if (colorArray.Contains(i) == useItAsAWhitelist)
			{
				_usableColorsBuffer.Add(_colorPalette.Colors[i]);
			}
		}

		StandardMaterial3D uniqueMaterial = (StandardMaterial3D)standardMaterial.Duplicate();
		int randomIndex = (int)(GD.Randi() % (uint)_usableColorsBuffer.Count);
		Color randomColor = _usableColorsBuffer[randomIndex];
		uniqueMaterial.AlbedoColor = randomColor;
		Mesh.SurfaceSetMaterial(materialIndex, uniqueMaterial);
	}
}
