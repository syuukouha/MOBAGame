using System.Collections;
using System.Collections.Generic;
using MOBACommon.Config;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    private Text nameText;
    private Text stateText;
    private Image headImage;
    private Image bgImage;
    
    //缓存
    private Sprite noConnectSprite;
    private Sprite noSelectSprite;

	// Use this for initialization
	void Start ()
	{
	    nameText = transform.Find("PlayerNameText").GetComponent<Text>();
	    stateText = transform.Find("StateText").GetComponent<Text>();
	    headImage = transform.Find("HeroHead").GetComponent<Image>();
	    bgImage = GetComponent<Image>();

	    noConnectSprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead + "noConnect") as Sprite;
        noSelectSprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead + "noSelect") as Sprite;
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
                //更新选择英雄头像
                headImage.sprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead +HeroData.GetHeroDataModel(selectModel.HeroID).Name) as Sprite;
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
}
