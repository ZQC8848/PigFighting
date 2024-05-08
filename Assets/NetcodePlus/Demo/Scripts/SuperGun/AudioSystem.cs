using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public class AudioSystem : MonoBehaviour
{
    static private AudioSystem instance;
    public List<AudioClip> audioClips;

    [SerializeField, Range(0, 1)]
    public float[] VolumeSetting;
    // 存储所有已经播放的声音
    private Dictionary<string, AudioSource> activeAudioSources = new Dictionary<string, AudioSource>();
    private void Start()
    {
        instance = this;
    }
    static public AudioSystem Get()
    {
        return instance;
    }
    public void PlaySound(string clipName, bool loop, float volume = 1.0f)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == clipName)
            {
                int index = audioClips.FindIndex(clip => clip.name == clipName);

                PlaySound(clip, clipName, VolumeSetting[index],loop);
            }
        }
    }
    public void PlaySound(string clipName, float volume = 1.0f)
    {
        foreach (AudioClip clip in audioClips)
        {
            if(clip.name == clipName)
            {
                int index = audioClips.FindIndex(clip => clip.name == clipName);
                PlaySound(clip, clipName, VolumeSetting[index]);
            }
        }
    }
    // 播放声音
    public void PlaySound(AudioClip clip, string soundName, float volume,bool loop = false)
    {
        // 如果已经有相同名字的声音在播放，则停止它
        if (activeAudioSources.ContainsKey(soundName))
        {
            activeAudioSources[soundName].Stop();
            activeAudioSources.Remove(soundName);
        }

        // 创建新的 AudioSource 并播放声音
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        audioSource.loop = loop;

        // 将 AudioSource 添加到 activeAudioSources 字典中
        activeAudioSources.Add(soundName, audioSource);

        // 启动协程，检查声音是否播放完毕，如果播放完毕则销毁对应的 AudioSource
        StartCoroutine(CheckSoundEnd(soundName, audioSource));
    }

    public void PlaySoundWaite(string clipName, float volume = 1.0f)
    {
        foreach (AudioClip clip in audioClips)
        {
            if (clip.name == clipName)
            {
                int index = audioClips.FindIndex(clip => clip.name == clipName);
                PlaySoudnWaite(clip, clipName, VolumeSetting[index]);
            }
        }
    }
    private void PlaySoudnWaite(AudioClip clip, string soundName, float volume, bool loop = false)
    {
        if (activeAudioSources.ContainsKey(soundName))
        {
            // 如果已经播放，则直接返回，不重新播放
            return;
        }
        // 创建新的 AudioSource 并播放声音
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
        audioSource.loop = loop;

        // 将 AudioSource 添加到 activeAudioSources 字典中
        activeAudioSources.Add(soundName, audioSource);

        // 启动协程，检查声音是否播放完毕，如果播放完毕则销毁对应的 AudioSource
        StartCoroutine(CheckSoundEnd(soundName, audioSource));
    }
    public void StopSound(string soundName)
    {
        if (activeAudioSources.ContainsKey(soundName))
        {
            activeAudioSources[soundName].Stop();
            activeAudioSources.Remove(soundName);
        }
    }
    // 检查声音是否播放完毕的协程
    private IEnumerator CheckSoundEnd(string soundName, AudioSource audioSource)
    {
        // 等待直到声音播放完毕
        while (audioSource.isPlaying)
        {
            yield return null;
        }

        // 当声音播放完毕后，销毁对应的 AudioSource
        Destroy(audioSource);

        // 从 activeAudioSources 字典中移除已经播放完毕的声音
        if (activeAudioSources.ContainsKey(soundName))
        {
            activeAudioSources.Remove(soundName);
        }
    }
}
