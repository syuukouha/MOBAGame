using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 资源类
/// </summary>
public class LoadAssets
{
    /// <summary>
    /// 资源信息
    /// </summary>
    public ResourceRequest resourceRequest;
    /// <summary>
    /// 回调的集合
    /// </summary>
    public List<IResourceListener> Listeners;
    /// <summary>
    /// 资源路径
    /// </summary>
    public string AssetPath;
    /// <summary>
    /// 资源类型
    /// </summary>
    public Type AssetType;
    /// <summary>
    /// 是否加载完成
    /// </summary>
    public bool IsDone
    {
        get { return resourceRequest != null && resourceRequest.isDone; }
    }
    /// <summary>
    /// 获取资源
    /// </summary>
    public object GetAsset
    {
        get
        {
            if (resourceRequest == null)
                return null;
            return resourceRequest.asset;
        }
    }
    /// <summary>
    /// 异步加载
    /// </summary>
    public void LoadAsync()
    {
        this.resourceRequest = Resources.LoadAsync(AssetPath, AssetType);
    }

    /// <summary>
    /// 添加回调
    /// </summary>
    /// <param name="listener"></param>
    public void AddListener(IResourceListener listener)
    {
        if(Listeners == null)
            Listeners = new List<IResourceListener>();
        if(Listeners.Contains(listener))
            return;
        Listeners.Add(listener);
    }
}
