using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Core.Sql
{
    public class DynamSqlFormater<T> where T : new()
    {
        private readonly PropertyInfo[] _properties;

        public List<string> Ignored { get; set; }

        public DynamSqlFormater()
        {
            var type = typeof(T);

            _properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }

        private string GetInsert()
        {
            var table = GetTableName();
            var builder = new StringBuilder($"INSERT INTO `{table}` (");
            string column;

            foreach (var property in _properties)
            {
                column = GetColumnName(property);
                if (!(Ignored ?? new List<string>()).Any(i => property.Name.Equals(i)))
                {
                    builder.Append($"`{column}`, ");
                }
            }
            builder.Remove(builder.Length - 2, 2).Append(") VALUES ");

            return builder.ToString();
        }

        private string GetValues(T @class)
        {
            var sJson = JsonSerializer.Serialize(@class);
            var json = JObject.Parse(sJson);
            var builder = new StringBuilder("(");

            foreach (var property in _properties)
            {
                if (!(Ignored ?? new List<string>()).Any(i => property.Name.Equals(i)))
                {
                    builder.Append($"{GetValue(@json, property.Name)}, ");
                }
            }
            builder.Remove(builder.Length - 2, 2).Append(")");

            return builder.ToString();
        }

        private static string GetValue(JObject json, string name)
        {
            var value = json[name].ToString();

            switch (json[name].Type)
            {
                case JTokenType.String:
                    value = $"'{value.Replace("'", "''")}'";
                    break;
                case JTokenType.Date:
                    value = DateTime.Parse(value).ToString("yyyy-MM-dd HH:mm:ss");
                    value = $"'{value.Replace("'", "''")}'";
                    break;
                case JTokenType.Float:
                    value = value.Replace(",", ".");
                    break;
                case JTokenType.Boolean:
                    value = value.Equals("True") ? "1" : "0";
                    break;
            }

            return value;
        }

        public string GetInsertIntoQuery(IReadOnlyList<T> data)
        {
            var sql = new StringBuilder();

            for (var i = 0; i < data.Count(); i++)
            {
                if ((i % 1000) == 0)
                {
                    sql.Append(GetInsert());
                }
                sql.Append($"{GetValues(data[i])},");   //TODO: надо исправить работу с JSON. Много жрет памяти и долго выпоянется 
            }

            return sql.Replace(",INSERT INTO", ";INSERT INTO").Remove(sql.Length - 1, 1).ToString();
        }

        private string GetColumnName(PropertyInfo property)
        {
            foreach (var attribute in property.GetCustomAttributes(true))
            {
                if (attribute is ColumnAttribute column)
                {
                    return column.Name;
                }
            }
            return property.Name.ToLower();
        }

        private string GetTableName()
        {
            foreach (var attribute in typeof(T).GetCustomAttributes())
            {
                if (attribute is TableAttribute column)
                {
                    return column.Name;
                }
            }
            return new T().ToString()?.Split('.').Last();
        }
    }
}
