using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel2 : MonoBehaviour
{
    [SerializeField] GameObject bottle21;
    [SerializeField] GameObject bottle24;
    [SerializeField] float delayLevel = 1f;
    void Start()
    {

    }
    void Update()
    {

        if (bottle21.GetComponent<BottleControl>().CheckBottle() ||
            bottle24.GetComponent<BottleControl>().CheckBottle())
        {
            Invoke("Level2", delayLevel);
        }

    }
    void Level2()
    {
        SceneManager.LoadScene(2);
    }
}
