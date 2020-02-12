using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Com.Addy.Network;

public class Distance : MonoBehaviour
{
    private float dist;
    public float threshold;
    public Transform hand;
    public Transform button;

    public Launcher launch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(hand.position, button.position);
        
        if (dist < threshold)
        {
            launch.Connect();
        }
    }
}
