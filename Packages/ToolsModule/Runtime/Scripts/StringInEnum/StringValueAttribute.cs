using System;
using System.Reflection;


namespace GRV.ToolsModule
{
    public enum StringEnum
    {
        [StringValue("1")]
        Qwerty,
        [StringValue("2")]
        Asdf,
        [StringValue("Place")]
        Place,
    }
    public class StringValueAttribute : System.Attribute
    {
        public string StringValue { get; protected set;}


        public StringValueAttribute(string value)
        {
            this.StringValue = value;
        }

        public static string GetStringValue(Enum value)
        {
            Type type = value.GetType();
            FieldInfo fieldInfo = type.GetField(value.ToString());
            StringValueAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(StringValueAttribute), false) as StringValueAttribute[];
            
            return attribs.Length > 0 ? attribs[0].StringValue : null;
        }
    }
}
