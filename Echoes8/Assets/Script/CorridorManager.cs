using UnityEngine;
using System.Collections.Generic;

public class CorridorManager : MonoBehaviour
{
    private int loopCount = 0;
    private bool hasAnomaly = false;
    
    [SerializeField]
    private bool persistAnomalies = true; // Anomalies persist between loops
    private HashSet<int> activeAnomalies = new HashSet<int>(); // Track which loops had anomalies

    public SimpleAIPath ai;
    public LoopEventManager eventManager;
    [Header("Player Spawn")]
    public Transform playerSpawnPoint; // Vị trí spawn của player

    public void OnLoopIncrease()
    {
        loopCount++;

        // Tắt tất cả anomaly đang hoạt động
        ResetAllAnomalies();

        if (ai != null) ai.ResetAI();

        AITriggerZone trigger = FindFirstObjectByType<AITriggerZone>();
        if (trigger != null) trigger.ResetTrigger();

        // Check if we should keep previous anomalies
        if (!persistAnomalies)
        {
            activeAnomalies.Clear();
        }

        // Get new anomaly state from event
        if (eventManager != null)
        {
            hasAnomaly = eventManager.ApplyLoopEvent(loopCount);
            if (hasAnomaly)
            {
                activeAnomalies.Add(loopCount);
            }
        }
        else
        {
            hasAnomaly = false;
        }

        Debug.Log($"Loop mới: {loopCount} | Anomaly hiện tại: {hasAnomaly} | Tổng anomaly: {activeAnomalies.Count}");
    }

    public void AddLoop()
    {
        OnLoopIncrease();
    }

    public void ResetLoop()
    {
        loopCount = 0;
        activeAnomalies.Clear(); // Clear all anomaly history

        // Tắt tất cả anomaly đang hoạt động
        ResetAllAnomalies();

        if (ai != null) ai.ResetAI();
        AITriggerZone trigger = FindFirstObjectByType<AITriggerZone>();
        if (trigger != null) trigger.ResetTrigger();
        // Reset anomaly state for loop 0
        if (eventManager != null)
        {
            hasAnomaly = eventManager.ApplyLoopEvent(loopCount);
            if (hasAnomaly)
            {
                activeAnomalies.Add(loopCount);
            }
        }
        else
        {
            hasAnomaly = false;
        }
        Debug.Log($"Loop reset → {loopCount} | Anomaly hiện tại: {hasAnomaly} | Tổng anomaly: {activeAnomalies.Count}");
    }

    private void ResetAllAnomalies()
    {
        var allAnomalyObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var obj in allAnomalyObjects)
        {
            if (obj is IAnomaly anomaly)
            {
                anomaly.ApplyNormal();
            }
        }
    }

    public void TeleportPlayerToSpawn()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && playerSpawnPoint != null)
        {
            var cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;
            player.transform.position = playerSpawnPoint.position;
            player.transform.rotation = playerSpawnPoint.rotation;
            if (cc != null) cc.enabled = true;
        }
    }

    public int GetLoopCount() => loopCount;
    public bool HasAnomaly() => hasAnomaly;
    
    // New methods for enhanced anomaly control
    public bool HasAnomalyInLoop(int loop) => activeAnomalies.Contains(loop);
    public int GetTotalAnomalies() => activeAnomalies.Count;
    public void ClearAnomalies() => activeAnomalies.Clear();
    public void SetPersistAnomalies(bool persist) => persistAnomalies = persist;

    // Gọi hàm này để đổi tất cả poster anomaly (ví dụ khi có sự kiện anomaly)
    public void TriggerAnomalyPosterChange()
    {
        PosterAnomaly[] anomalies = FindObjectsByType<PosterAnomaly>(FindObjectsSortMode.None);
        foreach (var anomaly in anomalies)
        {
            anomaly.ApplyAnomaly();
        }
        Debug.Log("Tất cả poster anomaly đã được đổi!");
    }
}
