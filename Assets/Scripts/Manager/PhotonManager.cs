using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Manager;
using Assets.Scripts.Receiver;
using ExitGames.Client.Photon;
using MOBACommon.Codes;
using UnityEngine;

public class PhotonManager : Singleton<PhotonManager>,IPhotonPeerListener
{
    #region Receivers
    /// <summary>
    /// 账号
    /// </summary>
    private AccountReceiver _accountReceiver;

    public AccountReceiver accountReceiver
    {
        get
        {
            if (_accountReceiver == null)
                _accountReceiver = FindObjectOfType<AccountReceiver>();
            return _accountReceiver;
        }
    }
    /// <summary>
    /// 角色
    /// </summary>
    private PlayerReceiver _playerReceiver;

    public PlayerReceiver playerReceiver
    {
        get
        {
            if (_playerReceiver == null)
                _playerReceiver = FindObjectOfType<PlayerReceiver>();
            return _playerReceiver;
        }
    }
    #endregion
    /// <summary>
    /// 代表客户端
    /// </summary>
    private PhotonPeer peer;
    /// <summary>
    /// IP地址
    /// </summary>
    private string serverAddress = "127.0.0.1:5055";
    /// <summary>
    /// 应用名称
    /// </summary>
    private string applicationName = "MOBAServer";
    /// <summary>
    /// 协议
    /// </summary>
    private ConnectionProtocol protocol = ConnectionProtocol.Udp;

    private bool isConnect = false;

    protected override void Awake()
    {
        base.Awake();
        peer = new PhotonPeer(this, protocol);
        peer.Connect(serverAddress, applicationName);
    }
    	
	// Update is called once per frame
	void Update () {
	    if (!isConnect)
	    {
            peer.Connect(serverAddress, applicationName);
        }
        peer.Service();
    }

    void OnApplicationQuit()
    {
        peer.Disconnect();
    }
    public void Request(byte OpCode, byte SubCode, params object[] parameters)
    {
        OperationRequest request  = new OperationRequest();
        request.OperationCode = OpCode;
        request.Parameters = new Dictionary<byte, object>();
        request.Parameters[80] = SubCode;
        for (int i = 0; i < parameters.Length; i++)
        {
            request.Parameters[(byte) i] = parameters[i];
        }
        peer.OpCustom(OpCode, request.Parameters, true);
        ////创建参数字典
        //Dictionary<byte, object> OpParameters = new Dictionary<byte, object>();
        ////指定子操作码
        //OpParameters[80] = SubCode;
        ////赋值参数
        //for (int i = 0; i < parameters.Length; i++)
        //{
        //    OpParameters[(byte)i] = parameters[i];
        //}
        ////发送
        //peer.OpCustom(OpCode, OpParameters, true);
    }

    #region Photon接口
    public void DebugReturn(DebugLevel level, string message)
    {

    }
    /// <summary>
    /// 接受服务器的响应
    /// </summary>
    /// <param name="operationResponse"></param>
    public void OnOperationResponse(OperationResponse operationResponse)
    {
        LogManager.Log(operationResponse.ToStringFull());
        byte opCode = operationResponse.OperationCode;
        byte subCode = (byte) operationResponse[80];
        //转接
        switch (opCode)
        {
            case OperationCode.AccountCode:
                accountReceiver.OnReceive(subCode, operationResponse);
                break;
            case OperationCode.PlayerCode:
                playerReceiver.OnReceive(subCode, operationResponse);
                break;

        }
    }

    public void OnStatusChanged(StatusCode statusCode)
    {
        LogManager.Log(statusCode);
        switch (statusCode)
        {
            case StatusCode.Connect:
                isConnect = true;
                break;
            case StatusCode.Disconnect:
                isConnect = false;
                Tips.Instance.Show("服务器连接失败", null);
                break;
        }
    }

    public void OnEvent(EventData eventData)
    {
        
    }

    #endregion

}
