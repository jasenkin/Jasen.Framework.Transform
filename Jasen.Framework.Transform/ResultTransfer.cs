using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Jasen.Framework.Transform
{
    public class ResultTransfer
    {
        public static IEnumerable<string> GenerateAndFilterLines(string content, bool skipFirstLine = false)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return new List<string>();
            }

            string[] rows = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length <= 1)
            {
                return new List<string>();
            }

            return skipFirstLine ? rows.Skip(1) : rows;
        }

        public static string GetLastLine(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return null;
            }

            string[] rows = content.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            if (rows.Length >= 1)
            {
                return rows.Last();
            }

            return null;
        }

        public static IEnumerable<IList<string>> Generate(IEnumerable<string> lines)
        {
            if (lines == null || lines.Count() == 0)
            {
                return new List<IList<string>>();
            }

            IList<IList<string>> entityRows = new List<IList<string>>();
            IList<string> propertyValues;

            foreach (string line in lines)
            {
                MatchCollection matches = Regex.Matches(line, @"\[\s*([^\[\]\s]*?)\]|\'([^\']*?)\'");
                propertyValues = new List<string>();

                foreach (Match match in matches)
                {
                    if (match.Groups[0].Value.StartsWith("["))
                    {
                        propertyValues.Add(match.Groups[1].Value.Trim());
                    }
                    else
                    {
                        propertyValues.Add(match.Groups[2].Value.Trim());
                    }
                }

                if (propertyValues.Count > 0)
                {
                    entityRows.Add(propertyValues);
                }
            }

            return entityRows;
        }

        public static IList<T> Generate<T>(string content, params string[] propertyNames) where T : new()
        {
            var lines = GenerateAndFilterLines(content);

            var entityRows = Generate(lines);

            return Parse<T>(entityRows, propertyNames);
        }

        public static IList<T> Parse<T>(IEnumerable<IEnumerable<string>> entityRows, params string[] propertyNames) where T : new()
        {
            if (entityRows == null || entityRows.Count() == 0)
            {
                return new List<T>();
            }

            IList<T> entities = new List<T>();
            var members = new DataMemberAttributeCollection(typeof(T), propertyNames);

            if (members.Count <= 1)
            {
                return new List<T>();
            }

            FuncProvider funcProvider = new FuncProvider();

            foreach (var propertyValues in entityRows)
            {
                if (propertyValues == null || propertyValues.Count() == 0)
                {
                    continue;
                }

                entities.Add(Generate<T>(propertyValues, members, funcProvider));
            }

            return entities;
        }

        private static T Generate<T>(IEnumerable<string> propertyValues, DataMemberAttributeCollection members,
            FuncProvider funcProvider) where T : new()
        {
            T entity = Activator.CreateInstance<T>();
            int memberCount = members.Count;
            int propertyCount = propertyValues.Count();

            if (memberCount == 0 || propertyCount == 0)
            {
                return entity;
            }

            int convertCount = Math.Min(memberCount, propertyCount);
            DataMemberAttribute currAttribute;
            PropertyInfo currPropertyInfo;

            int propertyValueIndex = 0;

            foreach (string propertyValue in propertyValues)
            {
                if (propertyValueIndex >= convertCount)
                {
                    break;
                }

                propertyValueIndex++;
                currAttribute = members[propertyValueIndex - 1];
                currPropertyInfo = currAttribute.PropertyInfo;

                if (propertyValue == null)
                {
                    currPropertyInfo.SetValue(entity, null, null);
                    continue;
                }

                if (propertyValue.GetType() == currAttribute.PropertyType)
                {
                    currPropertyInfo.SetValue(entity, propertyValue, null);
                }
                else
                {
                    object result = funcProvider.DynamicInvoke(currAttribute.PropertyType, (propertyValue ?? string.Empty).ToString());
                    currPropertyInfo.SetValue(entity, result, null);
                }
            }

            return entity;
        }
    }
}
