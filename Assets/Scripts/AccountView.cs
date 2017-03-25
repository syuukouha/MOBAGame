using System.Collections;
using System.Collections.Generic;
using LitJson;
using MOBACommon.Codes;
using MOBACommon.Dto;
using UnityEngine;
using UnityEngine.UI;

public class AccountView : UIBase,IResourceListener {
    #region 注册模块
    [Header("Register")]
    [SerializeField]
    private InputField _accountRegister;
    [SerializeField]
    private InputField _passwordRegister;
    [SerializeField]
    private InputField _passwordCheckRegister;
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
    [Header("Login")]
    [SerializeField]
    private InputField _accountLogin;
    [SerializeField]
    private InputField _passwordLogin;

    public  Button _loginButton;

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
        _loginButton.interactable = false;
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

    public override void Init()
    {
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "Hero", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "EnterGame", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "Click", typeof(AudioClip), this);
    }

    public override void OnDestory()
    {
        bgmAudioClip = null;
        enterAudioClip = null;
        clickAudioClip = null;
    }
    #endregion


    
    private AudioClip bgmAudioClip;
    private AudioClip enterAudioClip;
    private AudioClip clickAudioClip;

    public void OnLoaded(string assetPath, object asset)
    {
        AudioClip clip = asset as AudioClip;
        switch (assetPath)
        {
            case Paths.RES_UISOUND + "Hero":
                bgmAudioClip = clip;
                SoundManager.Instance.PlayBGM(bgmAudioClip);
                break;
            case Paths.RES_UISOUND + "EnterGame":
                enterAudioClip = clip;
                break;
            case Paths.RES_UISOUND + "Click":
                clickAudioClip = clip;
                break;
        }
    }
    /// <summary>
    /// 播放点击音效
    /// </summary>
    public void PlayClickAudio()
    {
        SoundManager.Instance.PlaySE(clickAudioClip);
    }
}
