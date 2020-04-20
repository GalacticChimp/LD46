using System;
using UnityEngine;
using UnityEngine.UI;

public class Machine : MonoBehaviour
{
    public static int numOfInstances = 0;
    public int Id;
    public string Name;
    public bool IsFixed { get; private set; }
    private SpriteRenderer spriteRenderer;
    public Sprite FixedSprite;
    public Sprite BrokenSprite;
    public int SecondsToFix;
    private float secondsTillFixed;
    public event Action<Machine> OnMachineFixed;
    public event Action OnMachineFixedSimple;
    private Slider repairProgressSlider;
    public ToolType ToolTypeRequiredToFix;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Id = numOfInstances++;
        repairProgressSlider = FindObjectOfType<Slider>();
        repairProgressSlider.value = 0;
    }

    private void Start()
    {
        repairProgressSlider.gameObject.SetActive(false);
    }

    public bool IsToolProperOne(ToolType toolType)
    {
        return toolType == ToolTypeRequiredToFix;
    }

    public void ProgressFix(ToolType tooltype)
    {
        if (IsFixed || !IsToolProperOne(tooltype))
        {
            return;
        }
        secondsTillFixed += Time.deltaTime;
        // Debug.Log($"Secs till fixed: {secondsTillFixed}");
        repairProgressSlider.gameObject.SetActive(true);
        repairProgressSlider.value = secondsTillFixed / SecondsToFix;
        if((int)secondsTillFixed >= SecondsToFix)
        {
            Fix();
        }
    }

    private void Fix()
    {
        IsFixed = true;
        spriteRenderer.sprite = FixedSprite;
        OnMachineFixed?.Invoke(this);
        OnMachineFixedSimple?.Invoke();
        repairProgressSlider.gameObject.SetActive(false);
    }

    public void Break()
    {
        IsFixed = false;
        spriteRenderer.sprite = BrokenSprite;
    }
}
