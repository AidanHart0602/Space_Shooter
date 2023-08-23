using System.Collections;
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
    private GameObject _shield;
    [SerializeField]
    private GameObject _deathExplosionPrefab;

    //Player Movements & laser behavior values
    [SerializeField]
    private float _speed = 3.5f;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _coolDown = 0;
    [SerializeField]
    private int _playerLives = 3;
    [SerializeField]
    private float _speedBoostIncrease = 1.4f;

    //triggers
    private bool _tripleShotTrigger = false;
    [SerializeField]
    private bool _shieldTrigger = false;

    //Handlers from other scripts
    private SpawnManager _spawnManager;
    private UiManager _uiManager;

    //UI and VFX
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private GameObject _leftThrustor, _rightThrustor;


    //Sound
    [SerializeField]
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource == null)
        {
            Debug.LogError("THE AUDIO SOURCE FOR THE PLAYER IS NULL");
        }

        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("SPAWN MANAGER IS NULL");
        }
        _uiManager = GameObject.Find("UiManager").GetComponent<UiManager>();
        if (_uiManager == null)
        {
            Debug.Log("ERROR UI MANAGER IS NULL");
        }
      
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        LaserSpawn();

    }
    void LaserSpawn()
    {
        //When I hit the space key
        //Spawn the laser game object

        if (Input.GetKey(KeyCode.Space) && Time.time > _coolDown)
        {
            _coolDown = Time.time + _fireRate;

            if (_tripleShotTrigger == true)
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Instantiate(_laserPrefab, transform.position + (new Vector3(0, 1.2f, 0)), Quaternion.identity);
            }
            _audioSource.Play();           
        }
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * _speed * Time.deltaTime);

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

    public void Damage()
    {
        if (_shieldTrigger == true)
        {

            _shield.SetActive(false);
            _shieldTrigger = false;

            return;
        }

        _playerLives--;

        _uiManager.UpdateLives(_playerLives);

        if(_playerLives == 2)
        {
            _rightThrustor.SetActive(true);
        }

        else if(_playerLives == 1)
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

    public void tripleShotActive()
    {
        _tripleShotTrigger = true;
        StartCoroutine(TripleShotCooldown());

    }

    public void speedBoostActive()
    {
        _speed *= _speedBoostIncrease;
        StartCoroutine(speedBoostCooldown());
    }


    public void shieldActive()
    {
        _shieldTrigger = true;
        _shield.SetActive(true);

    }
    IEnumerator TripleShotCooldown()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotTrigger = false;
    }
    IEnumerator speedBoostCooldown()
    {
        yield return new WaitForSeconds(8.0f);
        _speed = 6f;
    }

    public void addScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);

    }
}