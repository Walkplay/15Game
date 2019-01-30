using UnityEngine;

public class SceneController : MonoBehaviour
{
    [Header("Menu panels")]
    public GameObject gameOverPan;
    public GameObject startPan;
    [Header("Prefabs")]
    public Material[] colors; 
    public GameObject cubePrefab;
    [Tooltip("Offset beetwen cubes")]
    [Range(0,50)]
    public float offset = 0f;
    [Range(0,4)]
    public int tableSize;
    

    GameObject emptyCube;
    GameObject[,] cubeInstPool;
    Camera mainCam;


    void Awake()
    {
        int cubeNum;

        cubeInstPool = new GameObject[tableSize,tableSize];
        

        for (int i = 0; i < tableSize; i++)
        {
            for (int j = 0; j < tableSize; j++)
            {
               
                cubeNum = i * tableSize + j; //Set cube game number
                cubeInstPool[i,j] = Instantiate(cubePrefab, transform, false);

                if(cubeNum < tableSize*tableSize -1)
                    cubeInstPool[i, j].GetComponent<Renderer>().material = colors[cubeNum];
                if (i == 0 && j == 0) continue;
                if (i == 0)
                    cubeInstPool[i, j].transform.position = new Vector3(cubeInstPool[i, j - 1].transform.position.x + cubeInstPool[i,j].transform.localScale.x + offset, cubeInstPool[i, j -1].transform.position.y, cubeInstPool[i, j - 1].transform.position.z);
                else
                    cubeInstPool[i, j].transform.position = new Vector3(cubeInstPool[i - 1, j].transform.position.x, cubeInstPool[i - 1, j].transform.position.y, cubeInstPool[i - 1,j].transform.position.z - cubeInstPool[i,j].transform.localScale.z - offset);

            }
        }
        //Debug.Log(cubeInstPool[0,0].GetComponent<Renderer>().material.color);
        emptyCube = cubeInstPool[tableSize-1, tableSize-1];
        
    }

    private void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        emptyCube.SetActive(false); //Empty cube
        gameOverPan.SetActive(false);
        startPan.SetActive(true);
    }

    

    private void Update()
    {
        

        if (Input.GetMouseButtonDown(0) && !startPan.activeInHierarchy)// Get GameObject we clicked on
        {
            RaycastHit hit;
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out  hit))
            {
                //Debug.Log("You hit:" + hit.transform.localPosition);
                //Debug.Log("Space Pos is: " + spaceCube.transform.localPosition);
                

                if ( (hit.transform.localPosition - emptyCube.transform.localPosition).sqrMagnitude == Mathf.Pow( hit.transform.localScale.x + offset, 2)) //Check if hited cube have space to move
                {
                    SwapPosition(hit.transform.gameObject, emptyCube);
                    if (CheckForWin()) // if win the game show game over panels
                    {
                        gameOverPan.SetActive(true);
                        startPan.SetActive(true);

                    }
                }
                
            }
        }
    }

    bool CheckForWin()
    {
        int count = 0;
        for (int i = 0; i < tableSize; i++)
        {
            for (int j = 0; j < tableSize; j++)
            {
                if (cubeInstPool[i, j].GetComponent<Cube>().OnRightPlace())
                    count++;
            }
        }
        //Debug.Log("Cubes on right place: " + count);
        return count == 15;
    }

    void RandomGameGen()
    {
        int ranX, ranY;
        for (int i = 0; i < tableSize; i++)
        {
            for (int j = 0; j < tableSize; j++)
            {
                
                ranX = Random.Range(0, tableSize);
                ranY = Random.Range(0, tableSize);
                
                SwapPosition( cubeInstPool[i, j],cubeInstPool[ranX, ranY]);
                //Debug.Log("Random swap!");

            }
        }
    }

    void SwapPosition  (GameObject obj1, GameObject obj2)
    {
        Vector3 tmp = obj1.transform.position;
        obj1.transform.position = obj2.transform.position;
        obj2.transform.position = tmp;
    }

    public void PlayButton()
    {
        
        //SwapPosition(cubeInstPool[3, 2], cubeInstPool[3, 3]); //Debug mode;
        
        RandomGameGen();

        startPan.SetActive(false);
        if (gameOverPan.activeInHierarchy)
            gameOverPan.SetActive(false);
    }
    
}
