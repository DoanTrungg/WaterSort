using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4 : MonoBehaviour
{
    [SerializeField] GameObject bottle31;
    [SerializeField] GameObject bottle32;
    [SerializeField] GameObject bottle33;
    [SerializeField] GameObject bottl34;
    [SerializeField] GameObject bottle35;
    BottleControl bottleControl;
    [SerializeField] float delayLevel = 1f;
    void Start()
    {
    }
    void Update()
    {
        if ((bottle31.GetComponent<BottleControl>().CheckBottle() && bottle32.GetComponent<BottleControl>().CheckBottle() && bottle33.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle32.GetComponent<BottleControl>().CheckBottle() && bottle33.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle33.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle31.GetComponent<BottleControl>().CheckBottle() && bottle32.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle31.GetComponent<BottleControl>().CheckBottle() && bottle32.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle31.GetComponent<BottleControl>().CheckBottle() && bottle33.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle31.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle32.GetComponent<BottleControl>().CheckBottle() && bottle33.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle31.GetComponent<BottleControl>().CheckBottle() && bottle33.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()) ||
        (bottle32.GetComponent<BottleControl>().CheckBottle() && bottl34.GetComponent<BottleControl>().CheckBottle() && bottle35.GetComponent<BottleControl>().CheckBottle()))
        {
            Invoke("Level3", delayLevel);
        }
    }
    void Level3()
    {
        SceneManager.LoadScene(0);
    }
}
