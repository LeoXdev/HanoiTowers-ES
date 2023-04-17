using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// GameManager takes care of multiple game operations: <br></br>
/// - Generation of n disks at the start of the game. <br></br>
/// - Checking game rules (only one disk can be moved at the time, no larger disk may be placed on top of a smaller one, 
///                        a movement consist of moving only the disk at the top of any stack) <br></br>
/// - Checking win condition (all the disks are at the final tower).
///
/// !TODO: Line 102, This method ended working but now to fields reference the same collection: Tower._towersArray, GameManager.TowersArray
/// !TODO: Line 121, when player chooses the same tower as destination for a moved disk, disk moves upwards bi a little amount
/// !TODO: A MissingReferenceException appears sometimes when replaying the game after a victory.
/// </summary>
public class GameManager : MonoBehaviour
{
    //-------------------- Game start --------------------
    [SerializeField] private Tower _firstTower,
                                   _midTower,
                                   _lastTower;
    private Tower[] TowersArray;
    void Start()
    {
        TowersArray = new Tower[]{
            _firstTower,
            _midTower,
            _lastTower
        };
        DiskSelected = -1;

        //VictoryText.SetActive(false);
    }
    /// <summary>
    /// VictoryText holds a gameobject that'll display a victory message at the moment you win.
    /// </summary>
    //[SerializeField] private GameObject VictoryText;
    //[SerializeField] private GameObject MenuBG;
    void Update()
    {
        Debug.Log("Disk Selected = " + DiskSelected);
        // If all the disks (specified at the start) are in the final tower
        if (_lastTower.GetComponent<Tower>().Stack.Count == NumberOfDisks &&
            _lastTower.GetComponent<Tower>().Stack.Count > 0)
        {
            //MenuBG.SetActive(true);
            //VictoryText.SetActive(true);
            //GameReset();

            _particleSystem.gameObject.SetActive(true);
        }
    }
    // Victory VFX
    [SerializeField] ParticleSystem _particleSystem;

    /// <summary>
    /// NumberOfDisks holds the number of disks the game will have, the modification while the game's already running won't
    /// have any effect until a new game is started.
    /// </summary>
    public static int NumberOfDisks;
    [SerializeField] private InputField _Input_DisksNumber;

    /// <summary>
    /// _diskPrefab holds the prefab to be instantiated.
    /// </summary>
    [SerializeField] private GameObject _diskPrefab;
    /// <summary>
    /// CreateDisks creates the indicated number of disks (refer to NumberOfDisks). <br></br>
    /// </summary>
    public void CreateDisks()
    {
        // Input validation
        NumberOfDisks = _validateIntInput(_Input_DisksNumber);
        if (NumberOfDisks == -1)
        {
            Debug.Log("Invalid input, a number is required.");
            return;
        }

        // Instantiate the indicated number of disks at the corresponding positions
        for (int i = 0; i < NumberOfDisks; i++)
        {
            // A temporal variable is hold each iteration to change each prefab's scale more and more.
            var disk = Instantiate(
                _diskPrefab,
                _firstTower.BottommostPosition.position + new Vector3(0f, (i * 0.2f), 0f),
                _firstTower.BottommostPosition.rotation
                );

            //disk.transform.localScale = disk.transform.localScale + new Vector3((i * 0.2f), 0f, (i * 0.2f));
            disk.transform.localScale = disk.transform.localScale + new Vector3(
                NumberOfDisks - i,
                0f,
                NumberOfDisks - i
                );
            disk.GetComponent<Disk>().Value = NumberOfDisks - i;
            disk.transform.parent = _firstTower.transform;

            _firstTower.Stack.Push(disk);
        }

    }
    //-------------------- Game going --------------------
    /// <summary>
    /// MoveDisk is called by a Tower object with the corresponding parameters to help the GameManager move a disk.
    /// !TODO: This method ended working but now to fields reference the same collection: Tower._towersArray, GameManager.TowersArray
    /// </summary>
    /// <param name="diskToMove"></param>
    /// <param name="receivingTower"></param>
    /// <param name="towersArray"></param>
    public static void MoveDisk(GameObject diskToMove, Tower receivingTower, Tower[] towersArray)
    {
        // Case: the receiving tower doesn't have any disks
        if (receivingTower.Stack.Count == 0)
            diskToMove.transform.position = receivingTower.BottommostPosition.position;
        // Case: the receiving tower already has disks
        else
        {
            // Case: player wants to move a larger disk above a smaller one
            if (diskToMove.GetComponent<Disk>().Value > receivingTower.Stack.Peek().GetComponent<Disk>().Value)
            {
                Debug.Log("Cannot move a larger disk abover a smaller disk.");
                return;
            }
            // Case: player wants to move a disk into the same tower, still doesn't works
            if (diskToMove.GetComponent<Disk>().Value == receivingTower.Stack.Peek().GetComponent<Disk>().Value)
            {
                Debug.Log("Player tried to move a disk into the same tower, try again.");
                DiskSelected = -1;
                diskToMove.GetComponent<MeshRenderer>().material.color = Color.magenta;
                return;
            }
            // Case: player moves the disk into a different tower
            diskToMove.transform.position = receivingTower.Stack.Peek().transform.position + new Vector3(0.0f, 0.2f, 0.0f);
        }

        // We add to the top the selected disk while removing it from the previous Stack at the same time thanks to Pop()
        receivingTower.Stack.Push(towersArray[GameManager.DiskSelected - 1].Stack.Pop());
        diskToMove.transform.parent = receivingTower.transform;
        DiskSelected = -1;

        diskToMove.GetComponent<MeshRenderer>().material.color = Color.magenta;
        //_firstTower.BottommostPosition.position + new Vector3(0f, (i * 0.2f), 0f),
    }
    /// <summary>
    /// DiskSelected indicates whether a tower was already selected and the game is waiting for another tower to be clicked,
    /// proceeding to move the previously selected disk into the new tower. <br></br>
    /// 1 -> First Tower is selected <br></br>
    /// 2 -> Mid Tower is selected <br></br>
    /// 3 -> Last Tower is selected <br></br>
    /// -1 -> No tower is selected <br></br>
    /// </summary>
    public static int DiskSelected;
    //-------------------- Game reset and exit --------------------
    // Loading the scene again will reset the game state
    public void GameReset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    //----------------------- Helper methods -----------------------
    /// <summary>
    /// _validateIntInput indicates if a given InputField contains a valid int value (only numbers and not null).
    /// </summary>
    /// <param name="input">The input field to validate</param>
    /// <returns>The validated numeric value inside the field or -1 in case there's a bad input.</returns>
    private int _validateIntInput(InputField input)
    {
        int inputValue;
        try
        {
            inputValue = Int32.Parse(input.text);
        }
        catch (Exception e)
        {
            inputValue = -1;
        }

        return inputValue;
    }
}
