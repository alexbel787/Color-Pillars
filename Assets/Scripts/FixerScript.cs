using UnityEngine;

public class FixerScript : MonoBehaviour
{
    public bool isActive;
    [SerializeField] private float speed;
    [SerializeField] private MeshRenderer[] flagRenderers;
    [HideInInspector] public Vector3 startPos;

    private GameManagerScript GMS;

    public enum Colors
    {
        Red_Green,
        Red_Blue,
        Green_Blue
    }
    public Colors canFixColor;

    private void Start()
    {
        GMS = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
        startPos = transform.position;
        SetFlagsColors();
        if (!isActive) DisableFixer();
        else print(canFixColor.ToString());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pillar"))
        {
            if (canFixColor.ToString().Contains(collision.gameObject.GetComponent<PillarScript>().pillarColor))
            {
                collision.gameObject.GetComponent<MeshRenderer>().material = GMS.meterials[4];
                StartCoroutine(GMS.ChangeColorCoroutine());
                isActive = false;
                DisableFixer();
            }
            else
            {
                GMS.AS.clip = GMS.soundClips[1];
                GMS.AS.Play();
            }
        }
    }

    private void Update()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        transform.position += moveDir * speed;
    }

    public void SetFlagsColors()
    {
        if (canFixColor == Colors.Red_Green)
        {
            flagRenderers[0].material = GMS.meterials[0];
            flagRenderers[1].material = GMS.meterials[1];
        }
        else if (canFixColor == Colors.Red_Blue)
        {
            flagRenderers[0].material = GMS.meterials[0];
            flagRenderers[1].material = GMS.meterials[2];
        }
        else if (canFixColor == Colors.Green_Blue)
        {
            flagRenderers[0].material = GMS.meterials[1];
            flagRenderers[1].material = GMS.meterials[2];
        }
    }

    public void DisableFixer()
    {
        GetComponent<MeshRenderer>().material = GMS.meterials[4];
        this.enabled = false;
    }
}
