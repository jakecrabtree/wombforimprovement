using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSystem : MonoBehaviour
{
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    //Eventually need to get rid of this.
    public Transform crosshair;
    public SpriteRenderer crosshairSprite;
    public PlayerController playerController;
    //Eventually won't be used
    private bool ropeAttached;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private float ropeMaxCastDistance = 20f;
    private List<Vector2> ropePosition = new List<Vector2>();

    void Awake()
    {
        
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        //Used for crosshair, eventually won't need this
        var worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        var facingDirection = worldMousePosition - transform.position;
        var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
        if(aimAngle < 0f)
        {
            aimAngle = Mathf.PI * 2 + aimAngle;
        }

        var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
        playerPosition = transform.position;

        if (!ropeAttached)
        {
            SetCrosshairPosition(aimAngle);
        }
        else
        {
            crosshairSprite.enabled = false;
        }
        HandleInput(aimDirection);
    }
     private void SetCrosshairPosition(float aimAngle)
    {
        if (!crosshairSprite.enabled)
        {
            crosshairSprite.enabled = true;
        }
        var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
        var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

        var crossHairPosition = new Vector3(x, y, 0);
        crosshair.transform.position = crossHairPosition;
    }
    private void HandleInput(Vector2 aimDirection)
    {
        if (Input.GetMouseButton(0))
        {
            // 2
            if (ropeAttached) return;
            ropeRenderer.enabled = true;

            var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

            // 3
            if (hit.collider != null)
            {
                ropeAttached = true;
                if (!ropePosition.Contains(hit.point))
                {
                    // 4
                    // Jump slightly to distance the player a little from the ground after grappling to something.
                    transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
                    ropePosition.Add(hit.point);
                    ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
                    ropeJoint.enabled = true;
                    ropeHingeAnchorSprite.enabled = true;
                }
            }
            // 5
            else
            {
                ropeRenderer.enabled = false;
                ropeAttached = false;
                ropeJoint.enabled = false;
            }
        }

        if (Input.GetMouseButton(1))
        {
            ResetRope();
        }
    }

    // 6
    private void ResetRope()
    {
        ropeJoint.enabled = false;
        ropeAttached = false;
        playerController.isSwinging = false;
        ropeRenderer.positionCount = 2;
        ropeRenderer.SetPosition(0, transform.position);
        ropeRenderer.SetPosition(1, transform.position);
        ropePosition.Clear();
        ropeHingeAnchorSprite.enabled = false;
    }
}
