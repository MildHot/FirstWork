using UnityEngine;
using System.Collections;
using FrameWork;
public class MainView : BaseUI {

    public override EnumUIType GetUIType()
    {
        return EnumUIType.MainView;
    }

    public override UILayer GetUILayer()
    {
        return UILayer.Normal;
    }
}
