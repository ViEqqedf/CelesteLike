using System;
using UnityEngine;

namespace ViE.CelesteLike {
    public enum PlayState {
        Normal,
        Jump,
        Climb,
        Dash,
        Fall,
    }

    public class PlayerCharacter : MonoBehaviour {
        public float MoveSpeed = 9;
        public Vector3 Velocity;
        public Vector3 CenterOffset;

        private Rigidbody2D rig;
        private int playerLayerMask;
        private RaycastHit2D UpBox;
        private RaycastHit2D DownBox;
        private RaycastHit2D LeftBox;
        private RaycastHit2D RightBox;
        private Vector2 Position => transform.position - CenterOffset;
        private bool isOnGround => DownBox.collider != null;
        private Vector2 boxSize;

        private void Start() {
            rig = GetComponent<Rigidbody2D>();
            playerLayerMask = LayerMask.GetMask("Player");
            playerLayerMask = ~playerLayerMask;
            boxSize = new Vector2(1, 1.2f);
        }

        private void FixedUpdate() {
            HorizontalMove();
            rig.MovePosition(transform.position + Velocity * Time.fixedDeltaTime);
        }

        private void Update() {
            RayCastBox();
        }

        private void HorizontalMove() {
            var input = InputManager.Instance;
            if ((Velocity.x > 0 && input.MoveDir == -1) ||
                (Velocity.x < 0 && input.MoveDir == 1) ||
                input.MoveDir == 0 ||
                (isOnGround && input.v < 0) ||
                Mathf.Abs(Velocity.x) > MoveSpeed) {
                // decelerate section
                int hvDir = Velocity.x > 0 ? 1 : -1;
                float hMove = Mathf.Abs(Velocity.x);
                if (isOnGround) {
                    hMove -= MoveSpeed / 3;
                } else {
                    hMove -= MoveSpeed / 6;
                }

                if (hMove < 0.01f) {
                    hMove = 0;
                }

                Velocity.x = hMove * hvDir;
            } else {
                // accelerate section
                if (isOnGround && input.v < 0) {
                    return;
                }

                if (input.MoveDir == 1) {
                    if (isOnGround) {
                        Velocity.x += MoveSpeed / 6;
                    } else {
                        Velocity.x += MoveSpeed / 12;
                    }

                    if (Velocity.x > MoveSpeed) {
                        Velocity.x = MoveSpeed;
                    }
                } else if (input.MoveDir == -1 && !(isOnGround && input.v < 0)) {
                    if (isOnGround) {
                        Velocity.x -= MoveSpeed / 6;
                    } else {
                        Velocity.x -= MoveSpeed / 12f;
                    }

                    if (Velocity.x < -MoveSpeed) {
                        Velocity.x = -MoveSpeed;
                    }
                }
            }
        }

        private void Fall() {
            if (isOnGround) {
                Velocity.y = 0;
                return;
            }

            Velocity.y -= 150f * Time.deltaTime;
            Velocity.y = Mathf.Clamp(Velocity.y, -25, Velocity.y);
        }

        private void RayCastBox() {
            UpBox = Physics2D.BoxCast(Position, boxSize, 0, Vector3.up, 0.1f, playerLayerMask);
            DownBox = Physics2D.BoxCast(Position, boxSize, 0, Vector3.down, 0.1f, playerLayerMask);
            LeftBox = Physics2D.BoxCast(Position, boxSize, 0, Vector3.left, 0.1f, playerLayerMask);
            RightBox = Physics2D.BoxCast(Position, boxSize, 0, Vector3.right, 0.1f, playerLayerMask);
        }
    }
}