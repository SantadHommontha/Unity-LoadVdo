using UnityEngine;
using UnityEngine.Video;

public class LoadURL : MonoBehaviour
{
    [SerializeField] private LoadJson[] loadJsons;
    [SerializeField] private string[] vdoFloders;

[ContextMenu("PlayALL")]
    public void PlayAll()
    {
        for (int i = 0; i < loadJsons.Length; i++)
        {
            loadJsons[i].LoadAndPlayVideo(vdoFloders[i]);
        }
    }
  
}
