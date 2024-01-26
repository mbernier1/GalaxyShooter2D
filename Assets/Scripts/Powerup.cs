using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID;

    private Player _player;
    [SerializeField]
    private AudioClip _clip;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        
        if (_player == null )
        {
            Debug.LogError("Player is NULL");
        }
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -4)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            if(_player != null)
            {
                switch(_powerupID)
                {
                    case 0:
                        _player.TripleShotActive();
                        break;
                    case 1:
                        _player.SpeedBoostActive();
                        break;
                    case 2:
                        _player.ShieldActive();
                        break;
                    case 3:
                        _player.HealthGain();
                        break;
                    default:
                        Debug.Log("default case");
                        break;
                }
            }
            
            Destroy(this.gameObject);
        }
    }
}
