using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourTool : MonoBehaviour
{
    private static MonoBehaviourTool instance;

    [SerializeField] private GameObject emptyUIObjectPrefab;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject CreateEmptyUIObject(RectTransform parent, string name = "EmptyUIObject")
    {
        GameObject gameObject = Instantiate(instance.emptyUIObjectPrefab, parent, false);
        gameObject.name = name;

        return gameObject;
    }
}
