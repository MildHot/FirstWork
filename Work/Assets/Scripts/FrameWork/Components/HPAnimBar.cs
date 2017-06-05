using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/// <summary>
/// 血条
/// </summary>
public class HPAnimBar  {
    /**血条颜色阶段  黄>橙>紫>蓝>浅蓝>绿>红*/
    private string[] COLOR_STAGE= { "HPBAR_RED", "HPBAR_GREEN", "HPBAR_DANBLUE", "HPBAR_BLUE", "HPBAR_PINK", "HPBAR_PURPLE", "HPBAR_ORANGE" };

    private int MAX_STAGE = 7;

    private int otherId;

    private int m_totalVal = 0;

    private int m_oldVal = 0;

    private int m_stageNum = 0;
    private bool m_showPercent = false;

    private Image hpBarImg;
    private RectTransform hpBarRectTrans;

    private Image maskImg;
    private RectTransform maskRectTrans;

    private Image bgImg;
    private RectTransform bgRectTrans;

    private Text txtHp;

    private UGUIAltas hpAtals;

    private TweenScale maskTween;

    public void SetComponents(GameObject go)
    {
        hpBarImg = go.transform.FindChild("hpBarImg").GetComponent<Image>();
        hpBarRectTrans = hpBarImg.transform as RectTransform;
        maskImg = go.transform.FindChild("maskImg").GetComponent<Image>();
        maskRectTrans = maskImg.transform as RectTransform;
        bgImg = go.transform.FindChild("bgImg").GetComponent<Image>();
        bgRectTrans = bgImg.transform as RectTransform;
        txtHp = go.transform.FindChild("txtHp").GetComponent<Text>();
        hpAtals = go.transform.FindChild("hpAtlas").GetComponent<UGUIAltas>();
    }

    public void Reset()
    {
        if (maskTween != null)
        {
            maskTween.enabled = false;
            maskTween = null;
        }
        otherId = 0;
        m_totalVal = 0;
        m_oldVal = 0;
        m_stageNum = 0;
    }

    public void SetBasic(int stageNum, bool showPercent)
    {
        if (stageNum == 0)
        {
            stageNum = 1;
        }
        else if (stageNum > MAX_STAGE)
        {
            stageNum = MAX_STAGE;
        }
        m_stageNum = stageNum;
        m_showPercent = showPercent;
    }

    public int SetValue(int otherId, int curValue, int maxValue) {
        m_totalVal = maxValue;
        bool diffId = false;
        if (this.otherId != otherId)
        {
            this.m_oldVal = curValue;
            diffId = true;
        }
        this.otherId = otherId;
        int curStage = Mathf.CeilToInt((float)curValue / ((float)this.m_totalVal / this.m_stageNum));
        if (curStage <= 0)
        {
            curStage = 1;
        }
        else if (curStage > MAX_STAGE)
        {
            curStage = MAX_STAGE;
        }

        string barBmdName = COLOR_STAGE[curStage - 1];

        //满血的时候显示黄色
        if (curStage == m_stageNum)
        {
            barBmdName = COLOR_STAGE[MAX_STAGE - 1];
        }

        float perStageVal = (float)m_totalVal / m_stageNum;
      
        float oldWidth = (m_oldVal - (curStage - 1) * perStageVal) / perStageVal;
        float newWidth = (curValue - (curStage - 1) * perStageVal) / perStageVal;
        m_oldVal = curValue;

        this.hpBarImg.sprite = hpAtals.Get(barBmdName);
        Vector3 newScale = new Vector3(newWidth, 1, 1);
        hpBarRectTrans.localScale = newScale;

        string maskName = "HpMask_" + curStage;
        maskImg.sprite = hpAtals.Get(maskName);
        if (maskRectTrans.localScale.x > newWidth && !diffId)
        {
            if (maskRectTrans.localScale.x - newWidth > 0.05f)
            {
                maskRectTrans.localScale =new Vector3 ( newWidth + 0.05f,1,1);
            }
            maskTween = TweenScale.Begin(maskImg.gameObject, 0.3f, newScale);
        }
        else
        {
            if (maskTween != null)
            {
                maskTween.enabled = false;
                maskTween = null;
            }
            maskRectTrans.localScale = newScale;
        }

        if (curStage > 1)
        {
            bgImg.sprite = hpAtals.Get(COLOR_STAGE[curStage - 2]);
            bgImg.gameObject.SetActive(true);
        }
        else
        {
            bgImg.gameObject.SetActive(false);
        }

        if (m_showPercent)
        {
            float per = (float)curValue / maxValue * 100;
            txtHp.text = (per < 100 ? per.ToString("0.00") : 100.ToString()) + "%";
        }
        else
        {
            txtHp.text = curValue + "/" + maxValue;
        }
        return curStage;
    }
}
