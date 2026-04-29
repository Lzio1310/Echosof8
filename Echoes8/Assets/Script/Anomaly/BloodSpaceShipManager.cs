using UnityEngine;

// NOTE: GameObject ch?a BloodSpaceShipManager nên luôn active trong scene!
public class BloodSpaceShipManager : MonoBehaviour, IAnomaly
{
    public BloodSpaceShipj[] spaceships;
    private CorridorManager corridorManager;
    private bool isAnomalyActive = false;

    void Start()
    {
        corridorManager = FindFirstObjectByType<CorridorManager>();
        // KHÔNG t?t spaceship ? ?ây n?a, ch? qu?n lý qua ApplyAnomaly/ApplyNormal
    }

    public void ApplyAnomaly()
    {
        isAnomalyActive = true;
        foreach (var spaceship in spaceships)
        {
            if (spaceship != null)
            {
                spaceship.TriggerAnomaly();
            }
        }
    }

    public void ApplyNormal()
    {
        isAnomalyActive = false;
        foreach (var spaceship in spaceships)
        {
            if (spaceship != null)
            {
                spaceship.DeactivateAnomaly();
            }
        }
    }

    void Update()
    {
        // If CorridorManager exists and there's no anomaly in current loop,
        // ensure this anomaly is deactivated
        if (corridorManager != null && !corridorManager.HasAnomaly() && isAnomalyActive)
        {
            ApplyNormal();
        }
    }
}