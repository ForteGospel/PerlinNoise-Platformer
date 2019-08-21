using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] float speed = 1;
    void FixedUpdate()
    {
        Transform parentTransform = gameObject.GetComponentInParent<Transform>();
        //float speed = parentTransform. - transform.position.z;
        Vector2 parallax = new Vector2((parentTransform.position.x / transform.position.z) % transform.position.z, 0.0f);

        //Debug.Log("updating  to " + Vector2.left * Time.smoothDeltaTime * speed);
        GetComponent<Renderer>().material.mainTextureOffset = parallax;
    }
}
