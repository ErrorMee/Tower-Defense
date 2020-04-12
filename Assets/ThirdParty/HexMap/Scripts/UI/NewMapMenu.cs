using UnityEngine;

public class NewMapMenu : MonoBehaviour {

	public HexGrid hexGrid;

	public HexMapGenerator mapGenerator;

	bool generateMaps = true;

	bool wrapping = true;

	public void ToggleMapGeneration (bool toggle) {
		generateMaps = toggle;
	}

	public void ToggleWrapping (bool toggle) {
		wrapping = toggle;
	}

	public void Open () {
		gameObject.SetActive(true);
		HexMapCamera.Locked = true;
	}

	public void Close () {
		gameObject.SetActive(false);
		HexMapCamera.Locked = false;
	}

	public void CreateSmallMap () {
		CreateMap(3, 4);
	}

	public void CreateMediumMap () {
		CreateMap(6, 8);
	}

	public void CreateLargeMap () {
		CreateMap(12, 16);
	}

	void CreateMap (int x, int z) {

        x *= HexMetrics.chunkSizeX;
        z *= HexMetrics.chunkSizeZ;

        if (generateMaps) {
			mapGenerator.GenerateMap(x, z, wrapping);
		}
		else {
			hexGrid.CreateMap(x, z, wrapping);
		}
		HexMapCamera.ValidatePosition();
		Close();
	}
}