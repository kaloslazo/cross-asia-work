using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Water_Settings : MonoBehaviour
{
    public AudioManager audioManager;
    Material waterVolume;
    Material waterMaterial;
    private bool isPlayingSound = false;
    void Awake()
    {
        // Debug.Log("Water_Settings Awake is called.");
        if (audioManager == null)
        {
            Debug.Log("AudioManager is not assigned, trying to find one...");
            audioManager = FindObjectOfType<AudioManager>();
        }
    }

    void Start()
    {
        if (audioManager == null)
        {
            audioManager = FindObjectOfType<AudioManager>();
            if (audioManager == null)
            {
                Debug.LogError("An instance of AudioManager was not found in the scene.");
            }
            else
            {
                Debug.Log("AudioManager was found and assigned.");
            }
        }
    }

    void Update()
    {
        if (audioManager == null)
        {
            return;
        }

        if (waterVolume == null)
        {
            waterVolume = (Material)Resources.Load("Water_Volume");
            if (waterVolume == null)
            {
                Debug.LogError("Failed to load Water_Volume material");
                return;
            }
        }

        if (waterMaterial == null)
        {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            if (meshRenderer == null)
            {
                Debug.LogError("No MeshRenderer component found on the object.");
                return; // Sale del Update si no hay MeshRenderer
            }
            waterMaterial = meshRenderer.sharedMaterial;
        }

        if (waterMaterial != null && waterVolume != null)
        {
            waterVolume.SetVector("pos", new Vector4(0, (waterVolume.GetVector("bounds").y / -2) + transform.position.y + (waterMaterial.GetFloat("_Displacement_Amount") / 3), 0, 0));

            // Reproducir el sonido de las olas si a�n no se ha iniciado
            if (!isPlayingSound)
            {
                // Debug.LogWarning("Audio ejecutando OceanWaves");
                audioManager.Play("ocean-wave");
                isPlayingSound = true;
            }
        }
    }

    private void OnDestroy()
    {
        if (audioManager != null && audioManager.gameObject != null)
        {
            // stop audio
            audioManager.Stop("ocean-wave");
            isPlayingSound = false;
        }
    }
}
