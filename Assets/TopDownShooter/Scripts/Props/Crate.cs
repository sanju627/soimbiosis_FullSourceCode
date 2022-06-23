using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public GameObject[] commonItems;
    public Transform[] spawnPos;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Open()
    {
        anim.SetTrigger("open");

        for (int i = 0; i < spawnPos.Length; i++)
        {
            GameObject obj = commonItems[Random.Range(0, commonItems.Length)];
            Instantiate(obj, spawnPos[i].position, spawnPos[i].rotation);
        }
    }
}
