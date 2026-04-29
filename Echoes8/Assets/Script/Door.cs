using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool IsOpen = false;
    [SerializeField]
    private bool IsRotatingDoor = true;
    [SerializeField]
    private float Speed = 1f;
    [Header("Rotation Configs")]
    [SerializeField]
    private float RotationAmount = 90f;
    [SerializeField]
    private float ForwardDirection = 0;
    [Header("Sliding Configs")]
    [SerializeField]
    private Vector3 SlideDirection = Vector3.back;
    [SerializeField]
    private float SlideAmount = 1.9f;

    [Header("Audio")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip doorOpenSound;
    [SerializeField]
    private AudioClip doorCloseSound;

    private Vector3 StartRotation;
    private Vector3 StartPosition;
    private Vector3 Forward;

    private Coroutine AnimationCoroutine;

    private void Awake()
    {
        StartRotation = transform.rotation.eulerAngles;
        // Since "Forward" actually is pointing into the door frame, choose a direction to think about as "forward" 
        Forward = transform.right;
        StartPosition = transform.position;

        // Get or add AudioSource component
        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();
    }

    private void PlayDoorOpenSound()
    {
        if (audioSource != null && doorOpenSound != null)
        {
            audioSource.PlayOneShot(doorOpenSound);
        }
    }

    private void PlayDoorCloseSound()
    {
        if (audioSource != null && doorCloseSound != null)
        {
            audioSource.PlayOneShot(doorCloseSound);
        }
    }

    public void Open(Vector3 UserPosition)
    {
        if (!IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            PlayDoorOpenSound();

            if (IsRotatingDoor)
            {
                float dot = Vector3.Dot(Forward, (UserPosition - transform.position).normalized);
                Debug.Log($"Dot: {dot.ToString("N3")}");
                AnimationCoroutine = StartCoroutine(DoRotationOpen(dot));
            }
            else
            {
                AnimationCoroutine = StartCoroutine(DoSlidingOpen());
            }
        }
    }

    private IEnumerator DoRotationOpen(float ForwardAmount)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation;

        if (ForwardAmount >= ForwardDirection)
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y + RotationAmount, 0));
        }
        else
        {
            endRotation = Quaternion.Euler(new Vector3(0, StartRotation.y - RotationAmount, 0));
        }

        IsOpen = true;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private IEnumerator DoSlidingOpen()
    {
        Vector3 endPosition = StartPosition + SlideAmount * SlideDirection;
        Vector3 startPosition = transform.position;

        float time = 0;
        IsOpen = true;

        // Store all child transforms to ensure everything moves together
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        // Calculate the offset for each child from the door's position
        Vector3[] childOffsets = new Vector3[allChildren.Length];
        for (int i = 0; i < allChildren.Length; i++)
        {
            // Skip the door object itself (it's included in GetComponentsInChildren)
            if (allChildren[i] == transform) continue;
            childOffsets[i] = allChildren[i].position - transform.position;
        }

        while (time < 1)
        {
            // Move the door
            Vector3 newDoorPosition = Vector3.Lerp(startPosition, endPosition, time);
            transform.position = newDoorPosition;

            // Update all child positions to maintain their relative positions
            for (int i = 0; i < allChildren.Length; i++)
            {
                if (allChildren[i] == transform) continue;
                allChildren[i].position = newDoorPosition + childOffsets[i];
            }

            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    public void Close()
    {
        if (IsOpen)
        {
            if (AnimationCoroutine != null)
            {
                StopCoroutine(AnimationCoroutine);
            }

            PlayDoorCloseSound();

            if (IsRotatingDoor)
            {
                AnimationCoroutine = StartCoroutine(DoRotationClose());
            }
            else
            {
                AnimationCoroutine = StartCoroutine(DoSlidingClose());
            }
        }
    }

    private IEnumerator DoRotationClose()
    {
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.Euler(StartRotation);

        IsOpen = false;

        float time = 0;
        while (time < 1)
        {
            transform.rotation = Quaternion.Slerp(startRotation, endRotation, time);
            yield return null;
            time += Time.deltaTime * Speed;
        }
    }

    private IEnumerator DoSlidingClose()
    {
        Vector3 endPosition = StartPosition;
        Vector3 startPosition = transform.position;
        float time = 0;

        IsOpen = false;

        // Store all child transforms to ensure everything moves together
        Transform[] allChildren = GetComponentsInChildren<Transform>();

        // Calculate the offset for each child from the door's position
        Vector3[] childOffsets = new Vector3[allChildren.Length];
        for (int i = 0; i < allChildren.Length; i++)
        {
            // Skip the door object itself (it's included in GetComponentsInChildren)
            if (allChildren[i] == transform) continue;
            childOffsets[i] = allChildren[i].position - transform.position;
        }

        while (time < 1)
        {
            // Move the door
            Vector3 newDoorPosition = Vector3.Lerp(startPosition, endPosition, time);
            transform.position = newDoorPosition;

            // Update all child positions to maintain their relative positions
            for (int i = 0; i < allChildren.Length; i++)
            {
                if (allChildren[i] == transform) continue;
                allChildren[i].position = newDoorPosition + childOffsets[i];
            }

            yield return null;
            time += Time.deltaTime * Speed;
        }
    }
}