using UnityEngine;

namespace LYcommon
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>//ָ�������T�Ǽ̳���Singleton����
    {
        private static T instance;

        public static T Instance => instance;
        //��֤�������ظ�����
        protected virtual void Awake()
        {
            if ((Object)instance != (Object)null)
            {
                Object.Destroy(base.gameObject);
            }
            else
            {
                instance = (T)this;
            }
        }
    }
}
