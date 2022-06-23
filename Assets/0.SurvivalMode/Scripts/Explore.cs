using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explore : MonoBehaviour
{
    //void OnTriggerEnter(Collider col)
    //{
    //    if (col.transform.tag == "Player")
    //    {
    //        print("col.tranform" + col.transform.tag);

    //        col.transform.GetComponent<WeaponManger>().OpenMapScreen();
    //    }
    //}

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            print("col.tranform" + collision.transform.tag);

            //collision.transform.GetComponent<WeaponManger>().OpenMapScreen();
        }
    }
}
