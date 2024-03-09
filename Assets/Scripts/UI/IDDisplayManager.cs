using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IDDisplayManager : MonoBehaviour
{
    public static IDDisplayManager Instance { get; private set; }
    public Image idImage;
    //public TextMeshProUGUI nameText; 
    public TextMeshProUGUI expiryDateText; 
    public TextMeshProUGUI dateOfBirthText;

    

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SetIDInfo(Sprite imageSprite, string expiryDate, string dateOfBirth)
    {
        idImage.sprite = imageSprite; 
        expiryDateText.text = "EXP: " + expiryDate;
        dateOfBirthText.text = "DOB: " + dateOfBirth;
    }

    public void ClearIDInfo()
    {
        idImage.sprite = null;
        //nameText.text = "";
        expiryDateText.text = "";
        dateOfBirthText.text = "";
    }
}
