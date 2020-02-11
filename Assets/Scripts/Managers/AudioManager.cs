using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pixeye.Unity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    #region Values
    public int Smoothing;
    [Range(0, 1)]
    public float generalVolume, musicVolume;

    public AudioClip[] Sounds;
    [Foldout("AudioSources", true)]
    public AudioSource MusicPlayerI;
    public AudioSource MusicPlayerII;
    public AudioSource[] AudioPlayers;


    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();
    private AudioSource toDown;
    private AudioSource toUp;
    private float incrementUp;
    private float incrementDown;
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        Setup();
    }

    public void Setup()
    {
        MusicPlayerI.loop = true;
        MusicPlayerII.loop = true;

        for (int i = 0; i < Sounds.Length; i ++)
            if (!clips.ContainsKey(Sounds[i].name))
                clips.Add(Sounds[i].name, Sounds[i]);
    }


    public void PlaySound(string _sound, float _volume)
    {
        if (clips.ContainsKey(_sound))
            for (int i = 0; i < AudioPlayers.Length; i++)
            {
                AudioSource useSource = AudioPlayers[i];

                if (!useSource.isPlaying)
                {
                    useSource.clip = clips[_sound];
                    useSource.volume = _volume * generalVolume;
                    useSource.Play();
                    return;
                }
            }
    }

    public void PlayRandomSound(string[] _sounds, float _volume)
    {
        string sound = _sounds[Random.Range(0, _sounds.Length)];

        if (clips.ContainsKey(sound))
            for (int i = 0; i < AudioPlayers.Length; i++)
            {
                AudioSource useSource = AudioPlayers[i];

                if (!useSource.isPlaying)
                {
                    useSource.clip = clips[sound];
                    useSource.volume = _volume * generalVolume;
                    useSource.Play();
                    return;
                }
            }
    }

    public void PlayMusic(string _music, float _volume)
    {
        incrementUp = _volume * musicVolume / Smoothing;

        if (clips.ContainsKey(_music))
        {
            if (MusicPlayerI.isPlaying)
            {
                toUp = MusicPlayerII;
                toDown = MusicPlayerI;
                StartCoroutine(CrossFade(clips[_music]));
            }
            else
            {
                toUp = MusicPlayerI;
                toDown = MusicPlayerII;
                StartCoroutine(CrossFade(clips[_music]));
            }
        }
    }


    private IEnumerator CrossFade(AudioClip _music)
    {
        toUp.clip = _music;
        toUp.Play();
        toUp.volume = 0;
        incrementDown = toDown.volume / Smoothing;

        while(toDown.volume > 0)
        {
            yield return new WaitForEndOfFrame();
            toDown.volume -= incrementDown;
            toUp.volume += incrementUp;
        }
        toDown.Stop();
    }
}
