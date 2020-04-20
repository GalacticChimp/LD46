using UnityEngine;

public enum DoorState
{
    Unlocked,
    Locked,
    Inactive,
    Open
}

public class Door : MonoBehaviour
{
    public DoorState DoorState;
    public Sprite DoorOpen_Sprite;
    public Sprite DoorLocked_Sprite;
    public Sprite DoorUnlocked_Sprite;
    public Sprite DoorInactive_Sprite;
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    public Machine ActivateOnThisMachineFixed;
    public Machine UnlockOnThisMachineFixed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if(ActivateOnThisMachineFixed != null)
        {
            ActivateOnThisMachineFixed.OnMachineFixedSimple += Activate;
        }
        if(UnlockOnThisMachineFixed != null)
        {
            UnlockOnThisMachineFixed.OnMachineFixedSimple += Unlock;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(DoorState != DoorState.Unlocked)
        {
            return;
        }

        if (!collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
        {
            // Only open for Player
            return;
        }

        Open();
    }

    /// <summary>
    /// TODO This doesn't work because the collider needs to be active. Need to find a workaround.
    /// </summary>
    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (!collision.gameObject.TryGetComponent<PlayerController>(out var playerController))
    //    {
    //        // Only close for Player
    //        return;
    //    }
        
    //    Close();
    //}

    private void Open()
    {
        DoorState = DoorState.Open;
        spriteRenderer.sprite = DoorOpen_Sprite;
        boxCollider.isTrigger = true;
    }

    private void Close()
    {
        DoorState = DoorState.Unlocked;
        spriteRenderer.sprite = DoorUnlocked_Sprite;
        boxCollider.isTrigger = false;
    }

    private void Activate() 
    {
        if(UnlockOnThisMachineFixed != null)
        {
            DoorState = DoorState.Locked;
            spriteRenderer.sprite = DoorLocked_Sprite;
        }
        else
        {
            DoorState = DoorState.Unlocked;
            spriteRenderer.sprite = DoorUnlocked_Sprite;
        }

        boxCollider.isTrigger = false;
    }

    private void Unlock()
    {
        DoorState = DoorState.Unlocked;
        spriteRenderer.sprite = DoorUnlocked_Sprite;
        boxCollider.isTrigger = false;
    }
}
