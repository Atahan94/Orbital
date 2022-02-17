using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text title;
    ObjectMover Mover;
    public GameObject CameraMain, Light, Canvas;
    GameObject[] Spheres = new GameObject[3];
    public float SphereSpeed;
    GameObject sphereSlected = null;
    GameObject cube = null;
    Button Restart;
    

    Quaternion CamEnd = Quaternion.Euler(-17, 0, 0), LightEnd = Quaternion.Euler(340, 0, 0), 
        CamEndSc2 = Quaternion.Euler(25, 0, 0),CamFollowAngle = Quaternion.Euler(90,0,0),LightEndSc2 = Quaternion.Euler(360, 0, 0);
    public float speedCam, lightSpeed;

    public bool IsSceneSarted = false;
    public bool IsFinishedAll = false;
    bool checkBut = false;

    void Awake()
    {
      
        Mover = GameObject.Find("ObjectMover").GetComponent<ObjectMover>();
        Restart = GameObject.Find("Restart").GetComponent<Button>();
        Restart.gameObject.SetActive(false);
        DontDestroyOnLoad(Canvas);
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(Mover.gameObject);
        DontDestroyOnLoad(CameraMain);
        DontDestroyOnLoad(Light);
    }
    private void OnEnable()
    {
        Debug.Log("AnotherScene");
    }

    // Update is called once per frame
    void Update()
    {
        if (IsFinishedAll == false)
        {
            switch (SceneManager.GetActiveScene().buildIndex)
            {
                case 0:
                    bool[] rotsCheck = new bool[2] { Mover.Rotate(CameraMain, CamEnd, speedCam), Mover.Rotate(Light, LightEnd, lightSpeed) };
                    if (rotsCheck[0] && rotsCheck[1])
                    {
                        if (TitleFade(title, 6)) { SceneManager.LoadScene("SampleScene1"); }
                    }
                    break;
                case 1:
                    if (IsSceneSarted == false)
                    {

                        if (Mover.Rotate(CameraMain, CamEndSc2, speedCam) && Mover.Rotate(Light, LightEndSc2, lightSpeed))
                        {
                            for (int i = 0; i < Spheres.Length; i++)
                            {
                                Spheres[i] = GameObject.Find("Sphere_" + i);
                                DontDestroyOnLoad(Spheres[i]);
                            }
                            IsSceneSarted = true;
                        }
                    }
                    else if (IsSceneSarted == true)
                    {
                        Mover.RotateAround(Spheres[1], SphereSpeed);
                        Mover.RotateAround(Spheres[2], SphereSpeed);
                        if (ClickedOnSphere())
                        {
                            bool rotCheck = Mover.Rotate(CameraMain, CamFollowAngle, speedCam);
                            bool posCheck = Mover.FollowOtherObject(CameraMain, sphereSlected, speedCam);
                            //Debug.Log(Mover.Rotate(CameraMain, CamFollowAngle, speedCam) + "Move ->" + Mover.FollowOtherObject(CameraMain, sphereSlected, speedCam));
                            if (posCheck && rotCheck)
                            {

                                for (int i = 0; i < Spheres.Length; i++)
                                {
                                    if (!Spheres[i].Equals(sphereSlected))
                                    {

                                        if (ObjectFadeOut(Spheres[i].GetComponent<MeshRenderer>().material, 2) && i == Spheres.Length - 1)
                                        {

                                            SceneManager.LoadScene("SampleScene2");

                                        }

                                    }
                                }
                            }
                        }
                    }
                    break;
                case 2:
                    Mover.RotateAround(Spheres[1], SphereSpeed);
                    Mover.RotateAround(Spheres[2], SphereSpeed);
                    
                    if (cube == null)
                    {
                        cube = GetMeCube(GameObject.CreatePrimitive(PrimitiveType.Cube));
                        
                    }
                    if (ObjectFadeOut(cube.GetComponent<MeshRenderer>().material, 1f) && !checkBut)
                    {
                        Debug.Log("But");
                            Restart.gameObject.SetActive(true);
                            Restart.onClick.AddListener(Restarting);
                        checkBut = true;
                    }


                    break;
                default:
                    break;
            }
        }
      
    }
    public void Restarting()
    {
        IsFinishedAll = true;
        SceneManager.LoadScene(0);
            Destroy(Canvas);
            Destroy(Mover.gameObject);
            Destroy(CameraMain);
            Destroy(Light);
            for (int i = 0; i < Spheres.Length; i++)
            {
                Destroy(Spheres[i]);
            }
            Destroy(this.gameObject);
 
        
    }
    private GameObject GetMeCube(GameObject cube)
    {
        DontDestroyOnLoad(cube);
        cube.GetComponent<MeshRenderer>().material = sphereSlected.GetComponent<MeshRenderer>().material;
        cube.transform.parent = sphereSlected.transform;
        cube.transform.localScale = new Vector3(1, 1, 1);
        cube.transform.localPosition = new Vector3(1, 0, 0);

        return cube;
    }

    private bool ObjectFadeOut(Material material, float speed)
    {
        
        float sped = speed * Time.deltaTime;
        material.color = Color.Lerp(material.color, new Color(material.color.r, material.color.g, material.color.b, 0), sped);

        if (material.color == new Color(material.color.r, material.color.g, material.color.b, 0))
            return true;
        else
            return false;
    }

    private bool ClickedOnSphere() 
    {
        
        if (Input.GetMouseButtonDown(0)) 
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit)) 
            {
                
                if (hit.transform.name.Contains("Sphere_")) 
                {
                    sphereSlected = hit.transform.gameObject;
                    for (int i = 0; i < Spheres.Length; i++)
                    {
                        if (Spheres[i] != sphereSlected)
                            Spheres[i].GetComponent<SphereCollider>().enabled = false;
                    }
                    return true;
                }
            }
        }
        if (sphereSlected == null)
            return false;
        else
            return true;

    }


    private bool TitleFade(Text title, float sped) 
    {
            float speed = sped * Time.deltaTime;
           title.color = Color.Lerp(title.color, new Color(title.color.r, title.color.g, title.color.b, 0), speed);
        
        if (title.color == new Color(title.color.r, title.color.g, title.color.b, 0))
            return true;
        else
            return false;
    }
}
