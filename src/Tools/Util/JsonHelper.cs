using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Tools.Util
{
    public static class JsonHelper
    {
        public static object ToJson(this string Json)
        {
            return (Json == null) ? null : JsonConvert.DeserializeObject(Json);
        }

        /// <summary>
        /// 转为json格式，其中时间格式为yyyy-MM-dd HH:mm:ss
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            return ToJson(obj, "yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// 转为Json格式
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="datetimeformats">自定义时间格式</param>
        /// <returns></returns>
        public static string ToJson(this object obj,string datetimeformats)
        {
            IsoDateTimeConverter isoDateTimeConverter = new IsoDateTimeConverter()
            {
                DateTimeFormat = datetimeformats
            };
            return JsonConvert.SerializeObject(obj, isoDateTimeConverter);
        }

        public static T ToObject<T>(this string Json)
        {
            return (Json == null) ? default(T) : JsonConvert.DeserializeObject<T>(Json);
        }

        public static List<T> ToList<T>(this string Json)
        {
            return (Json == null) ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }

        public static DataTable ToTable(this string Json)
        {
            return (Json == null) ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }

        public static JObject ToJobject(this string Json)
        {
            return (Json == null) ? JObject.Parse("{}") : JObject.Parse(
                Json.Replace("&nbsp;",""));
        }

        /// <summary>
        /// Json特符字符过滤，参见http://www.json.org/
        /// </summary>
        /// <param name="sourceStr">要过滤的源字符串</param>
        /// <returns>返回过滤的字符串</returns>
        public static string JsonCharFilter(string sourceStr)
        {
            sourceStr = sourceStr.Replace("\\", "\\\\");
            sourceStr = sourceStr.Replace("\b", "");
            sourceStr = sourceStr.Replace("\t", "");
            sourceStr = sourceStr.Replace("\n", "");
            sourceStr = sourceStr.Replace("\f", "");
            sourceStr = sourceStr.Replace("\r", "");
            sourceStr = sourceStr.Replace("\"", "\\\"");
            sourceStr = sourceStr.Replace("'", "\'");
            sourceStr = sourceStr.Replace("&", "");
            return sourceStr;
        }
    }
}
