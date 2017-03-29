using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receiver;
using ExitGames.Client.Photon;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;

public class SelectReceiver :MonoBehaviour,IReceiver
{

    private SelectView selectView;

    //缓存
    private int _currentTeamID;
    private SelectModel[] _redTeamSelectModels;
    private SelectModel[] _blueTeamSelectModels;

    void Start()
    {
        selectView = GetComponent<SelectView>();
    }

    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpSelect.GetInfo:
                //先保存数据，然后更新显示
                _redTeamSelectModels = JsonMapper.ToObject<SelectModel[]>(response[0].ToString());
                _blueTeamSelectModels = JsonMapper.ToObject<SelectModel[]>(response[1].ToString());
                GetCurrentTeamID();
                OnGetInfo();
                break;
            case OpSelect.Enter:
                int playerID = (int) response[0];
                OnEnter(playerID);
                break;
        }
    }
    /// <summary>
    /// 当玩家进入
    /// </summary>
    /// <param name="currentTeamId"></param>
    private void OnEnter(int playerID)
    {
        foreach (SelectModel redTeamSelectModel in _redTeamSelectModels)
        {
            if (redTeamSelectModel.PlayerID == playerID)
            {
                redTeamSelectModel.IsEnter = true;
                OnGetInfo();
                return;
            }
        }
        foreach (SelectModel blueTeamSelectModel in _blueTeamSelectModels)
        {
            if (blueTeamSelectModel.PlayerID == playerID)
            {
                blueTeamSelectModel.IsEnter = true;
                OnGetInfo();
                return;
            }
        }

    }

    /// <summary>
    /// 获取队伍信息
    /// </summary>
    /// <param name="redTeamSelectModels"></param>
    /// <param name="blueTeamSelectModels"></param>
    private void OnGetInfo()
    {
        selectView.UpdateView(_currentTeamID, _redTeamSelectModels, _blueTeamSelectModels);
    }
    /// <summary>
    /// 获取当前玩家的队伍ID
    /// </summary>
    private void GetCurrentTeamID()
    {
        int playerID = GameData.Player.ID;
        for (int i = 0; i < _redTeamSelectModels.Length; i++)
        {
            if (_redTeamSelectModels[i].PlayerID == playerID)
                _currentTeamID = 1;
        }
        for (int i = 0; i < _blueTeamSelectModels.Length; i++)
        {
            if (_blueTeamSelectModels[i].PlayerID == playerID)
                _currentTeamID = 2;
        }
    }
}
