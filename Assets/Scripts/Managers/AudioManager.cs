using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    #region Values
    [Range(0, 1)]
    public float generalVolume, musicVolume;

    [SerializeField] private int Smoothing;
    [SerializeField] private AudioClip[] Sounds;

    private AudioSource MusicPlayerI;
    private AudioSource MusicPlayerII;
    private List<AudioSource> AudioPlayers = new List<AudioSource>();

    private Dictionary<string, AudioClip> clips = new Dictionary<string, AudioClip>();

    private AudioSource toDown;
    private AudioSource toUp;
    private float incrementUp;
    private float incrementDown;
    #endregion


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        Setup();
    }

    public void Setup()
    {
        MusicPlayerI = gameObject.AddComponent<AudioSource>();
        MusicPlayerII = gameObject.AddComponent<AudioSource>();
        AudioPlayers.Add (gameObject.AddComponent<AudioSource>());
        AudioPlayers.Add (gameObject.AddComponent<AudioSource>());
        AudioPlayers.Add (gameObject.AddComponent<AudioSource>());

        MusicPlayerI.loop = true;
        MusicPlayerII.loop = true;

        for (int i = 0; i < Sounds.Length; i ++)
            if (!clips.ContainsKey(Sounds[i].name))
                clips.Add(Sounds[i].name, Sounds[i]);
    }


    //Returns a sound from the sound dictio
    public AudioClip GetSound(string _sound)
    {
        if (clips.ContainsKey(_sound))
            return clips[_sound];

        return null;
    }

    public AudioClip GetRandomSound(string[] _sounds)
    {
        string sound = _sounds[Random.Range(0, _sounds.Length)];

        if (clips.ContainsKey(sound))
            return clips[sound];

        return null;
    }

    //PlaySounds
    public void PlaySound(string _sound, float _volume)
    {
        if (clips.ContainsKey(_sound))
            for (int i = 0; i < AudioPlayers.Count; i++)
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
            for (int i = 0; i < AudioPlayers.Count; i++)
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
