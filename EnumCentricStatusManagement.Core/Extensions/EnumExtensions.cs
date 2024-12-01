
using System;

namespace EnumCentricStatusManagement.Core
{
    public static class EnumExtensions
    {
        public static StatusAttribute GetEnumStatus(this Enum status)
        {
            var type = status.GetType();
            var memInfo = type.GetMember(status.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(StatusAttribute), false);
            return (StatusAttribute)attributes[0];
        }
    }
}
