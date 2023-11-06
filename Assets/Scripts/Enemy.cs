using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    public Text scoreText;

    void Start()
    {
        scoreText.text = "Score: " + Scoring.totalScore;
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
            Player player = other.transform.GetComponent<Player>();
            if (player != null)  
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
        else if(other.tag == "Laser")
        {
            Scoring.totalScore += 1;
            scoreText.text = "Score: " + Scoring.totalScore;

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
