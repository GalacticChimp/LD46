using UnityEngine;

public enum ToolType
{
    Wrench,
    MultiTool,
    Calibrator
}

public class Tool : MonoBehaviour
{
    public ToolType ToolType;
    public string Name;
}
