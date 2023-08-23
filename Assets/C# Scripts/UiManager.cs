using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class UiManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _textScore;
    [SerializeField]
    private Image _liveImage;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverText;
    [SerializeField]
    private Text _restartText;
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _textScore.text = "Score: " + 0;

        if(_gameManager == null)
        {
            Debug.Log("GAME MANAGER IS NULL");
        }
        //  _liveSprites[CurrentPlayerLives = 3]
    }


    void Update()
    {
 
    }
    // Update is called once per frame
    public void UpdateScore(int NewScore)
    {
        _textScore.text = "Score: " + NewScore.ToString();
    }

    public void UpdateLives(int currentLives) 
    {
        _liveImage.sprite = _liveSprites[currentLives];

        if(currentLives == 0)
        {
            GameOverCode();            
        }
    }

    void GameOverCode()
    {
        _gameManager.GameOverReset();
        StartCoroutine(GameOverFlicker());

    }

    IEnumerator GameOverFlicker()
    {
        _restartText.gameObject.SetActive(true);
         
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }

}
