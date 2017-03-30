using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Config;
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

    public void InitView(HeroModel heroModel)
    {
        //保存ID
        this.heroID = heroModel.ID;
        //获取头像和声音
        heroAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_SOUND_SelectHero + heroModel.Name) as AudioClip;
        headSprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead + heroModel.Name) as Sprite;
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
        SoundManager.Instance.PlaySE(heroAudioClip);
        //发起选人的请求
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Select, heroID);
    }
}
