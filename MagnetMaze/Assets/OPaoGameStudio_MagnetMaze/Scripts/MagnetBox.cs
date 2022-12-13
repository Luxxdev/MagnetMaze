using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OPaoGameStudio_MagnetMaze
{
    public class MagnetBox : MonoBehaviour
    {
        //[SerializeField] private Rigidbody2D rigidBody;
        public Transform playerBoxHolder;
        public Collider2D collisionBlocker;
        public Sprite[] spriteArray;
        public SpriteRenderer spriteRenderer;
        public bool canInteract = false;
        public bool startsMagnetized = false;
        public bool held = false;
        public bool heldSettings = false;
        public PlayerScript player = null;
        public bool conducting = false;
        public bool touchingPlug = false;
        public bool isHorizontal = false;
        public Vector2 magnetOrientation;
        public string lastPole = "Neutral";
        public List<Collider2D> polesArea;
        public GameObject polesAreaObject;
        private List<GameObject> touchingConductingBoxes = new List<GameObject>();
        //[SerializeField] private Collider2D coll;

        private void Start()
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collisionBlocker);
        }
        private void Update()
        {
            if (conducting)
            {
                spriteRenderer.sprite = spriteArray[2];
            }
            else
            {
                if (lastPole == "Neutral")
                {
                    spriteRenderer.sprite = spriteArray[0];
                }
                else
                {
                    spriteRenderer.sprite = spriteArray[1];
                }
            }


            //if (!polesArea[0].enabled && !polesArea[1].enabled)
            //{
            //   polesArea[0].enabled = true;
            //   polesArea[1].enabled = true;
            //}

            if (held)
            {
                if (!heldSettings)
                {
                    transform.parent = playerBoxHolder;
                    gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
                    collisionBlocker.enabled = false;
                    gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
                    float direction;
                    if (!player.isFacingRight)
                        direction = -1;
                    else
                        direction = 1;
                    if (!isHorizontal)
                    {
                        ChangePole(lastPole, new Vector2(direction, 0));
                    }
                    else
                    {
                        if (lastPole == "Positive")
                        {
                            if ((player.isFacingRight && magnetOrientation.x != -1) || (!player.isFacingRight && magnetOrientation.x != 1))
                            {
                                ChangePole(lastPole, new Vector2(direction, 0));
                            }
                        }
                        else if (lastPole == "Negative")
                        {
                            if ((player.isFacingRight && magnetOrientation.x != 1) || (!player.isFacingRight && magnetOrientation.x != -1))
                            {
                                ChangePole(lastPole, new Vector2(direction, 0));
                            }
                        }
                    }
                    polesAreaObject.SetActive(false);
                    heldSettings = true;
                }
                transform.position = playerBoxHolder.position;
            }
            else if (heldSettings)
            {
                if (lastPole != "Neutral")
                {
                    polesAreaObject.SetActive(true);
                }
                transform.parent = null;
                collisionBlocker.enabled = true;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
                heldSettings = false;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12 && collision.transform.parent.gameObject.GetComponent<MagnetBox>().conducting)
            {
                conducting = true;
                touchingConductingBoxes.Add(collision.transform.parent.gameObject);
                // print("entrou" + name + conducting);
            }
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12)
            {
                if (!collision.transform.parent.gameObject.GetComponent<MagnetBox>().conducting)
                {
                    if (touchingConductingBoxes.Count != 0 && touchingConductingBoxes.Contains(collision.transform.parent.gameObject))
                    {
                        touchingConductingBoxes.Remove(collision.transform.parent.gameObject);
                        if (touchingConductingBoxes.Count == 0)
                        {
                            conducting = false;
                            // print("stay" + name + conducting);
                        }
                    }
                }
                else if (collision.transform.parent.gameObject.GetComponent<MagnetBox>().conducting)
                {
                    if (touchingConductingBoxes.Count == 0 || !touchingConductingBoxes.Contains(collision.transform.parent.gameObject))
                    {
                        touchingConductingBoxes.Add(collision.transform.parent.gameObject);
                        conducting = true;
                        // print("stay" + name + conducting);
                    }
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 12 && !touchingPlug)
            {
                if (touchingConductingBoxes.Count != 0 && touchingConductingBoxes.Contains(collision.transform.parent.gameObject))
                {
                    touchingConductingBoxes.Remove(collision.transform.parent.gameObject);
                    // print(touchingConductingBoxes.Count);
                }

                if (touchingConductingBoxes.Count == 0)
                {
                    conducting = false;
                    // print("saiu" + name + conducting);
                }
            }
        }
        public void ChangePole(string pole, Vector2 direction)
        {
            transform.localScale = new Vector3(1, 1, 1);
            transform.eulerAngles = new Vector3(0, 0, 0);
            float multi;
            if (direction == Vector2.zero)
            {
                if (player.isFacingRight)
                {
                    direction.x = 1;
                }
                else
                {
                    direction.x = -1;
                }
            }
            if (direction.y == 0)
            {
                isHorizontal = true;
                multi = direction.x;
                transform.eulerAngles = new Vector3(0, 0, 90);
            }
            else
            {
                direction.y = -player.vertical;
                isHorizontal = false;
            }
            if (pole == "Positive")
            {
                direction *= -1;
            }
            if (isHorizontal)
            {
                multi = direction.x;
            }
            else
            {
                multi = direction.y;
            }
            if (magnetOrientation != direction && pole != "Neutral")
            {
                polesAreaObject.SetActive(true);
                transform.localScale = new Vector3(1, multi, 1);
                spriteRenderer.sprite = spriteArray[1];
                lastPole = pole;
                magnetOrientation = direction;
            }
            else
            {
                spriteRenderer.sprite = spriteArray[0];
                polesAreaObject.SetActive(false);
                held = false;
                lastPole = "Neutral";
                magnetOrientation = new Vector2(0, 0);
            }
            print(lastPole);
            print(magnetOrientation);
        }
    }
}
