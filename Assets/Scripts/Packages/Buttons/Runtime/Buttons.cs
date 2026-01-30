using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.startech.Buttons
{
    [RequireComponent(typeof(BoxCollider2D))]
    [AddComponentMenu("Buttons/Buttons")]
    public class Buttons : MonoBehaviour
    {
        [Header("当鼠标按下时触发的函数")] 
        public UnityEvent onClick; //当鼠标按下时触发的函数
        protected SpriteRenderer spriteRenderer;
        protected BoxCollider2D boxCollider2D;
        private bool onCooling;
        private AudioClip clickSound;
        public static AudioClip ClickSoundCache;

        [Header("点击间隔")] [Tooltip("点击的间隔，单位秒(s)")]
        [SerializeField]
        private float delay = 0.1f; //按钮的点击间隔
        [Header("激活")] [Tooltip("activated:按钮是否激活，如果没激活就不会响应点击。")]
        [SerializeField]
        private bool activated = true; //按钮是否已激活
        [Header("启用按钮遮挡检测")]
        [SerializeField]
        private bool enableBlockCheck = false; //启用按钮的遮挡检测，按钮被其他角色遮挡后会失效
        public float Delay
        {
            get => delay;
            set => delay = value;
        }
        
        public bool Activated
        {
            get => activated;
            set => activated = value;
        }
        
        public bool EnableBlockCheck
        {
            get => enableBlockCheck;
            set => enableBlockCheck = value;
        }

        
        private const string BLOCK_LAYER_NAME = "UI";//检测阻挡按钮的层
        

        protected void Start()
        {
            //设置对象的layer
            if (enableBlockCheck)
            {
                this.gameObject.layer = LayerMask.NameToLayer(BLOCK_LAYER_NAME);
            }

            //点击音效缓存读取
            if (ClickSoundCache == null)
            {
                ClickSoundCache = Resources.Load<AudioClip>("Audio/SoundEffects/clickButton");
            }

            clickSound = ClickSoundCache;

            spriteRenderer = this.GetComponent<SpriteRenderer>();
            //将按钮冷却置为false
            onCooling = false;
            if (!TryGetComponent<BoxCollider2D>(out boxCollider2D))//如果获取到了boxCollider2D的Component，就会赋值给成员变量boxCollider2D
            {
                activated = false;
                Debug.LogError($"你的\"{gameObject.name}\"对象没有BoxCollider2D组件!，无法使用Buttons功能！请为该对象添加BoxCollider2D组件！");
                ButtonUtils.StopApplicationInEditor("组件配置错误",
                    $"你的\"{gameObject.name}\"对象没有BoxCollider2D组件!，无法使用Buttons功能！请为该对象添加BoxCollider2D组件！",
                    "中止播放");
            }


            //将OnClick方法添加到onClick中
            this.AddDefaultListener();

            //子类初始化
            NextStart();
        }

        private void AddDefaultListener()
        {
            int persistentEventCount = onClick.GetPersistentEventCount();
            // Debug.Log(persistentEventCount);
            bool hasOnClickListener = false;
            for (int i = 0; i < persistentEventCount; i++)
            {
                string persistentMethodName = onClick.GetPersistentMethodName(i);
                if (persistentMethodName == "OnClick")
                {
                    hasOnClickListener = true;
                    break;
                }
            }

            if (!hasOnClickListener)
            {
                onClick.AddListener(OnClick);
            }
        }

        private void PlayClickSound()
        {
            /*if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudio(clickSound);
            }*/
        }

        protected virtual void Update()
        {
            if (activated == false || IsButtonEnable() == false )
            {
                return;
            }

            if (TouchMouse() == false)
            {
                MouseExit();
                return;
            }

            
            MouseEnter();
            if (!Input.GetMouseButtonDown(0))
            {
                return;
            }
            //Debug.Log("点击到了");
            if (onCooling)
            {
                return;
            }
            PlayClickSound();
            onClick.Invoke();
            /*if (!this.gameObject.activeInHierarchy)
            {
                return;
            }*/

            StartCoroutine(HandleButtonClick());
        }

        /// <summary>
        /// 判断鼠标是否移动至按钮上，通过boxCollider2D组件的collider检测。
        /// 如果按钮上面有其他角色且layer为Button，那么无法点击。
        /// 注：由于摄像机在负轴，所以z越小越高。
        /// </summary>
        /// <returns></returns>
        bool TouchMouse()
        {
            Vector3 mousePos = MousePosition.GetMousePosition();
            Bounds bounds = boxCollider2D.bounds;
            if (mousePos.x > bounds.min.x && mousePos.x < bounds.max.x && mousePos.y > bounds.min.y &&
                mousePos.y < bounds.max.y)
            {
                if (!enableBlockCheck)
                {
                    return true;
                }

                //在矩形内是否有其他角色，并判断角色的z值是否更小->按钮是否遮挡
                Collider2D[] colliders = Physics2D.OverlapBoxAll(bounds.center, bounds.size, 0f);
                foreach (Collider2D collider in colliders)
                {
                    //判断是否是自己的collider
                    if (collider.gameObject == this.gameObject)
                    {
                        continue;
                    }
                    if (collider.gameObject.transform.position.z < this.transform.position.z)
                    {
                        int layer = collider.gameObject.layer;
                        string layerName = LayerMask.LayerToName(layer);
                        if (layerName == BLOCK_LAYER_NAME)
                        {
                            return false; // 另一个按钮角色在该按钮上面
                        }
                    }
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// 协程，用于给按钮增加延时。
        /// </summary>
        /// <returns></returns>
        IEnumerator HandleButtonClick()
        {
            onCooling = true;
            yield return new WaitForSeconds(delay);
            onCooling = false;
        }

        /// <summary>
        /// 方便子类按钮新增自己的初始化。
        /// </summary>
        public virtual void NextStart()
        {
        }

        /// <summary>
        /// 鼠标移动至按钮时触发
        /// </summary>
        public virtual void MouseEnter()
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.65f);
        }


        /// <summary>
        /// 鼠标离开按钮时触发
        /// </summary>
        public virtual void MouseExit()
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        /// <summary>
        /// 用于子类重写按钮判定，判断按钮是否可以使用。
        /// </summary>
        /// <returns>
        /// 如果为false，则鼠标移动至按钮时不会有反应。
        /// </returns>
        public virtual bool IsButtonEnable()
        {
            return true;
        }
        
        // 默认添加到按钮上的点击事件
        public virtual void OnClick()
        {
        }
    }
}