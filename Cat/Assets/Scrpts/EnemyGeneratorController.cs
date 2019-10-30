using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour
{
    public float generatorTimer = 2.0f;
    public float startsIn;

    public Object[] enemyPrefabs;

    private int pointer;
    private Rigidbody2D rb2d;
    private Animator anim;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    void CreateEnemy(){
        pointer = Random.Range(0, enemyPrefabs.Length);
        Instantiate(enemyPrefabs[pointer], transform.position, Quaternion.identity);
        Debug.Log("Enemigo generado: "+ pointer.ToString());
    }

    public void StartGenerator(){
        InvokeRepeating("CreateEnemy", startsIn, generatorTimer);
    }

    public void CancelGenerator(bool stop = false){
        CancelInvoke("CreateEnemy");
        if (stop){
            Object[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in allEnemies){
                rb2d = enemy.GetComponent<Rigidbody2D>();
                rb2d.velocity = Vector2.zero;
                anim = enemy.GetComponent<Animator>();
                anim.enabled = false;
            }
        }
    }
}
