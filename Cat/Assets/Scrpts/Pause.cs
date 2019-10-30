using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    private bool active;
    private Canvas pause;

    // Start is called before the first frame update
    void Start(){
        pause = GetComponent<Canvas>();
        pause.enabled = false;
    }

    // Update is called once per frame
    void Update(){
        if (Input.GetKeyDown("p")){
            active = !active;
            pause.enabled = active;
            Time.timeScale = (active) ? 0.0f : 1.0f;
        }
    }
}
