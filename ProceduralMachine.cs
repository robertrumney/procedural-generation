using UnityEngine;

using UnityEngine.UI;
using UnityEngine.AI;

using System.Collections;
using System.Collections.Generic;

public class ProceduralMachine : MonoBehaviour
{
    #region Configuration
    public static ProceduralMachine instance;

    public bool disabled=false;
    public int range=20;
    public int amount=10;
    public int minThreshold=40;
    public int minNavmeshSafeDistance=100;

    public Slider qualitySlider;
    public List<ItemDefinition> spawn;
    public List<OptimizationVolume> volumes = new List<OptimizationVolume>(); 

    private RaycastHit hit;
    private Vector3 prevPoint;
    private OptimizationVolume prevOv;
    
    private float lastValue;
    private bool listeningToValueChange=false;

    [System.Serializable]
	  public class ItemDefinition
	  {
        public string name;
        public GameObject prefab;
        public Vector3 positionOffset;
        public Vector3 rotationOffset;
        public float odds;
    }

    private void Awake()
    {
        instance=this;
    }

    private void BeginValueChangeListen()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(ListenToValueChange());
    }

    private IEnumerator ListenToValueChange()
    {
        float time=0;

        while(true)
        {
            time+=Time.deltaTime;

            if(time > 2)
            {
                if(qualitySlider.value!=lastValue)
                {   
                    RepeatSpawn(qualitySlider.value);
                    listeningToValueChange=false;
                    yield break;
                }
            }

            yield return null;
        }
    }

    public void SetActiveAllObjects(bool x)
    {
        foreach(OptimizationVolume volume in volumes)
        {
            foreach(GameObject go in volume.spawnedItems)
            {
                if(go!=null)
                    go.SetActive(x);
            }
        }
    }

    
    public void Do(OptimizationVolume ov)
    {
        if(disabled) return;

        if(ov.CannotSpawnMoreItems(this)) return;
            
        Vector3 originPoint = this.transform.position + new Vector3(0,100,0);

        for(int i=0;i<amount;i++)
        {
            int rX = Random.Range(-range,range);
            int rY = Random.Range(-range,range);

            Vector3 _originPoint = originPoint + new Vector3(rX,0,rY);

            if (Physics.Raycast(_originPoint, -transform.up, out hit, 500)) 
            {
                if (hit.collider.gameObject.CompareTag("Ground") && hit.collider.name !="Terrain START")
                {
                    Vector3 targetPos = hit.point;

                    NavMeshHit _hit;

                    if(!NavMesh.SamplePosition(targetPos, out _hit, 1f, NavMesh.AllAreas))
                    {
                        float distance=Vector3.Distance(targetPos, this.transform.position);
                        
                        if(distance > minThreshold)
                        {
                            int x = Random.Range(0,spawn.Count);   
                            float r = Random.Range(0.0f,1.0f);

                            if(spawn[x].prefab==null) continue;

                            if(r<=spawn[x].odds)
                            {
                                float rot = Random.Range(0,360);

                                GameObject go = Instantiate(spawn[x].prefab,targetPos,Quaternion.identity) as GameObject;
                                Quaternion targetRotation = Quaternion.FromToRotation(go.transform.up, hit.normal) * go.transform.rotation;
                                go.transform.rotation = targetRotation;
                                go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x,rot,go.transform.localEulerAngles.z);

                                ov.SpawnItem(go);             
                            }
                        }
                    }
                }
            }
        }
    }

    public void ValueChanged(float x)
    {
        if(!listeningToValueChange)
        {
            lastValue=x;
            BeginValueChangeListen();
            listeningToValueChange=true;
        }
    }

    public void RepeatSpawn(float x)
    {
        if(disabled)
            return;

        prevOv.ForceClear();
        amount=Mathf.RoundToInt(x);
        SpawnAtPoint(prevOv,prevPoint);
    }

    public void SpawnAtPoint(OptimizationVolume ov,Vector3 originPoint)
    {
        prevPoint=originPoint;
        prevOv=ov;

        for(int i=0;i<amount;i++)
        {
            int rX = Random.Range(-range,range);
            int rY = Random.Range(-range,range);
            Vector3 _originPoint = originPoint + new Vector3(rX,0,rY);

            if (Physics.Raycast(_originPoint, -transform.up, out hit, 500)) 
            {
                if (hit.collider.gameObject.layer==10)
                {
                    Vector3 targetPos = hit.point;
                
                    int x = Random.Range(0,spawn.Count);   
                    float r = Random.Range(0.0f,1.0f);

                    float distance=Vector3.Distance(targetPos,Camera.main.transform.position);

                    if(distance > minThreshold)
                    {
                        if(r<=spawn[x].odds)
                        {
                            float rot = Random.Range(0,360);

                            GameObject go = Instantiate(spawn[x].prefab,targetPos,Quaternion.identity) as GameObject;
                            Quaternion targetRotation = Quaternion.FromToRotation(go.transform.up, hit.normal) * go.transform.rotation;
                            go.transform.rotation = targetRotation;
                            go.transform.localEulerAngles = new Vector3(go.transform.localEulerAngles.x,rot,go.transform.localEulerAngles.z);
                            ov.SpawnItem(go);             
                        }
                    }
                }
            }
        }
    }
}
