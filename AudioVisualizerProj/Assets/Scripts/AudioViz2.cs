using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioViz2 : MonoBehaviour {
    public float scale = 10;

    public float UpperShapeVal = 2.5f;
    public float LowerShapeVal = 2.3f;
    private float MovementS;
    private float CurrentScaler;
    List<GameObject> elements = new List<GameObject>();
    List<GameObject> ShapeCreation = new List<GameObject>();
    private float RandomX;
    private float RandomY;
	void Start () {
        CreateVisualisers();
        ScriptGrabber();
    }


    void CreateVisualisers()
    {
        for (int i = 0; i < AudioAnalyzer.bands.Length; i++)
        {
            Vector3 p = new Vector3(i, -5, -20);
            p = transform.TransformPoint(p);
            Quaternion q = Quaternion.AngleAxis(0 * i * Mathf.Rad2Deg, Vector3.up);
            q = transform.rotation * q;

            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetPositionAndRotation(p, q);
            cube.transform.parent = this.transform;
            cube.GetComponent<Renderer>().material.color = Color.HSVToRGB(
                i / (float)AudioAnalyzer.bands.Length
                , 1
                , 1
                );
            elements.Add(cube);
        }
    }

    public void ScriptGrabber()
        {
            GameObject MovementSpeed = GameObject.Find("CameraOne");
            CameraEdit.MoveForward mf = MovementSpeed.GetComponent<CameraEdit.MoveForward>();
            MovementS = mf.MovementSpeed;
        }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < elements.Count; i++) {
            elements[i].transform.Translate(Vector3.forward * MovementS / 2);
            Vector3 ls = elements[i].transform.localScale;
            CurrentScaler = Mathf.Lerp(ls.y, 1 + (AudioAnalyzer.bands[i] * scale), Time.deltaTime * 3.0f);
            ls.y = CurrentScaler;
            elements[i].transform.localScale = ls;
            if (CurrentScaler > LowerShapeVal & CurrentScaler < UpperShapeVal){
                int Shape = Random.Range(1, 4);
                Vector3 CurrentCameraPos = Camera.main.gameObject.transform.position;
                int numrand = Random.Range(1, 3);
                int randomheight = Random.Range(1, 3);
                float xrand = 0.0f;
                float yrand = 0.0f;
                if (numrand == 1){
                    xrand = Random.Range(-30.0f, 0.0f);
                    if (xrand > -5.0f){
                        if (randomheight == 1){
                            yrand = Random.Range(-30.0f, -5.0f);
                        }
                        else {
                            yrand = Random.Range(30.0f, 5.0f);
                        }
                    }
                    else {
                        yrand = Random.Range(30.0f, -25.0f);
                    }
                }
                if (numrand == 2){
                    xrand = Random.Range(30.0f, 0.0f);
                    if (xrand < 5.0f){
                        if (randomheight == 1){
                            yrand = Random.Range(-30.0f, -5.0f);
                        }
                        else {
                            yrand = Random.Range(30.0f, 5.0f);
                        }
                    }
                    else {
                        yrand = Random.Range(30.0f, -30.0f);
                    }
                }
                Vector3 Randomiser = new Vector3(xrand, yrand, 100.0f);
                Vector3 CurrentCameraPosRand = CurrentCameraPos + Randomiser;
                Quaternion q = Quaternion.AngleAxis(0 * i * Mathf.Rad2Deg, Vector3.up);

                //sphere
                if (Shape == 1){
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    sphere.transform.SetPositionAndRotation(CurrentCameraPosRand, q);
                    Vector3 SpherePos = sphere.transform.position;
                    Vector3 sphereScale = sphere.transform.localScale;
                    float newscale = Random.Range(1.0f,6.0f);
                    int plusminus = Random.Range(1,2);
                    if (plusminus == 1){
                    sphereScale = sphereScale + new Vector3(newscale,newscale,newscale);
                    }
                    if (plusminus == 2){
                    sphereScale = sphereScale - new Vector3(newscale,newscale,newscale);
                    }
                    sphere.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0.1f,1.0f),1,1);
                    Destroy(sphere, 10);
                }

                //cube
                if (Shape == 2){
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.SetPositionAndRotation(CurrentCameraPosRand, q);
                    Vector3 cubePos = cube.transform.position;
                    Vector3 cubeScale = cube.transform.localScale;
                    float newscale = Random.Range(1.0f,6.0f);
                    int plusminus = Random.Range(1,3);
                    if (plusminus == 1){
                    cubeScale = cubeScale + new Vector3(newscale,newscale,newscale);
                    }
                    if (plusminus == 2){
                    cubeScale = cubeScale - new Vector3(newscale,newscale,newscale);
                    }
                    cube.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0.1f,1.0f),1,1);
                    Destroy(cube, 10);
                }

                //capsule
                if (Shape == 3){
                    GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    capsule.transform.SetPositionAndRotation(CurrentCameraPosRand, q);
                    Vector3 capsulePos = capsule.transform.position;
                    Vector3 capsuleScale = capsule.transform.localScale;
                    float newscale = Random.Range(1.0f,6.0f);
                    int plusminus = Random.Range(1,3);
                    if (plusminus == 1){
                    capsuleScale = capsuleScale + new Vector3(newscale,newscale,newscale);
                    }
                    if (plusminus == 2){
                    capsuleScale = capsuleScale - new Vector3(newscale,newscale,newscale);
                    }
                    capsule.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0.1f,1.0f),1,1);
                    Destroy(capsule, 10);
                }

                //cylinder
                if (Shape == 4){
                    GameObject cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                    cylinder.transform.SetPositionAndRotation(CurrentCameraPosRand, q);
                    Vector3 cylinderPos = cylinder.transform.position;
                    Vector3 cylinderScale = cylinder.transform.localScale;
                    float newscale = Random.Range(1.0f,6.0f);
                    int plusminus = Random.Range(1,3);
                    if (plusminus == 1){
                    cylinderScale = cylinderScale + new Vector3(newscale,newscale,newscale);
                    }
                    if (plusminus == 2){
                    cylinderScale = cylinderScale - new Vector3(newscale,newscale,newscale);
                    }
                    cylinder.GetComponent<Renderer>().material.color = Color.HSVToRGB(Random.Range(0.1f,1.0f),1,1);
                    Destroy(cylinder, 10);
                }               
            }
        }
	}
}
