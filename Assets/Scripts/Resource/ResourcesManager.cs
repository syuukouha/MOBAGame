using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 资源管理类
/// </summary>
public class ResourcesManager : Singleton<ResourcesManager> {
    /// <summary>
    /// 已经加载的资源字典
    /// </summary>
    private Dictionary<string,object> loadedAssets = new Dictionary<string, object>();
    /// <summary>
    /// 正在加载的列表
    /// </summary>
    private List<LoadAssets> loadingList = new List<LoadAssets>();
    /// <summary>
    /// 等待加载的列表
    /// </summary>
    private Queue<LoadAssets> waitingQueue = new Queue<LoadAssets>();


    void Update()
    {
        if (loadingList.Count > 0)
        {
            for (int i = 0; i < loadingList.Count; i++)
            {
                if (loadingList[i].IsDone)
                {
                    LoadAssets loadAssets = loadingList[i];
                    for (int j = 0; j < loadAssets.Listeners.Count; j++)
                    {
                        loadAssets.Listeners[j].OnLoaded(loadAssets.AssetPath, loadAssets.GetAsset);
                    }
                    //添加到已经加载的字典里
                    loadedAssets.Add(loadAssets.AssetPath, loadAssets.GetAsset);
                    //从正在加载列表里移除
                    loadingList.RemoveAt(i);
                }
            }
        }

        while (waitingQueue.Count > 0 && loadingList.Count < 5)
        {
            LoadAssets loadAssets = waitingQueue.Dequeue();
            loadingList.Add(loadAssets);
            loadAssets.LoadAsync();
        }
    }

    /// <summary>
    /// 加载资源
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    /// <param name="assetType">资源类型</param>
    /// <param name="listener">回调</param>
    public void Load(string assetPath, Type assetType, IResourceListener listener)
    {
        //如果已经加载则返回
        if (loadedAssets.ContainsKey(assetPath))
        {
            listener.OnLoaded(assetPath,loadedAssets[assetPath]);
        }
        else
        {
            //没有就开始异步加载
            LoadAsync(assetPath, assetType, listener);
        }
    }
    /// <summary>
    /// 异步加载
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    /// <param name="assetType">资源类型</param>
    /// <param name="listener">回调</param>
    private void LoadAsync(string assetPath, Type assetType, IResourceListener listener)
    {
        //正在被加载，还没加载完成
        foreach (LoadAssets item in loadingList)
        {
            if (item.AssetPath == assetPath)
            {
                item.AddListener(listener);
                return;
            }
        }
        //等待的队列里面有
        foreach (LoadAssets item in waitingQueue)
        {
            if (item.AssetPath == assetPath)
            {
                item.AddListener(listener);
                return;
            }
        }
        //都没有 先创建
        LoadAssets loadAssets = new LoadAssets();
        loadAssets.AssetPath = assetPath;
        loadAssets.AssetType = assetType;
        loadAssets.AddListener(listener);
        //添加到等待队列
        waitingQueue.Enqueue(loadAssets);
    }
    /// <summary>
    /// 获取资源
    /// </summary>
    /// <param name="assetPath">资源路径</param>
    /// <returns></returns>
    public object GetAsset(string assetPath)
    {
        object obj = null;
        loadedAssets.TryGetValue(assetPath, out obj);
        return obj;
    }
    /// <summary>
    /// 释放资源
    /// </summary>
    /// <param name="assetPath"></param>
    public void ReleaseAsset(string assetPath)
    {
        if (loadedAssets.ContainsKey(assetPath))
        {
            loadedAssets[assetPath] = null;
            loadedAssets.Remove(assetPath);
        }
    }
    /// <summary>
    /// 强制释放所有
    /// </summary>
    public void ReleaseAll()
    {
        Resources.UnloadUnusedAssets();
        GC.Collect();
    }
}
