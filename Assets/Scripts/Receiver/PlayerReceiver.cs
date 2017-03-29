using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Receiver;
using ExitGames.Client.Photon;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;

public class PlayerReceiver : MonoBehaviour,IReceiver
{
    private MainView mainView;

    void Start()
    {
        mainView = GetComponent<MainView>();
    }
    public void OnReceive(byte subCode, OperationResponse response)
    {
        switch (subCode)
        {
            case OpPlayer.GetPlayerInfo:
                OnGetPlayerInfo(response.ReturnCode);
                break;
            case OpPlayer.CreatePlayer:
                OnCreatePlayer();
                break;
            case OpPlayer.Online:
                OnPlayerOnLine(JsonMapper.ToObject<PlayerDto>(response[0].ToString()));
                break;
            case OpPlayer.AddFriend:
                OnAddFriend(response.DebugMessage);
                break;
            case OpPlayer.AddFriendToClient:
                OnAddFriendToClient(response.ReturnCode,JsonMapper.ToObject<PlayerDto>(response[0].ToString()));
                break;
            case OpPlayer.FriendOnlineState:
                OnFriendOnlineState(response.ReturnCode, (int) response[0]);
                break;
            case OpPlayer.MatchComplete:
                OnMatchComplete();
                break;
        }
    }


    private void OnCreatePlayer()
    {
        //隐藏创建面板
        mainView.CreatePanelActive = false;
        //向服务器发起上线的请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.Online);
    }

    private void OnGetPlayerInfo(short responseReturnCode)
    {
        switch (responseReturnCode)
        {
            case 0:
                //角色存在,执行上线操作
                //向服务器发起上线的请求
                PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.Online);
                break;
            case -1:
                //非法登录
                break;
            case -2:
                //角色不存在，显示创建面板
                mainView.CreatePanelActive = true;
                break;
        }
    }
    /// <summary>
    /// 角色上线
    /// </summary>
    private void OnPlayerOnLine(PlayerDto playerDto)
    {
        //保存服务器传来的玩家数据
        GameData.Player = playerDto;
        //更新显示
        mainView.UpdateView(playerDto);
    }
    /// <summary>
    /// 加好友
    /// </summary>
    /// <param name="responseReturnCode"></param>
    private void OnAddFriend(string message)
    {
        Tips.Instance.Show(message, null);
    }

    /// <summary>
    /// 客户端收到加好友请求
    /// </summary>
    /// <param name="playerDto"></param>
    private void OnAddFriendToClient(short responseReturnCode, PlayerDto playerDto)
    {
        switch (responseReturnCode)
        {
            case 0:
                //在被加好友的客户端显示加好友面板
                mainView.ShowAddFriendRequestPanel(playerDto);
                break;
            case 1:
                //添加成功，更新显示
                mainView.UpdateView(playerDto);
                break;
        }
    }
    /// <summary>
    /// 好友在线状态改变
    /// </summary>
    /// <param name="responseReturnCode"></param>
    private void OnFriendOnlineState(short responseReturnCode,int friendID)
    {
        switch (responseReturnCode)
        {
            case 0:
                //在线
                mainView.UpdateFriendView(friendID, true);
                break;
            case 1:
                //离线
                mainView.UpdateFriendView(friendID, false);
                break;
        }
    }
    /// <summary>
    /// 匹配成功
    /// </summary>
    private void OnMatchComplete()
    {
        mainView.OnCompleteMatch();
    }
}
