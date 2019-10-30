using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;
    public GameObject coinGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    public ParticleSystem dust;

    private Animator animator;
    private AudioSource playerSounds;
    private float startY;

    // Start is called before the first frame update
    void Start(){
        animator = GetComponent<Animator>();
        playerSounds = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    // Update is called once per frame
    void Update(){
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool userAction = Input.GetKeyDown("space");
        bool grounded = startY == transform.position.y;

        if (gamePlaying && userAction && grounded){
            UpdateState("PlayerJump");
            playerSounds.clip = jumpClip;
            playerSounds.Play();
        }
       
    }

    public void UpdateState(string state = null) {
        if (state != null) {
            animator.Play(state);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if (collision.gameObject.tag == "Enemy"){
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("CancelGenerator", true);
            coinGenerator.SendMessage("CancelGenerator", true);
            game.SendMessage("ResetTimeScale", 1.0f);

            playerSounds.clip = dieClip;
            playerSounds.Play();
        }  else if (collision.gameObject.tag == "Point"){
            game.SendMessage("IncreasePoints", 10);
            Destroy(collision.transform.parent.gameObject);

            playerSounds.clip = pointClip;
            playerSounds.Play();
        } 
    }

    void GameReady(){
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    void DustPlay(){
        dust.Play();
    }

    void DustStop(){
        dust.Stop();
    }
}
