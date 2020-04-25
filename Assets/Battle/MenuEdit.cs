using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuEdit : MonoBehaviour
{
    [SerializeField] private SwitchManager tab;
    [SerializeField] private SwitchManager grid;
    [SerializeField] private EditTabs tabWindow;
    [SerializeField] private Button tabWindowCloseBtn;

    public Material terrainMaterial;

    void Start()
    {
        tab.OnEvents.AddListener(OnTab);
        tab.OffEvents.AddListener(OffTab);
        grid.OnEvents.AddListener(OnGrid);
        grid.OffEvents.AddListener(OffGrid);
        tabWindowCloseBtn.onClick.AddListener(TabWindowClose);
    }

    private void OnTab()
    {
        tabWindow.Show(true);
    }

    private void OffTab()
    {
        tabWindow.Show(false);
    }

    private void OnGrid()
    {
        terrainMaterial.EnableKeyword("GRID_ON");
    }

    private void OffGrid()
    {
        terrainMaterial.DisableKeyword("GRID_ON");
    }

    public void Show(bool isShow)
    {
        gameObject.SetActive(isShow);
        if (!isShow && tab.isOn)
        {
            tab.AnimateSwitch();
            tabWindow.Show(false);
        }
        if (isShow)
        {
            if (grid.isOn)
            {
                terrainMaterial.EnableKeyword("GRID_ON");
            }
            else
            {
                terrainMaterial.DisableKeyword("GRID_ON");
            }
        }
    }

    private void TabWindowClose()
    {
        if (tab.isOn)
        {
            tab.AnimateSwitch();
        }
    }
}
