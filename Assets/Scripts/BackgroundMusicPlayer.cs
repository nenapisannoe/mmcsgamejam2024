using UnityEngine;

public class BackgroundMusicPlayer : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip mainMenuMusic;

    [SerializeField] AudioClip levelMusic;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        GameController.Instance.onMainMenuShow += PlayMainMenuMusic;
        GameController.Instance.onLevelShow += PlayLevelMusic;
    }

    void PlayMainMenuMusic()
    {
        audioSource.resource = mainMenuMusic;
        audioSource.Play();
    }

    void PlayLevelMusic()
    {
        audioSource.resource = levelMusic;
        audioSource.Play();
    }

}
