using Unity.VisualScripting;
using UnityEngine;

namespace Source.UI.Map
{
    public class MapMarker : MonoBehaviour
    {
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1f, .5f, 0, 1f);
            Gizmos.DrawCube(transform.position,  Vector3.one * 0.4f);
            Gizmos.color = new Color(.9f, .45f, 0, 1f);
            Gizmos.DrawCube(transform.position,  Vector3.one * 0.35f);
            Gizmos.color = new Color(.8f, .4f, 0, 1f);
            Gizmos.DrawCube(transform.position,  Vector3.one * 0.3f);
            Gizmos.color = new Color(.7f, .35f, 0, 1f);
            Gizmos.DrawCube(transform.position,  Vector3.one * 0.25f);
        }
        
#endif
    }
}