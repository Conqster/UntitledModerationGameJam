using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject currentActiveObject;

    public void ChangeObject(GameObject objectToActivate)
    {
        currentActiveObject.SetActive(false);

        currentActiveObject = objectToActivate;

        currentActiveObject.SetActive(true);
    }
}
