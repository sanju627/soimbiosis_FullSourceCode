using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bandit_BP : MonoBehaviour
{
    public Bandit bandit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float amount)
    {
        bandit.TakeDamage(amount);
    }
}
