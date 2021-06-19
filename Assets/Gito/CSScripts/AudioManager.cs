using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource bgmSource, seSource;
    private static AudioManager _audioManager;
    private static AudioManager audioManager
    {
        get
        {
            return _audioManager == null ? _audioManager = GameObject.FindWithTag("AudioManager").GetComponent<AudioManager>() : _audioManager;
        }
    }

    public static void BGMStop()
    {
        audioManager._BGMStop();
    }

    private void _BGMStop()
    {
        bgmSource.Stop();
    }

    public static void PlayOneShot(AudioClip audioClip)
    {
        audioManager._PlayOneShot(audioClip);
    }

    private void _PlayOneShot(AudioClip audioClip)
    {
        if (audioClip != null)
        {
            seSource.PlayOneShot(audioClip);
        }
    }
}
