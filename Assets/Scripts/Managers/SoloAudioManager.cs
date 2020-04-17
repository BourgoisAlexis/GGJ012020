using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoloAudioManager : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Awake()
    {
        //Setup
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.spatialBlend = 1;
        _audioSource.minDistance = 0.2f;
        _audioSource.maxDistance = 1.5f;
    }

    public void PlaySound(string _sound, float _volume)
    {
        _audioSource.clip = AudioManager.Instance.GetSound(_sound);
        _audioSource.volume = _volume * AudioManager.Instance.generalVolume;
        _audioSource.Play();
    }

    public void PlayRandomSound(string[] _sounds, float _volume)
    {
        _audioSource.clip = AudioManager.Instance.GetRandomSound(_sounds);
        _audioSource.volume = _volume * AudioManager.Instance.generalVolume;
        _audioSource.Play();
    }
}
