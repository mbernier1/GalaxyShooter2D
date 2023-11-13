using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

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
        Player player = GameObject.Find("Player").GetComponent<Player>();

        if (other.tag == "Player")
        {
            if (player != null)  
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
        else if(other.tag == "Laser")
        {
            if(player != null)
            {
                Debug.Log("player score about to get called");
                player.PlayerScore();
                Debug.Log("player score updated");
            }

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
