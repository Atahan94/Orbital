using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMove : MonoBehaviour
{
    public GameObject Light;
    Quaternion end = Quaternion.Euler(-17,0,0);
    public float speed;
    public float Lightspeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Ligtsip = Lightspeed * Time.deltaTime;
        Light.transform.rotation = Quaternion.Lerp(Light.transform.rotation, Quaternion.Euler(340, Light.transform.rotation.y, Light.transform.rotation.z), Ligtsip);
        float sped = speed * Time.deltaTime;
        transform.rotation = Quaternion.Lerp(transform.rotation, end, sped);
        
    }
}
