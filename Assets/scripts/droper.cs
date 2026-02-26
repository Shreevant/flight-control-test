using UnityEngine;
using TMPro;
using System.Numerics;

public class SpawnObject : MonoBehaviour
{
    public GameObject objectPrefab;
    public Transform spawnPoint;
    public float destroyTime = 10f;
    public float spawnForce = 5f;

    public int maxDrops = 2;
    public float cooldownTime = 5f;

    public TextMeshProUGUI dropText; // Assign in Inspector

    private int currentDrops = 0;
    private bool isCoolingDown = false;

    public movement planeMovement; // drag plane in inspector

    void Start()
    {
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) &&
            !isCoolingDown &&
            !planeMovement.isGrounded)
        {
            Spawn();
            currentDrops++;

            UpdateUI();

            if (currentDrops >= maxDrops)
            {
                StartCoroutine(Cooldown());
            }
        }
    }

    void Spawn()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(spawnPoint.forward * spawnForce, ForceMode.Impulse);
        }

        Destroy(spawnedObject, destroyTime);
    }

    System.Collections.IEnumerator Cooldown()
    {
        isCoolingDown = true;
        dropText.text = "Cooling...";

        yield return new WaitForSeconds(cooldownTime);

        currentDrops = 0;
        isCoolingDown = false;

        UpdateUI();
    }

    void UpdateUI()
    {
        int dropsLeft = maxDrops - currentDrops;
        dropText.text = "BALOONS: " + dropsLeft;
    }
}