using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class BattleView : MonoBehaviour
{
    /// <summary>
    /// 头像
    /// </summary>
    public Image HeadImage;
    /// <summary>
    /// 经验条
    /// </summary>
    public Slider ExpBar;
    /// <summary>
    /// 等级
    /// </summary>
    public Text LvText;
    /// <summary>
    /// 血条
    /// </summary>
    public Slider HpBar;
    /// <summary>
    /// 血量
    /// </summary>
    public Text HpText;
    /// <summary>
    /// 蓝条
    /// </summary>
    public Slider MpBar;
    /// <summary>
    /// 蓝量
    /// </summary>
    public Text MpText;
    /// <summary>
    /// KDA
    /// </summary>
    public Text KDAText;
    /// <summary>
    /// 金钱
    /// </summary>
    public Text MoneyText;
    /// <summary>
    /// 攻击力
    /// </summary>
    public Text AttacText;
    /// <summary>
    /// 防御力
    /// </summary>
    public Text DefenceText;

    // Use this for initialization
    void Start ()
	{
        //向服务器发起已经进入的请求
	    PhotonManager.Instance.Request(OperationCode.BattleCode, OpBattle.Enter, GameData.Player.ID);
	}

    public void InitView(HeroModel heroModel)
    {
        HeadImage.sprite = ResourcesManager.Instance.GetAsset(Paths.RES_UIHeroHead + heroModel.Name) as Sprite;
        HpBar.value = (float)heroModel.CurrentHP/heroModel.MaxHP;
        HpText.text = string.Format("{0}/{1}", heroModel.CurrentHP, heroModel.MaxHP);
        MpBar.value = (float)heroModel.CurrentMP / heroModel.MaxMP;
        MpText.text = string.Format("{0}/{1}", heroModel.CurrentMP, heroModel.MaxMP);
        ExpBar.value = (float) heroModel.Exp/(heroModel.Lv*100);
        LvText.text = heroModel.Lv.ToString();
        KDAText.text = string.Format("CS:{0}  Kill:{1}  Dead:{2}", heroModel.CS, heroModel.Kill, heroModel.Dead);
        MoneyText.text = heroModel.Money.ToString();
        AttacText.text = heroModel.Attack.ToString();
        DefenceText.text = heroModel.Defence.ToString();
    }
}
