using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource source;

    public void Awake() => source = gameObject.AddComponent<AudioSource>();

    public static async void Play(AudioClips clip, int delay = 0)
    {
        if (delay > 0) await Task.Delay(delay);
        Instance.source.PlayOneShot(GetAudioClip(clip));
    }

    public static AudioClip GetAudioClip(AudioClips clip) => clip switch
    {
        AudioClips.button => GetButtonAudio(),
        _ => GetAudio(clip)
    };

    private static AudioClip GetAudio(AudioClips clip) => Resources.Load<AudioClip>($"Sounds/{clip}");

    private static AudioClip GetButtonAudio()
    {
        //var clip = Resources.Load<AudioClip>($"Sounds/button{buttonIndex % 5}");
        var clip = Resources.Load<AudioClip>($"Sounds/button");
        buttonIndex++;
        return clip;
    }

    private static int buttonIndex
    {
        get => PlayerPrefs.GetInt("buttonSound", 0);
        set => PlayerPrefs.SetInt("buttonSound", value);
    }
}

public enum AudioClips { noAvailableMove, move, expand, ball, victory, confetti , button, tick }
