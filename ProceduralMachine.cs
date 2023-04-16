using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class ProceduralMachine : MonoBehaviour
{
    // Singleton instance
    public static ProceduralMachine instance;

    // Configurable parameters
    public bool disabled = false;
    public int range = 20;
    public int amount = 10;
    public int minThreshold = 40;
    public int minNavmeshSafeDistance = 100;
    public Slider qualitySlider;
    public List<ItemDefinition> spawn;
    public List<OptimizationVolume> volumes = new List<OptimizationVolume>();

    // Private variables
    private RaycastHit hit;
    private Vector3 prevPoint;
    private OptimizationVolume prevOv;
    private float lastValue;
    private bool listeningToValueChange = false;

    // Item definition class
    [System.Serializable]
    public class ItemDefinition
    {
        public string name;
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public float odds;
    }

    // Called when the object is initialized
    private void Awake()
    {
        instance = this;
    }

    // Set the active state of all spawned objects
    public void SetActiveAllObjects(bool x)
    {
        foreach (OptimizationVolume volume in volumes)
        {
            foreach (GameObject go in volume.spawnedItems)
            {
                if (go != null)
                    go.SetActive(x);
            }
        }
    }

    // Spawn objects in the specified optimization volume
    public void Do(OptimizationVolume ov)
    {
        // If the machine is disabled or the volume cannot spawn more objects, return
        if (disabled) return;
        if (ov.CannotSpawnMoreItems(this)) return;

        // Calculate the origin point for spawning objects
        Vector3 originPoint = this.transform.position + new Vector3(0, 100, 0);

        // Spawn the specified amount of objects
        for (int i = 0; i < amount; i++)
        {
            // Randomize the spawn position within the specified range
            int rX = Random.Range(-range, range);
            int rY = Random.Range(-range, range);
            Vector3 _originPoint = originPoint + new Vector3(rX, 0, rY);

            // Check if the spawn position is valid
            if (Physics.Raycast(_originPoint, -transform.up, out hit, 500))
            {
                // Check if the hit object is the ground and not the starting terrain
                if (hit.collider.gameObject.CompareTag("Ground") && hit.collider.name != "Terrain START")
                {
                    Vector3 targetPos = hit.point;

                    // Check if the spawn point is on the NavMesh and within the minimum safe distance
                    NavMeshHit _hit;

                    if (!NavMesh.SamplePosition(targetPos, out _hit, 1f, NavMesh.AllAreas))
                    {
                        float distance = Vector3.Distance(targetPos, this.transform.position);

                        if (distance > minThreshold)
                        {
                            int x = Random.Range(0, spawn.Count);
                            float r = Random.Range(0.0f, 1.0f);

                            // Check if the selected object is valid and the spawn chance is met
                            if (spawn[x].prefab == null) continue;

                            if (r <= spawn[x].odds)
                            {
                                float rot = Random.Range(0, 360);

                                // Instantiate the object and apply rotation and position offsets
                                GameObject go = Instantiate(spawn[x].prefab, targetPos, Quaternion.identity) as GameObject;
                                Quaternion targetRotation = Quaternion.FromToRotation(go.transform.up, hit.normal) * go.transform.rotation;
                                go.transform.rotation = targetRotation;
                                go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, rot, go.transform.localEulerAngles.z);

                                // Add the object to the optimization volume
                                ov.SpawnItem(go);
                            }
                        }
                    }
                }
            }
        }
    }

    // Spawn objects at a specified point within an optimization volume
    public void SpawnAtPoint(OptimizationVolume ov, Vector3 originPoint)
    {
        // Store the point and volume for repeat spawning
        prevPoint = originPoint;
        prevOv = ov;

        // Spawn the specified amount of objects
        for (int i = 0; i < amount; i++)
        {
            // Randomize the spawn position within the specified range
            int rX = Random.Range(-range, range);
            int rY = Random.Range(-range, range);
            Vector3 _originPoint = originPoint + new Vector3(rX, 0, rY);

            // Check if the spawn position is valid
            if (Physics.Raycast(_originPoint, -transform.up, out hit, 500))
            {
                // Check if the hit object is on a specific layer
                if (hit.collider.gameObject.layer == 10)
                {
                    Vector3 targetPos = hit.point;

                    int x = Random.Range(0, spawn.Count);
                    float r = Random.Range(0.0f, 1.0f);

                    float distance = Vector3.Distance(targetPos, Camera.main.transform.position);

                    // Check if the selected object is valid and the spawn chance is met
                    if (distance > minThreshold)
                    {
                        if (r <= spawn[x].odds)
                        {
                            float rot = Random.Range(0, 360);

                            // Instantiate the object and apply rotation and position offsets
                            GameObject go = Instantiate(spawn[x].prefab, targetPos, Quaternion.identity) as GameObject;
                            Quaternion targetRotation = Quaternion.FromToRotation(go.transform.up, hit.normal) * go.transform.rotation;
                            go.transform.rotation = targetRotation;
                            go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x, rot, go.transform.localEulerAngles.z);
                            ov.SpawnItem(go);
                        }
                    }
                }
            }
        }
    }
}
