using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Runtime.CompilerServices.RuntimeHelpers;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _speedBoost = 2.0f; 
    [SerializeField]
    private GameObject _laserPreFab;
    [SerializeField]
    private float _fireRate;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private float _thrusterSpeed;

    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;
    
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    private float _canFire = -1.0f;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    private int _score;
    public int _ammo = 15;
    public SpriteRenderer _ren;
    public int _shieldHits = 3;
    private Animator _cameraShake;
    private BoosterController _boosterController;


    void Start()
    {
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponentInParent<UIManager>();
        _audioSource = GetComponent<AudioSource>();
        //_ren = GameObject.Find("Shield").GetComponent<SpriteRenderer>();
        _cameraShake = GameObject.Find("Main Camera").GetComponent<Animator>();
        _boosterController = GameObject.Find("BoosterBar").GetComponent<BoosterController>();

        if (_spawnManager == null )
        {
            Debug.LogError("The Spawn Manager is NULL");
        }

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }

        if(_audioSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL");
        }
        else
        {
            _audioSource.clip = _laserSoundClip;
        }

        if(_cameraShake == null)
        {
            Debug.LogError("Can't find main camera");
        }
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        if(_boosterController._booster == 100)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                _thrusterSpeed = 1.5f;
                _boosterController.UseBooster(0.5f);
            }
            else
            {
                _thrusterSpeed = 1.0f;
                _boosterController.FillBooster();
            }
        }
        

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);

        transform.Translate(direction * _speed * Time.deltaTime * _thrusterSpeed);
        
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x > 11.1f)
        {
            transform.position = new Vector3(-11.1f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.1f)
        {
            transform.position = new Vector3(11.1f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        if (_ammo > 0)
        {
            _fireRate = 0.15f;
            _canFire = Time.time + _fireRate;

            if (_isTripleShotActive == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPreFab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }

            PlayerAmmo();
            _audioSource.Play();
        }
        else if (_ammo <= 0)
        {
            _canFire = 0;
            _fireRate = 0;
        }
    }

    public void Damage()
    {
        _cameraShake.SetTrigger("PlayerDamaged");

        if (_isShieldActive == true)
        {
            _shieldHits -= 1;

            if (_shieldHits == 2)
            {
                _ren.GetComponent<SpriteRenderer>().material.color = Color.yellow;
            }
            else if(_shieldHits == 1)
            {
                _ren.GetComponent<SpriteRenderer>().material.color = Color.red;
            }
            else
            {
                _isShieldActive = false;
                _shieldVisualizer.SetActive(false);
                return;
            }
        }
        else
        {
            _lives -= 1;

            if(_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            _uiManager.UpdateLives(_lives);

            if (_lives < 1)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }
    }

    public void TripleShotActive()
    { 
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        while(_isTripleShotActive == true) 
        { 
            yield return new WaitForSeconds(5.0f);
            _isTripleShotActive = false;
        }
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        _speed *= _speedBoost;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        while (_isSpeedBoostActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _isSpeedBoostActive = false;
            _speed /= _speedBoost;
        }
    }

    public void ShieldActive()
    {
        _shieldHits = 3;
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    public void HealthGain()
    {
        if (_lives == 1) 
        { 
            _rightEngine.SetActive(false);
        }
        else if ( _lives == 2 ) 
        {
            _leftEngine.SetActive(false);

        }
        
        if ( _lives <= 2 )
        {
            _lives += 1;
        }

        _uiManager.UpdateLives(_lives);
    }

    public void PlayerScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void PlayerAmmo()
    {
        _ammo -= 1;
        _uiManager.UpdateAmmo(_ammo);
    }

    public void IncreaseAmmo()
    {
        _ammo += 15;
        _uiManager.UpdateAmmo(_ammo);
    }
}
