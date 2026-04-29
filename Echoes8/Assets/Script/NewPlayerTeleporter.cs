using UnityEngine;
using System.Collections;

public class NewPlayerTeleporter : MonoBehaviour
{
    public Transform TeleportZone;
    public bool isResetZone = false;   // Đánh dấu ResetZone hay EndZone
    private bool canTeleport = true;

    private void OnTriggerEnter(Collider other)
    {
        if (canTeleport && other.CompareTag("Player"))
        {
            StartCoroutine(TeleportPlayer(other));
        }
    }

    private IEnumerator TeleportPlayer(Collider other)
    {
        canTeleport = false;

        CharacterController cc = other.GetComponent<CharacterController>();
        if (cc != null)
        {
            // Offset + rotation để dịch chuyển mượt
            Vector3 localOffset = transform.InverseTransformPoint(other.transform.position);
            Quaternion relativeRotation = TeleportZone.rotation * Quaternion.Inverse(transform.rotation);

            cc.enabled = false;

            CorridorManager cm = FindFirstObjectByType<CorridorManager>();
            if (cm != null)
            {
                bool hasAnomaly = cm.HasAnomaly();

                if (isResetZone)
                {
                    if (hasAnomaly)
                    {
                        cm.AddLoop();
                        Debug.Log("✅ ResetZone + Anomaly → Loop +1");
                    }
                    else
                    {
                        cm.ResetLoop();
                        Debug.Log("❌ ResetZone + No Anomaly → Loop = 0");
                    }
                }
                else // EndZone
                {
                    if (!hasAnomaly)
                    {
                        cm.AddLoop();
                        Debug.Log("✅ EndZone + No Anomaly → Loop +1");
                    }
                    else
                    {
                        cm.ResetLoop();
                        Debug.Log("❌ EndZone + Anomaly → Loop = 0");
                    }
                }
            }

            // Teleport giữ nguyên offset & xoay tương đối
            other.transform.position = TeleportZone.TransformPoint(localOffset);
            other.transform.rotation = relativeRotation * other.transform.rotation;

            // Nếu ResetZone → xoay 180°
            if (isResetZone)
            {
                Vector3 dir = other.transform.position - TeleportZone.position;
                dir = Quaternion.Euler(0, 180f, 0) * dir;
                other.transform.position = TeleportZone.position + dir;

                other.transform.Rotate(Vector3.up, 180f);
            }

            cc.enabled = true;
        }

        yield return new WaitForSeconds(0.5f);
        canTeleport = true;
    }
}
