using UnityEngine;
using System.Collections;
using Pixeye.Unity;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    #region Values
    public AudioSource musicPlayerI;
    public AudioSource musicPlayerII;
    public int Smoothing;
    public AudioSource[] l_audioPlayers;

    private AudioSource toDown;
    private AudioSource toUp;
    private float incrementUp;
    private float incrementDown;

    [Range(0, 1)]
    public float generalVolume, musicVolume;


    [Foldout("Lapin", true)]
    public AudioClip nomDuSon;
    #endregion


    private void Awake()
    {
        DontDestroyOnLoad(this);

        if (Instance == null)
            Instance = this;
        else
            DestroyImmediate(gameObject);

        StartCoroutine("Video");
    }


    public void PlaySound(AudioClip sound, float volume)
    {
        for (int i = 0; i < l_audioPlayers.Length; i++)
        {
            AudioSource usedAudio = l_audioPlayers[i];
            if (!usedAudio.isPlaying)
            {
                usedAudio.clip = sound;
                usedAudio.volume = volume * generalVolume;
                usedAudio.Play();

                return;
            }
        }
    }

    public void PlayMusic(AudioClip music, float volume)
    {
        incrementUp = volume * musicVolume / Smoothing;
        if (musicPlayerI.isPlaying)
        {
            toUp = musicPlayerII;
            toDown = musicPlayerI;
            StartCoroutine("CrossFade", music);
        }
        else if(musicPlayerII.isPlaying)
        {
            toUp = musicPlayerI;
            toDown = musicPlayerII;
            StartCoroutine("CrossFade", music);
        }
        else
        {
            musicPlayerI.clip = music;
            musicPlayerI.Play();
            musicPlayerI.volume = volume * musicVolume;
        }
    }


    public IEnumerator CrossFade(AudioClip music)
    {
        toUp.clip = music;
        toUp.Play();
        toUp.volume = 0;
        incrementDown = toDown.volume / Smoothing;

        while(toDown.volume > 0)
        {
            yield return new WaitForSeconds(0.05f);
            toDown.volume -= incrementDown;
            toUp.volume += incrementUp;
        }
        toDown.Stop();
    }
}
