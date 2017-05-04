using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class MainView : UIBase
{

    #region 变量定义
    [Header("Create")]
    [SerializeField]
    private InputField nameInputField;
    [SerializeField]
    private Button createButton;
    [SerializeField]
    private GameObject createPanel; 

    [Header("Main")]
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text lvText;
    [SerializeField]
    private Slider expBar;

    [Header("Friend")]
    [SerializeField]
    private Transform friendSpawn;
    [SerializeField]
    private GameObject friendPrefab;
    [SerializeField]
    private InputField addFriendInputField;
    [SerializeField]
    private GameObject friendList;
    [SerializeField]
    private AddFriendRequestView addFriendRequestView;

    private List<FriendView> friendViews = new List<FriendView>();
    [Header("Match")]
    [SerializeField]
    private Button singleMatchButton;
    [SerializeField]
    private Button multiMatchButton;
    [SerializeField]
    private MatchView matchView;
    [SerializeField]
    private MatchCompleteView matchCompleteView;

    public bool SingleMatchInteractable
    {
        set { singleMatchButton.interactable = value; }
    }
    public bool MultiMatchInteractable
    {
        set { multiMatchButton.interactable = value; }
    }
    private AudioClip clickAudioClip;

    #endregion
    #region UIbase
    public override string UIName()
    {
        return Paths.RES_UIMain;
    }

    void Start()
    {
        clickAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Click") as AudioClip;
        //添加按键监听
        createButton.onClick.AddListener(OnCreateClick);
        //向服务器发起获取角色信息的请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.GetPlayerInfo);
    }
    #endregion
    #region 创建模块

    /// <summary>
    /// 创建按钮的点击状态
    /// </summary>
    public bool CreateInteractable
    {
        set { createButton.interactable = value; }
    }
    /// <summary>
    /// 创建面板的显示状态
    /// </summary>
    public bool CreatePanelActive
    {
        set { createPanel.SetActive(value); }
    }

    public void OnCreateClick()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        //输入检测
        if (string.IsNullOrEmpty(nameInputField.text))
            return;
        //发起创建请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.CreatePlayer, nameInputField.text);
        CreateInteractable = false;
    }
    #endregion

    /// <summary>
    /// 更新显示
    /// </summary>
    public void UpdateView(PlayerDto playerDto)
    {
        nameText.text = playerDto.Name;
        lvText.text = playerDto.Lv.ToString();
        expBar.value = playerDto.Exp/(playerDto.Lv*100.0f);
        //加载好友列表
        FriendDto[] friendDtos = playerDto.Friends;
        friendViews.Clear();
        GameObject go = null;
        foreach (FriendDto friendDto in friendDtos)
        {
            go = Instantiate(friendPrefab);
            go.transform.SetParent(friendSpawn);
            go.transform.localScale = Vector3.one;
            FriendView friendView = go.GetComponent<FriendView>();
            friendView.InitView(friendDto.ID, friendDto.Name, friendDto.IsOnline);
            friendViews.Add(friendView);
        }
    }

    #region 好友模块
    /// <summary>
    /// 添加好友按钮点击事件
    /// </summary>
    public void OnAddFriendClick()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        if (string.IsNullOrEmpty(addFriendInputField.text))
            return;
        //发送添加好友请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.AddFriend, addFriendInputField.text);
        addFriendInputField.text = string.Empty;
    }
    /// <summary>
    /// 显示加好友面板
    /// </summary>
    public void ShowAddFriendRequestPanel(PlayerDto playerDto)
    {
        addFriendRequestView.gameObject.SetActive(true);
        addFriendRequestView.UpdateView(playerDto);
    }
    /// <summary>
    /// 同意拒绝的点击事件
    /// </summary>
    /// <param name="result"></param>
    public void OnResultClick(bool result)
    {
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.AddFriendToClient, result,addFriendRequestView.ID);
        addFriendRequestView.gameObject.SetActive(false);
    }
    /// <summary>
    /// 好友列表按钮点击事件
    /// </summary>
    public void OnFriendListClick()
    {
        friendList.SetActive(!friendList.activeInHierarchy);
    }
    /// <summary>
    /// 更新好友在线状态
    /// </summary>
    /// <param name="friendID">好友ID</param>
    /// <param name="isOnLine">是否在线</param>
    public void UpdateFriendView(int friendID, bool isOnLine)
    {
        foreach (FriendView friendView in friendViews)
        {
            if (friendView.ID == friendID)
                friendView.UpdateView(isOnLine);
        }
    }
    #endregion
    #region 匹配模块
    /// <summary>
    /// 匹配开始
    /// </summary>
    public void OnStartMatch(bool isSingle)
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        SingleMatchInteractable = false;
        MultiMatchInteractable = false;
        if (isSingle)
        {
            //发起请求
            PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.MatchStart, GameData.Player.ID);
            //显示匹配面板
            matchView.StartMatch();
        }
        else
        {
            //发起请求

            //TODO
        }

    }
    /// <summary>
    /// 取消匹配
    /// </summary>
    public void OnStopMatch()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        SingleMatchInteractable = true;
        MultiMatchInteractable = true;
        //发起请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.MatchStop, GameData.Player.ID);
        //关闭匹配面板
        matchView.StopMatch();
    }
    /// <summary>
    /// 完成匹配
    /// </summary>
    public void OnCompleteMatch()
    {
        //关闭匹配面板
        OnStopMatch();
        //显示匹配完成面板
        matchCompleteView.Show();
        SingleMatchInteractable = false;
        MultiMatchInteractable = false;
    }

    public void OnSureButtonClick()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        //隐藏主界面UI
        UIManager.Instance.HideUI(Paths.RES_UIMain);
        //显示选人界面UI
        UIManager.Instance.ShowUI(Paths.RES_UISelect);
        SingleMatchInteractable = true;
        MultiMatchInteractable = true;
    }
    #endregion

}
