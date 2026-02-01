using UnityEngine;

public class ObjectInteractionTrigger : MonoBehaviour
{
    public CameraFocusOnObject focusScript;
    public PollockPainter paintScript;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"[Trigger ENTER] con: {other.name}, tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Trigger] Jugador ha entrado en el rango, activando focus y pintura");

            if (focusScript != null)
            {
                focusScript.StartFocus();
            }
            else
            {
                Debug.LogWarning("[Trigger] focusScript NO asignado");
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log($"[Trigger EXIT] con: {other.name}, tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("[Trigger] Jugador ha salido del rango, desactivando focus y pintura");

            if (focusScript != null)
            {
                focusScript.StopFocus();
            }
        }
    }
}