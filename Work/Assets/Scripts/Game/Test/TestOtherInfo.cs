using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FrameWork;
public class TestOtherInfo : MonoBehaviour {

   
    public GameObject objNomal;
    private List normalList;

    //public GameObject objHp;
    //public HPAnimBar hpBar;

    //private int hp = 10000;

    //private int num = 0;
	void Start () {
        //hpBar = new HPAnimBar();
        //hpBar.SetComponents(objHp);
        //hpBar.SetBasic(7, true);
        //hpBar.SetValue(1001, 10000, 10000);

        normalList = this.objNomal.GetComponent<List>();
        this.normalList.Count = 60;
        this.normalList.OnClickItem = this.OnClick;
	}




   

    private void OnClick(int index)
    {
        string sss = " sss = ";
        for (int i = 0; i < normalList.Selecteds.Count; i++)
        {
            sss += " ( " + normalList.Selecteds[i] + ")";
        }
        //var item=normalList .GetItem(index);
        //Debug.Log("  单选    " + index+"  item   "+item.gameObject.name);
        Debug.Log("  单选    " + index+"  多选   "+sss);
    }

   
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //hp -= Random.Range(10, 50);

            //if (hp >0)
            //{
            //   num= hpBar.SetValue(1001, hp, 10000);
            //}
            //else
            //{
            //  num=  hpBar.SetValue(0, 0, 10000);
            //}

            //Debug.Log("  剩余血条   " + num);

            
           // normalList.ScrollTop();
            normalList.ScrollByAxialRow(1);
        }
	}

  
}
