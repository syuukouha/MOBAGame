using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Config;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class SelectView : UIBase
{
    [SerializeField]
    private UIPlayer[] _redTeam;
    [SerializeField]
    private UIPlayer[] _blueTeam;
    [SerializeField]
    private GameObject heroPanel;
    [SerializeField]
    private GameObject heroPrefab;

    [SerializeField]
    private Button sureButton;
    [SerializeField]
    private Text timerText;

    private float timer;
    private bool timerStart;

    //缓存
    private Dictionary<int,UIHero> uiHeroes = new Dictionary<int, UIHero>();


    void Update()
    {
        if (timerStart)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                timer = 0;
            }
            timerText.text = timer.ToString("N0");
        }
        else
        {
            timerText.text = "";
        }

    }

    #region UIBase
    public override string UIName()
    {
        return Paths.RES_UISelect;
    }

    public override void Init()
    {
        sureButton.onClick.AddListener(OnSureButtonClick);
    }

    public override void OnShow()
    {
        base.OnShow();
        //发起进入房间的请求
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Enter);
        timerStart = false;
    }

    public override void OnDestory()
    {

    } 
    #endregion
    /// <summary>
    /// 更新试图显示
    /// </summary>
    /// <param name="redTeamSelectModels"></param>
    /// <param name="blueTeamSelectModels"></param>
    public void UpdateView(int currentTeamID,SelectModel[] redTeamSelectModels, SelectModel[] blueTeamSelectModels)
    {
        //已选择英雄的ID
        List<int> selectedHeroID = new List<int>();
        //玩家是否已准备的标志位
        bool isReady = false;
        //不管是红方还是蓝方都优先显示在左边
        //红方
        if (currentTeamID == 1)
        {
            for (int i = 0; i < redTeamSelectModels.Length; i++)
            {

                _redTeam[i].UpdateView(redTeamSelectModels[i]);
            }
            for (int i = 0; i < blueTeamSelectModels.Length; i++)
            {
                _blueTeam[i].UpdateView(blueTeamSelectModels[i]);
            }

            foreach (SelectModel redTeamSelectModel in redTeamSelectModels)
            {
                //如果准备，设置准备标志位为true,
                if (redTeamSelectModel.IsReady)
                {
                    isReady = true;
                }
                else
                {
                    //如果选择了英雄，保存已选择英雄的ID，更新确认按钮可点击状态
                    if (redTeamSelectModel.HeroID != -1)
                    {
                        selectedHeroID.Add(redTeamSelectModel.HeroID);
                        sureButton.interactable = true;
                    }
                    else
                    {
                        sureButton.interactable = false;
                    }
                }              
            }

        }
        //蓝方
        if (currentTeamID == 2)
        {
            for (int i = 0; i < blueTeamSelectModels.Length; i++)
            {
                _redTeam[i].UpdateView(blueTeamSelectModels[i]);
            }
            for (int i = 0; i < redTeamSelectModels.Length; i++)
            {
                _blueTeam[i].UpdateView(redTeamSelectModels[i]);
            }
            foreach (SelectModel blueTeamSelectModel in blueTeamSelectModels)
            {
                //如果准备，设置准备标志位为true
                if (blueTeamSelectModel.IsReady)
                {
                    isReady = true;
                }
                else
                {
                    //如果选择了英雄，保存已选择英雄的ID，更新确认按钮可点击状态
                    if (blueTeamSelectModel.HeroID != -1)
                    {
                        selectedHeroID.Add(blueTeamSelectModel.HeroID);
                        sureButton.interactable = true;
                    }
                    else
                    {
                        sureButton.interactable = false;
                    }
                }
            }
        }
        
        //更新选择英雄面板的可选状态
        foreach (UIHero uiHero in uiHeroes.Values)
        {
            //如果当前英雄已经被选择，或者玩家已准备的话 更新选择英雄面板的可选状态
            if (selectedHeroID.Contains(uiHero.HeroId) || isReady)
            {
                uiHero.Interactable = false;
            }
            else
            {
                uiHero.Interactable = true;
            }
        }
    }
    /// <summary>
    /// 初始化选择英雄面板,开启选人计时
    /// </summary>
    public void InitSelectHeroPanel(int[] heroIDs)
    {
        GameObject hero;
        for (int i = 0; i < heroIDs.Length; i++)
        {
            if (uiHeroes.ContainsKey(heroIDs[i]))
                continue;
            hero = Instantiate(heroPrefab);
            UIHero uiHero = hero.GetComponent<UIHero>();
            uiHero .InitView(HeroData.GetHeroModel(heroIDs[i]));
            hero.transform.SetParent(heroPanel.transform);
            hero.transform.localScale = Vector3.one;
            uiHeroes.Add(heroIDs[i], uiHero);
        }
        timerStart = true;
        timer = 60;
    }
    /// <summary>
    /// 确认选择
    /// </summary>
    private void OnSureButtonClick()
    {
        sureButton.interactable = false;
        //给服务器发送准备请求
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Ready);
    }
}
