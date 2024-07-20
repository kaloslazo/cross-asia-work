using UnityEngine;

public class FootstepSound : MonoBehaviour
{
    private Rigidbody rb;
    public float footstepRate = 0.5f; // Tiempo en segundos entre cada sonido de paso
    private float nextFootstepTime = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (rb.velocity.magnitude > 10f && Time.time >= nextFootstepTime)
        {
            PlayFootstepSound();
            nextFootstepTime = Time.time + footstepRate; // Actualiza el próximo tiempo permitido para el sonido de paso
        }
    }

    void PlayFootstepSound()
    {
        AudioManager.instance.Play("footsteps");
    }
}
