using UnityEngine;

[System.Serializable]
public class LoopEvent
{
    public int loopNumber;
    public string customMessage;

    [Header("Audio")]
    public AudioClip ambientSound;

    [Header("Lighting")]
    public Color lightColor = Color.white;

    [Header("Anomaly Control")]
    public bool allowAnomaly = false;     // loop này có anomaly không
    [Range(0f, 1f)]
    public float anomalyChance = 1f;      // xác suất anomaly xuất hiện
    public GameObject[] possibleAnomalyObjects; // danh sách các object có thể thay đổi ở loop này
}
