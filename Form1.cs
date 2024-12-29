using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json.Nodes;
using System.Web;
using aria2c;

namespace AlistDownloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string path = "";
        public List<string> files = new List<string>();
        public bool iscache = false;
        private async void Login_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            if (Endpoint.Text == String.Empty)
            {
                Logbox.AppendText("请输入服务器地址！\r\n");
                return;
            }
            try
            {
                await List("");
            }
            catch (Exception ex)
            {
                Logbox.AppendText(ex.Message + "\r\n");
            }
        }
        public async Task List(string url)
        {
            UriPath.Text = url;
            listView1.Items.Clear();
            if (path.Contains("/"))
            {
                listView1.Items.Add(new ListViewItem(new string[] { "..", "-", "返回上级", "-", "-" }));
            }
            Uri u = new Uri(new Uri(Endpoint.Text), "api/fs/list?path=" + GetURL(url));
            //Logbox.AppendText("正在获取文件列表...\r\n");
            JsonNode node = JsonNode.Parse(await Get(u.AbsoluteUri, Auth.Text))!;
            int status = (int)node["code"]!;
            if (status != 200)
            {
                Logbox.AppendText("请求失败：" + node["message"]! + "\r\n");
                return;
            }
            for (int i = 0; i < (int)node["data"]["total"]!; i++)
            {
                var file = node["data"]["content"][i];
                listView1.Items.Add(new ListViewItem(new string[] { (string)file["name"], CountSize((long)file["size"]), isDir((bool)file["is_dir"]), (string)file["modified"], ((int)file["type"]).ToString() }));
            }
        }
        public async Task<string> Get(string url, string cookie)
        {
            var handler = new HttpClientHandler() { UseCookies = true };
            handler.ServerCertificateCustomValidationCallback = delegate { return true; };
            HttpClient client = new HttpClient(handler);
            if (cookie != String.Empty)
            {

                client.DefaultRequestHeaders.Add("Authorization", cookie);
            }
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        public string isDir(bool isdir)
        {
            if (isdir == true)
            {
                return "文件夹";
            }
            else
            {
                return "文件";
            }
        }
        public static string CountSize(long Size)
        {
            string m_strSize = "";
            long FactSize = 0;
            FactSize = Size;
            if (FactSize <= 0)
                m_strSize = "-";
            else if (FactSize < 1024.00)
                m_strSize = FactSize.ToString("F2") + " B";
            else if (FactSize >= 1024.00 && FactSize < 1048576)
                m_strSize = (FactSize / 1024.00).ToString("F2") + " KB";
            else if (FactSize >= 1048576 && FactSize < 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00).ToString("F2") + " MB";
            else if (FactSize >= 1073741824)
                m_strSize = (FactSize / 1024.00 / 1024.00 / 1024.00).ToString("F2") + " GB";
            return m_strSize;
        }
        public string ResStringRemoveChar(char c, string str)
        {
            string strr = str;
            try
            {
                int index = str.LastIndexOf(c);
                //string lstrr = strr.Substring(0, index);
                strr = strr.Remove(index);
                return strr;
            }
            catch (Exception)
            {
                return strr;
            }
        }
        private async void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            {
                return;
            }
            if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件")
            {
                Logbox.AppendText("暂不支持预览文件.\r\n");
            }
            else if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
            {
                path += "/" + listView1.SelectedItems[0].Text.ToString();
                await List(path);
            }
            else if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "返回上级")
            {
                path = ResStringRemoveChar('/', path);
                await List(path);
            }
            else
            {
                return;
            }
        }

        private void Logout_Click(object sender, EventArgs e)
        {
            path = String.Empty;
            listView1.Items.Clear();
            Logbox.Clear();
            files.Clear();
            UriPath.Clear();
            iscache = false;

        }
        public static string GetURL(string url)

        {

            string URLcode = "";
            URLcode = Uri.EscapeDataString(url);
            return URLcode;

        }
        private void listView1_MouseDown(object sender, MouseEventArgs e)
        {
            ListView listView = (ListView)sender;
            ListViewItem item = listView.GetItemAt(e.X, e.Y);
            if (item != null && e.Button == MouseButtons.Right)
            {
                //cmsListViewItem是我们添加的菜单控件
                this.contextMenuStrip1.Show(listView, e.X, e.Y);
            }
        }

        private async void 下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            files.Clear();

            List<string> lists = new List<string>();
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件")
                {
                    Uri s = new Uri(new Uri(Endpoint.Text), "/d" + GetURL(UriPath.Text + "/" + listView1.SelectedItems[0].Text.ToString()));
                    lists.Clear();
                    lists.Add(s.ToString());
                }
                else if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
                {
                    lists.Clear();
                    lists = await GetFileList(UriPath.Text + "/" + listView1.SelectedItems[0].Text.ToString());

                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                Logbox.AppendText(ex.Message + "\r\n");
            }
            files = lists;
            iscache = true;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "";
            sfd.Filter = "文本文件|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(sfd.FileName, true, System.Text.Encoding.Default);
                if (iscache == true)
                {
                    foreach (string l in files)
                    {
                        sw.WriteLine(l);
                    }
                }
                else
                {
                    foreach (string l in lists)
                    {
                        sw.WriteLine(l);
                    }
                }

                sw.Close();
                Logbox.AppendText("下载链接已保存到文件，可以使用下载器批量下载。\r\n");
            }


        }
        private async Task<List<string>> GetFileList(string url)
        {
            List<string> listoffile = new List<string>();
            Uri u = new Uri(new Uri(Endpoint.Text), "api/fs/list?path=" + GetURL(url));
            //
            JsonNode node = JsonNode.Parse(await Get(u.AbsoluteUri, Auth.Text))!;
            int status = (int)node["code"]!;
            if (status != 200)
            {
                Logbox.AppendText("请求失败：" + node["message"]! + "\r\n" + url);
                return listoffile;
            }
            for (int i = 0; i < (int)node["data"]["total"]!; i++)
            {
                try
                {
                    var file = node["data"]["content"][i];
                    if ((bool)file["is_dir"] == false)
                    {
                        //Uri u1 = new Uri(new Uri(Endpoint.Text), "api/fs/get?path=" + HttpUtility.UrlEncode(url + "/" + (string)file["name"]));
                        Uri u1 = new Uri(new Uri(Endpoint.Text), "/d" + GetURL(url + "/" + (string)file["name"]));
                        listoffile.Add(u1.AbsoluteUri);
                    }
                    else
                    {
                        List<string> ls = await GetFileList(url + "/" + (string)file["name"]);
                        foreach (string s in ls)
                        { listoffile.Add(s); }
                    }
                }
                catch (Exception ex)
                {
                    Logbox.AppendText("获取失败：" + ex.Message + "\r\n");
                    continue;
                }

            }
            return listoffile;
        }

        private async void 发送到Aria2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string lf = string.Empty;
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            if (folderBrowser.ShowDialog() == DialogResult.OK)
            {
                string selectedFolderPath = folderBrowser.SelectedPath; // 用户选择的路径
                lf = selectedFolderPath;
            }
            files.Clear();

            List<string> lists = new List<string>();
            try
            {
                if (listView1.SelectedItems.Count == 0)
                {
                    return;
                }
                if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件")
                {
                    Uri s = new Uri(new Uri(Endpoint.Text), "/d" + GetURL(UriPath.Text + "/" + listView1.SelectedItems[0].Text.ToString()));
                    lists.Clear();
                    lists.Add(s.ToString());
                }
                else if (listView1.SelectedItems[0].SubItems[2].Text.ToString() == "文件夹")
                {
                    lists.Clear();
                    lists = await GetFileList(UriPath.Text + "/" + listView1.SelectedItems[0].Text.ToString());

                }
                else
                {
                    return;
                }
                files = lists;
                if (await aria2c.Aria2.ConnectServer(Aria2.Text) == true)
                {
                    foreach (string s in files)
                    {
                        await aria2c.Aria2.AddUri(s, lf);
                    }

                    Logbox.AppendText("已经发送到aira2c，请查看aria2c。\r\n");
                }
                else
                {
                    Logbox.AppendText("aria2c连接失败。\r\n");
                }
            }
            catch (Exception ex)
            {
                Logbox.AppendText("下载失败：" + ex.Message + "\r\n");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logbox.Clear();
        }
        private void 直接下载ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("喵呜。");
        }
    }
}
