using UnityEngine;

public class AITriggerZone : MonoBehaviour
{
    private bool triggeredThisLoop = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggeredThisLoop) return; // chỉ cho gọi 1 lần mỗi loop

        if (other.CompareTag("Player"))
        {
            SimpleAIPath ai = FindFirstObjectByType<SimpleAIPath>();
            if (ai != null)
            {
                ai.StartMove();
                triggeredThisLoop = true;
            }
        }
    }

    public void ResetTrigger()
    {
        triggeredThisLoop = false; // reset khi loop mới
    }
}
