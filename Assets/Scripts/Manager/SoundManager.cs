using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 声音管理类
/// </summary>
public class SoundManager : Singleton<SoundManager>
{
    /// <summary>
    /// 用来播放背景音乐（循环）
    /// </summary>
    [SerializeField]
    private AudioSource bgmAudioSource;

    /// <summary>
    /// 特效音
    /// </summary>
    [SerializeField]
    private AudioSource seAudioSource;
	// Use this for initialization
	void Start ()
	{
	    bgmAudioSource.loop = true;
	    bgmAudioSource.playOnAwake = true;
	    seAudioSource.loop = false;
	    seAudioSource.playOnAwake = false;
    }

    #region 背景音乐
    public float BGMVolume
    {
        get { return bgmAudioSource.volume; }
        set { bgmAudioSource.volume = value; }
    }

    public void PlayBGM(AudioClip clip)
    {
        if(clip == null)
            return;
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBGM()
    {
        bgmAudioSource.clip = null;
        bgmAudioSource.Stop();
    }
    #endregion
    #region 特效音

    public void PlaySE(AudioClip clip)
    {
        if (clip == null)
            return;
        seAudioSource.clip = clip;
        seAudioSource.Play();
    }
    #endregion

}
