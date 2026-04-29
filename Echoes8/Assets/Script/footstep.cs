using UnityEngine;

public class footstep : MonoBehaviour
{
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;  // Array of different footstep sounds
    public float stepDelay = 0.5f;      // Delay between footsteps
    
    private float nextStepTime;
    private bool isMoving;
    private int currentStepIndex = 0; // L?u v? trí âm thanh hi?n t?i

    private void Update()
    {
        // Check if any movement key is being held down
        isMoving = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
                   Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);

        if (isMoving && Time.time >= nextStepTime)
        {
            PlayFootstep();
            nextStepTime = Time.time + stepDelay;
        }
    }

    private void PlayFootstep()
    {
        if (footstepAudioSource != null)
        {
            // N?u có footstepSounds, phát l?n l??t t?ng âm thanh
            if (footstepSounds != null && footstepSounds.Length > 0)
            {
                footstepAudioSource.PlayOneShot(footstepSounds[currentStepIndex]);
                currentStepIndex = (currentStepIndex + 1) % footstepSounds.Length;
            }
            // N?u không có, phát clip m?c ??nh
            else if (footstepAudioSource.clip != null)
            {
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
            }
        }
    }
}
