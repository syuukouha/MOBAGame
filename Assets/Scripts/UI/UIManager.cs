using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI管理类
/// </summary>
public class UIManager : Singleton<UIManager>, IResourceListener
{
    /// <summary>
    /// UI名字和面板的映射关系
    /// </summary>
    private Dictionary<string, UIBase> UIDictionary = new Dictionary<string, UIBase>();

    /// <summary>
    /// 添加UI
    /// </summary>
    /// <param name="uiBase"></param>
    public void AddUI(UIBase uiBase)
    {
        if (uiBase == null)
            return;
        UIDictionary.Add(uiBase.UIName(), uiBase);
    }

    /// <summary>
    /// 删除UI
    /// </summary>
    /// <param name="uiBase"></param>
    public void DeleteUI(UIBase uiBase)
    {
        if (uiBase == null)
            return;
        if (!UIDictionary.ContainsValue(uiBase))
            return;
        UIDictionary.Remove(uiBase.UIName());
    }

    /// <summary>
    /// 显示UI，没有就创建一个
    /// </summary>
    public void ShowUI(string uiName)
    {
        if (UIDictionary.ContainsKey(uiName))
        {
            UIBase uiBase = UIDictionary[uiName];
            uiBase.OnShow();
        }
        else
        {
            ResourcesManager.Instance.Load(uiName, typeof(GameObject), this);
        }
    }
    /// <summary>
    /// 隐藏UI
    /// </summary>
    /// <param name="uiName"></param>
    public void HideUI(string uiName)
    {
        if (UIDictionary.ContainsKey(uiName))
        {
            UIBase uiBase = UIDictionary[uiName];
            uiBase.OnHide();
        }
    }
    /// <summary>
    /// 加载完成回调
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetPath, object asset)
    {
        var ui = Instantiate(asset as GameObject);
        UIBase uiBase = ui.GetComponent<UIBase>();
        uiBase.OnShow();
        AddUI(uiBase);
    }

}