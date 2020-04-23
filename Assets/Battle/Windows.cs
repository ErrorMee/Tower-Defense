using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Windows : MonoBehaviour
{
    [SerializeField] private SwitchManager testMode;

    [SerializeField] private Transform editTransforms;
    [SerializeField] private Transform testTransforms;

    [SerializeField] private SwitchManager editSet;
    [SerializeField] private ModalWindowManager settingTabs;
    [SerializeField] private Button settingTabsCloseBtn;

    void Start()
    {
        testMode.OnEvents.AddListener(OnTest);
        testMode.OffEvents.AddListener(OffTest);
        OffTest();

        editSet.OnEvents.AddListener(OnEditSet);
        editSet.OffEvents.AddListener(OffEditSet);

        ClickListener.Get(settingTabsCloseBtn.gameObject).SetClickHandler(SettingTabsClose);
    }

    private void OnTest()
    {
        if (editSet.isOn)
        {
            editSet.AnimateSwitch();
            settingTabs.CloseWindow();
        }
        editTransforms.gameObject.SetActive(false);
        testTransforms.gameObject.SetActive(true);
    }

    private void OffTest()
    {
        editTransforms.gameObject.SetActive(true);
        testTransforms.gameObject.SetActive(false);
    }


    private void OnEditSet()
    {
        settingTabs.OpenWindow();
    }

    private void OffEditSet()
    {
        settingTabs.CloseWindow();
    }

    private void SettingTabsClose(GameObject gameObject)
    {
        if (editSet.isOn)
        {
            editSet.AnimateSwitch();
        }
    }
}
