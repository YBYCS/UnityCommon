using System;

namespace LYcommon
{
    /// <summary>
    /// �߽���Զ�����������׳�
    /// </summary>
    public class CustomException : ApplicationException
    {

        public CustomException(string message) : base(message) { }

        public override string Message
        {
            get
            {
                return base.Message;
            }
        }
    }
}

