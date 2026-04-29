using UnityEngine;
using TMPro;

public class LoopCount : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI loopText;
    private CorridorManager corridorManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        corridorManager = FindFirstObjectByType<CorridorManager>();
        if (corridorManager == null)
        {
            Debug.LogWarning("CorridorManager not found in the scene!");
        }
        UpdateLoopDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLoopDisplay();
    }

    private void UpdateLoopDisplay()
    {
        if (corridorManager != null && loopText != null)
        {
            int currentLoop = corridorManager.GetLoopCount();
            loopText.text = $"Loop: {currentLoop}";
        }
    }
}
