using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;

public class SelectView : UIBase
{
    [SerializeField]
    private UIPlayer[] _redTeam;
    [SerializeField]
    private UIPlayer[] _blueTeam;

    #region UIBase
    public override string UIName()
    {
        return Paths.RES_UISelect;
    }

    public override void Init()
    {
        
    }

    public override void OnShow()
    {
        base.OnShow();
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Enter);
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
        }
    }
}
