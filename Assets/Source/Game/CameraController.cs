using UnityEngine;

namespace Source.Game
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private LevelGrid grid;
        
        private void Start()
        {
            var (minX, maxX, minY, maxY) = grid.GetBounds();
            var cam = GetComponent<Camera>();
            
            var center = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, cam.transform.position.z);
            cam.transform.position = center;

            var width = maxX - minX;
            var height = maxY - minY;

            cam.orthographicSize = Mathf.Max(height / 2f, width / (2f * cam.aspect));
        }
        
    }
}