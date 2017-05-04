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

    //音效的List
    //private List<AudioSource> seAudioSourceList = new List<AudioSource>();

    ///// <summary>
    ///// 等待的队列
    ///// </summary>
    //private Queue<AudioClip> seAudioClipQueue = new Queue<AudioClip>();

    // Use this for initialization
    void Start ()
	{
	    bgmAudioSource.loop = true;
	    bgmAudioSource.playOnAwake = true;
        seAudioSource.loop = false;
        seAudioSource.playOnAwake = false;
    }

    void Update()
    {
        //    //如果这个 AudioSource不在播放，并且等待队列的音效文件数量大于0
        //    if (!seAudioSource.isPlaying && seAudioClipQueue.Count >= 0)
        //    {
        //        AudioClip audioClip = seAudioClipQueue.Dequeue();
        //        seAudioSource.clip = audioClip;
        //        seAudioSource.Play();
        //    }
        //if (seAudioSourceList.Count>=0)
        //{
        //    for (int i = 0; i < seAudioSourceList.Count; i++)
        //    {
        //        if (!seAudioSourceList[i].isPlaying)
        //        {
        //            Destroy(seAudioSourceList[i]);
        //            seAudioSourceList.Remove(seAudioSourceList[i]);
        //        }
        //    }
        //}
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

    public void PlaySE(AudioClip clip,bool isOneShot = true)
    {
        if (clip == null)
            return;
        if (isOneShot)
        {
            seAudioSource.PlayOneShot(clip);
        }
        else
        {
            seAudioSource.clip = clip;
            seAudioSource.Play();
        }
        //AudioSource seAudioSource = gameObject.AddComponent<AudioSource>();
        //seAudioSource.loop = false;
        //seAudioSource.playOnAwake = false;
        //seAudioSourceList.Add(seAudioSource);
        //seAudioSource.clip = clip;
        //seAudioSource.Play();
        //seAudioClipQueue.Enqueue(clip);
    }
    #endregion

}
