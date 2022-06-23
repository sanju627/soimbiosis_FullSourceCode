using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    public float damage;
    public float atkRange;
    public float useEnergy;
    public LayerMask enemyLayer;
    public Transform atkPoint;

    [Header("SFX")]
    public AudioClip[] SwingSFX;
    public AudioClip[] hitSFX;

    AudioSource audio;
    Player player;
    EnergySystem energySystem;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        energySystem = player.GetComponent<EnergySystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack()
    {
        Collider[] col = Physics.OverlapSphere(atkPoint.position, atkRange, enemyLayer);

        audio.PlayOneShot(SwingSFX[Random.Range(0, SwingSFX.Length)]);

        foreach(Collider c in col)
        {
            energySystem.UseEnergy(3f);

            Zombie_BP z = c.GetComponent<Zombie_BP>();
            if(z != null)
            {
                z.TakeDamage(damage);
            }

            OBJ o = c.GetComponent<OBJ>();
            if (o != null)
            {
                o.TakeDamage(damage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(atkPoint.position, atkRange);
    }
}
