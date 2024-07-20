using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destroy the casing object")][SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")][SerializeField] private float shotPower = 500f;
    [Tooltip("Casing Ejection Speed")][SerializeField] private float ejectPower = 150f;
    public AudioSource source;
    public AudioClip fireSound;

    void Start()
    {
        // Ensure all references are set up correctly
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();
    }

    public void PullTheTrigger()
    {
        // Trigger the shooting animation
        gunAnimator.SetTrigger("Fire");

        // Call Shoot function
        Shoot();

        // Optionally call CasingRelease if it needs to be synchronized with shooting
        CasingRelease();
    }

    // This function handles the creation and behavior of the bullet
    void Shoot()
    {
        source.PlayOneShot(fireSound);

        if (muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
            Destroy(tempFlash, destroyTimer);
        }

        if (!bulletPrefab) return;

        GameObject bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        Destroy(bullet, 2f); // Destruye la bala después de 2 segundos para limpiar

        PerformRaycast();
    }

    void PerformRaycast()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Bullet");
        layerMask = ~layerMask; // Asegúrate de que esto esté configurado correctamente para ignorar la capa de balas.

        RaycastHit hit;
        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, 100, layerMask))
        {
            Debug.Log("Raycast hit: " + hit.transform.name);
            if (hit.transform.CompareTag("Zombie"))
            {
                ZombieBehavior zombie = hit.transform.GetComponent<ZombieBehavior>();
                if (zombie != null)
                {
                    zombie.TakeDamage(25);
                }
            }
        }
    }

    // This function handles the casing ejection
    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab) return;
        GameObject tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation);
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(ejectPower, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);
        Destroy(tempCasing, destroyTimer);
    }
}
