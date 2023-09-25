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
    [SerializeField]
    private Text _ammoText;
    private GameManager _gameManager;
    [SerializeField]
    private Slider _slider;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _textScore.text = "Score: " + 0;

        if(_gameManager == null)
        {
            Debug.Log("GAME MANAGER IS NULL");
        }

        _ammoText.text = "Ammunition: " + 15.ToString();
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

    public void UpdateThrusterSlider(float CurrentThrusterCharge)
    {
        _slider.value = Mathf.Clamp(_player.currentThrusterCharge / _player.maxThrusterCharge, 0, 1f);

    }

    public float GetThrustValue()
    {
        return _slider.value;
    }


    void GameOverCode()
    {
        _gameManager.GameOverReset();
        StartCoroutine(GameOverFlicker());

    }

    public void AmmoTextUpdate(int bulletAmount)
    {
        _ammoText.text = "Ammunition: " + bulletAmount.ToString();
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
