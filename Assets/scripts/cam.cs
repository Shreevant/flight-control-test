using UnityEngine;
using System.IO;

public class ThirdPersonPlaneCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Views")]
    public Vector3[] offsets;
    public bool[] isFirstPersonView;   // Mark which views are FPP
    private int currentViewIndex = 0;

    public float followSpeed = 8f;
    public float rotationSpeed = 6f;

    public float speedZoomOutMultiplier = 0.08f;
    public float maxZoomOut = 20f;

    private Rigidbody targetRb;


    void Start()
    {
        if (target != null)
            targetRb = target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            currentViewIndex++;
            if (currentViewIndex >= offsets.Length)
                currentViewIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CaptureScreenshot();
        }
    }

    void LateUpdate()
    {
        if (target == null || offsets.Length == 0 ||
            isFirstPersonView.Length != offsets.Length)
            return;

        Vector3 currentOffset = offsets[currentViewIndex];

        if (isFirstPersonView[currentViewIndex])
        {
            // FIRST PERSON — NO SMOOTHING

            transform.position = target.TransformPoint(currentOffset);
            transform.rotation = target.rotation;

            return; // IMPORTANT: stop here
        }

        // THIRD PERSON BELOW

        float speed = 0f;
        if (targetRb != null)
            speed = targetRb.velocity.magnitude;

        float dynamicZoom = Mathf.Clamp(speed * speedZoomOutMultiplier, 0, maxZoomOut);

        Vector3 desiredPosition = target.TransformPoint(
            currentOffset - new Vector3(0, 0, dynamicZoom)
        );

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPosition,
            followSpeed * Time.deltaTime
        );

        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRotation,
            rotationSpeed * Time.deltaTime
        );
    }

    public void CaptureScreenshot()
    {
        Camera cam = GetComponent<Camera>();

        int width = Screen.width;
        int height = Screen.height;

        RenderTexture rt = new RenderTexture(width, height, 24);
        cam.targetTexture = rt;

        Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);

        cam.Render();

        RenderTexture.active = rt;
        screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        screenshot.Apply();

        cam.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);

        byte[] bytes = screenshot.EncodeToPNG();

        string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
        path = Path.Combine(path, "screenshot.png");

        File.WriteAllBytes(path, bytes);

        Debug.Log("Screenshot saved to: " + path);
    }
}