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
        StartCoroutine(PlayColorSequence(true));

        //Se suma hasta que este completado
        //Se repite en bucle
        //currentColorCount++;
    }

    public override void PuzzleWon()
    {
        OnPuzzleWin.Invoke();
        EndGame();
    }

    public override void EndGame()
    {
        currentColorCount = startingColorCount;
        StopAllCoroutines();
        ResetColors();
        this.GetComponentInParent<InteractablePuzzle>().EndInteractActions();
    }

    private void ResetColors()
    {
        colors = null;
        for(int i = 0; i<active.Length; i++)
        {
            active[i].SetActive(false);
            inactive[i].SetActive(true);
        }
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


    public IEnumerator PlayColorSequence(bool first = false)
    {
        if (first)
        {
            yield return new WaitForSeconds(1.5f);
        }


        for (int i = 0; i < colors.Length; i++)
        {
            //Play sound [i]

            //Change Material [i]
            StartCoroutine(HighlightColor(colors[i]));
            yield return new WaitForSeconds(colorLasting);

            yield return new WaitForSeconds(delayBetweenColors);

            answering = true;
            currentAnswer = 0;
        }
    }

    public IEnumerator HighlightColor(int i, bool answering = false)
    {
        
        active[i].SetActive(true);
        inactive[i].SetActive(false);

        if (!answering)
        {
            AudioManager.PlaySound2D("Sound/Simon/Simon" + i.ToString());
            yield return new WaitForSeconds(colorLasting);
        }
        else
        {
            yield return new WaitForSeconds(colorLasting / 2);
        }

        active[i].SetActive(false);
        inactive[i].SetActive(true);
    }


    public void CheckAnswer(int i)
    {
        if (answering)
        {
            StartCoroutine(HighlightColor(i, true));

            if (i == colors[currentAnswer])
            {
                AudioManager.PlaySound2D("Sound/Simon/Simon" + i.ToString());
                Debug.Log("Acertaste");
                currentAnswer++;
                if (currentAnswer >= colors.Length)
                {
                    answering = false;
                    Debug.Log("Todo bien, todo correcto, y yo que me alegro.");
                    if (currentColorCount - startingColorCount >= roundsToSucceed)
                    {
                        PuzzleWon();
                    }
                    else
                        StartCoroutine(IncreasePhase());
                }
            }
            else
            {
                Debug.Log("Fallaste");
                AudioManager.PlaySound2D("Sound/Simon/Fail");
                EndGame();
            }
        }
    }

    private IEnumerator IncreasePhase()
    {
        //Start coroutine de acertaste
        yield return new WaitForSeconds(2);
        answering = false;
        currentColorCount++;
        colors = GetRandomNumers(currentColorCount);
        StartCoroutine(PlayColorSequence());
    }
}