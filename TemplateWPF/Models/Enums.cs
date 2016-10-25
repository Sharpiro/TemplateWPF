using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace TemplateWpf.Models
{
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum RoleItem
    {
        [Description("Cool Item")]
        CoolItem,
        [Description("Not Cool Item")]
        NotCoolItem
    }

    public class EnumDescriptionConverter : EnumConverter
    {
        public EnumDescriptionConverter(Type type) : base(type) { }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType != typeof(string))
                return base.ConvertTo(context, culture, value, destinationType);

            var fieldInfo = value?.GetType().GetField(value.ToString());
            var attribute = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false)
                .SingleOrDefault() as DescriptionAttribute;

            return attribute != null ? attribute.Description : value.ToString();
        }
    }
}