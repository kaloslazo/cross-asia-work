using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class PlayerDamageEffect : MonoBehaviour
{
    public Volume globalVolume;
    private Vignette vignetteEffect;

    void Start()
    {
        if (globalVolume.profile.TryGet(out Vignette vignette))
        {
            vignetteEffect = vignette;
        }
    }

    public void TriggerDamageEffect()
    {
        Debug.Log("Triggering damage effect");
        if (vignetteEffect != null)
        {
            vignetteEffect.color.value = Color.red;
            Invoke("ResetVignetteEffect", 0.5f);  // Reemplaza la coroutine por un Invoke para probar
        }
    }

    private void ResetVignetteEffect()
    {
        if (vignetteEffect != null)
            vignetteEffect.color.value = Color.black;
    }
}
