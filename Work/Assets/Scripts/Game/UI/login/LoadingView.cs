using UnityEngine;
using System.Collections;
using FrameWork;
public class LoadingView : BaseUI {

    public override EnumUIType GetUIType()
    {
        return EnumUIType.LoadingView;
    }

    public override UILayer GetUILayer()
    {
        return UILayer.Normal;
    }
}
