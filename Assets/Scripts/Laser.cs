using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f;

    void Update()
    {
        //Vector3 direction = new Vector3(transform.position.x, 1, 0);

        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        if (transform.position.y > 8f)
        {
            Destroy(gameObject);
        }
    }
}
