using UnityEngine;

public class PlayerAudioPlayer : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField] AudioClip[] walkSoundClips;

    [SerializeField] AudioClip jumpStart;

    [SerializeField] AudioClip jumpLand;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

   void PlayWalkSound()
   {
        int randomIndex = UnityEngine.Random.Range(0, walkSoundClips.Length);
        audioSource.resource = walkSoundClips[randomIndex];

        audioSource.Play();
   }

    void PlayJumpStart()
    {
        audioSource.resource = jumpStart;

        audioSource.Play();
    }

    public void PlayJumpLand()
    {
        audioSource.resource = jumpLand;

        audioSource.Play();
    }
}
