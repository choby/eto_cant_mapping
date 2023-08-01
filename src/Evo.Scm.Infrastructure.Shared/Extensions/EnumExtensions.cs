using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Evo.Scm.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 枚举描述特征取值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum val)
        {
            if (val == null)
                return String.Empty;
            var field = val.GetType().GetField(val.ToString());
            if (field == null)
                return String.Empty;
            var customAttribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            if (customAttribute == null)
                return val.ToString();
            else
                return ((DescriptionAttribute)customAttribute).Description;
        }
    }
}
