using UnityEngine;

public class LoopRotate : MonoBehaviour
{
    [SerializeField] private Vector3 axis = Vector3.up;
    [SerializeField] private float speed = 30f;
    private bool active;
    public void SetActive(bool v) => active = v;
    void Update(){ if (active) transform.Rotate(axis, speed * Time.deltaTime, Space.Self); }
}
