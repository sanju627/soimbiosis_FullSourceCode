using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class popTXT : MonoBehaviour
{
	public TextMeshProUGUI msgTXT;
	public Animator textAnim;
	public float showRate;

	float nextShowTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopText(string msg)
    {
    	if(Time.time >= nextShowTime)
    	{
	    	msgTXT.text = msg;
	    	textAnim.SetTrigger("pop");	

	    	nextShowTime = Time.time + 1f/ showRate;
	    }
    }
}
