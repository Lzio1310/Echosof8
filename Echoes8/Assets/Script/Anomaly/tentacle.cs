using UnityEngine;

public class Tentacle : MonoBehaviour, IAnomaly
{
    [Header("References")]
    public Animator animator;
    public Collider resetTriggerCollider; // Collider that will trigger the reset when hit

    private bool isAnomalyActive = false;
    private bool hasAttackedThisLoop = false;

    void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public void StartAttack(Vector3 playerPosition)
    {
        if (!isAnomalyActive || hasAttackedThisLoop) return;

        if (animator != null)
        {
            animator.SetTrigger("Attack");
            hasAttackedThisLoop = true;
        }
    }

    public void ApplyAnomaly()
    {
        isAnomalyActive = true;
        hasAttackedThisLoop = false; // Reset the attack flag when anomaly is applied
        if (animator != null)
            animator.SetBool("IsActive", true);
    }

    public void ApplyNormal()
    {
        isAnomalyActive = false;
        hasAttackedThisLoop = false;
        if (animator != null)
            animator.SetBool("IsActive", false);
    }

    // Gọi từ Animation Event ở cuối Retract
    public void ResetToIdle()
    {
        if (animator != null)
            animator.Play("Idle");
    }

    // Gọi khi xúc tu thực sự đánh trúng player (ví dụ: từ Animation Event hoặc collider va chạm)
    public void OnTentacleHitPlayer()
    {
        ResetPlayer();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPlayer();
        }
    }

    private void ResetPlayer()
    {
        CorridorManager corridorManager = FindFirstObjectByType<CorridorManager>();
        if (corridorManager != null)
        {
            Debug.Log("[Tentacle] Teleporting player to spawn!");
            corridorManager.TeleportPlayerToSpawn();
            corridorManager.ResetLoop();
        }
    }
}
