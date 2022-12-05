using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //move camera to player
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y+20, Camera.main.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
