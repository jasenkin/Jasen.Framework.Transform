using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    public class SortedMemberTransfer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dataTable"></param>
        /// <returns></returns>
        public static List<T> ToEntities<T>(DataTable dataTable) where T : new()
        { 
            var entities = new List<T>();

            if (dataTable == null || dataTable.Rows == null
                || dataTable.Rows.Count == 0)
            {
                return entities;
            }

            foreach (DataRow dataRow in dataTable.Rows)
            {
                entities.Add(ToEntity<T>(dataRow));
            }

            return entities;
        }

        private static T ToEntity<T>(DataRow dataRow) where T : new()
        {
            if (dataRow == null)
            {
                return default(T);
            }

            T entity = Activator.CreateInstance<T>();

            var members = new DataMemberAttributeCollection(typeof(T));
            DataMemberAttribute currAttribute;
            PropertyInfo currPropertyInfo;
            int count = members.Count;
            int columnCount = dataRow.ItemArray.Length;
            var func = new FuncProvider();
            object currentValue = null;

            for (int columnIndex = 0; columnIndex < columnCount; columnIndex++)
            {
                if (columnIndex > count - 1)
                {
                    continue;
                }

                currentValue = dataRow[columnIndex];
                currAttribute = members[columnIndex];
                currPropertyInfo = currAttribute.PropertyInfo;

                if (currentValue is DBNull)
                {
                    currPropertyInfo.SetValue(entity, null, null);
                    continue;
                }

                if (currentValue.GetType() == currAttribute.PropertyType)
                {
                    currPropertyInfo.SetValue(entity, currentValue, null);
                }
                else
                {
                    object result = func.DynamicInvoke(currAttribute.PropertyType, (currentValue ?? string.Empty).ToString());
                    currPropertyInfo.SetValue(entity, result, null);
                }
            }

            return entity;
        }
    }
}
