using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml.Serialization;
using System.Net;
using MODEL;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Mail;
using System.ComponentModel;

namespace DAL
{
    public static class Tools
    {
        private static object lockIPN = new object();
        public static void WriteLog(string path, string log)
        {
            lock (lockIPN)
            {
                string domain = AppDomain.CurrentDomain.BaseDirectory;
                if (!path.StartsWith(domain))
                {
                    path = domain + path;
                }
                File.AppendAllText(path, log);
            }
        }

        public static string _QueryStringKey = "hyper556"; //URL传输参数加密Key
        public static string _QueryStringKey2 = "hypers56"; //URL传输参数加密Key2（加密数字参数用）
        public static string _QueryStringKey3 = "hyper139"; //URL传输参数加密Key3（资料ID专用）

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();//把字符串放到byte数组中

            byte[] inputByteArray = Encoding.Default.GetBytes(pToEncrypt);

            des.Key = Encoding.ASCII.GetBytes(sKey);//建立加密对象的密钥和偏移量
            des.IV = Encoding.ASCII.GetBytes(sKey);//原文使用ASCIIEncoding.ASCII方法的GetBytes方法
            MemoryStream ms = new MemoryStream();//使得输入密码必须输入英文文本
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        public static void WriteResponse(HttpResponse response, object p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = Encoding.ASCII.GetBytes(sKey);//建立加密对象的密钥和偏移量，此值重要，不能修改
            des.IV = Encoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();//建立StringBuilder对象，CreateDecrypt使用的是流对象，必须把解密后的文本变成流对象

            return Encoding.Default.GetString(ms.ToArray());
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string EncryptMD5(String input)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length]; System.Array.Copy(res, temp, res.Length);
            return new String(temp);
        }

        public static string HtmlEncode(string str)
        {
            str = str.Replace(" ", "&nbsp;");
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\'", "&apos;");
            str = str.Replace("\"", "&quot;");
            //str = str.Replace("/", "&#47;");
            return str;
        }

        public static string HtmlDecode(string str)
        {
            str = str.Replace("&nbsp;", " ");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&apos;", "\'");
            str = str.Replace("&quot;", "\"");
            //str = str.Replace("/", "&#47;");
            return str;
        }

        public static string HtmlEncodeWithoutSpace(string str)
        {
            str = str.Replace("<", "&lt;");
            str = str.Replace(">", "&gt;");
            str = str.Replace("\'", "&apos;");
            str = str.Replace("\"", "&quot;");
            return str;
        }

        public static string HtmlDecodeWithoutSpace(string str)
        {
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&apos;", "\'");
            str = str.Replace("&quot;", "\"");
            //str = str.Replace("/", "&#47;");
            return str;
        }

        #region 按字符串分隔字符串, 并去除空的部分
        public static List<string> SplitByStr(string sour, string spli)
        {
            return sour.Split(new string[] { spli }, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        #endregion

        #region 截取字符串后几位
        /// <summary>
        /// 截取字符串后几位
        /// </summary>
        /// <param name="str">要截取的字符串</param>
        /// <param name="num">返回的具体位数</param>
        /// <returns>返回结果的字符串</returns>
        public static string GetLastStr(string str, int num)
        {
            int count = 0;
            if (str.Length > num)
            {
                count = str.Length - num;
                str = str.Substring(count, num);
            }
            return str;
        }
        #endregion

        #region 将数字按对应单位省略
        /// <summary>
        /// 将数字按对应单位省略
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static List<string> SimplifiedNum(int num)
        {
            List<string> result = new List<string>();
            string strNum = num.ToString();
            int Length = strNum.Length;
            if (Length > 3)
            {
                int Count = (Length - 1) / 3;
                double sNum = num / Math.Pow(10, (Count * 3) - 1);
                sNum = Math.Floor(sNum);
                sNum = sNum / 10;
                string Unit = "";
                switch (Count)
                {
                    case 1: Unit = "k"; break;
                    case 2: Unit = "mil"; break;
                    default: Unit = "bn"; break;
                }
                result.Add(sNum.ToString());
                result.Add(Unit);
                return result;
            }
            result.Add(num.ToString());
            return result;
        }
        #endregion

        #region 随机x位的生成字符串
        public static string GetString(int count)
        {
            string checkCode = string.Empty; //存放随机码的字符串   
            Random random = new Random();

            for (int i = 0; i < count; i++) //产生count位校验码
            {
                int number = random.Next(62);
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57   
                }
                else if (number < 36)
                {
                    number += 55;    //字母A-Z编码在65-90   
                }
                else
                {
                    number += 61;    //字母a-z编码在97-122   
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }
        #endregion

        #region 重写页面内容
        public static void WriteResponse(HttpResponse Response, string res)
        {
            Response.Clear();
            Response.ContentType = "text/html";
            Response.Write(res);
            Response.End();
        }
        #endregion

        #region 一般处理程序输出内容
        public static void WriteResponseAshx(HttpResponse Response, string res)
        {
            Response.ContentType = "text/html";
            Response.Write(res);
        }
        #endregion

        #region 随机生成算数验证码
        public static void Captcha(ref int x, ref int y, ref int cal)
        {
            Random rd = new Random();
            x = rd.Next(1, 10);
            y = rd.Next(1, 10);
            cal = rd.Next(1, 3);
            if (x < y)
            {
                cal = 1;
            }
        }
        #endregion

        #region 将html文本转化为 文本内容方法TextNoHTML
        /// <summary>
        /// 将html文本转化为 文本内容方法TextNoHTML
        /// </summary>
        /// <param name="Htmlstring">HTML文本值</param>
        /// <returns></returns>
        public static string TextNoHTML(string Htmlstring)
        {
            if (string.IsNullOrEmpty(Htmlstring))
            {
                return "";
            }
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([/r/n])[/s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "/", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "/xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "/xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "/xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "/xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(/d+);", "", RegexOptions.IgnoreCase);
            //替换掉 < 和 > 标记
            Htmlstring = Htmlstring.Replace("<", "");
            Htmlstring = Htmlstring.Replace(">", "");
            Htmlstring = Htmlstring.Replace("\r\n", "");
            Htmlstring = Htmlstring.Replace("\r", "");
            Htmlstring = Htmlstring.Replace("\n", "");
            //返回去掉html标记的字符串
            return Htmlstring;
        }
        #endregion

        #region 生成短连接
        //public static string GetShortURL(string url)
        //{
        //    string err = "";
        //    string res = url;
        //    for (int i = 0; i < 3; i++)
        //    {
        //        string tmp = MyTwitterAPI.ShortenUrl.ShortenUrlByBitly(url, false, "c4f993ab7f3b1c213fbf9ed4d090c5c09e32daa8", "", ref err);
        //        if (!string.IsNullOrEmpty(tmp))
        //        {
        //            res = tmp;
        //            break;
        //        }
        //    }
        //    return res;
        //}
        #endregion

        #region XML序列化和反序列化
        #region 反序列化
        //// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static object XMLDeserialize(Type type, string xml)
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(type);
                    return xmldes.Deserialize(sr);
                }
            }
            catch
            {
                return null;
            }
        }
        /**/
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type"></param>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static object XMLDeserialize(Type type, Stream stream)
        {
            XmlSerializer xmldes = new XmlSerializer(type);
            return xmldes.Deserialize(stream);
        }
        #endregion

        #region 序列化
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string XMLSerializer(Type type, object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(type);
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();

            sr.Dispose();
            Stream.Dispose();

            return str;
        }
        #endregion
        #endregion

        #region 获取文件的MD5码
        public static string GetMD5HashFromFile(string fileName)
        {
            FileStream file = null;
            string returnStr = "";
            try
            {

                file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                returnStr = sb.ToString();
            }
            catch (Exception ex)
            {
                returnStr = "Get MD5 error : " + ex.Message;
            }
            finally
            {
                if (file != null)
                {
                    file.Close();
                    file.Dispose();
                }
            }
            return returnStr;
        }
        #endregion

        #region 登录保存用户信息
        public static Users GetUserByCookie(HttpCookie ck)
        {
            if (ck != null)
            {
                UserDAO ud = new UserDAO();
                try
                {
                    string tmp = Tools.Decrypt(ck.Value, Tools._QueryStringKey);
                    string[] tmps = tmp.Split('|');
                    Users u = new Users();
                    u.UseName = tmps[0];
                    u.Pwd = tmps[1];
                    u = ud.LoginUser(u);
                    return u;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        #endregion

        #region 登录保存用户信息
        public static UserExtend GetUserExtendByCookie(HttpCookie ck)
        {
            try
            {
                return JsonConvert.DeserializeObject<UserExtend>(Decrypt(ck.Value.ToString(), _QueryStringKey3));
            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region unix时间戳转换（毫秒）
        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return Convert.ToInt64((dateTime - start).TotalMilliseconds);
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0);
            return start.AddMilliseconds(timestamp);
        }
        #endregion

        #region 判断字符串是否为空（去空格）
        public static bool IsStrEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return true;
            }
            else if("".Equals(str.Trim()))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 检测链接是否有效
        /// <summary>
        /// 检测链接是否有效
        /// </summary>
        /// <param name="url">链接地址</param>
        /// <param name="timeout">超时时间（毫秒）</param>
        /// <returns></returns>
        public static bool CheckWebURL(string url, int timeout)
        {
            HttpWebRequest req = null;
            try
            {
                req = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
                req.Method = "HEAD";
                req.Timeout = timeout;
                HttpWebResponse res = (HttpWebResponse)req.GetResponse();
                return res.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (req != null)
                {
                    req.Abort();
                    req = null;
                }
            }
        }
        #endregion

        /// <summary>
        /// 获取工作平台@用户的字符串
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static List<string> GetAtList(string msg)
        {
            List<string> m = new List<string>();
            Regex regEm = new Regex(@"@user[^\s]+\s");
            var g = regEm.Matches(msg);
            for (int i = 0; i < g.Count; i++)
            {
                if (g[i].Groups[0].Value.StartsWith("@users"))
                {
                    continue;
                }
                m.Add(g[i].Groups[0].Value);
            }
            return m;
        }

        #region 获取评论中所含的任务链接
        public static List<string> GetUrlTaskList(string msg)
        {
            List<string> m = new List<string>();
            Regex regEm = new Regex(@"\[#task\-[^\s]+\s");
            var g = regEm.Matches(msg);
            for (int i = 0; i < g.Count; i++)
            {
                m.Add(g[i].Groups[0].Value);
            }
            return m;
        }
        #endregion

        #region 获取评论回复字符串
        public static string GetChild(string msg)
        {
            string Back = "";
            Regex regEm = new Regex(@"回复 user[^\s]+\s");
            var g = regEm.Matches(msg);
            if (g.Count == 1)
            {
                Back = g[0].Groups[0].Value;
            }
            return Back;
        }

        #endregion

        #region 转码
        public static string Ma(string mess)
        {
            return Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(mess));
        }
        #endregion
        
        /// <summary>
        /// 图片处理
        /// </summary>
        /// <param name="path"></param>
        /// <param name="fileSaveUrl"></param>
        /// <param name="maxWidth"></param>
        /// <param name="maxHeight"></param>
        /// <returns></returns>
        public static string Zoom(string path, string fileSaveUrl, int maxWidth, int maxHeight, int MIN_VALUE, int CLIP_VALUE, double CLIP_RATIO)
        {
            var urlName = path.Split('/').LastOrDefault();
            FileStream fileStream = new FileStream(HttpContext.Current.Server.MapPath(path), FileMode.OpenOrCreate, FileAccess.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);
            //将图片流嵌入image
            System.Drawing.Image initImage = System.Drawing.Image.FromStream(stream, true);
            //图片小于模板直接保存
            if (initImage.Width <= maxWidth && initImage.Height <= maxHeight)
            {
                fileSaveUrl = fileSaveUrl + urlName.Split('.')[0] + "_" + initImage.Width + "×" + initImage.Height + "." + urlName.Split('.').LastOrDefault();
                initImage.Save(HttpContext.Current.Server.MapPath(fileSaveUrl));
                return fileSaveUrl;
            }
            else
            {
                ////模版的宽高比例
                double templateRate = (double)maxWidth / maxHeight;//(width:640,height:400)
                ////图片的宽高比例
                double initRate = (double)initImage.Width / initImage.Height;
                if (initImage.Width <= maxWidth)
                {
                    maxWidth = initImage.Width;
                    maxHeight = initImage.Height;
                }
                else
                {
                    //对模板进行判断
                    if (initRate < templateRate)
                    {
                        if ((maxWidth / maxHeight) * initImage.Height < MIN_VALUE)
                        {
                            maxHeight = CLIP_VALUE;
                            maxWidth = (int)(CLIP_VALUE / CLIP_RATIO);
                        }
                    }
                    else if (initImage.Width / initImage.Height > maxWidth / MIN_VALUE)
                    {
                        if (initImage.Height > CLIP_VALUE)
                        {
                            maxHeight = CLIP_VALUE;
                            maxWidth = 300;
                        }
                        else if (initImage.Height <= CLIP_VALUE)
                        {
                            maxHeight = initImage.Height;
                            maxWidth = (int)(initImage.Height * CLIP_RATIO);
                        }
                    }
                    else
                    {
                        maxHeight = (int)(maxWidth / initRate);
                    }
                }
                templateRate = (double)maxWidth / maxHeight;
                fileSaveUrl = fileSaveUrl + urlName.Split('.')[0] + "_" + maxWidth + "×" + maxHeight + "." + urlName.Split('.').LastOrDefault();
                //图片与模版比例相等，直接等比例缩放
                if (templateRate == initRate)
                {
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                    templateImage.Save(HttpContext.Current.Server.MapPath(fileSaveUrl));
                }
                //图片与模版比例不等，裁剪后缩放
                else
                {
                    //裁剪对象
                    System.Drawing.Image pickedImage = null;
                    System.Drawing.Graphics pickedG = null;

                    //定位
                    Rectangle fromR = new Rectangle(0, 0, 0, 0);//图片裁剪定位
                    Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                    //宽为标准进行裁剪
                    if (templateRate > initRate)
                    {
                        //裁剪对象
                        pickedImage = new System.Drawing.Bitmap(initImage.Width, (int)System.Math.Floor(initImage.Width / templateRate));
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        //裁剪位置定位
                        fromR.X = 0;
                        fromR.Y = (int)System.Math.Floor((initImage.Height - initImage.Width / templateRate) / 2);
                        fromR.Width = initImage.Width;
                        fromR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                        //裁剪目标定位
                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = initImage.Width;
                        toR.Height = (int)System.Math.Floor(initImage.Width / templateRate);
                    }
                    //高为标准进行裁剪
                    else
                    {
                        pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(initImage.Height * templateRate), initImage.Height);
                        pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                        fromR.X = (int)System.Math.Floor((initImage.Width - initImage.Height * templateRate) / 2);
                        fromR.Y = 0;
                        fromR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        fromR.Height = initImage.Height;

                        toR.X = 0;
                        toR.Y = 0;
                        toR.Width = (int)System.Math.Floor(initImage.Height * templateRate);
                        toR.Height = initImage.Height;
                    }

                    //设置质量(最高)
                    pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    //裁剪图片
                    pickedG.DrawImage(initImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);
                    //按模版大小生成最终图片
                    System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                    System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                    templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                    templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;
                    templateG.Clear(Color.White);
                    templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //保存缩略图
                    templateImage.Save(HttpContext.Current.Server.MapPath(fileSaveUrl));

                    //释放资源
                    templateG.Dispose();
                    templateImage.Dispose();

                    pickedG.Dispose();
                    pickedImage.Dispose();
                }
            }

            //释放资源
            initImage.Dispose();
            return fileSaveUrl;
        }

        public static int GetStringLengthNoHTML(string text)
        {
            if (string.IsNullOrEmpty(text))
                text = "";
            Regex reg = new Regex(@"");
            text = reg.Replace(text, "");
            return text.Length;
        }

        /// <summary>
        /// 忽略HTML标签进行字符串裁剪
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxlenth"></param>
        /// <returns></returns>
        public static string SubStringNoHTML(string text, int maxlenth)
        {
            string newtext = "";
            int length = 0;
            Stack<int> xxx = new Stack<int>();
            for (int i = 0; i < text.Length; i++)
            {
                char ch = text[i];

                if (ch == '<')
                {
                    xxx.Push(i);
                    newtext += ch;
                }
                else if (ch == '>')
                {
                    if (xxx.Count > 0)
                    {
                        xxx.Pop();
                    }
                    newtext += ch;
                }
                else if (xxx.Count > 0)
                {
                    newtext += ch;
                }
                else
                {
                    if (length < maxlenth)
                    {
                        newtext += ch;
                        length++;
                    }
                }
            }

            //去除最后一个文字内容后的空标签
            //string[] array = Regex.Split(newtext, @"[^>\s]<");
            //string tmp = "<" + array.Last();
            //newtext = newtext.Replace(tmp, "");
            //newtext += DelEmptyLab("<" + array.Last());

            newtext = Regex.Replace(newtext, @"<img.*?>", "");
            newtext = Regex.Replace(newtext, @"<br.*?>", " ");
            newtext = Regex.Replace(newtext, @"<hr.*?>", "");

            return newtext;
        }

        /// <summary>
        /// 去除空标签
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string DelEmptyLab(string html)
        {
            Regex reg = new Regex(@"<([a-z]+?)(?:\s+?[^>]*?)?>\s*?<\/\1>");
            string tmp = reg.Replace(html, "");
            if(tmp != html)
            {
                html = DelEmptyLab(tmp);
            }
            return html;
        }

        /// <summary>
        /// 将所有a标签替换成span
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string ReplaceLabA(string html)
        {
            html = Regex.Replace(html, @"<a.*?>", "<span class=\"lab-a\">");
            html = html.Replace("</a>", "</span>");
            html = html.Replace("</ a>", "</span>");
            return html;
        }

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="codeName">加密采用的编码方式</param>
        /// <param name="source">待加密的明文</param>
        /// <returns></returns>
        public static string EncodeBase64(Encoding encoding, string source)
        {
            string encode = "";
            byte[] bytes = encoding.GetBytes(source);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = source;
            }
            return encode;
        }

        /// <summary>
        /// Base64加密，采用utf8编码方式加密
        /// </summary>
        /// <param name="source">待加密的明文</param>
        /// <returns>加密后的字符串</returns>
        public static string EncodeBase64(string source)
        {
            return EncodeBase64(Encoding.UTF8, source);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="codeName">解密采用的编码方式，注意和加密时采用的方式一致</param>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(Encoding encoding, string result)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(result);
            try
            {
                decode = encoding.GetString(bytes);
            }
            catch
            {
                decode = result;
            }
            return decode;
        }

        /// <summary>
        /// Base64解密，采用utf8编码方式解密
        /// </summary>
        /// <param name="result">待解密的密文</param>
        /// <returns>解密后的字符串</returns>
        public static string DecodeBase64(string result)
        {
            return DecodeBase64(Encoding.UTF8, result);
        }

        /// <summary>
        /// 获取质量，效率评分的评定阶级
        /// </summary>
        /// <param name="score">分数</param>
        /// <returns></returns>
        public static string GetScoreName(int score)
        {
            if (score < 3)
                return "很低";
            else if (score < 5)
                return "低";
            else if (score < 7)
                return "正常";
            else if (score < 9)
                return "高";
            else if (score == 9)
                return "很高";
            else
                return "完美";
        }

        /// <summary>
        /// 获取部门树的所有子级HTML结构
        /// </summary>
        /// <param name="rch"></param>
        /// <returns></returns>
        public static string GetReachSubsTree(Rearches rch)
        {
            string res = "";
            if (rch.Subs.Count > 0)
            {
                res += "<ul>";
                foreach (Rearches sub in rch.Subs)
                {
                    res += "<li data-value=\"" + sub.Reach.RId + "\"><div>" + sub.Reach.Name + "</div>";
                    res += GetReachSubsTree(sub);
                    res += "</li>";
                }
                res += "</ul>";
            }
            return res;
        }
    }
}
