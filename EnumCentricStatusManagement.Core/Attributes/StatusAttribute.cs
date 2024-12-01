
using System;

namespace EnumCentricStatusManagement.Core
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class StatusAttribute : Attribute
    {
        public string Message { get; }
        public Enum Type { get; }

        public StatusAttribute(string message, StatusType type)
        {
            if (!type.GetType().IsEnum)
                throw new ArgumentException("Parameter must be an enum.");


            Message = message;
            Type = type;
        }

        public StatusAttribute(string message, Enum type)
        {
            Message = message;
            Type = type;
        }
    }
}