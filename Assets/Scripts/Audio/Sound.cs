using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class Sound : MonoBehaviour
{
    internal AudioClip soundClip;
    internal AudioSource sound;
    void Awake() {
        soundClip = GetComponent<AudioClip>();
        sound = GetComponent<AudioSource>();
    }

    virtual public void PlaySound(string _sound) {
        soundClip = Resources.Load<AudioClip>(_sound);
        sound.clip = soundClip;
        StartCoroutine(soundCoroutine());
    }

    IEnumerator soundCoroutine() {
        while (true) {
            switch (soundClip.loadState) {
                case AudioDataLoadState.Loaded:
                    sound.clip = soundClip;
                    sound.Play();
                    yield return new WaitForSeconds(soundClip.length);
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
