using UnityEngine;

/// <summary>
/// Этот сприпт вешается на игрока и позваляет ему испускать импульс при взаимодействии с триггером модуля
/// </summary>
[HelpURL("https://docs.google.com/document/d/13twCVY4ZIH703ITRmfQk5E3lZHeE70QPFVjIvLkaRBE/edit?usp=sharing")]
public class LogicModuleReactor : MonoBehaviour
{
    public static string interactiveTag = "Interactable";

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals(interactiveTag))
        {
            if(other.TryGetComponent(out InteractiveArea loc))
            {
                loc.ActivateModule();
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals(interactiveTag))
        {
            if (other.TryGetComponent(out InteractiveArea loc))
            {
                if (!loc.enterOnly)
                {
                    loc.ActivateModule();
                }
            }
        }
    }
}
