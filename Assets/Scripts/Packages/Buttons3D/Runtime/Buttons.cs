using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace com.startech.Buttons
{
    [AddComponentMenu("Buttons/Buttons")]
    public class Buttons : MonoBehaviour
    {
        [FormerlySerializedAs("onClick")]
        [Header("当鼠标按下时触发的函数")]
        [SerializeField]
        private UnityEvent onClickEvent; //当鼠标按下时触发的函数
        private SpriteRenderer spriteRenderer;
        private bool onCooling;
        private AudioClip clickSound;
        public static AudioClip ClickSoundCache;

        [Header("点击间隔")] [Tooltip("点击的间隔，单位秒(s)")]
        [SerializeField]
        private float delay = 0.1f; //按钮的点击间隔
        [Header("激活")] [Tooltip("activated:按钮是否激活，如果没激活就不会响应点击。")]
        [SerializeField]
        private bool activated = true; //按钮是否激活
        [Header("启用按钮遮挡检测")]
        [SerializeField]
        private bool enableBlockCheck = false; //启用按钮的遮挡检测，按钮被其他角色遮挡后会失效


        [Header("是否开启默认特效")]
        [SerializeField]
        private bool enableDefaultSpecialEffect=false;  //是否开启默认特效

        public UnityEvent OnClickEvent
        {
            get => onClickEvent;
            set => onClickEvent = value;
        }

        public SpriteRenderer SpriteRenderer
        {
            get => spriteRenderer;
            set => spriteRenderer = value;
        }

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

        
        public bool Interactable=> activated && !onCooling&&IsLogicEnabled();
        
        
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


            //将OnClick方法添加到onClick中
            this.AddDefaultListener();

            //子类初始化
            NextStart();
        }

        private void AddDefaultListener()
        {
            int persistentEventCount = onClickEvent.GetPersistentEventCount();
            for (int i = 0; i < persistentEventCount; i++)
            {
                string persistentMethodName = onClickEvent.GetPersistentMethodName(i);
            }

            onClickEvent.AddListener(OnClick);
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
            if (!Interactable)
            {
                return;
            }

            if (!TouchMouse3D())
            {
                MouseExit();
                return;
            }

            
            MouseEnter();
            if (Input.GetMouseButtonDown(0))
            {
                Click();
            }
        }
        
        
        private void Click()
        {
            Debug.Log("点击到了");
            if (onCooling)
            {
                return;
            }
            PlayClickSound();
            onClickEvent.Invoke();
            StartCoroutine(HandleButtonClick());
        }
        
        private bool TouchMouse3D()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.gameObject == this.gameObject)
                {
                    return true;
                }
            }
            return false;
        }
        
        
        /// <summary>
        /// 协程，用于给按钮增加延时。
        /// </summary>
        /// <returns></returns>
        private IEnumerator HandleButtonClick()
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
        /// 鼠标移动至按钮时触发，默认有颜色改变特效
        /// </summary>
        public virtual void MouseEnter()
        {
            if (!enableDefaultSpecialEffect)
            {
                return;
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.65f);
        }


        /// <summary>
        /// 鼠标离开按钮时触发
        /// </summary>
        public virtual void MouseExit()
        {
            if (!enableDefaultSpecialEffect)
            {
                return;
            }
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }

        protected virtual bool IsLogicEnabled()
        {
            return true;
        }

        
        // 默认添加到按钮上的点击事件
        public virtual void OnClick()
        {
        }
    }
}