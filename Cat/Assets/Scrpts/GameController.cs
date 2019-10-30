using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { Idle, Playing, Ended, Ready};

public class GameController : MonoBehaviour
{
    public float parallaxSpeed = 0.0f;
    public RawImage background;
    public RawImage platform;
    public GameObject uiIdle;
    public GameObject uiScore;
    public GameObject uiEnded;
    public GameObject exitButton;
    public GameObject pause;
    public GameState gameState = GameState.Idle;
    public Text textPoints;
    public Text recordText;
    public GameObject player;
    public GameObject enemyGenerator;
    public GameObject coinGenerator;

    public float scaleTime = 5.0f;
    public float scaleInc = 0.1f;
    public float maxScale = 3.0f;
    public bool active = false;

    private AudioSource musicTheme;
    private int points = 0;
    private float scale = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        musicTheme = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
    }

    // Update is called once per frame
    void Update() {

        bool userAction = Input.GetKeyDown("space"); //|| Input.GetMouseButtonDown(0));

        //Escena principal.
        if (gameState == GameState.Idle && userAction) {
            gameState = GameState.Playing;
            uiIdle.SetActive(false);
            uiScore.SetActive(true);
            exitButton.SetActive(false);
            player.SendMessage("UpdateState", "PlayerRun");
            enemyGenerator.SendMessage("StartGenerator");
            coinGenerator.SendMessage("StartGenerator");
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
            InvokeRepeating("Points", 0.7f, 0.7f);

            musicTheme.Play();
        }
        //Juego en marcha. 
        else if (gameState == GameState.Playing){
            Parallax();
            if (Input.GetKeyDown("p")){
                active = !active;
                pause.SetActive(active);
                Time.timeScale = (active) ? 0.0f : (scale*0.1f)+1.0f;
            }
            if (Time.timeScale > maxScale) ResetTimeScale(maxScale);
            if (active && Input.GetKeyDown("m")){
                RestartGame();
            }
        }

        else if (gameState == GameState.Ended){
            musicTheme.Pause();
            CancelInvoke("Points"); 
        }

        //Juego praparado para reiniciarse. 
        else if (gameState == GameState.Ready){

            uiEnded.SetActive(true);
            
            if (Input.GetKeyDown("r")){
                RestartGame();
            }
        }
    }

    void Parallax()
    {
        float finalSpeed = parallaxSpeed * Time.deltaTime;
        background.uvRect = new Rect(background.uvRect.x + finalSpeed, 0.0f, 1.0f, 1.0f);
        platform.uvRect = new Rect(platform.uvRect.x + finalSpeed * 4, 0.0f, 1.0f, 1.0f);
    }

    void RestartGame(){
        ResetTimeScale();
        SceneManager.LoadScene("Principal");
        gameState = GameState.Playing;
    }

    void GameTimeScale(){
        Time.timeScale += scaleInc;
        scale += 1.0f; 
        Debug.Log("Ritmo incrementado: " + Time.timeScale.ToString());
    }

    public void ResetTimeScale(float newTimeScale = 1.0f){
        CancelInvoke("GameTimeScale");
        Time.timeScale = newTimeScale;
        Debug.Log("Ritmo reestablecido: " + Time.timeScale.ToString());
    }

    public void Points(){
        IncreasePoints(1);
    }

    public void IncreasePoints(int  increased = 0){
        if(increased == 0) textPoints.text = (++points).ToString();
        else{
            points += increased;
            textPoints.text = (points).ToString();
        }
        if(points > GetMaxScore()){
            recordText.text = "BEST: " + points.ToString();
            SaveScore(points);
        }
    }

    public int GetMaxScore(){
        return PlayerPrefs.GetInt("MaxPoints", 0);
    }

    public void SaveScore(int currentPoints){
        PlayerPrefs.SetInt("MaxPoints", currentPoints);
    }

    public void Menu(){
        ResetTimeScale();
        SceneManager.LoadScene("Principal");
        gameState = GameState.Playing;
    }

    public void Pause(){
        active = !active;
        pause.SetActive(active);
        Time.timeScale = (active) ? 0.0f : (scale * 0.1f) + 1.0f;
    }
}
