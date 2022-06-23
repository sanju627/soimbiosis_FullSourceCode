using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Melee : MonoBehaviour
{
	public NPC npc;
	public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	npc.melee = true;
        npc.gun = false;

        anim.SetLayerWeight(anim.GetLayerIndex("Rifle"), 0);
        anim.SetLayerWeight(anim.GetLayerIndex("Melee"), 1);
        anim.SetLayerWeight(anim.GetLayerIndex("Pistol"), 0);
    }
}
