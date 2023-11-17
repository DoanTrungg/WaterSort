using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3 : MonoBehaviour
{
    [SerializeField] GameObject bottle21;
    [SerializeField] GameObject bottle22;
    [SerializeField] GameObject bottle23;
    [SerializeField] GameObject bottle24;
    [SerializeField] GameObject bottle25;
    BottleControl bottleControl;
    [SerializeField] float delayLevel = 1f;
    void Start()
    {
    }
    void Update()
    {
        if ((bottle21.GetComponent<BottleControl>().CheckBottle() && bottle22.GetComponent<BottleControl>().CheckBottle() && bottle23.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle22.GetComponent<BottleControl>().CheckBottle() && bottle23.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle23.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle21.GetComponent<BottleControl>().CheckBottle() && bottle22.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle21.GetComponent<BottleControl>().CheckBottle() && bottle22.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle21.GetComponent<BottleControl>().CheckBottle() && bottle23.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle21.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle22.GetComponent<BottleControl>().CheckBottle() && bottle23.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle21.GetComponent<BottleControl>().CheckBottle() && bottle23.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle22.GetComponent<BottleControl>().CheckBottle() && bottle24.GetComponent<BottleControl>().CheckBottle() && bottle25.GetComponent<BottleControl>().CheckBottle()))
        {
            Invoke("Level2", delayLevel);
        }
    }
    void Level2()
    {
        SceneManager.LoadScene(3);
    }
}
