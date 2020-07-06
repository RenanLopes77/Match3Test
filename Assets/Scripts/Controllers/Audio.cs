using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour {
    [SerializeField] private AudioClip clearSound = null;
    [SerializeField] private AudioClip gameSong = null;
    [SerializeField] private AudioClip selectSound = null;
    [SerializeField] private AudioClip swapSound = null;
    private AudioSource audioSource = null;

    private void Start() {
        SetAudioSource();
        SetGameSong();
    }

    private void SetAudioSource() {
        if (this.audioSource == null) {
            this.audioSource = GameObject.FindWithTag("GameAudio").GetComponent<AudioSource>();
        }
    }

    private void SetGameSong() {
        if (this.audioSource.clip.name != this.gameSong.name) {
            this.audioSource.clip = this.gameSong;
        }
    }

    public void PlaySwapSound() {
        this.audioSource.PlayOneShot(this.swapSound, 0.3f);
    }

    public void PlayclearSound() {
        this.audioSource.PlayOneShot(this.clearSound);
    }

    public void PlaySelectSound() {
        this.audioSource.PlayOneShot(this.selectSound);
    }
}