using UnityEngine;

public class MoveAssistantPanelScript : MonoBehaviour
{

   [SerializeField] private GameObject attachedObj;
    private Vector3 attatchedObjPos;
    private Vector3 offset;

    void Start()
    {
        offset = this.transform.position - attachedObj.transform.position;
    }
    void LateUpdate()
    {
        AdjustPanelPosition();
    }
    public void AdjustPanelPosition()
    {
        attatchedObjPos = attachedObj.transform.position;
        this.transform.position = attatchedObjPos + offset;
    }
}
