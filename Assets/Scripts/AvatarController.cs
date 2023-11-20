using UnityEngine;
[System.Serializable]
public class MapTransform
{
    public Transform vrTarget;
    public Transform ikTarget;
    public Vector3 trackingPositionOffset;
    public Vector3 trackingRotationOffset;
    public void VRMapping()
    {
        //ikTarget.position = vrTarget.TransformPoint(trackingPositionOffset);
        //ikTarget.rotation = vrTarget.rotation * Quaternion.Euler(trackingRotationOffset);
        ikTarget.SetPositionAndRotation(vrTarget.TransformPoint(trackingPositionOffset), vrTarget.rotation * Quaternion.Euler(trackingRotationOffset));
    }
}
public class AvatarController : MonoBehaviour
{
    [SerializeField] private MapTransform head;
    [SerializeField] private MapTransform leftHand;
    [SerializeField] private MapTransform rightHand;

    [SerializeField] private float turnSmoothness;
    [SerializeField] Transform ikHead;
    [SerializeField] Vector3 headBodyoffset;


    private void LateUpdate()
    {
        transform.position = ikHead.position + headBodyoffset;
        transform.forward = Vector3.Lerp(transform.forward, Vector3.ProjectOnPlane(ikHead.forward, Vector3.up).normalized, Time.deltaTime * turnSmoothness);

        head.VRMapping();
        leftHand.VRMapping();
        rightHand.VRMapping();

    }
}
    