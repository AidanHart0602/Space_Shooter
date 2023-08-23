using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TripleShotPowerUp : MonoBehaviour
{
    [SerializeField]
    private float PowerUpSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
   
    
    }

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
            if (player != null)
            {
                player.tripleShotActive();
            }
            Destroy(this.gameObject);
        } 
    }
}
