using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed;
    void Start()
    {
        speed = 15.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.D))
     {
         transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
     }
     if (Input.GetKey(KeyCode.A))
     {
         transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
     }
     if (Input.GetKey(KeyCode.W))
     {
         transform.position += new Vector3(0, 0, speed * Time.deltaTime);
     }
     if (Input.GetKey(KeyCode.S))
     {
         transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
     }
     if(Input.GetKey(KeyCode.Space)){
         transform.position += new Vector3(0, speed * Time.deltaTime, 0);
     }
     if(Input.GetKey(KeyCode.LeftControl)){
         transform.position -= new Vector3(0, speed * Time.deltaTime, 0);
     }
     if(Input.GetKey(KeyCode.RightArrow)){
         transform.Rotate(new Vector3 (0f, (speed-5) * Time.deltaTime, 0f));
     }
     if(Input.GetKey(KeyCode.LeftArrow)){
         transform.Rotate(new Vector3 (0f, -(speed-5) * Time.deltaTime, 0f));
     }
     if(Input.GetKey(KeyCode.UpArrow)){
         transform.Rotate(new Vector3 (-(speed-5) * Time.deltaTime, 0f, 0f));
     }
     if(Input.GetKey(KeyCode.DownArrow)){
         transform.Rotate(new Vector3 (+(speed-5) * Time.deltaTime, 0f, 0f));
     }
    }
}
