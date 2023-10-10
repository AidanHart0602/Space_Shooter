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
    private TMP_Text _gameOverText;
    [SerializeField]
    private TMP_Text _restartText;
    [SerializeField]
    private TMP_Text _ammoText;
    private GameManager _gameManager;
    [SerializeField]
    private Slider _slider;
    private Player _player;
    [SerializeField]
    private TMP_Text _waveText;

    // Start is called before the first frame update
    void Start()
    {

        _waveText.gameObject.SetActive(false);
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

    public void UpdateWaves(int CurrentWave) 
    {
        _waveText.text = "Wave " + CurrentWave + " incoming!";
        _waveText.gameObject.SetActive(true);
        StartCoroutine(DisableWaveText());
    }

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

    public void UpdateThrusterSlider(float DeltaTime)
    {
        //   _slider.value = Mathf.Clamp(_player.currentThrusterCharge / _player.maxThrusterCharge, 0, 1f);

        _slider.value -= DeltaTime / 5f;
    }

    public bool RequestThrust() 
    {
        return _slider.value > 0;

    }
    public void RechargeThruster() 
    {
        StartCoroutine(RechargeThrusterRoutine());
    }

    IEnumerator DisableWaveText()
    { 
            yield return new WaitForSeconds(3.0f);
            _waveText.gameObject.SetActive(false);
    }

    IEnumerator RechargeThrusterRoutine() 
    {
        while (_slider.value < 1f) 
        {

            _slider.value += Time.deltaTime / 10f;
            yield return new WaitForEndOfFrame();
        }
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
