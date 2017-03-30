using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 游戏初始化
/// </summary>
public class GameInit : MonoBehaviour,IResourceListener
{
	// Use this for initialization
	void Start ()
    {
        //加载音乐
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "Hero", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "EnterGame", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_UISOUND + "Click", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SelectHero + "狐狸", typeof(AudioClip), this);
        ResourcesManager.Instance.Load(Paths.RES_SOUND_SelectHero + "瞎子", typeof(AudioClip), this);
        //加载头像资源
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "noSelect", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "noConnect", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "瞎子", typeof(Sprite), this);
        ResourcesManager.Instance.Load(Paths.RES_UIHeroHead + "狐狸", typeof(Sprite), this);
        //加载登录UI
        UIManager.Instance.ShowUI(Paths.RES_UIAccount);
    }
    /// <summary>
    /// 加载回调
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="asset"></param>
    public void OnLoaded(string assetPath, object asset)
    {

    }
}
