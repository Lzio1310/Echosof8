using UnityEngine;

public class PosterAnomaly : MonoBehaviour, IAnomaly
{
    public SpriteRenderer posterRenderer;    // Chỗ hiển thị poster
    public Sprite normalSprite;              // Ảnh bình thường
    public Sprite anomalySprite;             // Ảnh anomaly

    // Đặt poster về ảnh bình thường
    public void ApplyNormal()
    {
        if (normalSprite != null)
        {
            posterRenderer.sprite = normalSprite;
        }
    }

    // Đổi sang ảnh anomaly
    public void ApplyAnomaly()
    {
        if (anomalySprite != null)
        {
            posterRenderer.sprite = anomalySprite;
        }
    }
}
