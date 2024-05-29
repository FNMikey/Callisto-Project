using UnityEngine;

public class MiningScript : MonoBehaviour
{

    public GameObject kamienPrefab;
    public GameObject srebroPrefab;
    public GameObject zlotoPrefab;
    public int health = 10;

    [Range(0, 100)] private int kamienChance = 50;
    [Range(0, 100)] private int srebroChance = 10;
    [Range(0, 100)] private int zlotoChance = 5;

    public float miningRange = 7.0f;
    private float spawnRadius;

    public GameObject miningParticlesPrefab;

    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Collider collider = GetComponent<Collider>();
        spawnRadius = collider.bounds.extents.magnitude;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryMine();
        }
    }

    void TryMine()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance <= miningRange)
        {
            int randomValue = Random.Range(0, 100);

            if (randomValue < kamienChance)
            {
                SpawnItem(kamienPrefab);
            }
            else if (randomValue < kamienChance + srebroChance)
            {
                SpawnItem(srebroPrefab);
            }
            else if (randomValue < kamienChance + srebroChance + zlotoChance)
            {
                SpawnItem(zlotoPrefab);
            }

            PlayMiningParticles();

            TakeDamage();

        }
    }

    void SpawnItem(GameObject itemPrefab)
    {
        Vector3 spawnPosition = transform.position + Random.insideUnitSphere * spawnRadius;
        spawnPosition.y = transform.position.y;
        Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
    }

    void TakeDamage()
    {

        health -= 2;

        if (health <= 0)
        {

            Destroy(gameObject);

        }

        Debug.Log(health.ToString());

    }

    void PlayMiningParticles()
    {
        if (miningParticlesPrefab != null)
        {
            Vector3 direction = (transform.position - player.transform.position).normalized;
            Vector3 spawnPosition = player.transform.position + direction * 1f;
            Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject particles = Instantiate(miningParticlesPrefab, spawnPosition, rotation);
            ParticleSystem particleSystem = particles.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            else
            {
                ParticleSystem[] childParticleSystems = particles.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in childParticleSystems)
                {
                    ps.Play();
                }
            }

            Destroy(particles, particleSystem.main.duration + particleSystem.main.startLifetime.constantMax);
        }
        else
        {
            Debug.LogWarning("Mining particles prefab is not assigned.");
        }
    }

}
