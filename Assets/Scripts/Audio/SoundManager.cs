using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject soundPrefab, untimedSoundPrefab, musicPrefab;
    [SerializeField] private ArrayList audioSourceArray;

    public enum Sounds {

    }

    void Awake() {
        Instance = this;
    }

    void Start() {
        switch (SceneManager.GetActiveScene().buildIndex) {
            case 0:
                MenuMusic();
                break;
            case 1:
                NewMusic();
                break;
        }    
    }

    public void NewMusic() {
        int musicIndex = Random.Range(1, 6);
        GameObject soundObject = Instantiate(musicPrefab);
        Sound sound = soundObject.GetComponent<Sound>();
        AudioSource soundClip = soundObject.GetComponent<AudioSource>();
        sound.PlaySound("Music" + musicIndex);
    }

    public void MenuMusic() {
        GameObject soundObject = Instantiate(musicPrefab);
        Sound sound = soundObject.GetComponent<Sound>();
        AudioSource soundClip = soundObject.GetComponent<AudioSource>();
        sound.PlaySound("MenuMusic");
    }

    public void PlaySound(string soundString) {
        GameObject soundObject = Instantiate(soundPrefab);
        Sound sound = soundObject.GetComponent<Sound>();
        sound.PlaySound(soundString);
    }

    public void PlayUntimedSound(string soundString) {
        GameObject soundObject = Instantiate(untimedSoundPrefab);
        UntimedSound sound = soundObject.GetComponent<UntimedSound>();
        AudioSource soundSource = soundObject.GetComponent<AudioSource>();
        soundSource.ignoreListenerPause = true;
        sound.PlaySound(soundString);
    }
}
