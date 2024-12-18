using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip backgroundMusic;
    public AudioClip moveSound;
    public AudioClip dropSound;
    public AudioClip rotateSound;
    public AudioClip newGameSound;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        audioSource.clip = backgroundMusic;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayMoveSound()
    {
        PlaySound(moveSound);
    }

    public void PlayDropSound()
    {
        PlaySound(dropSound);
    }

    public void PlayRotateSound()
    {
        PlaySound(rotateSound);
    }

    public void PlayNewGameSound()
    {
        PlaySound(newGameSound);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
