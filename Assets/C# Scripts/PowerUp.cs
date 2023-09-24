using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float PowerUpSpeed = 3.0f;
    [SerializeField]
    private int PowerUpID;
    [SerializeField]
    private AudioClip _collectedAudio;
    // Update is called once per frame

    void Update()
    { 
       transform.Translate(Vector3.down * PowerUpSpeed * Time.deltaTime);

        if(transform.position.y < -7)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
       if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_collectedAudio, transform.position);
            switch (PowerUpID)
            {
                case 0:
                    player.tripleShotActive();   
                    break;
                case 1:
                    player.speedBoostActive();
                    break;
                case 2:
                    player.shieldActive();
                    break;
                case 3:
                    player.medKitActive();
                    break;
                case 4:
                    player.giantLaserActive();
                    break;
                case 5:
                    player.ammoPowerUpActive();
                    break;
                default:
                    Debug.Log("Default value");
                    break;
            }
            Destroy(this.gameObject);
        }

    }
}