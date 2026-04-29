using UnityEngine;

public class GameSceneManager : MonoBehaviour
{
    void Start()
    {
        // Khi vừa vào scene game → ẩn và khóa chuột
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
