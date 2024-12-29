using System;
using System.IO;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


//Aria2详细信息请看
//https://aria2.github.io/manual/en/html/aria2c.html#rpc-interface

namespace aria2c
{
    //定义一个类用于转码成JSON字符串发送给Aria2 json-rpc接口
    public class JsonClass
    {
        public String _jsonrpc { get; set; }
        public String _id { get; set; }
        public String _method { get; set; }
        public List<String> _params { get; set; }
        
    }
 
    public class Aria2
    {
        public static ClientWebSocket webSocket;          //用于连接到Aria2Rpc的客户端
        public static CancellationToken cancellationToken; //传播有关应取消操作的通知
 
        /// <summary>
        /// 连接Aria2Rpc服务器
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<bool> ConnectServer(string uri)
        {
            webSocket = new ClientWebSocket();          //用于连接到Aria2Rpc的客户端
            cancellationToken = new CancellationToken(); //传播有关应取消操作的通知
            bool status = false;  //储存连接状态
            try
            {
                //连接服务器
                await Aria2.webSocket.ConnectAsync(new Uri(uri), Aria2.cancellationToken);
            }
            catch
            {
                status = false;
            }
            //检查连接是否成功
            if (Aria2.webSocket.State == WebSocketState.Open)
            {
                status = true;
            }
            return status;
        }
 
 
        /// <summary>
        /// JsonClass类转为json格式
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static string ToJson(JsonClass json,string path)
        {
            string str = "";
            str = "{" + "\"jsonrpc\":\"" + json._jsonrpc + "\",\"id\":\"" + json._id + "\",\"method\":\"" + json._method + "\",\"params\":[";
            for (int i = 0; i < json._params.Count; i++)
            {
                if(json._method == "aria2.addUri")
                {
                    str += "[\"" + json._params[i] + "\"]";
                }
                else if(json._method == "aria2.tellStatus")
                {
                    if(i == 0)
                    {
                        str += "\"" + json._params[i] + "\"";
                    }
                    else
                    {
                        str += "[\"" + json._params[i] + "\"]";
                    }
                }
                else
                {
                    str += "\"" + json._params[i] + "\"";
                }
                if (json._params.Count - 1 > i)
                {
                    str += ",";
                }
            }
            str += ", {\"dir\":\"" + path + "\"}]}";
            //最后得到类似
            //{"jsonrpc":"2.0","id":"qwer","method":"aria2.addUri","params:"[["http://www.baidu.com"]]}
            return str;
 
        }
 
 
        /// <summary>
        /// 发送json并返回json消息
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private static async Task<string> SendAndReceive(JsonClass json,string path)
        {
            string str = "";
            str = ToJson(json,path);  //转换为json字符串
            try
            {
                //发送json数据
                await Aria2.webSocket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(str)), WebSocketMessageType.Text, true, Aria2.cancellationToken);
                var result = new byte[1024];
                //接收数据
                await Aria2.webSocket.ReceiveAsync(new ArraySegment<byte>(result), new CancellationToken());
                str += "\r\n" + Encoding.UTF8.GetString(result, 0, result.Length) + "\r\n";
            }
            catch
            {
                str = "连接错误";
            }
            return str;
        }
 
 
        /// <summary>
        /// 添加新的下载
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public static async Task<string> AddUri(string uri,string localpath)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.addUri";
            List<string> paramslist = new List<string>();
            string u = Uri.UnescapeDataString(uri);
            Uri uri1 = new Uri(u);
            //添加下载地址
            paramslist.Add(u);
            //paramslist.Add("{\"dir\":\"" + uri1.AbsolutePath + "\"}");
            json._params = paramslist;
            string url3 = uri1.AbsolutePath.Substring(2);
            string url4 = Uri.UnescapeDataString(localpath + url3.Replace(url3.Substring(url3.LastIndexOf(@"/") + 1), ""));
            str = await Aria2.SendAndReceive(json,url4.Replace(@"\","/"));
            return str;
        }
 /*
 
        /// <summary>
        /// 上传“.torrent”文件添加BitTorrent下载
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> AddTorrent(string path)
        {
            string str = "";
            string fsbase64= "";
            byte[] fs = File.ReadAllBytes(path);
            fsbase64 = Convert.ToBase64String(fs);  //转为Base64编码
 
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.addTorrent";
            List<string> paramslist = new List<string>();
            //添加“.torrent”文件本地地址
            paramslist.Add(fsbase64);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 删除已经停止的任务，强制删除请使用ForceRemove方法
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> Remove(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.remove";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 强制删除
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> ForceRemove(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.forceRemove";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 暂停下载,强制暂停请使用ForcePause方法
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> Pause(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.pause";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 暂停全部任务
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> PauseAll()
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.pauseAll";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add("");
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 强制暂停下载
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> ForcePause(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.forcePause";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 强制暂停全部下载
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> ForcePauseAll()
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.forcePauseAll";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add("");
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 把正在下载的任务状态改为等待下载
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> PauseToWaiting(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.unpause";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 把全部在下载的任务状态改为等待下载
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> PauseToWaitingAll()
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.unpauseAll";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add("");
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 返回下载进度
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> TellStatus(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.tellStatus";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            paramslist.Add("completedLength\", \"totalLength\",\"downloadSpeed");
 
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 返回全局统计信息，例如整体下载和上载速度
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> GetGlobalStat(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.getGlobalStat";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 返回由gid（字符串）表示的下载中使用的URI 。响应是一个结构数组
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> GetUris(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.getUris";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 
 
        /// <summary>
        /// 返回由gid（字符串）表示的下载文件列表
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        public static async Task<string> GetFiles(string gid)
        {
            string str = "";
            var json = new JsonClass();
            json._jsonrpc = "2.0";
            json._id = "qwer";
            json._method = "aria2.getFiles";
            List<string> paramslist = new List<string>();
            //添加下载地址
            paramslist.Add(gid);
            json._params = paramslist;
            str = await Aria2.SendAndReceive(json);
            return str;
        }
 */
    }
}