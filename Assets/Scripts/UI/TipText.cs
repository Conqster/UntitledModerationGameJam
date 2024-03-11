using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// This is added to the Tip Mask
public class TipText : MonoBehaviour
{
    public static TipText instance;

    [SerializeField] float speed = 0.1f;
    [SerializeField] string[] tips = new string[0];

    [SerializeField] RectTransform textObject;
    TextMeshProUGUI text;
    Vector3 leftPointOfMask;
    Vector3 leftPointOffset;

    Vector3 rightPointOfMask;

    Vector3 rightPointOfText;
    Vector3 rightPointOffset;

    Vector3 heightOffset;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogWarning("Too Many Tip Text");
            Destroy(gameObject);
        }

        text = GetComponentInChildren<TextMeshProUGUI>();

        Vector3[] corners = new Vector3[4];

        leftPointOffset = new Vector3(GetComponent<RectTransform>().rect.width / 2, 0f);
        leftPointOfMask = transform.position - leftPointOffset;
        rightPointOfMask = transform.position + leftPointOffset;

        textObject.GetWorldCorners(corners);
        rightPointOfText = new Vector3(text.preferredWidth * textObject.localScale.x / 2, 0);
        
        rightPointOfText = textObject.position + rightPointOffset;


        if(tips != null && tips.Length > 0 && tips[0] != null) 
        {
            text.text = tips[0];
        }
    }

    private IEnumerator Start()
    {
        float lerpVal = 0;
        bool offScreen = false;


        while(offScreen == false)
        {
            textObject.position -= Vector3.left * speed;
            rightPointOfText = textObject.position + rightPointOffset;
            if (rightPointOfText.x <= leftPointOfMask.x)
            {
                offScreen = true;
                
            }


            yield return new WaitForFixedUpdate();
        }

        NextTip();
        StartCoroutine(Start());
    }

    public static void AssignDate(string date)
    {
        if (instance.tips == null) instance.tips = new string[] { date };
        else if (instance.tips.Length == 0) instance.tips = new string[] { date };
        else
        {
            instance.tips[0] = date;
        }

        if(instance.text == null)
        {
            instance.text = instance.GetComponentInChildren<TextMeshProUGUI>();
        }

        instance.text.text = instance.tips[0];
    }

    void NextTip()
    {
        if (tips.Length <= 1) return;

        text.text = tips[Random.Range(0, tips.Length)];

        rightPointOffset = new Vector3(text.preferredWidth * textObject.localScale.x / 2, 0);
        textObject.position = rightPointOfMask + rightPointOffset;
    }
}
