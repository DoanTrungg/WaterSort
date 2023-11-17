using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public BottleControl firstBottle;
    public BottleControl secondBottle;

    float bottleUp = 0.7f;

    float bottleDown = -0.7f;
    // BottleControl[] bottles;
    RaycastHit2D hit;

    // void Awake()
    // {
    //     bottles = FindObjectsOfType<BottleControl>();
    // }
    void Start()
    {
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            //  if (hit.collider != null || (firstBottle.hasWater || secondBottle.hasWater))
            if (hit.collider != null)
            {
                if (hit.collider.GetComponent<BottleControl>() != null)
                {
                    if (firstBottle == null)
                    {
                        firstBottle = hit.collider.GetComponent<BottleControl>();
                        if (firstBottle.numberOfColorsInBottle == 0 || firstBottle.CheckBottle())
                        {
                            firstBottle = null;
                            return;
                        }

                        if (firstBottle.numberOfColorsInBottle != 0) // plus
                        {
                            firstBottle.transform.position = new Vector3(firstBottle.transform.position.x,
                                                                         firstBottle.transform.position.y + bottleUp,
                                                                         firstBottle.transform.position.z);
                        }
                    }
                    else
                    {
                        if (firstBottle == hit.collider.GetComponent<BottleControl>())
                        {

                            if (firstBottle.numberOfColorsInBottle != 0) // plus
                            {
                                firstBottle.transform.position = new Vector3(firstBottle.transform.position.x,
                                                 firstBottle.transform.position.y + bottleDown,
                                                 firstBottle.transform.position.z);
                            }
                            firstBottle = null;
                        }

                        else
                        {

                            secondBottle = hit.collider.GetComponent<BottleControl>();
                            firstBottle.bottleControl = secondBottle;

                            if (secondBottle.CheckBottle())
                            {
                                secondBottle = null;
                                return;
                            }
                            // foreach (BottleControl bottle in bottles)
                            // {
                            //     if (!bottle.hasWater)
                            //     {
                            //         return;
                            //     }
                            // }

                            firstBottle.UpdateTopColorValues();
                            secondBottle.UpdateTopColorValues();


                            if (secondBottle.FillBottleCheck(firstBottle.topColor) == true)
                            {
                                firstBottle.StartColorTransfer();
                                firstBottle = null;
                                secondBottle = null;
                            }
                            else
                            {
                                if (firstBottle.numberOfColorsInBottle != 0) // plus    
                                {
                                    firstBottle.transform.position = new Vector3(firstBottle.transform.position.x,
                                                                                 firstBottle.transform.position.y + bottleDown,
                                                                                 firstBottle.transform.position.z);
                                }
                                // if (firstBottle.CheckBottle() || firstBottle.numberOfColorsInBottle == 0)
                                // { firstBottle = null; }
                                firstBottle = null;
                                secondBottle = null;

                            }
                        }
                    }
                }
            }
            // else // tab anywhere on the screen to deslecet bottles
            // {
            //     if (firstBottle.numberOfColorsInBottle != 0)
            //     {
            //         firstBottle.transform.position = new Vector3(firstBottle.transform.position.x,
            //                                                      firstBottle.transform.position.y + bottleDown,
            //                                                      firstBottle.transform.position.z);
            //     }
            //     firstBottle = null;
            //     secondBottle = null;
            // }
        }
    }

}


