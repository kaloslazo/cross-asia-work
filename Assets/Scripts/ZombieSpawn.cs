using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieSpawn : MonoBehaviour
{
    public GameObject zombiePrefab;
    public int maxZombies = 10;
    public float spawnRadius = 50f;
    public float spawnInterval = 5f;
    public float heightOffset = 0.5f;

    private List<GameObject> zombies;

    void Start()
    {
        zombies = new List<GameObject>();
        StartCoroutine(SpawnZombies());
    }

    IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            if (zombies.Count < maxZombies)
            {
                Vector3 spawnPosition = RandomNavSphere(transform.position, spawnRadius, -1);
                if (spawnPosition != Vector3.zero)
                {
                    spawnPosition += Vector3.up * heightOffset;
                    Quaternion spawnRotation = Quaternion.identity; // Utilizamos una rotaci�n neutra
                    GameObject zombie = Instantiate(zombiePrefab, spawnPosition, spawnRotation, transform); // Establecer este objeto como padre
                    zombies.Add(zombie);

                    // Aseg�rate de que la barra de salud se inicializa correctamente
                    ZombieBehavior zombieBehavior = zombie.GetComponent<ZombieBehavior>();
                    if (zombieBehavior != null)
                    {
                        zombieBehavior.InitializeHealthBar();
                    }
                    else
                    {
                        Debug.LogError("ZombieBehavior component not found on the zombie prefab");
                    }
                }
            }
            // Eliminamos de la lista los zombies que han sido destruidos
            zombies.RemoveAll(zombie => zombie == null);
        }
    }


    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        for (int i = 0; i < 30; i++)  // Corregido: se a�ade '<' entre 'i' y '30'
        {
            Vector3 randDirection = Random.insideUnitSphere * dist + origin;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randDirection, out navHit, dist, layermask))
            {
                return navHit.position;
            }
        }
        return Vector3.zero;
    }
}
