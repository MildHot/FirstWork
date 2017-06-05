using UnityEngine;
using System.Collections;
using FrameWork;
using UnityEngine.UI;
using System;

public class TestOne : BaseUI {

    private Button button;
    public Button btnTouch;
    public Text txtContent;

    public override EnumUIType GetUIType()
    {
        return EnumUIType.TestOne;
    }
    public override UILayer GetUILayer()
    {
        return UILayer.Normal;
    }

    protected override void OnAwake()
    {
        button = this.transform.FindChild("Button").GetComponent<Button>();
        btnTouch = this.transform.FindChild("btnTouch").GetComponent<Button>();
        txtContent = this.transform.FindChild("txtContent").GetComponent<Text>();
        txtContent.gameObject.SetActive(false);
    }

    protected override void OnStart()
    {
        button.onClick.AddListener(OnClickBTN);
        //EventTriggerListener.Get(btnTouch.gameObject).SetEventHandle(EnumTouchEventType.OnTouchBegin, TouchBegin);
        //EventTriggerListener.Get(btnTouch.gameObject).SetEventHandle(EnumTouchEventType.OnTouchEnd, TouchEnd);
    }

    private void TouchEnd(GameObject _listener, object _args, object[] _params)
    {
        txtContent.gameObject.SetActive(false);
        txtContent.text = "  触摸结束";
    }

    private void TouchBegin(GameObject _listener, object _args, object[] _params)
    {
        txtContent.gameObject.SetActive(true);
        txtContent.text = "  触摸开始 ";
    }

    private void OnClickBTN()
    {
        UIManager.Instance.OpenUICloseOthers (EnumUIType.TestTwo);
    }

    
}
