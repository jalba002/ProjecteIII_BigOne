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

    public Material[] materials;
    public string[] paths;

    public Renderer pantalla;
    private Material _initMat;

    public Renderer bulb;
    public Material bulbOn;
    private Material _initBulbMat;

    public Animator computerAnim;

    

    private int currentAnswer;

    public int[] colors;

    private int _currentColorCount;    

    public bool answering = false;
    //llista de materials a canviar del simon

    void Start()
    {
        _currentColorCount = startingColorCount;
        answering = false;
        _initMat = pantalla.material;
        _initBulbMat = bulb.material;
        
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
        colors = GetRandomNumers(_currentColorCount);

        //Iniciar la primera secuencia
        StartCoroutine(PlayColorSequence(true));

        //Se suma hasta que este completado
        //Se repite en bucle
        //currentColorCount++;
    }

    public override void PuzzleWon()
    {
        OnPuzzleWin.Invoke();
        computerAnim.SetTrigger("Open");
        //GameManager.Instance.CanvasController.ShowHint("You won!!", true);
        EndGame();
    }

    public override void EndGame()
    {
        _currentColorCount = startingColorCount;
        StopAllCoroutines();
        ResetColors();
        this.GetComponentInParent<InteractablePuzzle>().EndInteractActions();
    }

    private void ResetColors()
    {
        colors = null;
        pantalla.material = _initMat;
        bulb.material = _initBulbMat;
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

            if(i == colors.Length - 1)
            {
                answering = true;
                bulb.material = bulbOn;
                currentAnswer = 0;
            }            
        }
    }

    public IEnumerator HighlightColor(int i, bool answering = false)
    {
        Debug.Log("Doing this");
        pantalla.material = materials[i];
        pantalla.material.color = Color.white;
        SoundManager.Instance.PlayEvent(paths[i], transform, 1);
        if (!answering)
        {            
            yield return new WaitForSeconds(colorLasting);
        }
        else
        {
            yield return new WaitForSeconds(colorLasting / 2);
        }
        
        pantalla.material = _initMat;
    }


    public void CheckAnswer(int i)
    {
        if (answering)
        {
            StartCoroutine(HighlightColor(i, true));

            if (i == colors[currentAnswer])
            {
                
                Debug.Log("Acertaste");
                currentAnswer++;
                if (currentAnswer >= colors.Length)
                {
                    answering = false;
                    bulb.material = _initBulbMat;
                    Debug.Log("Todo bien, todo correcto, y yo que me alegro.");
                    if (_currentColorCount - startingColorCount >= roundsToSucceed)
                    {
                        StartCoroutine(WaitAndPuzzleWon());
                    }
                    else
                        StartCoroutine(IncreasePhase());
                }
            }
            else
            {
                Debug.Log("Fallaste");

                GameManager.Instance.CanvasController.ShowHint("YOU MISSED", false);

                AudioManager.PlaySound2D("Sound/Simon/Fail");
                EndGame();
            }
        }
    }

    public IEnumerator WaitAndPuzzleWon()
    {
        yield return new WaitForSeconds(0.5f);
        PuzzleWon();
    }

    private IEnumerator IncreasePhase()
    {
        //Start coroutine de acertaste
        yield return new WaitForSeconds(2);
        bulb.material = _initBulbMat;
        answering = false;
        _currentColorCount++;
        colors = GetRandomNumers(_currentColorCount);
        StartCoroutine(PlayColorSequence());
    }
}