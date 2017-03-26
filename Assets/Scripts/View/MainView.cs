using System.Collections;
using System.Collections.Generic;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class MainView : UIBase,IResourceListener
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
    [SerializeField]
    private GameObject friends;

    private AudioClip clickAudioClip;

    #endregion
    #region UIbase
    public override string UIName()
    {
        return Paths.RES_UIMain;
    }

    public override void Init()
    {
        clickAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Click") as AudioClip;
        //添加按键监听
        createButton.onClick.AddListener(OnCreateClick);
        //向服务器发起获取角色信息的请求
        PhotonManager.Instance.Request(OperationCode.PlayerCode, OpPlayer.GetPlayerInfo);
    }

    public override void OnDestory()
    {

    } 
    #endregion
    /// <summary>
    /// 加载回调
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetPath, object asset)
    {
        
    }
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
         
    }

}
