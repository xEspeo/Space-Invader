namespace OpenCvSharp.Demo
{
    using UnityEngine;
    using OpenCvSharp;
    using System;
    using UnityEngine.XR.ARFoundation;
    using global::Unity.Collections;
    using global::Unity.Collections.LowLevel.Unsafe;


    public class LaserDetection : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        Point maxLoc;

        private ARCameraBackground arCameraBackground;
        ARCameraManager arCameraManager;

        [SerializeField] private RenderTexture renderTexture;
        private Texture2D tex;

        void Awake()
        {
            arCameraBackground = GetComponent<ARCameraBackground>();
            arCameraManager = GetComponent<ARCameraManager>();
        }

        // Update is called once per frame
        void Update()
        {
            getTexture2();
            //getTexture();
            //laserDetection();
            Ray ray = new Ray(cam.transform.position, new Vector3(maxLoc.X, maxLoc.Y));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                int objLayer = hit.collider.gameObject.layer;
                if (objLayer == 7) // Interactables
                {
                    hit.collider.GetComponent<Interactable>().BaseInteract();
                }
            }
        }

        private void laserDetection()
        {
            Mat src = Cv2.ImDecode(tex.EncodeToJPG(), ImreadModes.Color);
            Mat hsv = new Mat();
            //Cv2.CvtColor(src, hsv, ColorConversionCodes.RGBA2BGR);
            Cv2.CvtColor(hsv, hsv, ColorConversionCodes.BGR2HSV);

            print(tex.GetPixel(0, 0));
            print(tex.GetPixel(100, 100));

            var scalar_lred = new Scalar(0, 0, 255);
            var scalar_ured = new Scalar(255, 255, 255);

            Mat lower_red = new Mat(1, 3, hsv.Type(), scalar_lred);
            Mat upper_red = new Mat(1, 3, hsv.Type(), scalar_ured);

            Mat mask = hsv.InRange(lower_red, upper_red);

            Cv2.MinMaxLoc(src, out double minVal, out double maxVal, out Point minLoc, out maxLoc, mask);

            Cv2.Circle(src, maxLoc.X, maxLoc.Y, 20, scalar_lred, 2, LineTypes.AntiAlias);
            Cv2.ImShow("Laser Tracking", src);
        }

        private void getTexture()
        {
            cam.Render();
            //Copy the camera background to a RenderTexture
            Graphics.Blit(null, renderTexture, arCameraBackground.material);

            // Copy the RenderTexture from GPU to CPU
            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = renderTexture;

            tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, true);
            tex.ReadPixels(new UnityEngine.Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            tex.Apply();

            RenderTexture.active = activeRenderTexture;
        }

        private void getTexture2()
        {
            if (!arCameraManager.TryAcquireLatestCpuImage(out UnityEngine.XR.ARSubsystems.XRCpuImage image))
                return;

            var conversionParams = new UnityEngine.XR.ARSubsystems.XRCpuImage.ConversionParams
            {
                // Get the entire image.
                inputRect = new RectInt(0, 0, image.width, image.height),

                // Downsample by 2.
                outputDimensions = new Vector2Int(image.width / 2, image.height / 2),

                // Choose RGBA format.
                outputFormat = TextureFormat.RGBA32,

                // Flip across the vertical axis (mirror image).
                transformation = UnityEngine.XR.ARSubsystems.XRCpuImage.Transformation.MirrorY
            };

            // See how many bytes you need to store the final image.
            int size = image.GetConvertedDataSize(conversionParams);

            // Allocate a buffer to store the image.
            var buffer = new NativeArray<byte>(size, Allocator.Temp);

            // Extract the image data
            unsafe
            {
                image.Convert(conversionParams, new IntPtr(buffer.GetUnsafePtr()), buffer.Length);
            }

            // The image was converted to RGBA32 format and written into the provided buffer
            // so you can dispose of the XRCpuImage. You must do this or it will leak resources.
            image.Dispose();

            // At this point, you can process the image, pass it to a computer vision algorithm, etc.
            // In this example, you apply it to a texture to visualize it.

            // You've got the data; let's put it into a texture so you can visualize it.
            tex = new Texture2D(
                conversionParams.outputDimensions.x,
                conversionParams.outputDimensions.y,
                conversionParams.outputFormat,
                false);

            tex.LoadRawTextureData(buffer);
            tex.Apply();

            // Ich weiß eigentlich nicht ob die Methode überhaupt aufgerufen wird, weil keine Fehlermeldung kommt.
            laserDetection();

            // Done with your temporary data, so you can dispose it.
            buffer.Dispose();
        }
    }
}
