using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y <= -4) 
        {
            float randX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player")
        {
            if (_player != null)  
            {
                _player.Damage();
            }

            Destroy(this.gameObject);
        }
        else if(other.tag == "Laser")
        {
            int points = Random.Range(5, 13);
            if(_player != null)
            {
                _player.PlayerScore(points);
            }

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
