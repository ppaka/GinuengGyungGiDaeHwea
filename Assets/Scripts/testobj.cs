using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testobj : MonoBehaviour
{

    private void Start()
    {
        Destroy(gameObject, 10);
    }

    void Update()
    {
        transform.position += (transform.localRotation * -transform.up) * Time.deltaTime;
    }
}
