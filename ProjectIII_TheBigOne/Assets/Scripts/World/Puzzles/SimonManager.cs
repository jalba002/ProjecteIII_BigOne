using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonManager : Puzzle
{
    // Start is called before the first frame update
    public int startingColorCount = 5;
    public float delayBetweenColors = .1f;
    public float colorLasting = 1f;
    public int roundsToSucceed = 3;

    public GameObject[] inactive;
    public GameObject[] active;

    private int currentAnswer;

    public int[] colors;

    private int currentColorCount;

    public bool answering = false;
    //llista de materials a canviar del simon

    void Start()
    {
        currentColorCount = startingColorCount;
        answering = false;
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            StartGame();
        }

        if (answering)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                CheckAnswer(0);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                CheckAnswer(1);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                CheckAnswer(2);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                CheckAnswer(3);
            }
        }
    }
#endif

    public override void StartGame()
    {
        //Acercar el item y poner al player en modo puzle

        //Conseguir el patron de colores
        colors = GetRandomNumers(currentColorCount);

        //Iniciar la primera secuencia
        StartCoroutine(PlayColorSequence());

        //Se suma hasta que este completado
        //Se repite en bucle
        //currentColorCount++;
    }

    public override void PuzzleWon()
    {
    }

    public override void EndGame()
    {
        colors = null;
        StopAllCoroutines();
    }

    public int[] GetRandomNumers(int numbersToGet)
    {
        int[] colors = new int[numbersToGet];
        for (int i = 0; i < numbersToGet; i++)
        {
            colors[i] = Random.Range(0, 4);
        }

        return colors;
    }


    public IEnumerator PlayColorSequence()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            //Play sound [i]

            //Change Material [i]
            active[colors[i]].SetActive(true);
            inactive[colors[i]].SetActive(false);

            yield return new WaitForSeconds(colorLasting);

            active[colors[i]].SetActive(false);
            inactive[colors[i]].SetActive(true);

            yield return new WaitForSeconds(delayBetweenColors);

            answering = true;
            currentAnswer = 0;
        }
    }

    public void CheckAnswer(int i)
    {
        if (i == colors[currentAnswer])
        {
            Debug.Log("Acertaste");
            currentAnswer++;
            if (currentAnswer >= colors.Length)
            {
                Debug.Log("Todo bien, todo correcto, y yo que me alegro.");
                //Next phase
                IncreasePhase();
            }
        }
        else
        {
            Debug.Log("Fallaste");
        }
    }

    private void IncreasePhase()
    {
        //Start coroutine de acertaste

        answering = false;
        currentColorCount++;
        if (currentColorCount - startingColorCount > roundsToSucceed)
        {
            PuzzleWon();
        }

        colors = GetRandomNumers(currentColorCount);
        StartCoroutine(PlayColorSequence());
    }
}