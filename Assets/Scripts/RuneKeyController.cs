using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneKeyController : MonoBehaviour
{
    private GameObject go;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        go = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        go.transform.Rotate(Vector3.up, speed * Time.deltaTime, Space.Self);
    }
}
