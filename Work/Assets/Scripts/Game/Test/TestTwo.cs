using UnityEngine;
using System.Collections;
using FrameWork;
using UnityEngine.UI;
using System;

public class TestTwo : BaseUI {

    private Button Button;

    public override UILayer GetUILayer()
    {
        return UILayer.Normal;
    }

    public override EnumUIType GetUIType()
    {
        return EnumUIType.TestTwo;
    }

    protected override void OnAwake()
    {
        Button = this.transform.FindChild("Button").GetComponent<Button>();
    }

    protected override void OnStart()
    {
        Button.onClick.AddListener(OnClickBTN);
    }

    private void OnClickBTN()
    {
        UIManager.Instance.OpenUICloseOthers(EnumUIType.TestOne);
    }

   

    
}
