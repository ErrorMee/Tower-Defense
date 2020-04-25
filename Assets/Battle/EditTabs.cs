using Michsky.UI.ModernUIPack;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditTabs : MonoBehaviour
{
    [SerializeField] private HexMapEditor hexMapEditor;
    [SerializeField] private ModalWindowManager tabWindow;

    [SerializeField] private Toggle[] terrainToggles;

    [SerializeField] private Toggle heightToggle;
    [SerializeField] private SliderManager height;
    [SerializeField] private SliderManager brush;

    [SerializeField] private Toggle[] wallToggles;
    [SerializeField] private Toggle[] roadToggles;
    [SerializeField] private Toggle[] riverToggles;
    [SerializeField] private Toggle waterToggle;
    [SerializeField] private SliderManager water;

    [SerializeField] private Toggle urbanToggle;
    [SerializeField] private SliderManager urban;
    [SerializeField] private Toggle farmToggle;
    [SerializeField] private SliderManager farm;
    [SerializeField] private Toggle plantToggle;
    [SerializeField] private SliderManager plant;
    [SerializeField] private Toggle specialToggle;
    [SerializeField] private SliderManager special;

    [SerializeField] private NewMapMenu mapMenu;
    [SerializeField] private Button newBtn;
    [SerializeField] private Button loadBtn;
    [SerializeField] private SaveLoadMenu saveLoadMenu;
    [SerializeField] private Button saveBtn;

    private void Start()
    {
        for (int i = 0; i < terrainToggles.Length; i++)
        {
            SetToggleEvent(terrainToggles[i], hexMapEditor.SetTerrainTypeIndex, i - 1);
        }

        heightToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplyElevation(select);
        });

        height.sliderEvent.AddListener((value) => {
            hexMapEditor.SetElevation((int)value);
        });

        brush.sliderEvent.AddListener((value) => {
            hexMapEditor.SetBrushSize((int)value);
        });

        for (int i = 0; i < wallToggles.Length; i++)
        {
            SetToggleEvent(wallToggles[i], hexMapEditor.SetWalledMode, i);
        }
        for (int i = 0; i < roadToggles.Length; i++)
        {
            SetToggleEvent(roadToggles[i], hexMapEditor.SetRoadMode, i);
        }
        for (int i = 0; i < riverToggles.Length; i++)
        {
            SetToggleEvent(riverToggles[i], hexMapEditor.SetRiverMode, i);
        }

        waterToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplyWaterLevel(select);
        });
        water.sliderEvent.AddListener((value) => {
            hexMapEditor.SetWaterLevel((int)value);
        });


        urbanToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplyUrbanLevel(select);
        });
        urban.sliderEvent.AddListener((value) => {
            hexMapEditor.SetUrbanLevel((int)value);
        });

        farmToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplyFarmLevel(select);
        });
        farm.sliderEvent.AddListener((value) => {
            hexMapEditor.SetFarmLevel((int)value);
        });

        plantToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplyPlantLevel(select);
        });
        plant.sliderEvent.AddListener((value) => {
            hexMapEditor.SetPlantLevel((int)value);
        });

        specialToggle.onValueChanged.AddListener((select) => {
            hexMapEditor.SetApplySpecialIndex(select);
        });
        special.sliderEvent.AddListener((value) => {
            hexMapEditor.SetSpecialIndex((int)value);
        });

        newBtn.onClick.AddListener(() => { mapMenu.Open(); });
        loadBtn.onClick.AddListener(() => { saveLoadMenu.Open(false); });
        saveBtn.onClick.AddListener(() => { saveLoadMenu.Open(true); });
    }

    private void SetToggleEvent(Toggle toggle, Action<int> call, int index)
    {
        toggle.onValueChanged.AddListener((select) => {
            if (select)
            {
                call(index);
            }
        });
    }

    public void Show(bool isShow)
    {
        if (isShow)
        {
            tabWindow.OpenWindow();
        }
        else
        {
            tabWindow.CloseWindow();
        }
    }
}