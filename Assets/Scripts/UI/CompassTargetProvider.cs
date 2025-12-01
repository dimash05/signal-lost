using System.Linq;
using UnityEngine;

public class CompassTargetProvider : MonoBehaviour
{
    [SerializeField] private FinalConsole finalConsole;

    public Transform CurrentTarget
    {
        get
        {
            var om = ObjectiveManager.Instance;
            if (!om) return null;

            if (om.AllTerminalsActive)
            {
                if (!finalConsole) finalConsole = FindObjectOfType<FinalConsole>(true);
                return finalConsole ? finalConsole.transform : null;
            }

            var player = Camera.main ? Camera.main.transform : transform;
            var nearest = om.Terminals
                .Where(t => t && !t.IsActivated)
                .OrderBy(t => Vector3.SqrMagnitude(t.transform.position - player.position))
                .FirstOrDefault();

            return nearest ? nearest.transform : null;
        }
    }
}
