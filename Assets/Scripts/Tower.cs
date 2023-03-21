using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Stack<GameObject> Stack { get; private set; }
    void Start()
    {
        Stack = new Stack<GameObject>();
    }
    /// <summary>
    /// BottommostPosition holds a transform with the lowest possible position for a disk.
    /// The largest disk will start here at the start of the game.
    /// </summary>
    public Transform BottommostPosition;


    //-------------------- Click detection --------------------
    /// <summary>
    /// _towerNumber is needed by GameManager to acknowledge which of the three towers is selected.
    /// This value is unique for each tower.
    /// </summary>
    [SerializeField] private int _towerNumber = 0;

    [SerializeField] private Tower[] _towersArray;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Stack clicked: " + this.name);

            // Case: game is waiting for another Stack/Tower to be clicked
            if (GameManager.DiskSelected != -1)
            {
                // case when waiting for another tower to be selected and this tower gets selected
                GameManager.MoveDisk(_towersArray[GameManager.DiskSelected - 1].Stack.Peek(), this, _towersArray);
            }
            // Case: No Stack/Tower is currently selected (DiskSelected == -1)
            else
            {
                // Emptiness check (not null because Stack/Tower is already intialized but may be empty)
                if (Stack.Count > 0)
                {
                    Stack.Peek().GetComponent<MeshRenderer>().material.color = Color.white;
                    GameManager.DiskSelected = _towerNumber;
                }
                else
                {
                    Debug.Log("Cannot select a disk, tower is empty.");
                }
            }

            
        }
    }
}
