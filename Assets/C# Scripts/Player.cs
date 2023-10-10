﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //Prefabs & Game Objects
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    
    [SerializeField]
    private GameObject _deathExplosionPrefab;
    [SerializeField]
    private GameObject _giantLaserPrefab;
    

    //Player Movements & other values
    [SerializeField]
    private float _baseSpeed = 4f;
    [SerializeField]
    private float _currentSpeed = 4f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _coolDown = 0f;
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private float _speedBoostIncrease = 1.6f;
    [SerializeField]
    public int ammoCount = 15;
    private float horizontalInput = 0;
    private float verticalInput = 0;

    //Shield Related Variables
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject _damagedShield;
    [SerializeField]
    private GameObject _criticalShield;
    private int _shieldHealthCount = 3;
    [SerializeField]
    private bool _shieldTrigger = false;

    //Thruster related variables 
    [SerializeField]
    private float _thrusterSpeed = 1.5f;
    public float currentThrusterCharge = 0f;
    public float maxThrusterCharge = 100f;
    [SerializeField]
    private float _thrusterUsage = 40f;
    private bool _thrusterReady = true;


    //triggers & bools
    private bool _tripleShotTrigger = false;



    //Handlers from other scripts
    private SpawnManager _spawnManager;
    private UiManager _uiManager;
    private CameraShake _cameraShake;

    //UI and VFX
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private GameObject _leftThrustor, _rightThrustor;


    //Sound
    [SerializeField]
    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _outOfAmmoSoundFile;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 0);

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("THE AUDIO SOURCE FOR THE PLAYER IS NULL");
        }

        _cameraShake = GameObject.Find("CameraContainer").GetComponent<CameraShake>();

        if(_cameraShake == null)
        {
            Debug.LogError("CAMERA SHAKE IS NULL");
        }
        
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SPAWN MANAGER IS NULL");
        }

        _uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        if (_uiManager == null)
        {
            Debug.Log("UI MANAGER IS NULL");
        }

    }

    // Update is called once per frame
    void Update()
    {
        PlayerSprint();
        CalculateMovement();
        LaserSpawn();
        HealthUpdate();
    }

    private void PlayerSprint()
    {
         horizontalInput = Input.GetAxis("Horizontal");
         verticalInput = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (_uiManager.GetThrustValue() > 0 && _thrusterReady == true)
            {
                transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _baseSpeed * _thrusterSpeed * Time.deltaTime);
                currentThrusterCharge -= _thrusterUsage * Time.deltaTime;
                _uiManager.UpdateThrusterSlider(currentThrusterCharge);
            }
            else
            {
                transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _baseSpeed * Time.deltaTime);
            }
        }

        else if (_uiManager.GetThrustValue() == 0)
        {
            transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _baseSpeed * Time.deltaTime);
            _thrusterReady = false;
            StartCoroutine(ThrusterCooldown());
            return;
        }
    }


    void CalculateMovement()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");


        if (currentThrusterCharge < maxThrusterCharge)
        {
            currentThrusterCharge += (_thrusterUsage/2) * Time.deltaTime;
            _uiManager.UpdateThrusterSlider(currentThrusterCharge);
        }


        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _currentSpeed * Time.deltaTime);


        if (transform.position.y < -4.6f)
        {
            transform.position = new Vector3(transform.position.x, -4.6f, 0);
        }
        else if (transform.position.y > 6)
        {
            transform.position = new Vector3(transform.position.x, 6, 0);
        }

        if (transform.position.x > 10.35f)
        {
            transform.position = new Vector3(-10.3f, transform.position.y, 0);
        }

        else if (transform.position.x < -10.35f)
        {
            transform.position = new Vector3(10.3f, transform.position.y, 0);
        }
    }

    void LaserSpawn()
    {
        //When I hit the space key
        //Spawn the laser game object

        if (Input.GetKey(KeyCode.Space) && Time.time > _coolDown)
        {
            if (ammoCount > 0)
            {
                ammoCount--;
                AmmoCount(ammoCount);

                _coolDown = Time.time + _fireRate;

                if (_tripleShotTrigger == true)
                {
                    Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
                    _audioSource.Play();
                }
                else
                {
                    Instantiate(_laserPrefab, transform.position + (new Vector3(0, 1.2f, 0)), Quaternion.identity);
                    _audioSource.Play();
                }
            }

            else if (ammoCount == 0)
            {
                AudioSource.PlayClipAtPoint(_outOfAmmoSoundFile, transform.position);
                return;
            }
        }
    }

    public void AmmoCount(int bullets)
    {
        ammoCount = bullets;
        _uiManager.AmmoTextUpdate(ammoCount);
    }

    public void Damage()
    {
        StartCoroutine(_cameraShake.ShakeTheCamera());
        if (_shieldTrigger == true)
        {
            //Every time that the shield is hit,
            //the health will be lowered, allowing the shield method to use a different if statement.
            _shieldHealthCount--;
            ShieldDurability();
            return;
        }

        _playerLives--;
        _uiManager.UpdateLives(_playerLives);


        if (_playerLives == 2)
        {
            _rightThrustor.SetActive(true);
            
        }

        else if (_playerLives == 1)
        {
            _leftThrustor.SetActive(true);

        }

        else if (_playerLives < 1)
        {

            _spawnManager.PlayerDeath();
            Instantiate(_deathExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

    }
    public void AmmoPowerUpActive()
    {
        ammoCount = ammoCount + 15;
        _uiManager.AmmoTextUpdate(ammoCount);
    }
    public void TripleShotActive()
    {
        _tripleShotTrigger = true;
        StartCoroutine(TripleShotCooldown());

    }

    public void SpeedBoostActive()
    {
        _currentSpeed *= _speedBoostIncrease;
        StartCoroutine(SpeedBoostCooldown());
    }

    public void ShieldActive()
    {
        _shieldTrigger = true;
        _shield.SetActive(true);

    }

    void ShieldDurability()
    {
        //This method is used for keeping track of which shield objects are being used as it gets more damaged.
        if (_shieldHealthCount == 2)
        {
            _shield.SetActive(false);
            _damagedShield.SetActive(true);
        }

        else if (_shieldHealthCount == 1)
        {
            _damagedShield.SetActive(false);
            _criticalShield.SetActive(true);
        }
        else if (_shieldHealthCount == 0)
        {
            _criticalShield.SetActive(false);
            _shieldTrigger = false;
            _shieldHealthCount = 3;
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }

    public void MedKitActive()
    {
        if(_playerLives < 3)
        {
            _playerLives = _playerLives + 1;
        }
        return;
    }

    private void HealthUpdate()
    {
        if(_playerLives == 3)
        {
            _rightThrustor.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
        if(_playerLives == 2)
        {
            _leftThrustor.SetActive(false);
            _uiManager.UpdateLives(_playerLives);
        }
    }

    public void GiantLaserActive()
    {
        _giantLaserPrefab.SetActive(true);
        StartCoroutine(GiantLaserCooldown());
    }

    IEnumerator ThrusterCooldown() 
    {
        yield return new WaitForSeconds(3.0f);
        _thrusterReady = true;
    }

    IEnumerator GiantLaserCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _giantLaserPrefab.SetActive(false);
    }

    IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotTrigger = false;
    }
    IEnumerator SpeedBoostCooldown()
    {
        yield return new WaitForSeconds(8.0f);
        _currentSpeed = _baseSpeed;
    }
}