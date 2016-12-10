using UnityEngine;
using UnityEditor;

public class AssetCard
{
    [MenuItem("Assets/Create/NewCard")]
    public static void CreateAsset ()
    {
        ScriptableObjectUtility.CreateAsset<ScriptableCard> ();
    }
}
