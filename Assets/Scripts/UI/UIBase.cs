using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// UI基类
/// </summary>
public abstract class UIBase : MonoBehaviour
{
    /// <summary>
    /// UI名称
    /// </summary>
    public abstract string UIName();

    protected CanvasGroup _canvasGroup;
    /// <summary>
    /// 初始化
    /// </summary>
    public abstract void Init();

    // Use this for initialization
	void Awake () {
		Init();
	    if (_canvasGroup == null)
	        _canvasGroup = gameObject.AddComponent<CanvasGroup>();
	    else
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// 显示
    /// </summary>
    public virtual void OnShow()
    {
        _canvasGroup.alpha = 1;
        _canvasGroup.interactable = true;
        _canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 隐藏
    /// </summary>
    public virtual void OnHide()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// 销毁
    /// </summary>
    public abstract void OnDestory();
}
