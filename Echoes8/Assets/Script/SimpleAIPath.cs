using UnityEngine;

public class SimpleAIPath : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 2f;
    public float rotationSpeed = 5f;
    public float stopDistance = 0.2f;
    public bool loop = false;
    public bool startOnTrigger = true;

    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepSounds;
    public float stepDelay = 0.5f;

    private int currentIndex = 0;
    private Animator animator;
    private bool canMove = false;
    private float nextStepTime;
    private int currentStepIndex = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (!startOnTrigger) canMove = true;
        
        if (footstepAudioSource == null)
        {
            footstepAudioSource = gameObject.AddComponent<AudioSource>();
        }
        // Thiết lập âm thanh 3D cho bước chân
        footstepAudioSource.spatialBlend = 1f; // 1 = 3D
        footstepAudioSource.minDistance = 1f; // Khoảng cách nghe rõ nhất
        footstepAudioSource.maxDistance = 15f; // Xa nhất còn nghe được
        footstepAudioSource.rolloffMode = AudioRolloffMode.Logarithmic;
    }

    void Update()
    {
        if (!canMove || waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        Vector3 move = direction * speed * Time.deltaTime;

        if (direction.magnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            
            // Play footstep when moving
            if (Time.time >= nextStepTime)
            {
                PlayFootstep();
                nextStepTime = Time.time + stepDelay;
            }
        }

        transform.position += move;
        animator.SetFloat("Speed", direction.magnitude * speed);

        if (Vector3.Distance(transform.position, target.position) < stopDistance)
        {
            currentIndex++;

            if (currentIndex >= waypoints.Length)
            {
                if (loop)
                {
                    currentIndex = 0;
                }
                else
                {
                    canMove = false; // đứng yên tại chỗ
                    animator.SetFloat("Speed", 0f);
                }
            }
        }
    }

    private void PlayFootstep()
    {
        if (footstepAudioSource != null)
        {
            if (footstepSounds != null && footstepSounds.Length > 0)
            {
                footstepAudioSource.PlayOneShot(footstepSounds[currentStepIndex]);
                currentStepIndex = (currentStepIndex + 1) % footstepSounds.Length;
            }
            else if (footstepAudioSource.clip != null)
            {
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
            }
        }
    }

    public void StartMove()
    {
        canMove = true;
    }

    public void ResetAI()
    {
        currentIndex = 0;
        canMove = false; // đợi trigger
        animator.SetFloat("Speed", 0f);

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            transform.rotation = waypoints[0].rotation;
        }
    }
}
