using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutsideSceneManager : MonoBehaviour
{
    public GameObject[] tree;
    public GameObject Charon;
    private float xCharon, yCharon, zCharon, bobCharon = 0.0005f;
    public GameObject Ferry;
    private float xFerry, yFerry, zFerry, bobFerry = 0.001f;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("INITILIZING OUTSIDESCENEMANAGER (should only be once");
        //Spawn trees
        for (int i = 0; i < 5; i++)
        {
            Instantiate(tree[Random.Range(0,3)], new Vector3(Random.Range(9.5f, 11f),-0.8f, Random.Range(-12.92f, 3.24f)), Quaternion.identity);
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(-11f, -10.56f), -0.8f, Random.Range(-12.92f, 3.24f)), Quaternion.identity);
        }
        for (int i = 0; i < 15; i++)
        {
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(8.5f, 49.13f), -0.8f, Random.Range(-10.99f, 3.24f)), Quaternion.identity);
            Instantiate(tree[Random.Range(0, 3)], new Vector3(Random.Range(-48.7f, -9.56f), -0.8f, Random.Range(-10.99f, 3.24f)), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        xCharon = Charon.transform.position.x; yCharon = Charon.transform.position.y; zCharon = Charon.transform.position.z;
        yCharon += bobCharon;
        if (yCharon + bobCharon > 2) bobCharon = -0.0005f;
        if (yCharon + bobCharon < 1.3) bobCharon = 0.0005f;
        Charon.transform.position = new Vector3(xCharon, yCharon + bobCharon, zCharon);

        xFerry = Ferry.transform.position.x; yFerry = Ferry.transform.position.y; zFerry = Ferry.transform.position.z;
        yFerry += bobFerry;
        if (yFerry + bobFerry > 0) bobFerry = -0.001f;
        if (yFerry + bobFerry < -0.34) bobFerry = 0.001f;
        Ferry.transform.position = new Vector3(xFerry+bobFerry, yFerry + bobFerry, zFerry);
    }
}
