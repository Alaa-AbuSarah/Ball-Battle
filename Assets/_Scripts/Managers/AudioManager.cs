using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioClip music = null;
    [SerializeField] private AudioClip click = null;

    [Space]

    [SerializeField] private AudioSource musicSource = null;
    [SerializeField] private AudioSource sfxSource = null;

    private void Start()
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 0.5f) => sfxSource.PlayOneShot(clip, volume);
    public void PlayClick() => PlaySFX(click);
}
