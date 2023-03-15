using System;

namespace LYcommon
{
    /// <summary>
    /// 边界的自定义错误内容抛出
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

