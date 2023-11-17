using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BottleControl : MonoBehaviour
{

    public Color[] bottleColors;
    public SpriteRenderer bottleMask;
    public AnimationCurve scaleAndRotationCure;
    public AnimationCurve fillAmountCurve;
    public AnimationCurve rotationSpeed;

    public float[] fillAmounts;
    public float[] rotationValues;

    private int rotationIndex = 0; // cx

    [Range(0, 4)]
    public int numberOfColorsInBottle = 4;

    public Color topColor; // cx
    public int numberofTopColorLayers = 1; // cx

    public BottleControl bottleControl;
    public bool justThisBottle = false;
    private int numberOfColorsToTransfer = 0;

    public Transform leftRotationPoint;
    public Transform rightRotationPoint;
    private Transform choseRotationPoint;

    private float directionMultiplier = 1.0f;

    Vector3 originalPosition;
    Vector3 starPosition;
    Vector3 endPosition;

    public LineRenderer lineRenderer;

    [SerializeField] ParticleSystem particle;
    // [SerializeField] Light lightt;

    [SerializeField] AudioClip waterPour;

    public AudioSource collectableSound;
    [SerializeField] AudioClip finishPour;
    bool hasPlayed = false;

    [SerializeField] ParticleSystem confetiiParticle;

    // BoxCollider2D cBottle;
    private float addAmount;

    // public bool hasWater = true;

    // private bool pouring = false;
    private int newNumberOfColorsInOtherBottle;

    void Start()
    {

        // cBottle = GetComponent<BoxCollider2D>();
        bottleMask.material.SetFloat("_FillAmount", fillAmounts[numberOfColorsInBottle]); // so lg màu nc trong bình
        originalPosition = transform.position;
        // UpdateProperty();
        UpdateColorsOnShader();
        UpdateTopColorValues();
    }

    // void UpdateProperty()
    // {
    //     if (numberOfColorsInBottle == 0)
    //     {
    //         hasWater = false;
    //     }
    // }

    void Update()
    {

        if (Input.GetKeyUp(KeyCode.P) && justThisBottle == true)
        {
            UpdateTopColorValues();
            if (bottleControl.FillBottleCheck(topColor))
            {
                ChoseRotationPointAndDirection();

                numberOfColorsToTransfer = Mathf.Min(numberofTopColorLayers, 4 - bottleControl.numberOfColorsInBottle);
                for (int i = 0; i < numberOfColorsToTransfer; i++)
                {
                    bottleControl.bottleColors[bottleControl.numberOfColorsInBottle + i] = topColor;
                }
                bottleControl.UpdateColorsOnShader();
            }
            calculateRotationIndex(4 - bottleControl.numberOfColorsInBottle);
        }

        Sound();
        // DoneBottle();

    }

    public void StartColorTransfer()
    {
        newNumberOfColorsInOtherBottle = bottleControl.numberOfColorsInBottle;
        ChoseRotationPointAndDirection();

        numberOfColorsToTransfer = Mathf.Min(numberofTopColorLayers, 4 - bottleControl.numberOfColorsInBottle);

        newNumberOfColorsInOtherBottle += numberOfColorsToTransfer;

        for (int i = 0; i < numberOfColorsToTransfer; i++)
        {
            bottleControl.SetColors(bottleControl.numberOfColorsInBottle + i, topColor);
        }

        if (bottleControl.numberOfColorsInBottle < 4)
        {
            for (int i = bottleControl.numberOfColorsInBottle; i < 4; i++)
            {
                bottleControl.SetColors(i, topColor);
            }
        }
        bottleControl.UpdateColorsOnShader();

        calculateRotationIndex(4 - bottleControl.numberOfColorsInBottle);

        transform.GetComponent<SpriteRenderer>().sortingOrder += 2;
        bottleMask.sortingOrder += 2;
        StartCoroutine(MoveBottle());

    }
    IEnumerator MoveBottle()
    {
        starPosition = transform.position;
        if (choseRotationPoint == leftRotationPoint)
        {
            endPosition = bottleControl.rightRotationPoint.position;
        }
        else
        {
            endPosition = bottleControl.leftRotationPoint.position;
        }

        float t = 0;

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(starPosition, endPosition, t);
            t += Time.deltaTime * 2;


            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;
        StartCoroutine(RotateBottle());

    }


    IEnumerator MoveBottleBack()
    {
        starPosition = transform.position;
        endPosition = originalPosition;

        float t = 0;

        while (t <= 1)
        {
            transform.position = Vector3.Lerp(starPosition, endPosition, t);
            t += Time.deltaTime * 2;


            yield return new WaitForEndOfFrame();
        }

        transform.position = endPosition;

        transform.GetComponent<SpriteRenderer>().sortingOrder -= 2;
        bottleMask.sortingOrder -= 2;


    }

    void UpdateColorsOnShader()
    {
        bottleMask.material.SetColor("_C1", bottleColors[0]);
        bottleMask.material.SetColor("_C2", bottleColors[1]);
        bottleMask.material.SetColor("_C3", bottleColors[2]);
        bottleMask.material.SetColor("_C4", bottleColors[3]);

    }
    public float timeToRotate = 1.0f;
    IEnumerator RotateBottle()
    {
        float t = 0;
        float lerpValue; // gia tri thay đổi
        float angleValue; // (góc = lerpValue) được tạo bởi 2 dth (x=o va y = lerpValue)

        float lastAngleValue = 0;
        while (t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleValue = Mathf.Lerp(0.0f, directionMultiplier * rotationValues[rotationIndex], lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);
            transform.RotateAround(choseRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);
            bottleMask.material.SetFloat("_SAR", scaleAndRotationCure.Evaluate(angleValue));

            if (fillAmounts[numberOfColorsInBottle] > fillAmountCurve.Evaluate(angleValue) + 0.005f) // nc bot depend numberOfColorInBottle
            {
                if (lineRenderer.enabled == false)
                {
                    lineRenderer.startColor = topColor;
                    lineRenderer.endColor = topColor;
                    lineRenderer.SetPosition(0, choseRotationPoint.position);
                    lineRenderer.SetPosition(1, choseRotationPoint.position - Vector3.up * 5.2f);
                    collectableSound.PlayOneShot(waterPour, 4.5f);
                    lineRenderer.enabled = true;
                }

                bottleMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));
                addAmount = fillAmountCurve.Evaluate(lastAngleValue) - fillAmountCurve.Evaluate(angleValue);

                bottleControl.FilUp(addAmount);
            }

            t += Time.deltaTime * rotationSpeed.Evaluate(angleValue);
            lastAngleValue = angleValue;
            yield return new WaitForEndOfFrame();
        }
        angleValue = directionMultiplier * rotationValues[rotationIndex];
        //transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMask.material.SetFloat("_SAR", scaleAndRotationCure.Evaluate(angleValue));
        bottleMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));

        numberOfColorsInBottle -= numberOfColorsToTransfer;
        bottleControl.numberOfColorsInBottle += numberOfColorsToTransfer;
        bottleControl.numberofTopColorLayers += numberOfColorsToTransfer;

        lineRenderer.enabled = false;

        StartCoroutine(RotateBottleBack());

    }

    IEnumerator RotateBottleBack()
    {
        float t = 0;
        float lerpValue; // gia tri thay đổi
        float angleValue; // (góc = lerpValue) được tạo bởi 2 dth (x=o va y = lerpValue)

        float lastAngleValue = directionMultiplier * rotationValues[rotationIndex];

        while (t < timeToRotate)
        {
            lerpValue = t / timeToRotate;
            angleValue = Mathf.Lerp(directionMultiplier * rotationValues[rotationIndex], 0.0f, lerpValue);

            //transform.eulerAngles = new Vector3(0, 0, angleValue);
            transform.RotateAround(choseRotationPoint.position, Vector3.forward, lastAngleValue - angleValue);
            bottleMask.material.SetFloat("_SAR", scaleAndRotationCure.Evaluate(angleValue));

            lastAngleValue = angleValue;

            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        UpdateTopColorValues();
        angleValue = 0;
        transform.eulerAngles = new Vector3(0, 0, angleValue);
        bottleMask.material.SetFloat("_SAR", scaleAndRotationCure.Evaluate(angleValue));

        StartCoroutine(MoveBottleBack());
    }

    public int UpdateTopColorValues()
    {
        if (numberOfColorsInBottle != 0)
        {
            numberofTopColorLayers = 1;

            topColor = bottleColors[numberOfColorsInBottle - 1];

            if (numberOfColorsInBottle == 4)
            {

                if (bottleColors[3].Equals(bottleColors[2]))
                {
                    numberofTopColorLayers = 2;
                    if (bottleColors[2].Equals(bottleColors[1]))
                    {
                        numberofTopColorLayers = 3;
                        if (bottleColors[1].Equals(bottleColors[0]))
                        {
                            numberofTopColorLayers = 4;
                        }
                    }
                }

            }
            else if (numberOfColorsInBottle == 3)
            {
                if (bottleColors[2].Equals(bottleColors[1]))
                {
                    numberofTopColorLayers = 2;
                    if (bottleColors[1].Equals(bottleColors[0]))
                    {
                        numberofTopColorLayers = 3;
                    }
                }
            }
            else if (numberOfColorsInBottle == 2)
            {
                if (bottleColors[1].Equals(bottleColors[0]))
                {
                    numberofTopColorLayers = 2;
                }
            }

            rotationIndex = 3 - (numberOfColorsInBottle - numberofTopColorLayers);
        }
        return numberofTopColorLayers;

    }

    public bool FillBottleCheck(Color colorToCheck)
    {
        if (numberOfColorsInBottle == 0)
        {
            return true;
        }
        else
        {
            if (numberOfColorsInBottle == 4)
            {
                return false;
            }
            else
            {
                if (topColor.Equals(colorToCheck))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }

    private void calculateRotationIndex(int numberOfEmptySpacesInSecondBottle)
    {
        rotationIndex = 3 - (numberOfColorsInBottle - Mathf.Min(numberOfEmptySpacesInSecondBottle, numberofTopColorLayers));
    }

    private void FilUp(float fillAmountToAdd)
    {
        bottleMask.material.SetFloat("_FillAmount", bottleMask.material.GetFloat("_FillAmount") + fillAmountToAdd);
    }

    public void AdjustFillAmount(int value)
    {

        bottleMask.material.SetFloat("_FillAmount", fillAmounts[value]);
    }
    private void ChoseRotationPointAndDirection()
    {
        if (transform.position.x > bottleControl.transform.position.x)
        {
            choseRotationPoint = leftRotationPoint;
            directionMultiplier = -1.0f;
        }
        else
        {
            choseRotationPoint = rightRotationPoint;
            directionMultiplier = 1.0f;
        }
    }
    public bool CheckBottle()
    {

        if (numberOfColorsInBottle == 4)
        {
            if (bottleColors[1].Equals(bottleColors[0]) &&
                bottleColors[2].Equals(bottleColors[1]) &&
                bottleColors[3].Equals(bottleColors[2]))
            {
                return true;
            }
        }
        return false;
    }


    public void Sound()
    {
        if (CheckBottle() && !hasPlayed)
        {
            collectableSound.PlayOneShot(finishPour);
            SetColorParticle();
            // DoneBottle();
            hasPlayed = true;
        }
    }

    public void SetColorParticle()
    {
        ParticleSystem.MainModule settings = particle.main;
        settings.startColor = new ParticleSystem.MinMaxGradient(bottleColors[1]);
        particle.Play();
        confetiiParticle.Play();
        // SetColorLight(true);
    }

    public void SetColors(int pos, Color color)
    {
        bottleColors[pos] = color;
    }

    // public void SetColorLight(bool On)
    // {
    //     lightt.color = (bottleColors[1]) * Time.deltaTime;

    //     On = false;
    // }


    // void DoneBottle() // lock bottle when it is full  
    // {
    //     if (CheckBottle() && !hasPlayed)
    //     {
    //         cBottle.enabled = !cBottle.enabled;
    //         hasPlayed = true;
    //     }
    // }


}

