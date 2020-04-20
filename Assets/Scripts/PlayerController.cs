using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject PlayerObject;
    private Rigidbody2D playerRB;
    [Range(0, 10)]
    public float PlayerAcceleration;
    private Tool pickedUpItem;
    private bool isHoldingItem => pickedUpItem != null;
    private GameController gameController;

    private ContactFilter2D itemContactFilter2D;
    private ContactFilter2D machineContactFilter2D;
    // Start is called before the first frame update
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playerRB = PlayerObject.GetComponent<Rigidbody2D>();
        itemContactFilter2D = new ContactFilter2D
        {
            useTriggers = true,
            layerMask = new LayerMask { value = 8 } // items layer 
        };
        machineContactFilter2D = new ContactFilter2D
        {
            useTriggers = true,
            layerMask = new LayerMask { value = 9 } // machines layer 
        };
    }

    // Update is called once per frame
    void Update()
    {
        // Block controls if game not running
        if(gameController.GameState != GameState.Running)
        {
            return;
        }

        // Item movement
        if (isHoldingItem)
        {
            pickedUpItem.transform.position = playerRB.transform.position;
        }

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            playerRB.AddForce(new Vector2(0, PlayerAcceleration));
        }
        else if (Input.GetKey(KeyCode.S))
        {
            playerRB.AddForce(new Vector2(0, -PlayerAcceleration));
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerRB.AddForce(new Vector2(-PlayerAcceleration, 0));
        }
        else if (Input.GetKey(KeyCode.D))
        {
            playerRB.AddForce(new Vector2(PlayerAcceleration, 0));
        }
        // Actions
        // Interact/Use item
        if (Input.GetMouseButton(0))
        {
            // Use Item
            if (isHoldingItem)
            {
                Collider2D[] colliders = new Collider2D[5];
                if (playerRB.OverlapCollider(machineContactFilter2D, colliders) > 0)
                {
                    var machine = colliders.FirstOrDefault(x => x.TryGetComponent<Machine>(out var temp)).GetComponent<Machine>();
                    if (machine != null)
                    {
                        machine.ProgressFix(pickedUpItem.ToolType);
                    }
                }
                return;
            }
            // TODO Interact

            
        }
        // Grab/drop items
        else if (Input.GetMouseButtonDown(1))
        {
            if (isHoldingItem)
            {
                DropItem();
                return;
            }

            Collider2D[] colliders = new Collider2D[5];
            if (playerRB.OverlapCollider(itemContactFilter2D, colliders) > 0)
            {
                var wrench = colliders.FirstOrDefault(x => x.TryGetComponent<Tool>(out var temp)).GetComponent<Tool>();
                if (wrench != null)
                {
                    Pickup(wrench);
                }
            }
        }

    }

    public void Pickup(Tool item)
    {
        pickedUpItem = item;
        pickedUpItem.transform.position = playerRB.position;
    }

    /// <summary>
    /// TODO maybe make it throw instead of drop?
    /// </summary>
    public void DropItem()
    {
        pickedUpItem = null;
    }
}
