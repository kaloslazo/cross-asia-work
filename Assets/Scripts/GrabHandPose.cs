using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class GrabHandPose : MonoBehaviour
{
    public HandData rightHandPosePrefab;  // Prefab de la mano personalizada para cuando se agarra el arma
    public GameObject rightController; // Controlador de la mano derecha en el XR Rig

    private HandData instantiatedHandPose; // Instancia de la mano personalizada

    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);

        if (!rightHandPosePrefab || !rightController)
            Debug.LogError("Right hand pose prefab or controller is not assigned in the Inspector!");
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        Debug.Log("SetupPose: Pose setup started.");
        rightController.SetActive(false);
        HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
        if (handData != null)
        {
            handData.animator.enabled = false;

            // Instancia la mano personalizada si aún no existe
            if (instantiatedHandPose == null)
            {
                instantiatedHandPose = Instantiate(rightHandPosePrefab, handData.transform);
                instantiatedHandPose.transform.localPosition = Vector3.zero;
                instantiatedHandPose.transform.localRotation = Quaternion.identity;
            }

            // Activa la mano personalizada y oculta el controlador
            instantiatedHandPose.gameObject.SetActive(true);
            rightController.SetActive(false);
            Debug.Log("Right controller has been hidden.");
        }
        else
        {
            Debug.LogError("SetupPose: HandData component not found on the interactor.");
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        Debug.Log("UnSetPose: Pose unset started.");
        rightController.SetActive(true);
        HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
        if (handData != null)
        {
            handData.animator.enabled = true;

            // Oculta la mano personalizada y muestra el controlador original
            if (instantiatedHandPose != null)
            {
                instantiatedHandPose.gameObject.SetActive(false);
            }
            rightController.SetActive(true);
            Debug.Log("Right controller has been shown.");
        }
        else
        {
            Debug.LogError("UnSetPose: HandData component not found on the interactor.");
            // Asegura que el controlador se muestre incluso si hay un error
        }
    }
}
