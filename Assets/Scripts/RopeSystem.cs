using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RopeSystem : MonoBehaviour
{
    public GameObject ropeHingeAnchor;
    public DistanceJoint2D ropeJoint;
    public Transform ropeAnchor;
    public PlayerController playerController;
    private Vector2 playerPosition;
    private Rigidbody2D ropeHingeAnchorRb;
    private SpriteRenderer ropeHingeAnchorSprite;

    public LineRenderer ropeRenderer;
    public LayerMask ropeLayerMask;
    private List<Vector2> ropePositions = new List<Vector2>();

    private bool distanceSet;
    private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
    public float climbSpeed = 3f;
    private bool isColliding;

    private float maxRopeLength = 5;
    private float ropeLength;
    private float jointDist = 0f;
    private bool setDistance = true;


    void Awake()
    {
        ropeJoint.enabled = false;
        playerPosition = transform.position;
        ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
        ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
        ropePositions.Add(ropeAnchor.position);
        ropeJoint.distance = Vector2.Distance(transform.position, ropeAnchor.position);
        ropeHingeAnchorSprite.enabled = true;
    }

    void Update()
    {
        playerPosition = transform.position;

        if (ropeLength >= maxRopeLength && playerController.isGrounded == false)
        {
            playerController.isSwinging = true;
        }
        else
        {
            playerController.isSwinging = false;
            setDistance = true;
        }
        playerController.ropeHook = ropePositions.Last();
        // 1
        if (ropePositions.Count > 0)
        {
            // 2
            var lastRopePoint = ropePositions.Last();
            var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);

            // 3
            if (playerToCurrentNextHit)
            {
                var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
                if (colliderWithVertices != null)
                {
                    var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);
                    ropePositions.Add(closestPointToHit);
                    wrapPointsLookup.Add(closestPointToHit, 0);
                    distanceSet = false;
                }
            }


        }
        if (ropeLength >= maxRopeLength)
        {
            if (setDistance == true)
            {
                ropeJoint.distance = jointDist;
                setDistance = false;
            }
            ropeJoint.enabled = true;
        }
        else
        {
            ropeJoint.enabled = false;
        }

        UpdateRopePositions();
        HandleRopeLength();
        HandleRopeUnwrap();

    }

    // 6

    private void UpdateRopePositions()
    {
        // 2
        ropeRenderer.positionCount = ropePositions.Count + 1;

        // 3
        for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
        {
            if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
            {
                ropeRenderer.SetPosition(i, ropePositions[i]);

                // 4
                if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
                {
                    var ropePosition = ropePositions[ropePositions.Count - 1];
                    if (ropePositions.Count == 1)
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                    else
                    {
                        ropeHingeAnchorRb.transform.position = ropePosition;
                        if (!distanceSet)
                        {
                            ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                            distanceSet = true;
                        }
                    }
                }
                // 5
                else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
                {
                    var ropePosition = ropePositions.Last();
                    ropeHingeAnchorRb.transform.position = ropePosition;
                    if (!distanceSet)
                    {
                        ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
                        distanceSet = true;
                    }
                }
            }
            else
            {
                // 6
                ropeRenderer.SetPosition(i, transform.position);
            }
        }
    }

    // 1
    private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
    {
        // 2
        var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
            position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)),
            position => polyCollider.transform.TransformPoint(position));

        // 3
        var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
        return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
    }
    private void HandleRopeLength()
    {
        if (ropePositions.Count > 0)
        {
            jointDist = Vector2.Distance(transform.position, ropePositions.Last());
        }
        // 1
        if (Input.GetKey("z") == true)
        {
            ropeJoint.enabled = false;
            gameObject.GetComponent<Rigidbody2D>().simulated = false;
            transform.position = Vector3.MoveTowards(transform.position, ropePositions.Last(), climbSpeed * Time.deltaTime);
            //ropeJoint.distance -= Time.deltaTime * climbSpeed;
        }
        if (Input.GetKeyUp("z"))
        {
            gameObject.GetComponent<Rigidbody2D>().simulated = true;
        }
        else if (Input.GetKey("x") == true)
        {
            ropeJoint.distance += Time.deltaTime * climbSpeed;
        }
        ropeLength = 0;
        ropeLength += jointDist;
        if (ropePositions.Count > 1)
        {
            for (int i = 0; i < ropePositions.Count - 1; i++)
            {
                ropeLength += Vector2.Distance(ropePositions[i], ropePositions[i + 1]);
            }
        }
    }
    private void HandleRopeUnwrap()
    {
        if (ropePositions.Count <= 1)
        {
            return;
        }

        // Hinge = next point up from the player position
        // Anchor = next point up from the Hinge
        // Hinge Angle = Angle between anchor and hinge
        // Player Angle = Angle between anchor and player

        // 1
        var anchorIndex = ropePositions.Count - 2;
        // 2
        var hingeIndex = ropePositions.Count - 1;
        // 3
        var anchorPosition = ropePositions[anchorIndex];
        // 4
        var hingePosition = ropePositions[hingeIndex];
        // 5
        var hingeDir = hingePosition - anchorPosition;
        // 6
        var hingeAngle = Vector2.Angle(anchorPosition, hingeDir);
        // 7
        var playerDir = playerPosition - anchorPosition;
        // 8
        var playerAngle = Vector2.Angle(anchorPosition, playerDir);


        if (!wrapPointsLookup.ContainsKey(hingePosition))
        {
            Debug.LogError("We were not tracking hingePosition (" + hingePosition + ") in the look up dictionary.");
            return;
        }

        if (playerAngle < hingeAngle)
        {
            // 1
            if (wrapPointsLookup[hingePosition] == 1)
            {
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }

            // 2
            wrapPointsLookup[hingePosition] = -1;
        }
        else
        {
            // 3
            if (wrapPointsLookup[hingePosition] == -1)
            {
                UnwrapRopePosition(anchorIndex, hingeIndex);
                return;
            }

            // 4
            wrapPointsLookup[hingePosition] = 1;
        }
    }

    private void UnwrapRopePosition(int anchorIndex, int hingeIndex)
    {
        // 1
        var newAnchorPosition = ropePositions[anchorIndex];
        wrapPointsLookup.Remove(ropePositions[hingeIndex]);
        ropePositions.RemoveAt(hingeIndex);

        // 2
        ropeHingeAnchorRb.transform.position = newAnchorPosition;
        distanceSet = false;

        // Set new rope distance joint distance for anchor position if not yet set.
        if (distanceSet)
        {
            return;
        }
        ropeJoint.distance = Vector2.Distance(transform.position, newAnchorPosition);
        distanceSet = true;
    }

}
