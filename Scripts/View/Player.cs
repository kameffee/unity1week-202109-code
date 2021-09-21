using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace kameffee.unity1week202109.View
{
    public sealed class Player : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D rigidbody2D;

        [SerializeField]
        private Transform characterRoot;

        [SerializeField]
        private Transform[] stanPatterns;

        [SerializeField]
        private GameObject skateBoardPrefab;

        [FormerlySerializedAs("playerSettings")]
        [SerializeField]
        private CharacterSettings characterSettings;

        public CharacterSettings CharacterSettings => characterSettings;

        [Header("Status")]
        [SerializeField]
        private float moveSpeed = 1;

        [SerializeField]
        private float jumpPower = 10;

        [SerializeField]
        private float rotateAngle = 360f;

        [SerializeField]
        private float autoRotateAngle = 45;

        // 着地許容角度
        [SerializeField]
        private float landingAngle = 60f;

        [SerializeField]
        private LayerMask hitLayerMask;

        [Header("Particle")]
        [SerializeField]
        private ParticleSystem dustEffect;

        [SerializeField]
        private ParticleSystem speedUpEffect;

        [SerializeField]
        private TrailRenderer trailRenderer;

        [SerializeField]
        private ParticleSystem stanDustEffect;

        public float MoveSpeed => moveSpeed;

        // ジャンプ通知
        public IObservable<Unit> OnJump => onJump;
        private readonly Subject<Unit> onJump = new Subject<Unit>();

        // 着地
        public IReadOnlyReactiveProperty<bool> IsGround => isGround;
        private readonly ReactiveProperty<bool> isGround = new ReactiveProperty<bool>();

        // 転んだ通知
        public IObservable<Unit> OnStan => onStan;
        private readonly Subject<Unit> onStan = new Subject<Unit>();

        // 回転成功通知
        public IObservable<int> OnSuccessSpin => onSuccessSpin;
        private readonly Subject<int> onSuccessSpin = new Subject<int>();

        // スピード
        public IReadOnlyReactiveProperty<Vector2> Speed => speed;
        private readonly ReactiveProperty<Vector2> speed = new ReactiveProperty<Vector2>();

        // 空中にいるときの重力
        private float gravityWhenAir = 2;

        // 着地中の重力
        private float gravityWhenGround = 10;

        private Transform rootTransform;

        private RaycastHit2D hit;

        // 空中でいるときに落下地点を測るためのやつ
        private RaycastHit2D longHit;

        // 地面の法線
        private Vector3 normalVector;

        // 水平方向ベクトル
        private Vector3 onPlaneVector;

        // トータル回転数
        private float totalRotateAngle;

        // 自動移動フラグ
        private bool isAutoMove = false;

        // 地面判定中か
        private bool isGroundRaycast = true;

        // ジャンプ中
        private bool isJump = false;

        // 回転中
        private bool isSpining;

        // ジャンプした時間のキャッシュ
        private float onJumpTime;

        private void Awake()
        {
            rootTransform = transform;
        }

        private void Start()
        {
            // 地面検知可能制御
            this.UpdateAsObservable()
                .Where(_ => isJump)
                .Where(_ => onJumpTime + 0.2f < Time.time)
                .Subscribe(_ => isGroundRaycast = true)
                .AddTo(this);

            // 空中 & 押してない
            this.UpdateAsObservable()
                .Where(_ => !isSpining)
                .Where(_ => !isGround.Value)
                .Where(_ => onJumpTime + 0.2f < Time.time)
                .Subscribe(_ =>
                {
                    // 落下地点と平行になるようにゆっくり補正する
                    longHit = Physics2D.Raycast(rootTransform.position, Vector2.down, 100f, hitLayerMask);
                    bool isHit = longHit.collider != null;

                    // 内部の回転
                    if (isHit)
                    {
                        Debug.DrawRay(transform.position, (Vector3)longHit.point - transform.position, Color.magenta);
                        Debug.DrawRay(longHit.point, (longHit.point + longHit.normal) - longHit.point, Color.magenta);

                        // 自動でゆっくりと地面に垂直に直っていく
                        var toRotate = Quaternion.FromToRotation(Vector2.up, Vector3.up);
                        characterRoot.localRotation = Quaternion.RotateTowards(
                            characterRoot.localRotation,
                            toRotate,
                            autoRotateAngle * Time.deltaTime);
                    }
                })
                .AddTo(this);

            // 自動移動
            this.FixedUpdateAsObservable()
                .Where(_ => isAutoMove)
                .Where(_ => !isJump)
                .Where(_ => isGround.Value)
                .Subscribe(_ =>
                {
                    // 地面に沿って移動
                    var vector = Vector2.right * moveSpeed;
                    onPlaneVector = Vector3.ProjectOnPlane(vector, normalVector);
                    rigidbody2D.velocity = (onPlaneVector);
                })
                .AddTo(this);

            // 着地
            this.OnCollisionEnter2DAsObservable()
                .Where(_ => isGroundRaycast)
                .Where(collision2D => collision2D.gameObject.CompareTag("Ground"))
                .Subscribe(collision2D =>
                {
                    bool isStan = false;
                    // 着地点の法線を取る
                    var hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, hitLayerMask);
                    if (hit.collider != null)
                    {
                        var normal = hit.normal;
                        var angle = Vector2.Angle(characterRoot.TransformDirection(Vector3.up), normal);
                        // Debug.Log($"angle: {angle}");
                        if (Mathf.Abs(angle) > landingAngle)
                        {
                            isStan = true;
                            onStan.OnNext(Unit.Default);
                        }
                    }

                    characterRoot.localRotation = Quaternion.Euler(0, 0, 0);

                    isGround.Value = true;
                    if (!isStan)
                    {
                        onSuccessSpin.OnNext((int)(totalRotateAngle / (360 - 90f)));
                    }
                })
                .AddTo(this);

            this.OnCollisionStay2DAsObservable()
                .Where(collision2D => collision2D.gameObject.CompareTag("Ground"))
                .Subscribe(collision2D =>
                {
                    characterRoot.localRotation = Quaternion.Euler(0, 0, 0);
                    normalVector = collision2D.contacts[0].normal;
                    isGround.Value = true;
                })
                .AddTo(this);

            // 跳んだ時
            this.OnCollisionExit2DAsObservable()
                .Where(collision2D => collision2D.gameObject.CompareTag("Ground") && collision2D.contactCount == 0)
                .Subscribe(_ =>
                {
                    isGround.Value = false;
                    totalRotateAngle = 0;
                })
                .AddTo(this);

            // スピード計測
            this.UpdateAsObservable()
                .Subscribe(_ => speed.Value = rigidbody2D.velocity)
                .AddTo(this);

            IsGround
                .Where(isGround => isGround)
                .Subscribe(_ =>
                {
                    rigidbody2D.gravityScale = gravityWhenGround;
                    trailRenderer.emitting = false;
                    dustEffect.Play();
                })
                .AddTo(this);

            IsGround
                .Where(isGround => !isGround)
                .Subscribe(_ =>
                {
                    rigidbody2D.gravityScale = gravityWhenAir;
                    trailRenderer.emitting = true;
                    dustEffect.Stop();
                })
                .AddTo(this);

            OnStan
                .Subscribe(_ =>
                {
                    var skateBoard = Instantiate(skateBoardPrefab, transform.position, Quaternion.identity);
                    var rigidbody = skateBoard.GetComponent<Rigidbody2D>();
                    rigidbody.AddTorque(2f, ForceMode2D.Impulse);
                    rigidbody.AddForce((new Vector2(.5f, 1f).normalized) * 3f, ForceMode2D.Impulse);
                })
                .AddTo(this);

            // IsGround.Subscribe(isGround => Debug.Log($"IsGround:{isGround}")).AddTo(this);
        }

        public void Initialize(Vector2 position)
        {
            transform.position = position;
            transform.rotation = Quaternion.identity;
            rigidbody2D.velocity = Vector2.zero;

            isJump = false;
            isSpining = false;

            SetStanState(false);
        }

        public void SetJumpPower(float jumpPower) => this.jumpPower = jumpPower;

        public void SetRotateAngle(float angle) => this.rotateAngle = angle;

        public void SetMoveSpeed(float speed) => moveSpeed = speed;

        public void SetStanState(bool isStan)
        {
            if (isStan)
            {
                stanDustEffect.Play();
            }

            characterRoot.gameObject.SetActive(!isStan);
            if (isStan)
            {
                stanPatterns[Random.Range(0, stanPatterns.Length)].gameObject.SetActive(isStan);
            }
            else
            {
                foreach (var stanPattern in stanPatterns)
                {
                    stanPattern.gameObject.SetActive(false);
                }
            }
        }

        public void SetAutoMove(bool autoMove)
        {
            this.isAutoMove = autoMove;
            rigidbody2D.simulated = autoMove;
        }

        /// <summary>
        /// スピードアップ時
        /// </summary>
        /// <param name="count"></param>
        public void OnSpeedUp(int count)
        {
            speedUpEffect.Emit(1);
        }

        public void Jump()
        {
            if (isJump) return;

            Debug.Log("Jump");


            // 前方上方向へジャンプ
            rigidbody2D.velocity = GetJumpVelocity();

            rigidbody2D.gravityScale = gravityWhenAir;
            isJump = true;
            isGroundRaycast = false;
            onJumpTime = Time.time;
        }

        Vector2 GetJumpVelocity()
        {
            var vector = Vector2.right * jumpPower;
            onPlaneVector = Vector3.ProjectOnPlane(vector, normalVector);
            return (Vector2.up * jumpPower + (Vector2)onPlaneVector).normalized * rigidbody2D.velocity.magnitude * 1.5f;
        }

        public void Spin()
        {
            Debug.Log("Spin");
            isSpining = true;

            // 空中回転
            var angle = rotateAngle * Time.deltaTime;
            characterRoot.rotation = Quaternion.Euler(0, 0, angle) * characterRoot.rotation;
            totalRotateAngle += angle;
        }

        public void SpinStop()
        {
            isSpining = false;
        }

        /// <summary>
        /// 地面についた
        /// </summary>
        public void OnTouchGround()
        {
            // なんかする
            isJump = false;
        }

        private void Update()
        {
            hit = Physics2D.Raycast(rootTransform.position, Vector2.down, 100f, hitLayerMask);
            if (hit.collider != null)
            {
                var to = Quaternion.FromToRotation(Vector2.up, hit.normal);
                var rotate = Quaternion.RotateTowards(rootTransform.rotation,
                    to,
                    180 * Time.fixedDeltaTime);
                SetRootRotateWithIgnoreCharacterRoot(rotate);
            }
            else
            {
                var to = Quaternion.FromToRotation(Vector2.up, Vector2.up);
                var rotate = Quaternion.RotateTowards(rootTransform.rotation,
                    to,
                    180 * Time.fixedDeltaTime);
                SetRootRotateWithIgnoreCharacterRoot(rotate);
            }
        }

        /// <summary>
        /// 子のキャラクターの回転を保持したまま,ルートを回転させる.
        /// </summary>
        /// <param name="toRotate"></param>
        private void SetRootRotateWithIgnoreCharacterRoot(Quaternion toRotate)
        {
            var tmp = characterRoot.rotation;
            transform.rotation = toRotate;
            characterRoot.rotation = tmp;
        }

        private void OnDrawGizmos()
        {
            // Gizmos.color = hit.collider != null ? Color.red : Color.green;
            // Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector2.down) * 1.7f);
            //
            // if (hit.collider != null)
            // {
            //     Gizmos.DrawSphere(hit.point, 0.1f);
            //     Gizmos.DrawRay(hit.point, hit.normal);
            // }
            //
            // Gizmos.color = Color.blue;
            // Gizmos.DrawRay(transform.position, onPlaneVector);
            //
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawRay(transform.position, GetJumpVelocity());
        }
    }
}