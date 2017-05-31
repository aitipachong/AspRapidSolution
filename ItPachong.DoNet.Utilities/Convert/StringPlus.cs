// ********************************************************************
// * 项目名称：		    ItPachong.DoNet.Utilities
// * 程序集名称：	    ItPachong.DoNet.Utilities.Convert
// * 文件名称：		    StringPlus.cs
// * 编写者：		    赖强
// * 编写日期：		    2017-05-15
// * 程序功能描述：
// *        字符串转换扩展类
// *            1、GetStrArray(string str, char speater, bool toLower)  把字符串按照分隔符转换成 List
// *            2、GetStrArray(string str) 把字符串转 按照,分割换为数据
// *            3、GetArrayStr(List list, string speater) 把 List 按照分隔符组装成 string
// *            4、GetArrayStr(List list)  得到数组列表以逗号分隔的字符串
// *            5、GetArrayValueStr(Dictionary<int, int> list)得到数组列表以逗号分隔的字符串
// *            6、DelLastComma(string str)删除最后结尾的一个逗号
// *            7、DelLastChar(string str, string strchar)删除最后结尾的指定字符后的字符
// *            8、ToSBC(string input)转全角的函数(SBC case)
// *            9、ToDBC(string input)转半角的函数(SBC case)
// *            10、GetSubStringList(string o_str, char sepeater)把字符串按照指定分隔符装成 List 去除重复
// *            11、GetCleanStyle(string StrList, string SplitString)将字符串样式转换为纯字符串
// *            12、GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)将字符串转换为新样式
// *            13、SplitMulti(string str, string splitstr)分割字符串
// *            14、SqlSafeString(string String, bool IsDel)
// *
// * 程序变更日期：
// * 程序变更者：
// * 变更说明：
// * 
// ********************************************************************
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ItPachong.DoNet.Utilities.Convert
{
    /// <summary>
    /// 字符串转换扩展类
    /// 1、GetStrArray(string str, char speater, bool toLower)  把字符串按照分隔符转换成 List
    /// 2、GetStrArray(string str) 把字符串转 按照,分割换为数据
    /// 3、GetArrayStr(List list, string speater) 把 List 按照分隔符组装成 string
    /// 4、GetArrayStr(List list)  得到数组列表以逗号分隔的字符串
    /// 5、GetArrayValueStr(Dictionary<int, int> list)得到数组列表以逗号分隔的字符串
    /// 6、DelLastComma(string str)删除最后结尾的一个逗号
    /// 7、DelLastChar(string str, string strchar)删除最后结尾的指定字符后的字符
    /// 8、ToSBC(string input)转全角的函数(SBC case)
    /// 9、ToDBC(string input)转半角的函数(SBC case)
    /// 10、GetSubStringList(string o_str, char sepeater)把字符串按照指定分隔符装成 List 去除重复
    /// 11、GetCleanStyle(string StrList, string SplitString)将字符串样式转换为纯字符串
    /// 12、GetNewStyle(string StrList, string NewStyle, string SplitString, out string Error)将字符串转换为新样式
    /// 13、SplitMulti(string str, string splitstr)分割字符串
    /// 14、SqlSafeString(string String, bool IsDel)
    /// </summary>
    public class StringPlus
    {
        /// <summary>
        /// 把字符串按照指定分隔符转换成 List<string>
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符字符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach(string s in ss)
            {
                if(!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower) strVal = s.ToLower();
                    list.Add(strVal);
                }
            }

            return list;
        }

        /// <summary>
        /// 把字符串按“，”分割为字符串数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] GetStrArray(string str)
        {
            return str.Split(new char[] { ',' });
        }

        /// <summary>
        /// 把List<string>按照分隔符组装成string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < list.Count; i++)
            {
                if(i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 把List<object>按照","组装成string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<object> list)
        {
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < list.Count; i++)
            {
                if(i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i].ToString());
                    sb.Append(",");
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 把Dictionary<int, object>键值对集合中的值用“，”分隔符组装成字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, object> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach(KeyValuePair<int, object> kvp in list)
            {
                sb.Append(kvp.Value.ToString() + ",");
            }

            if(list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 删除字符串末尾的逗号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }

        /// <summary>
        /// 删除字符串末尾的指定字符
        /// </summary>
        /// <param name="str"></param>
        /// <param name="lastChar"></param>
        /// <returns></returns>
        public static string DelLastChar(string str, string lastChar)
        {
            return str.Substring(0, str.LastIndexOf(lastChar));
        }

        /// <summary>
        /// 半角转全角函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角
            char[] c = input.ToCharArray();
            for(int i = 0; i < c.Length; i++)
            {
                if(c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if(c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }

            return new string(c);
        }

        /// <summary>
        /// 全角转半角函数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for(int i = 0; i < c.Length; i++)
            {
                if(c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if(c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }

            return new string(c);
        }

        /// <summary>
        /// 把字符串按照指定分隔符拆分成List<string>
        /// </summary>
        /// <param name="o_str"></param>
        /// <param name="sepeater"></param>
        /// <returns></returns>
        public static List<string> GetSubStringList(string o_str, char sepeater)
        {
            List<string> list = new List<string>();
            string[] ss = o_str.Split(sepeater);
            foreach(string s in ss)
            {
                if(!string.IsNullOrEmpty(s) && s != sepeater.ToString())
                {
                    list.Add(s);
                }
            }

            return list;
        }

        /// <summary>
        /// 将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="splitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string strList, string splitString)
        {
            string returnValue = "";
            if (string.IsNullOrEmpty(strList))
            {
                returnValue = "";
            }
            else
            {
                string newString = "";
                newString = strList.Replace(splitString, "");
                returnValue = newString;
            }

            return returnValue;
        }

        /// <summary>
        /// 将字符串转换为新样式
        /// </summary>
        /// <param name="strList"></param>
        /// <param name="newStyle"></param>
        /// <param name="splitString"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static string GetNewStyle(string strList, string newStyle, string splitString, out string error)
        {
            string returnValue = "";
            if(string.IsNullOrEmpty(strList))
            {
                returnValue = "";
                error = "请输入需要划分格式的字符串";
            }
            else
            {
                int strListLength = strList.Length;
                int newStyleLength = GetCleanStyle(newStyle, splitString).Length;
                if(strListLength != newStyleLength)
                {
                    returnValue = "";
                    error = "样式格式的长度与输入的字符创长度不符，请重新输入";
                }
                else
                {
                    string lengStr = "";
                    for(int i = 0; i < newStyle.Length; i++)
                    {
                        if(newStyle.Substring(i, 1) == splitString)
                        {
                            lengStr = lengStr + "," + i;
                        }
                    }
                    if (lengStr != "") lengStr = lengStr.Substring(1);

                    string[] str = lengStr.Split(',');
                    foreach(string bb in str)
                    {
                        strList = strList.Insert(int.Parse(bb), splitString);
                    }

                    returnValue = strList;
                    error = "";
                }
            }

            return returnValue;
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static string[] SplitMulti(string str, string splitStr)
        {
            string[] strArray = null;
            if((str != null) && (str != ""))
            {
                strArray = new System.Text.RegularExpressions.Regex(splitStr).Split(str);
            }
            return strArray;
        }

        /// <summary>
        /// SQL安全字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="isDel"></param>
        /// <returns></returns>
        public static string SqlSafeString(string str, bool isDel)
        {
            if(isDel)
            {
                str = str.Replace("'", "");
                str = str.Replace("\"", "");
                return str;
            }
            str = str.Replace("'", "&#39;");
            str = str.Replace("\"", "&#34;");
            return str;
        }

        /// <summary>
        /// 获取正确的Id，如果不是正整数，返回0
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static int StrToId(string _value)
        {
            if (IsNumberId(_value))
                return int.Parse(_value);
            else
                return 0;
        }

        /// <summary>
        /// 检查一个字符串是否是纯数字构成，一般用于查询字符串参数的有效性验证（0除外）
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool IsNumberId(string _value)
        {
            return QuickValidate("^[1-9]*[0-9]*$", _value);
        }

        /// <summary>
        /// 快速验证一个字符串是否符合指定的正则表达式。
        /// </summary>
        /// <param name="_express"></param>
        /// <param name="_value"></param>
        /// <returns></returns>
        public static bool QuickValidate(string _express, string _value)
        {
            if (_value == null) return false;
            System.Text.RegularExpressions.Regex myRegex = new System.Text.RegularExpressions.Regex(_express);
            if(_value.Length == 0)
            {
                return false;
            }

            return myRegex.IsMatch(_value);
        }
    }
}