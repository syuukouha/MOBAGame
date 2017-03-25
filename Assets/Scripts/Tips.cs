using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tips : Singleton<Tips>
{
    /// <summary>
    /// 显示文字
    /// </summary>
    public Text message;

    public GameObject TipsPanel;

    private Action OnCompleted;

    void Start()
    {
        TipsPanel.SetActive(false);
    }

    public void Show(string text,Action action)
    {
        TipsPanel.SetActive(true);
        message.text = text;
        OnCompleted = action;
    }

    public void OnClick()
    {
        TipsPanel.SetActive(false);
        if (OnCompleted != null)
        {
            OnCompleted();
            OnCompleted = null;
        }
    }
}
