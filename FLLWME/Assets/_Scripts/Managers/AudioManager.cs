using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;

    [Header("Mixers")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioMixerGroup bgmMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    

    [Header("Sources")]
    public AudioSource bgmSrc;
    public List<AudioSource> sources;

    private void Start() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public AudioManager GetInstance() {
        return instance;
    }

    public void PlayBGM(AudioClip bgm, bool isLooped = true) {
        if(bgmSrc == null) {
            bgmSrc = GetFreeAudioSource();
        }

        bgmSrc.clip = bgm;
        bgmSrc.loop = isLooped;
        bgmSrc.Play();
    }

    public void PlaySfx(AudioClip clip) {
        AudioSource src = GetFreeAudioSource();
        src.clip = clip;
        src.Play();
    }

    private AudioSource GetFreeAudioSource() {
        foreach (AudioSource source in sources) {
            if (!source.isPlaying) {
                return source;
            }
        }

        return null;
    }
}
