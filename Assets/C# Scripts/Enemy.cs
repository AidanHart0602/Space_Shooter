using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour

{
    //Number Variables
    [SerializeField]
    private float _speed = 4.0f;
    private float _randomX = 0;
    private float _laserCooldown;


    //Handles from other Scripts
    private Player _player;
    private Animator _anim;

    //Variables that access Information from the object
    private Collider2D _collider2D;
    [SerializeField]
    private GameObject _enemyLaserPrefab;


    //Bools
    private bool _laserTrigger = true;
    //Audio
    [SerializeField]
    private AudioSource _audioSource;

    void Start()
    {
        
        _audioSource = GetComponent<AudioSource>();

        _player = GameObject.Find("Player").GetComponent<Player>();

        _collider2D = GetComponent<Collider2D>();

        if(_collider2D == null)
        {
            Debug.LogError("COLLIDER COMPONENT IS NULL");
        }
        if (_player == null)
        {
            Debug.LogError("PLAYER IS NULL");
        }
        _anim = GetComponent<Animator>();

        if (_anim == null)
        {
            Debug.LogError("ANIMATIONS ARE NULL");
        }

        if (_audioSource == null)
        {
            Debug.LogError("AUDIO SOURCE FOR THE ENEMY IS NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        enemyMovement();
        enemyLaser();

    }

    void enemyMovement()
    {
        //Move down 4 meters per second
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        //When at the bottom of the screen, respawn at top
        if (transform.position.y < -5)
        {
            _randomX = (Random.Range(-10.3f, 10.3f));
            transform.position = new Vector3(_randomX, 6.5f, 0);
        }
    }
    void enemyLaser()
    {
        
        if (Time.time > _laserCooldown && _laserTrigger == true)
        {
            float _fireRate = Random.Range(1.5f, 2.0f);
            _laserCooldown = Time.time + _fireRate;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].enemyLaserTriggerTrue();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>(); 
        
            if (player != null)
            {
                if(_anim != null)
                {
                    _collider2D.enabled = !_collider2D.enabled;
                    _laserTrigger = false;
                    _speed = 0;
                    _anim.SetTrigger("enemyDeathTrigger");
                    player.Damage();
                    _audioSource.Play();
                }

            }
            Destroy(gameObject, 2.6f);
        }
        if (other.tag == "Laser")
        { 
            Destroy(other.gameObject);

            if (_player != null)  
            {
                 _collider2D.enabled = !_collider2D.enabled;
                _laserTrigger = false;
                _speed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.addScore(10);
                _audioSource.Play();

            }
            Destroy(this.gameObject, 2.6f);
        }
        if (other.tag == "GiantLaser")
        {
            if (_player != null)
            {
                _collider2D.enabled = !_collider2D.enabled;
                _laserTrigger = false;
                _speed = 0;
                _anim.SetTrigger("enemyDeathTrigger");
                _player.addScore(10);
                _audioSource.Play();

            }
            Destroy(this.gameObject, 2.6f);
        }
    }

}