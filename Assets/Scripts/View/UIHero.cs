using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Config;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class UIHero : MonoBehaviour
{
    private Image headImage;
    private Button heroButton;
    private int heroID;
    private AudioClip heroAudioClip;
    private Sprite headSprite;
	// Use this for initialization
	void Awake ()
	{
	    headImage = GetComponent<Image>();
	    heroButton = GetComponent<Button>();
	    heroButton.onClick.AddListener(OnHeroClick);
	    heroID = -1;
	}

    public void InitView(HeroDataModel heroDataModel)
    {
        //保存ID
        this.heroID = heroDataModel.ID;
        //获取头像和声音
        heroAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_SOUND_SelectHero + heroDataModel.Name) as AudioClip;
        headSprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead + heroDataModel.Name) as Sprite;
        //更新头像
        headImage.sprite = headSprite;
    }
    /// <summary>
    /// 英雄是否可选择
    /// </summary>
    public bool Interactable
    {
        get { return heroButton.interactable; }
        set { heroButton.interactable = value; }
    }

    public int HeroId
    {
        get { return heroID; }
    }

    /// <summary>
    /// 选择英雄事件
    /// </summary>
    private void OnHeroClick()
    {
        //播放音效
        SoundManager.Instance.PlaySE(ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Select") as AudioClip);
        SoundManager.Instance.PlaySE(heroAudioClip);
        //发起选人的请求
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Select, heroID);
    }
}
