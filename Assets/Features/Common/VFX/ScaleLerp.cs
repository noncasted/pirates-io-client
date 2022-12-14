using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Common.VFX
{
    public class ScaleLerp : MonoBehaviour
    {
        public List<Transform> scales;
        public bool doIt;
        public bool usePos;

        private void OnValidate()
        {
            if (doIt)
            {
                doIt = false;
                scales = scales.OrderBy(i => i.position.x).ToList();
                for (var i = 0; i < scales.Count; i++)
                    scales[i].localScale = Vector3.Lerp(scales[0].localScale, scales[scales.Count - 1].localScale,
                        i / (float)(scales.Count - 1));

                if (usePos)
                    for (var i = 0; i < scales.Count; i++)
                        scales[i].localPosition = Vector3.Lerp(scales[0].localPosition,
                            scales[scales.Count - 1].localPosition, i / (float)(scales.Count - 1));
            }
        }
    }
}