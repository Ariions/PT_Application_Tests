using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DetectColition : MonoBehaviour
{
    Rigidbody rigidbody;
    public bool collided = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other != this)
        {
            collided = true;
        }
    }
}
