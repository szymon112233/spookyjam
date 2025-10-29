// using System.Collections.Generic;
using Godot;
using Godot.Collections;

public partial class NPCColourPicker : MeshInstance3D
{
	[Export] private int randomSeed = -1;
	[Export] private ColorPalette _colorPalette;
	[Export] private Array<int> _skinColorsWhitelist;
	[Export] private Dictionary<int, int>  _skinColorsWhitelistWithWeight;

	private System.Collections.Generic.List<Color> _usableColorsBuffer = new();

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

		

		int totalWeights = 0;

		foreach (var keyValuePair in _skinColorsWhitelistWithWeight)
		{
			totalWeights += keyValuePair.Value;
		}
		
		int randomNumber = (int)(GD.Randi() % totalWeights);

		int index = 0;

		int value = -1;
		foreach (var keyValuePair in _skinColorsWhitelistWithWeight)
		{
			value += keyValuePair.Value;
			if (value >= randomNumber)
			{
				index = keyValuePair.Key;
				break;
			}
		}

		for (int i = 0; i < _colorPalette.Colors.Length; i++)
		{
			if (colorArray.Contains(i) == useItAsAWhitelist)
			{
				_usableColorsBuffer.Add(_colorPalette.Colors[i]);
			}
		}
		
		GD.Print(index);

		StandardMaterial3D uniqueMaterial = (StandardMaterial3D)standardMaterial.Duplicate();
		// int randomIndex = (int)(GD.Randi() % (uint)_usableColorsBuffer.Count);
		// Color randomColor = _usableColorsBuffer[randomIndex];
		Color randomColor = _colorPalette.Colors[index];
		uniqueMaterial.AlbedoColor = randomColor;
		Mesh.SurfaceSetMaterial(materialIndex, uniqueMaterial);
	}
}
