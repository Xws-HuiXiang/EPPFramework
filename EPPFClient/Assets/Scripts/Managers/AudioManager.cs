using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoSingleton<AudioManager>
{
    private AudioSource[] audioSourceArray;
    /// <summary>
    /// 本物体上AudioSource数组
    /// </summary>
    public AudioSource[] AudioSourceArray { get { return audioSourceArray; } }

    /// <summary>
    /// 背景音乐音量
    /// </summary>
    public float BGMVolume { get; set; }
    /// <summary>
    /// 背景音乐是否静音
    /// </summary>
    public bool BGMMute { get; set; }
    /// <summary>
    /// 音效音量
    /// </summary>
    public float EffectVolume { get; set; }
    /// <summary>
    /// 音效是否静音
    /// </summary>
    public bool EffectMute { get; set; }

    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        audioSourceArray = this.gameObject.GetComponentsInChildren<AudioSource>();
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public AudioSource PlayBGM(AudioClip audioClip)
    {
        return PlayAudioClip(audioClip, true, BGMMute, BGMVolume, null);
    }

    /// <summary>
    /// 播放音效音乐
    /// </summary>
    /// <param name="audioClip"></param>
    /// <returns></returns>
    public AudioSource PlayEffect(AudioClip audioClip)
    {
        return PlayAudioClip(audioClip, false, EffectMute, EffectVolume, null);
    }

    /// <summary>
    /// 播放AudioClip
    /// </summary>
    /// <param name="audioClip"></param>
    /// <param name="isLoop"></param>
    /// <param name="volume"></param>
    /// <param name="callback"></param>
    public AudioSource PlayAudioClip(AudioClip audioClip, bool isLoop, bool isMute, float volume, Action callback)
    {
        if (audioClip)
        {
            AudioSource audioSource = GetAudioSource();

            audioSource.clip = audioClip;
            audioSource.loop = isLoop;
            audioSource.mute = isMute;
            audioSource.volume = volume;
            if(callback != null)
            {
                StartCoroutine(AudioClipPlayDoneCallbackIE(audioClip.length, isLoop, callback));
            }

            audioSource.Play();

            return audioSource;
        }

        FDebugger.LogWarning("要播放的audioClip为空");

        return null;
    }

    /// <summary>
    /// AudioClip播放完成时的回调函数
    /// </summary>
    /// <param name="delay">延迟时间，单位为秒</param>
    /// <param name="callback">延迟时间后调用的函数</param>
    /// <returns></returns>
    private IEnumerator AudioClipPlayDoneCallbackIE(float delay, bool isLoop, Action callback)
    {
        yield return new WaitForSeconds(delay);

        if(callback != null)
        {
            callback.Invoke();
        }

        //循环
        if (isLoop)
        {
            StartCoroutine(AudioClipPlayDoneCallbackIE(delay, isLoop, callback));
        }
    }

    /// <summary>
    /// 停止所有的音乐播放完成的协程
    /// </summary>
    public void StopAllIEnumerator()
    {
        StopAllCoroutines();
    }

    /// <summary>
    /// 从本物体上获取一个没有在播放音频的AudioSource组件
    /// </summary>
    /// <returns></returns>
    public AudioSource GetAudioSource()
    {
        foreach(AudioSource audioSource in AudioSourceArray)
        {
            //如果当前audioSource没有播放音频，则返回
            if (!audioSource.isPlaying)
            {
                return audioSource;
            }
        }

        //没有找到一个空闲的AudioSource则在本物体上添加一个
        AudioSource audioSourceComponent = this.gameObject.AddComponent<AudioSource>();
        audioSourceComponent.playOnAwake = false;
        List<AudioSource> tempList = AudioSourceArray.ToList();
        tempList.Add(audioSourceComponent);
        audioSourceArray = tempList.ToArray();

        return audioSourceComponent;
    }
}
