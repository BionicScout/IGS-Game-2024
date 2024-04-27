using UnityEngine.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public Sound[] sounds;

    List<int> soundTrackIndexes = new List<int>();
    int currentSoundTrack;
    Sound currentTrack;

    public static AudioManager instance;
    public bool fade;
    public int fadeTime = 2;
    float fadeTimer = 0;

    public bool loopTrack = true;
    int nextLoop = -1;

    public Sound genericSound;

    void Awake() {

        if (instance == null)
            instance = this;
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        for (int i = 0; i < sounds.Length; i++) {
            sounds[i].source = gameObject.AddComponent<AudioSource>();
            sounds[i].source.clip = sounds[i].clip;

            sounds[i].source.volume = sounds[i].volume;
            sounds[i].source.pitch = sounds[i].pitch;
            sounds[i].source.loop = sounds[i].loop;

			if (sounds[i].soundTrack) {
                soundTrackIndexes.Add(i);
			}
        }
    }

    private void Start() {
        currentSoundTrack = 0; //UnityEngine.Random.Range(0, soundTrackIndexes.Count-1);
		currentTrack = sounds[soundTrackIndexes[currentSoundTrack]];
		Play(currentTrack.name);
    }

    void Update() {
        if (soundTrackIndexes.Count == 0)
            return;

		if (!currentTrack.source.isPlaying) {
            if(!loopTrack) {

                currentSoundTrack++;

                if(currentSoundTrack >= soundTrackIndexes.Count) {
                    currentSoundTrack = 0;

                }
            }

			currentTrack = sounds[soundTrackIndexes[currentSoundTrack]];
			Play(currentTrack.name);
		}

        if(fade) {
            fadeTimer += Time.deltaTime;

            if(fadeTimer > fadeTime) {
                fade = false;
                fadeTimer = 0;
                sounds[currentSoundTrack].source.Stop();

                if(nextLoop != -1) {
                    currentSoundTrack = nextLoop;
                    nextLoop = -1;
                }

                if(currentSoundTrack >= soundTrackIndexes.Count) {
                    currentSoundTrack = 0;
                }

                currentTrack = sounds[soundTrackIndexes[currentSoundTrack]];
                Play(currentTrack.name);
            }
            else {
                AudioSource s = sounds[soundTrackIndexes[currentSoundTrack]].source;
                s.volume = sounds[soundTrackIndexes[currentSoundTrack]].volume * ((fadeTime-fadeTimer) / fadeTime);
                Debug.Log(s.volume);
            }

        }
	}

    public void Play(string name) {
        Sound s = null;
        foreach(Sound sound in sounds) {
            if(sound.name == name) {
                s = sound;
                break;
            }
        }

        if(s == null) {
            Debug.LogWarning("Audio \"" + name + "\" not found");
            genericSound.source.Play();
            return;
        }

        s.source.Play();
    }

    public void PlayFromSoundtrack(string name) {
        int index = -1;
        for(int i = 0; i < sounds.Length; i++) {
            Sound sound = sounds[i];
            if(sound.name == name) {
                index = i;
                break;
            }
        }

        if(index == -1) {
            Debug.LogWarning("Audio \"" + name + "\" not found");
            return;
        }

        fade = true;
        nextLoop = index;
    }

}
