using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{
	// Start is called before the first frame update
	public GameObject[] ItemPrefab;
	void Start()
    {
        InvokeRepeating("SpawnObjectAtRandom", 5f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
		//if (Input.GetKeyDown(KeyCode.Space)) SpawnObjectAtRandom();
	}

	void SpawnObjectAtRandom()
	{
		Vector3 spawnPosition = new Vector3(
			Random.Range(-30 / 2f, 30 / 2f),
			Random.Range(-30 / 2f, 30 / 2f),
			transform.position.z
		);

		GameObject gameObject = Instantiate(ItemPrefab[Random.Range(0, ItemPrefab.Length)], spawnPosition, Quaternion.identity);
		Destroy(gameObject, 20f); 
	}
}
