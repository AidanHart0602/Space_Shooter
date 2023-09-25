using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float powerUpSpeed = 3.0f;
    [SerializeField]
    private int powerUpID;
    [SerializeField]
    private AudioClip _collectedAudio;
    // Update is called once per frame

    void Update()
    { 
       transform.Translate(Vector3.down * powerUpSpeed * Time.deltaTime);

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
            switch (powerUpID)
            {
                case 0:
                    player.TripleShotActive();   
                    break;
                case 1:
                    player.SpeedBoostActive();
                    break;
                case 2:
                    player.ShieldActive();
                    break;
                case 3:
                    player.MedKitActive();
                    break;
                case 4:
                    player.GiantLaserActive();
                    break;
                case 5:
                    player.AmmoPowerUpActive();
                    break;
                default:
                    Debug.Log("Default value");
                    break;
            }
            Destroy(this.gameObject);
        }

    }
}