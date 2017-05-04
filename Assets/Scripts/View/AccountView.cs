using System.Collections;
using System.Collections.Generic;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class AccountView : UIBase
{

    #region 变量定义
    [Header("Register")]
    [SerializeField]
    private InputField _accountRegister;
    [SerializeField]
    private InputField _passwordRegister;
    [SerializeField]
    private InputField _passwordCheckRegister;

    [Header("Login")]
    [SerializeField]
    private InputField _accountLogin;
    [SerializeField]
    private InputField _passwordLogin;
    [SerializeField]
    private Button _loginButton;

    private AudioClip bgmAudioClip;
    private AudioClip enterAudioClip;
    private AudioClip clickAudioClip;

    #endregion

    #region 注册模块

    /// <summary>
    /// 注册点击事件
    /// </summary>
    public void OnRegisterBtnClick()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
        if (string.IsNullOrEmpty(_accountRegister.text) || string.IsNullOrEmpty(_passwordRegister.text) ||
            !_passwordRegister.text.Equals(_passwordCheckRegister.text))
        {
            return;
        }
        //发送请求（非数据模型）
        PhotonManager.Instance.Request(OperationCode.AccountCode, OpAccount.Register, _accountRegister.text,
            _passwordRegister.text);
    }
    #endregion
    #region 登录模块

    public bool LoginInteractable
    {
        set { _loginButton.interactable = value; }
    }

    /// <summary>
    /// 进入游戏点击事件
    /// </summary>
    public void OnLoginBtnClick()
    {
        SoundManager.Instance.PlaySE(enterAudioClip);
        if (string.IsNullOrEmpty(_accountLogin.text) || string.IsNullOrEmpty(_passwordLogin.text))
        {
            return;
        }
        //创建传输模型
        AccountDto dto = new AccountDto()
        {
            Account = _accountLogin.text,
            Password = _passwordLogin.text
        };
        //发送请求
        PhotonManager.Instance.Request(OperationCode.AccountCode, OpAccount.Login, JsonMapper.ToJson(dto));
        LoginInteractable = false;
    }

    public void ClearText()
    {
        _accountLogin.text = null;
        _accountRegister.text = null;
        _passwordCheckRegister.text = null;
        _passwordLogin.text = null;
        _passwordRegister.text = null;
    }
    #endregion
    #region UIBase
    public override string UIName()
    {
        return Paths.RES_UIAccount;
    }

    void Start()
    {
        bgmAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Hero") as AudioClip;
        enterAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "EnterGame") as AudioClip;
        clickAudioClip = ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Click") as AudioClip;

        SoundManager.Instance.PlayBGM(bgmAudioClip);
    }

    void OnDestroy()
    {
        bgmAudioClip = null;
        enterAudioClip = null;
        clickAudioClip = null;
    }
    #endregion

    /// <summary>
    /// 播放点击音效
    /// </summary>
    public void PlayClickAudio()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
    }
}
