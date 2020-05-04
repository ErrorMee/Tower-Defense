using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuTest : MonoBehaviour
{
    [SerializeField] private SwitchManager grid;

    public Material terrainMaterial;

    private void Start()
    {
        grid.OnEvents.AddListener(OnGrid);
        grid.OffEvents.AddListener(OffGrid);
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (isShow)
        {
            if (grid.isOn)
            {
                terrainMaterial.EnableKeyword("GRID_ON");
            }
            else {
                terrainMaterial.DisableKeyword("GRID_ON");
            }
        }
    }

    private void OnGrid()
    {
        terrainMaterial.EnableKeyword("GRID_ON");
    }

    private void OffGrid()
    {
        terrainMaterial.DisableKeyword("GRID_ON");
    }
}
