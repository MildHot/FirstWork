using UnityEngine;
using System.Collections;
using FrameWork;
public class StartGame : MonoBehaviour {


    void Awake()
    {
        ModuleManager.Instance.RegisterLoadingModule();
    }

	void Start () {
        ModuleManager.Instance.RegisterAllModule();
        LoadUI();
	}
	
    private void LoadUI()
    {
        UIManager.Instance.OpenUI(EnumUIType.LoadinView);
        //EnumUIType[] preUIs = { EnumUIType.MainView, EnumUIType.TestOne, EnumUIType.TestTwo };
        //UIManager.Instance.PreLoadUI(preUIs);
    }
}
