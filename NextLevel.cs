using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    [SerializeField] GameObject bottle1;
    [SerializeField] GameObject bottle12;
    BottleControl bottleControl; //script
    [SerializeField] float delayLevel = 1f;
    void Start()
    {

    }
    void Update()
    {
        if (bottle1.GetComponent<BottleControl>().CheckBottle() || bottle12.GetComponent<BottleControl>().CheckBottle())
        {
            Invoke("Level1", delayLevel);
        }


    }
    void Level1()
    {
        SceneManager.LoadScene(1);
    }

}
