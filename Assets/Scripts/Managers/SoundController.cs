using UnityEngine;

public class SoundController : MonoBehaviour
{
    [SerializeField] private AudioClip _harborSounds;
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _musicSource;



    private void Awake()
    {
        var sfxSource = _sfxSource.GetComponent<AudioSource>();
        var musicSource = _musicSource.GetComponent<AudioSource>();

        _sfxSource.clip = _harborSounds;

        _musicSource.clip = _backgroundMusic;
    }

    private void Start()
    {
        _sfxSource.Play();
        _musicSource.Play();
    }
}
