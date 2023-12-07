using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;

    private Player _player;
    private Animator _animator;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if( _player == null )
        {
            Debug.LogError("Player is NULL");
        }
        _animator = GetComponent<Animator>();
        if( _animator == null )
        {
            Debug.LogError("Animitaor is NULL");
        }
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

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
        else if(other.tag == "Laser")
        {
            int points = Random.Range(5, 13);
            if(_player != null)
            {
                _player.PlayerScore(points);
            }

            Destroy(other.gameObject);

            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
    }
}
