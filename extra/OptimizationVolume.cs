using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class OptimizationVolume : MonoBehaviour 
{
	public bool autoSpawnOnStart=false;
	public bool useSpawning=false;
	
	public GameObject[] items;
	public List<GameObject> spawnedItems = new List<GameObject>();
	
	private bool activated=false;

	private void Awake () 
	{
		GetComponent<MeshRenderer> ().enabled = false;
		SetItems (false);
	}

	private IEnumerator Start()
	{
		ProceduralMachine.instance.volumes.Add(this);

		if(autoSpawnOnStart) AutoSpawn();

		yield return new WaitForSeconds(2);

		activated=true;
		this.GetComponent<Collider>().enabled=false;

		yield return null;

		this.GetComponent<Collider>().enabled=true;
	}

	private void AutoSpawn()
	{
		ProceduralMachine.instance.SpawnAtPoint(this,this.transform.position);
	}

	private void OnTriggerEnter (Collider x) 
	{
		if (x.CompareTag ("Player"))
		{
			if (activated)
			{
				if(useSpawning)
				{
					ProceduralMachine.instance.Do(this);
				}

				SetItems (true);
			}
		}
	}

	private void OnTriggerExit (Collider x) 
	{
		if (x.CompareTag ("Player"))
		{
			SetItems (false);
		}
	}

	public void ForceClear()
	{
		if(spawnedItems.Count>0)
		{
			foreach(GameObject go in spawnedItems)
			{
				Destroy(go);
			}

			spawnedItems.Clear();
		}
	}

	private void SetItems(bool x)
	{
		foreach (GameObject item in items)
		{
			if(item)item.SetActive (x);
		}
	}

	public void SpawnItem(GameObject go)
	{
		spawnedItems.Add(go);
	}

	public bool CannotSpawnMoreItems(ProceduralMachine pm)
	{
		if(spawnedItems.Count > pm.amount) return true; else return false;
	}
}
