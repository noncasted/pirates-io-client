using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.VFX
{
    public class Floated : MonoBehaviour
    {
        [SerializeField] private List<Transform> _targets; 
        [SerializeField][Range(0f,1f)] private List<float> _floats;
        [SerializeField] private float _speed;
        [SerializeField] private float _forceY = 0.1F;
        [SerializeField] private Vector2 _angles;
        private void OnEnable() => StartCoroutine(Floating());
        IEnumerator Floating()
        { 
            List<Vector3> startPoses = new List<Vector3>();
            _floats = new List<float>();
            foreach (var target in _targets)
            {
                startPoses.Add(target.localPosition);
                _floats.Add(Random.value);
            }
            while (true)
            {
                for (int i = 0; i < _targets.Count; i++)
                {
                    _floats[i] += Time.deltaTime * _speed;
                    float t = Mathf.PI * 2f * _floats[i];
                    float cos = Mathf.Cos(t);
                    _targets[i].localPosition = startPoses[i] + Vector3.up * cos * _forceY;
                    float angle = Mathf.LerpAngle(_angles.x, _angles.y, (cos + 1) * 0.5f);
                    _targets[i].rotation = Quaternion.Euler(0, 0, angle);
                    yield return null;
                }
            }
        }
    }
}
