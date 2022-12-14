using UnityEngine;

namespace Common.VFX.Textures
{
    public class WawesLookRotrator : MonoBehaviour
    {
        private Vector3 currentPos;
        private Vector3 lastPos;

        private void Update()
        {
            currentPos = transform.position;
            transform.right = currentPos - lastPos;
            lastPos = Vector3.Lerp(lastPos, currentPos, 15 * Time.deltaTime);
        }
    }
}