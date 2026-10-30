using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _backgroundMusic;
    [SerializeField] private AudioClip _shotSound;

    [Header("Volumes")]
    [SerializeField] private float _menuMusicVolume = 0.6f; 
    [SerializeField] private float _gameMusicVolume = 0.2f;

    public void PlayMenuMusic() => SetMusicVolume(_menuMusicVolume);
    public void PlayGameMusic() => SetMusicVolume(_gameMusicVolume);

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (_musicSource == null)
        {
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
        }
        if (_sfxSource == null)
        {
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.loop = false;
        }

        if (_backgroundMusic != null)
        {
            _musicSource.clip = _backgroundMusic;
            _musicSource.volume = _gameMusicVolume;
            _musicSource.Play();
        }
    }

    public void PlayShootSound()
    {
        if (_shotSound != null && _sfxSource != null)
        {
            if (!_sfxSource.isPlaying)
            {
                _sfxSource.clip = _shotSound;
                _sfxSource.Play();
            }
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (_musicSource != null)
        {
            _musicSource.volume = volume;
        }
    }

}