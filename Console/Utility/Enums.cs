using System.ComponentModel;
using System.Reflection;

namespace ElevatorApp.Utility
{
    /// <summary>
    /// Class dedicated for definition of constant variables
    /// </summary>
    public enum Direction
    {
        [Description("is moving up")]
        Up,

        [Description("is moving down")]
        Down,

        [Description("is stationary")]
        Stationary
    }

    public enum Responses
    {
        [Description("valid input")]
        ValidInput,

        [Description("Invalid input")]
        InvalidInput,

        [Description("Capacity exceeded")]
        CapacityExceeded
    }

    /// <summary>
    /// Method to read description properties of the enums.
    /// </summary>
    public static class EnumHelper
    {
        public static string GetDescription(Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute == null ? value.ToString() : attribute.Description;
        }
    }
}