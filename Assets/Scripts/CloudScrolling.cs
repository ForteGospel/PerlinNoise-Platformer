using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudScrolling : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        GetComponent<Renderer>().material.mainTextureOffset += Vector2.left * Time.smoothDeltaTime * -0.01f;
    }
}
