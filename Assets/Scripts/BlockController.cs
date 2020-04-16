using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockController : MonoBehaviour
{

    public string horizontalAxe = "";
    public string verticalAxe = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown(horizontalAxe))
            Debug.Log("rotation");
        if (Input.GetButtonDown(verticalAxe))
            Debug.Log("scale");
    }
}
