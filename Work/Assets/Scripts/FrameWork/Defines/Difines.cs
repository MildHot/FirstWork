using UnityEngine;
using System.Collections;
namespace FrameWork
{
    public enum EnumObjectState
    {
        None,
        Initial,
        Loading,
        Ready,
        Disabled,
        Closing,
    }

    public enum EnumUIType : int
    {
        None = -1,
        TestOne = 0,
        TestTwo,
        LoadinView,
        LoadingView,
        MainView,
    }

    public class UIPathDefines
    {
        /// <summary>
        /// UI预制体
        /// </summary>
        public const string UI_PREFAB = "Prefabs/";



        public static string GetPrefabsPathByType(EnumUIType _uiType)
        {
            string _path = "";
            switch (_uiType)
            {
                case EnumUIType.TestOne:
                    _path = UI_PREFAB + "TestOne";
                    break;
                case EnumUIType.TestTwo:
                    _path = UI_PREFAB + "TestTwo";
                    break;

                case EnumUIType.LoadinView:
                    _path = UI_PREFAB + "LoadinView";
                    break;

                case EnumUIType.LoadingView:
                    _path = UI_PREFAB + "LoadingView";
                    break;

                case EnumUIType.MainView:
                    _path = UI_PREFAB + "MainView";
                    break;

                default:
                    Debug.LogWarning(" Difine - GetPrefabsPathByType - not find EnumType  type  =  " + _uiType);
                    break;
            }
            return _path;
        }

        public static System.Type GetUIScriptByType(EnumUIType _uiType)
        {
            System.Type _scriptType = null;
            switch (_uiType)
            {
                case EnumUIType.TestOne:
                    _scriptType = typeof(TestOne);
                    break;
                case EnumUIType.TestTwo:
                    _scriptType = typeof(TestTwo);
                    break;
                case EnumUIType.LoadinView:
                    _scriptType = typeof(LoaginView);
                    break;
                case EnumUIType.LoadingView:
                    _scriptType = typeof(LoadingView);
                    break;
                case EnumUIType.MainView:
                    _scriptType = typeof(MainView);
                    break;
                default:
                    Debug.Log("  Difines - GetUIScriptByType - Not Find EnumUIType! type: " + _uiType.ToString());
                    break;
            }
            return _scriptType;
        }
    }

    public class Defines
    {

    }
}

