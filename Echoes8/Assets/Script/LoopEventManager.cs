using UnityEngine;

public class LoopEventManager : MonoBehaviour
{
    public LoopEvent[] loopEvents;
    public AudioSource ambientSource;
    public Light corridorLight;

    // Trả về true nếu anomaly xuất hiện
    public bool ApplyLoopEvent(int loop)
    {
        // Reset tất cả anomaly objects về trạng thái bình thường
        var allAnomalyObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
        foreach (var obj in allAnomalyObjects)
        {
            if (obj is IAnomaly anomaly)
            {
                anomaly.ApplyNormal();
            }
        }

        foreach (var evt in loopEvents)
        {
            if (evt.loopNumber == loop)
            {
                Debug.Log("Loop Event: " + evt.customMessage);

                // âm thanh
                if (evt.ambientSound != null && ambientSource != null)
                {
                    ambientSource.clip = evt.ambientSound;
                    ambientSource.Play();
                }

                // ánh sáng
                if (corridorLight != null)
                    corridorLight.color = evt.lightColor;

                // anomaly objects
                if (evt.allowAnomaly && evt.possibleAnomalyObjects.Length > 0)
                {
                    if (Random.value <= evt.anomalyChance)
                    {
                        // chỉ chọn 1 object duy nhất để anomaly
                        int index = Random.Range(0, evt.possibleAnomalyObjects.Length);
                        var selectedObject = evt.possibleAnomalyObjects[index];
                        
                        if (selectedObject != null)
                        {
                            var anomaly = selectedObject.GetComponent<IAnomaly>();
                            if (anomaly != null)
                            {
                                anomaly.ApplyAnomaly();
                                return true; // anomaly xảy ra
                            }
                        }
                    }
                }

                return false; // không có anomaly
            }
        }

        return false;
    }
}
