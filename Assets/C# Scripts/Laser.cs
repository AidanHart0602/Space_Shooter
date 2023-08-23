using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _laserSpeed = 8.0f;
    private bool _enemyLaserTrigger = false;
    // Update is called once per frame
    void Update()
    {
        if (_enemyLaserTrigger == false)
        {
            playerLaserMovement();
        }
        else
        {
            enemyLaserMovement();
        }
    }
    void playerLaserMovement()
    {

        //When the laser is spawned
        //translate laser up
        transform.Translate(Vector3.up * _laserSpeed * Time.deltaTime);

        //if laser position for y is greater than 8
        //Destroy it
        if (transform.position.y > 8)
        {
            if(transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    void enemyLaserMovement()
    {
        //When the laser is spawned
        //translate laser down
        transform.Translate(Vector3.down * _laserSpeed * Time.deltaTime);

        //if laser position for y is less than -8
        //Destroy it
        if (transform.position.y < -8)
        {
            if (transform.parent != null)
            {
                
                Destroy(transform.parent.gameObject);
            }
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(collision.tag == "Player" && _enemyLaserTrigger == true)
        {
            if(player != null)
            {
                player.Damage();
            }
       
        }
    }
    public void enemyLaserTriggerTrue()
    {
        _enemyLaserTrigger = true;
    }
}