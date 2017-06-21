using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using MOBACommon.Codes;
using UnityEngine;
using UnityEngine.UI;

public class ChatView : MonoBehaviour
{
    public Text chatText;
    private InputField chatInputField;
    private Button sendButton;
    private Scrollbar scrollbar;
    // Use this for initialization
	void Awake ()
	{
	    chatInputField = transform.Find("ChatInputField").GetComponent<InputField>();
	    sendButton = transform.Find("SendButton").GetComponent<Button>();
	    scrollbar = transform.Find("Scrollbar").GetComponent<Scrollbar>();
	    sendButton.onClick.AddListener(OnSendClick);
	}
    /// <summary>
    /// 
    /// </summary>
    private void OnSendClick()
    {
        string text = chatInputField.text;
        if (string.IsNullOrEmpty(text))
            return;
        SoundManager.Instance.PlaySE(ResourcesManager.Instance.GetAsset(Paths.RES_UISOUND + "Click") as AudioClip);
        //给服务器发送 聊天请求
        PhotonManager.Instance.Request(OperationCode.SelectCode, OpSelect.Chat, text);
        //清空输入
        chatInputField.text = string.Empty;
    }

    public void Clear()
    {
        chatText.text = string.Empty;
        chatInputField.text = string.Empty;
    }
    public void UpdateChatText(string text)
    {
        chatText.text += text + "\n";
        scrollbar.value = 0;
    }
}
