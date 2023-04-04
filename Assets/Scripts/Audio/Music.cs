using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : Sound
{
    void Awake()
    {
        soundClip = GetComponent<AudioClip>();
        sound = GetComponent<AudioSource>();
    }

    override public void PlaySound(string _sound) {
        soundClip = Resources.Load<AudioClip>(_sound);
        soundClip.LoadAudioData();
        sound.clip = soundClip;
        StartCoroutine(musicCoroutine());
    }

    IEnumerator musicCoroutine() {
        while (true) {
            switch (soundClip.loadState) {
                case AudioDataLoadState.Loaded:
                    sound.clip = soundClip;
                    sound.Play();
                    yield return new WaitForSeconds(soundClip.length + 2);
                    SoundManager.Instance.NewMusic();
                    Destroy(this.gameObject);
                    yield break;
                case AudioDataLoadState.Failed:
                case AudioDataLoadState.Unloaded:
                    yield break;
                default:
                    yield return new WaitForEndOfFrame();
                    break;
            }
        }
    }
}
