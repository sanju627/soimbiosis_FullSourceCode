using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public OBJ tree;
    public bool isSpawning;
    public GameObject treePrefab;
    public float spawnDelay;

    [Header("VFX")]
    public ParticleSystem dustVFX;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn(0f));
        Debug.Log("Spawning");
    }

    // Update is called once per frame
    void Update()
    {
        tree = GetComponentInChildren<OBJ>();

        if(tree == null && !isSpawning)
        {
            StartCoroutine(Spawn(spawnDelay));
        }
    }

    IEnumerator Spawn(float delay)
    {
        isSpawning = true;

        yield return new WaitForSeconds(delay);

        dustVFX.Play();
        GameObject g = Instantiate(treePrefab, transform.position, transform.rotation);
        g.transform.SetParent(transform);

        isSpawning = false;
    }
}
