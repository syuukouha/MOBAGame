using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receiver;
using ExitGames.Client.Photon;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                OnEnter(response.ReturnCode,(int)response[0]);
                break;
            case OpSelect.Cancel:
                OnCancel();
                break;
            case OpSelect.Select:
                OnSelect((int) response[0], (int) response[1]);
                break;
            case OpSelect.Ready:
                OnReady((int)response[0]);
                break;
            case OpSelect.Chat:
                OnChat(response[0].ToString());
                break;
            case OpSelect.StartBattle:
                SceneManager.LoadScene("Battle");
                break;
        }
    }

    /// <summary>
    /// 更新聊天
    /// </summary>
    /// <param name="str"></param>
    private void OnChat(string str)
    {
        selectView.UpdateChatView(str);
    }

    /// <summary>
    /// 玩家准备
    /// </summary>
    /// <param name="playerID"></param>
    private void OnReady(int playerID)
    {
        foreach (SelectModel redTeamSelectModel in _redTeamSelectModels)
        {
            if (redTeamSelectModel.PlayerID == playerID)
            {
                redTeamSelectModel.IsReady = true;
                OnGetInfo();
                return;
            }
        }
        foreach (SelectModel blueTeamSelectModel in _blueTeamSelectModels)
        {
            if (blueTeamSelectModel.PlayerID == playerID)
            {
                blueTeamSelectModel.IsReady = true;
                OnGetInfo();
                return;
            }
        }
    }

    /// <summary>
    /// 英雄选择
    /// </summary>
    /// <param name="playerID"></param>
    /// <param name="heroID"></param>
    private void OnSelect(int playerID, int heroID)
    {
        foreach (SelectModel redTeamSelectModel in _redTeamSelectModels)
        {
            if (redTeamSelectModel.PlayerID == playerID)
            {
                redTeamSelectModel.HeroID = heroID;
                OnGetInfo();
                return;
            }
        }
        foreach (SelectModel blueTeamSelectModel in _blueTeamSelectModels)
        {
            if (blueTeamSelectModel.PlayerID == playerID)
            {
                blueTeamSelectModel.HeroID = heroID;
                OnGetInfo();
                return;
            }
        }
    }

    /// <summary>
    /// 取消进入房间
    /// </summary>
    private void OnCancel()
    {
        //隐藏选人界面
        UIManager.Instance.HideUI(Paths.RES_UISelect);
        //显示主界面
        UIManager.Instance.ShowUI(Paths.RES_UIMain); 
    }

    /// <summary>
    /// 玩家进入房间
    /// </summary>
    /// <param name="responseReturnCode">0代表进入房间成功，1代表全部人进入房间</param>
    /// <param name="playerID"></param>
    private void OnEnter(short responseReturnCode, int playerID)
    {
        switch (responseReturnCode)
        {
            case 0:
                //有玩家进入房间
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
                break;
            case 1:
                //全部玩家进入房间
                //初始化玩家拥有的英雄
                selectView.InitSelectHeroPanel(GameData.Player.HeroID);
                break;
        }   
    }

    /// <summary>
    /// 获取队伍信息
    /// </summary>
    /// <param name="redTeamSelectModels"></param>
    /// <param name="blueTeamSelectModels"></param>
    private void OnGetInfo()
    {
        //更新显示
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
