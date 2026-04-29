using UnityEngine;

public class TentacleTrigger : MonoBehaviour
{
    public Tentacle tentacle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            tentacle.StartAttack(other.transform.position);
        }
    }
}
