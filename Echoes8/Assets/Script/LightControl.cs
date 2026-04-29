using UnityEngine;
using System.Collections;

public class LightControl : MonoBehaviour, IAnomaly
{
    public Light[] pointLights;
    private float[] originalIntensities;
    private float transitionDuration = 2; // Duration in seconds for the fade effect
    private Coroutine[] fadeCoroutines;
    
    void Start()
    {
        // Không tự động lấy pointLights nữa, chỉ dùng mảng đã gán trong Inspector
        if (pointLights == null)
            pointLights = new Light[0];
        
        // Store original intensities
        originalIntensities = new float[pointLights.Length];
        fadeCoroutines = new Coroutine[pointLights.Length];
        
        for (int i = 0; i < pointLights.Length; i++)
        {
            if (pointLights[i] != null && pointLights[i].type == LightType.Point)
            {
                originalIntensities[i] = pointLights[i].intensity;
            }
        }
    }

    public void ApplyAnomaly()
    {
        // Start fading out all point lights
        for (int i = 0; i < pointLights.Length; i++)
        {
            if (pointLights[i] != null && pointLights[i].type == LightType.Point)
            {
                // Stop any existing fade coroutine for this light
                if (fadeCoroutines[i] != null)
                    StopCoroutine(fadeCoroutines[i]);
                    
                // Start new fade out coroutine
                fadeCoroutines[i] = StartCoroutine(FadeLightIntensity(pointLights[i], pointLights[i].intensity, 0.5f));
            }
        }
    }

    public void ApplyNormal()
    {
        // Restore original intensities with fade effect
        for (int i = 0; i < pointLights.Length; i++)
        {
            if (pointLights[i] != null && pointLights[i].type == LightType.Point)
            {
                // Stop any existing fade coroutine for this light
                if (fadeCoroutines[i] != null)
                    StopCoroutine(fadeCoroutines[i]);
                    
                // Start new fade in coroutine
                fadeCoroutines[i] = StartCoroutine(FadeLightIntensity(pointLights[i], pointLights[i].intensity, originalIntensities[i]));
            }
        }
    }

    private IEnumerator FadeLightIntensity(Light light, float startIntensity, float targetIntensity)
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / transitionDuration;
            light.intensity = Mathf.Lerp(startIntensity, targetIntensity, normalizedTime);
            yield return null;
        }
        
        light.intensity = targetIntensity; // Ensure we reach exactly the target value
    }
}
