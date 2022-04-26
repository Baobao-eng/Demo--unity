using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private float mouseSen;
    private Transform parent;
    private AudioSource music;
    private Spawn stopPlay;
    // Start is called before the first frame update
    void Start()
    {
        stopPlay = GameObject.Find("SpawnManager").GetComponent<Spawn>();
        music = GetComponent<AudioSource>();
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        if(stopPlay.GameOverCheck == true)
        {
            music.Stop();
        }
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSen * Time.deltaTime;

        parent.Rotate(Vector3.up, mouseX);
    }
}
