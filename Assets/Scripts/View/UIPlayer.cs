using System.Collections;
using System.Collections.Generic;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour,IResourceListener
{
    private Text nameText;
    private Text stateText;
    private Image headImage;
    private Image bgImage;
    
    //缓存
    private Sprite noConnectSprite;
    private Sprite noSelectSprite;

    void Awake()
    {
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "noSelect", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "noConnect", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "Ahri_Square_0", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "LeeSin_Square_0", typeof(Sprite), this);
    }
	// Use this for initialization
	void Start ()
	{
	    nameText = transform.FindChild("PlayerNameText").GetComponent<Text>();
	    stateText = transform.FindChild("StateText").GetComponent<Text>();
	    headImage = transform.FindChild("HeroHead").GetComponent<Image>();
	    bgImage = GetComponent<Image>();
	}

    public void UpdateView(SelectModel selectModel)
    {
        nameText.text = selectModel.PlayerName;
        //判断玩家是否进入
        if (!selectModel.IsEnter)
        {
            headImage.sprite = noConnectSprite;
            stateText.text = "正在连接...";
        }
        else
        {
            if (selectModel.HeroID != -1) //-1代表没有选择英雄
            {
                //TODO
            }
            else
            {
                //如果没有选择英雄
                headImage.sprite = noSelectSprite;
                stateText.text = "正在选择...";
            }
            //判断是否准备
            if (selectModel.IsReady)
            {
                bgImage.color = Color.green;
                stateText.text = "已准备";
            }
            else
            {
                bgImage.color = Color.white;
                stateText.text = "正在选择...";
            }
        }
    }

    public void OnLoaded(string assetPath, object asset)
    {
        switch (assetPath)
        {
            case Paths.RES_UIHeroHead + "noSelect":
                noSelectSprite = asset as Sprite;
                break;
            case Paths.RES_UIHeroHead + "noConnect":
                noConnectSprite = asset as Sprite;
                break;
        }
    }
}
