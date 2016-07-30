using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
    [MenuItem("Assets/Create/NewCard")]
    public static void CreateAsset ()
    {
        ScriptableObjectUtility.CreateAsset<CardHandler> ();
    }
}
