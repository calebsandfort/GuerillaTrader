using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GuerillaTrader.Framework
{
    #region EnumExtensions
    public static class EnumExtensions
    {
        #region GetDisplay
        public static String GetDisplay(this Enum value)
        {
            // Get the type
            Type type = value.GetType();

            // Get fieldinfo for this type
            FieldInfo fieldInfo = type.GetField(value.ToString());

            // Get the stringvalue attributes
            EnumDisplayAttribute[] attribs = fieldInfo.GetCustomAttributes(
                typeof(EnumDisplayAttribute), false) as EnumDisplayAttribute[];

            // Return the first if there was a match.
            return attribs.Length > 0 ? attribs[0].Display : String.Empty;
        }
        #endregion

        #region GetValues
        public static List<T> GetValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }
        #endregion

        public static List<int> FlaggedEnumToList<T>(T flaggedVals)
            where T : struct
        {
            List<int> list = new List<int>();
            Enum flaggedValsAsEnum = (Enum)(object)flaggedVals;

            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>().ToList())
            {
                if (flaggedValsAsEnum.HasFlag((Enum)(object)item))
                {
                    list.Add((int)(object)item);
                }
            }

            return list;
        }

        public static T ListToFlaggedEnum<T>(List<int> list)
            where T : struct
        {
            int flaggedVal = 0;

            foreach (T item in Enum.GetValues(typeof(T)).Cast<T>().ToList())
            {
                if (list.Contains((int)(object)item))
                {
                    flaggedVal += (int)(object)item;
                }
            }

            return (T)Enum.ToObject(typeof(T), flaggedVal);
        }
    }
    #endregion

    #region Attributes
    #region DisplayAttribute
    /// <summary>
    /// This attribute is used to represent a string value
    /// for a value in an enum.
    /// </summary>
    public class EnumDisplayAttribute : Attribute
    {
        #region Properties
        /// <summary>
        /// Holds the stringvalue for a value in an enum.
        /// </summary>
        public String Display { get; protected set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor used to init a StringValue Attribute
        /// </summary>
        /// <param name="value"></param>
        public EnumDisplayAttribute(String value)
        {
            this.Display = value;
        }
        #endregion
    }
    #endregion
    #endregion
}
