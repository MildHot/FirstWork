using UnityEngine;
using System.Collections;
using FrameWork;
public class LoaginView : BaseUI {


    public override EnumUIType GetUIType()
    {
        return EnumUIType.LoadinView;
    }

    public override UILayer GetUILayer()
    {
        return UILayer.Normal;
    }
}
