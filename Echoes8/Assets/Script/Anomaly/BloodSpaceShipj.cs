using UnityEngine;

public class BloodSpaceShipj : MonoBehaviour
{
    private bool isAnomalyActive = false;
    
    void Start()
    {
        // Initially deactivate the GameObject
        gameObject.SetActive(false);
    }

    void OnEnable()
    {
        isAnomalyActive = true;
    }

    void OnDisable()
    {
        isAnomalyActive = false;
    }

    // Call this method to trigger the anomaly
    public void TriggerAnomaly()
    {
        if (!isAnomalyActive)
        {
            gameObject.SetActive(true);
            isAnomalyActive = true;
        }
    }

    // Call this method to deactivate the anomaly
    public void DeactivateAnomaly()
    {
        if (isAnomalyActive)
        {
            gameObject.SetActive(false);
            isAnomalyActive = false;
        }
    }

    public bool IsActive()
    {
        return isAnomalyActive;
    }
}
