using UnityEngine;

public enum SurfaceType { Sand, Rock, Metal }

public class SurfaceTag : MonoBehaviour
{
    public SurfaceType type = SurfaceType.Sand;
}
