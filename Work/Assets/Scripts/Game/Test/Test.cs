using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork;
public class Test : MonoBehaviour
{

    void Start()
    {
          UIManager.Instance.OpenUI(EnumUIType.TestOne);

        //float time = System.Environment.TickCount;
        //for (int i = 0; i < 1000; i++)
        //{
        //    GameObject go = null;
        //    //自带加载
        //    //go = Instantiate(Resources.Load<GameObject>("Prefabs/Cube"));

        //    //同步加载
        //    // go = ResManager.Instance.LoadInstance("Prefabs/Cube") as GameObject;

        //    //协同加载
        //    //ResManager.Instance.LoadCorotineInstance("Prefabs/Cube",(_obj)=>{
        //    //     go=_obj as GameObject ;
        //    //     go.transform.position = UnityEngine.Random.insideUnitSphere * 50;
        //    // }) ;

        //    //异步加载
        //    ResManager.Instance.LoadAsyncInstance ("Prefabs/Cube", (_obj) =>
        //    {
        //        go = _obj as GameObject;
        //        go.transform.position = UnityEngine.Random.insideUnitSphere * 50;
        //    });

        //   // go.transform.position = UnityEngine.Random.insideUnitSphere * 50;
        //}
        //Debug.Log("Times   =  " + (System.Environment.TickCount - time) * 1000);
    }

    void Update()
    {

    }
}
