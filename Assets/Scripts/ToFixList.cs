using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ToFixList : MonoBehaviour
{
    public List<Machine> machines { get; private set; }
    public GameObject ToFixListEntryPrefab;
    public GameObject ContentPanel;
    private List<TextMeshProUGUI> machineTexts;
    public event Action MachineWasFixed;
    // Start is called before the first frame update
    void Start()
    {
        machineTexts = new List<TextMeshProUGUI>();
        machines = FindObjectsOfType<Machine>().OrderBy(x => x.Id).ToList();
        machines.ForEach(x => {
            x.OnMachineFixed += MachineFixed;
            var text = Instantiate(ToFixListEntryPrefab, ContentPanel.transform).GetComponent<TextMeshProUGUI>();
            text.text = $"Fix {x.Name ?? "Machine"}";
            machineTexts.Add(text);
        });
    }

    private void MachineFixed(Machine machine)
    {
        machineTexts[machine.Id].fontStyle = FontStyles.Strikethrough;
        MachineWasFixed?.Invoke();
    }
}
