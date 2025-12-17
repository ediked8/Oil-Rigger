using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComsumePrefab : MonoBehaviour
{
    TextMeshProUGUI buttenText;
    Button button;
    public CrudeOilScript data;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        buttenText = GetComponentInChildren<TextMeshProUGUI>();
        button = GetComponent<Button>();
        buttenText.text = $"{data.crudeOilData.oil_name} : {data.crudeOilData.oil_cost}";
        button.onClick.AddListener(() => GameManager.Instance.storageManager.AddComsumeItem(data));


    }
 
}
