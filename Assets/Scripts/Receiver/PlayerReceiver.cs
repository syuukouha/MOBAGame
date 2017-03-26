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
        }
    }

    private void OnCreatePlayer()
    {
        //隐藏创建面板
        GetComponent<MainView>().CreatePanelActive = false;
        //向服务器发起上线的请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.Online);
    }

    private void OnGetPlayerInfo(short responseReturnCode)
    {
        switch (responseReturnCode)
        {
            case 0:
                //角色存在,执行上线操作
                break;
            case -1:
                //非法登录
                break;
            case -2:
                //角色不存在，显示创建面板
                GetComponent<MainView>().CreatePanelActive = true;
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
        GetComponent<MainView>().UpdateView(playerDto);
    }
}
