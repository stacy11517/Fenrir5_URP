using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldFingerrr : MonoBehaviour
{
    public LevelLoader levelLoader;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P)) 
        {
            levelLoader.LoadNextLevel();
        }
    }
}
