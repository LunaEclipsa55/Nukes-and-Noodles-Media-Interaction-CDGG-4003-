using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static MusicManager Instance;
    private AudioSource audioSource;
    public AudioClip backgroundMusic;
    [SerializeField] private Slider musicSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if(backgroundMusic != null)
        {
            PlayBackgroundMusic(false, backgroundMusic);
        }

        //musicSlider.value = 0.2f; 
        musicSlider.onValueChanged.AddListener(delegate { SetVolume(musicSlider.value); });

    }

    public static void SetVolume(float volume)
    {
        Instance.audioSource.volume = volume;
    }
    public static void PlayBackgroundMusic(bool resetSong, AudioClip audioClip = null)
    {
        if(audioClip != null)
        {
            Instance.audioSource.clip = audioClip;
        }
        if (Instance.audioSource.clip != null)
        {
            if (resetSong)
            {
                Instance.audioSource.Stop();
            }
            Instance.audioSource.Play();
        }

    }
    public static void PauseBackgroundMusic()
    {
        Instance.audioSource.Pause();
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        
        musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();

        if (musicSlider != null)
        {
            musicSlider.onValueChanged.RemoveAllListeners(); 
            musicSlider.onValueChanged.AddListener(SetVolume);

            musicSlider.value = audioSource.volume;
        }
    }
}
