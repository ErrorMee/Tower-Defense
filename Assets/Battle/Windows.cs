using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Windows : MonoBehaviour
{
    [SerializeField] private SwitchManager menuSwitch;

    [SerializeField] private MenuEdit menuEdit;
    [SerializeField] private MenuTest menuTest;

    [SerializeField] private HexMapEditor hexMapEditor;
    [SerializeField] private HexGameUI gameUI;
    [SerializeField] private TouchController touchController;
    void Start()
    {
        menuSwitch.OnEvents.AddListener(OnMenu);
        menuSwitch.OffEvents.AddListener(OffMenu);
        OffMenu();
    }

    private void OnMenu()
    {
        menuEdit.Show(false);
        menuTest.Show(true);

        hexMapEditor.SetEditMode(false);
        gameUI.SetEditMode(false);
        touchController.SetEditMode(false);
    }

    private void OffMenu()
    {
        menuEdit.Show(true);
        menuTest.Show(false);

        hexMapEditor.SetEditMode(true);
        gameUI.SetEditMode(true);
        touchController.SetEditMode(true);
    }
}
