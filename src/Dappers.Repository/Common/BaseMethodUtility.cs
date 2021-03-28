using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;

namespace Dappers.Repository
{
    public static class BaseMethodUtility
    {
        /// <summary>
        /// Dapper - 生成插入语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetCreateSql<T>()
        {
            IEnumerable<string> fields = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute))).Select(p => p.Name);
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            string fieldNames = string.Join(", ", fields);
            string fieldParameters = string.Join(", @", fields);
            string sql = $"INSERT INTO [dbo].[{tableName}]({fieldNames}) VALUES(@{fieldParameters})";
            return sql;
        }
        /// <summary>
        /// 批量新增 ，大数据 新增使用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetRoutineCreateSql<T>(List<T> list)
        {
            //声明字段列表
            List<string> listFiledList = new List<string>();

            var strsql = new StringBuilder();
            IEnumerable<string> fields = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute))).Select(p => p.Name);
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            string fieldNames = string.Join(", ", fields);
            strsql.Append($"INSERT INTO [dbo].[{tableName}]({fieldNames}) VALUES");
            //循环存储
            foreach (var item in list)
            {
                //声明值列表
                List<string> listValueList = new List<string>();
                var fieldList = fields.ToList();
                listValueList = GetFieldValue(fieldList, item);
                string strValueText = string.Join(",", listValueList);
                strsql.AppendFormat("({0}),", strValueText);
            }
            var inserSql = strsql.ToString();
            var sql = inserSql.Substring(0, inserSql.LastIndexOf(','));
            return sql;
        }

        /// <summary>
        /// Dapper - 生成更新语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetUpdateSql<T>()
        {
            var strsql = new StringBuilder();
            IEnumerable<string> fields = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType == typeof(KeyAttribute))).Select(p => p.Name);
            string tableId = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType != typeof(ExplicitKeyAttribute))).Select(p => p.Name).First();
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            strsql.Append($"UPDATE [dbo].[{tableName}] SET");
            var fieldList = fields.ToList();
            for (int i = 0; i < fieldList.Count(); i++)
            {
                strsql.AppendFormat(" {0}=@{0},", fieldList.ToList()[i]);
            }
            strsql.AppendFormat(" WHERE {0}=@{0}", tableId);
            var inserSql = strsql.ToString();
            var sql = inserSql.Substring(0, inserSql.LastIndexOf(','));
            sql += inserSql.Substring(inserSql.LastIndexOf(',') + 1);
            return sql;
        }

        /// <summary>
        /// Dapper - 生成删除语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetDeleteSql<T>(string KeyValue = "")
        {
            var strsql = new StringBuilder();
            string tableId = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType != typeof(ExplicitKeyAttribute))).Select(p => p.Name).First();
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            strsql.Append($"DELETE FROM [dbo].[{tableName}] ");
            if (string.IsNullOrWhiteSpace(KeyValue))
            {
                strsql.AppendFormat(" WHERE {0}=@{0}", tableId);
            }
            else
            {
                strsql.AppendFormat(" WHERE {0}={1}", tableId, KeyValue);
            }
            var sql = strsql.ToString();
            return sql;
        }

        /// <summary>
        /// Dapper - 生成查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetQuerySql<T>(string KeyValue = "")
        {
            var strsql = new StringBuilder();
            string tableId = typeof(T).GetProperties().
                    SkipWhile(p => p.CustomAttributes.Any(a => a.AttributeType != typeof(ExplicitKeyAttribute))).Select(p => p.Name).First();
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            strsql.Append($"SELECT * FROM [dbo].[{tableName}] ");
            if (string.IsNullOrWhiteSpace(KeyValue))
            {
                strsql.AppendFormat(" WHERE {0}=@{0}", tableId);
            }
            else
            {
                strsql.AppendFormat(" WHERE {0}={1}", tableId, KeyValue);
            }
            var sql = strsql.ToString();
            return sql;
        }

        /// <summary>
        /// Dapper - 生成查询语句
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetQueryListSql<T>()
        {
            var strsql = new StringBuilder();
            string tableName = ((Dapper.Contrib.Extensions.TableAttribute)typeof(T).
                GetCustomAttributes(typeof(Dapper.Contrib.Extensions.TableAttribute), true).First()).Name;
            strsql.Append($"SELECT * FROM [dbo].[{tableName}] ");
            var sql = strsql.ToString();
            return sql;
        }

        internal static List<string> GetFieldValue(List<string> fieldList, object item)
        {
            //声明值列表
            List<string> listValueList = new List<string>();
            foreach (var dtColumn in fieldList)
            {
                int i = fieldList.IndexOf(dtColumn);
                var properInfo = item.GetType().GetProperty(dtColumn);
                Type type = properInfo.PropertyType;
                object FieldValue = properInfo.GetValue(item, null);
                var ttt = GetFieldValue(type, FieldValue);
                listValueList.Add(ttt);
            }
            return listValueList;
        }
        internal static string GetFieldValue(Type type, object val)
        {
            //如果属性不存在，返回 null
            if (val == null)
                return "null";
            string FieldValue = val.ToString();
            //根据属性的类型决定是否加“双引号”
            if (type.IsGenericType)
            {
                if (type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    Type originalType = type.GetGenericArguments()[0];
                    if (originalType == typeof(int) || originalType == typeof(byte))
                    {
                        return FieldValue;
                    }
                    else if (originalType == typeof(DateTime))
                    {
                        return "'" + FieldValue + "'";
                    }
                    else
                    {
                        return "null";
                    }
                }
            }
            if ((type == typeof(int) || type == typeof(byte)))
            {
                return FieldValue;
            }
            else if (type == typeof(string))
            {
                return "'" + FieldValue + "'";
            }
            else if (type == typeof(DateTime))
            {
                return "'" + FieldValue + "'";
            }
            else
            {
                return "null";
            }
        }
    }
}
