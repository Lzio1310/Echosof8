using UnityEngine;

public class TentacleSound : MonoBehaviour
{
    [Header("Audio Source")]
    public AudioSource audioSource; // nơi phát âm thanh

    [Header("Clips")]
    public AudioClip attackClip;
    public AudioClip wriggleClip;
    public AudioClip retractClip;

    void Awake()
    {
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
    }

    // Animation Event sẽ gọi các hàm này
    public void PlayAttackSound()
    {
        if (attackClip != null && audioSource != null)
            audioSource.PlayOneShot(attackClip);
    }

    public void PlayWriggleSound()
    {
        if (wriggleClip != null && audioSource != null)
            audioSource.PlayOneShot(wriggleClip);
    }

    public void PlayRetractSound()
    {
        if (retractClip != null && audioSource != null)
            audioSource.PlayOneShot(retractClip);
    }
}
