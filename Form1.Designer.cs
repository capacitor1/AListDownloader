namespace AlistDownloader
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            listView1 = new ListView();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            contextMenuStrip1 = new ContextMenuStrip(components);
            下载ToolStripMenuItem = new ToolStripMenuItem();
            直接下载ToolStripMenuItem = new ToolStripMenuItem();
            发送到Aria2ToolStripMenuItem = new ToolStripMenuItem();
            Login = new Button();
            groupBox1 = new GroupBox();
            button1 = new Button();
            label4 = new Label();
            Aria2 = new TextBox();
            Logout = new Button();
            Logbox = new RichTextBox();
            label2 = new Label();
            Auth = new TextBox();
            label1 = new Label();
            Endpoint = new TextBox();
            label3 = new Label();
            UriPath = new TextBox();
            contextMenuStrip1.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // listView1
            // 
            listView1.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5 });
            listView1.ContextMenuStrip = contextMenuStrip1;
            listView1.FullRowSelect = true;
            listView1.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            listView1.Location = new Point(12, 165);
            listView1.MultiSelect = false;
            listView1.Name = "listView1";
            listView1.Size = new Size(1240, 500);
            listView1.TabIndex = 0;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.View = View.Details;
            listView1.DoubleClick += listView1_DoubleClick;
            listView1.MouseDown += listView1_MouseDown;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "名称";
            columnHeader1.Width = 700;
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "大小";
            columnHeader2.Width = 120;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "描述";
            columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "日期";
            columnHeader4.Width = 250;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "类型ID";
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { 下载ToolStripMenuItem, 直接下载ToolStripMenuItem, 发送到Aria2ToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(149, 70);
            // 
            // 下载ToolStripMenuItem
            // 
            下载ToolStripMenuItem.Name = "下载ToolStripMenuItem";
            下载ToolStripMenuItem.Size = new Size(148, 22);
            下载ToolStripMenuItem.Text = "导出下载链接";
            下载ToolStripMenuItem.Click += 下载ToolStripMenuItem_Click;
            // 
            // 直接下载ToolStripMenuItem
            // 
            直接下载ToolStripMenuItem.Name = "直接下载ToolStripMenuItem";
            直接下载ToolStripMenuItem.Size = new Size(148, 22);
            直接下载ToolStripMenuItem.Text = "直接下载";
            直接下载ToolStripMenuItem.Click += 直接下载ToolStripMenuItem_Click;
            // 
            // 发送到Aria2ToolStripMenuItem
            // 
            发送到Aria2ToolStripMenuItem.Name = "发送到Aria2ToolStripMenuItem";
            发送到Aria2ToolStripMenuItem.Size = new Size(148, 22);
            发送到Aria2ToolStripMenuItem.Text = "发送到Aria2";
            发送到Aria2ToolStripMenuItem.Click += 发送到Aria2ToolStripMenuItem_Click;
            // 
            // Login
            // 
            Login.Location = new Point(1159, 51);
            Login.Name = "Login";
            Login.Size = new Size(75, 23);
            Login.TabIndex = 1;
            Login.Text = "登录";
            Login.UseVisualStyleBackColor = true;
            Login.Click += Login_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(button1);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(Aria2);
            groupBox1.Controls.Add(Logout);
            groupBox1.Controls.Add(Logbox);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(Auth);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(Endpoint);
            groupBox1.Controls.Add(Login);
            groupBox1.Location = new Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1240, 118);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "工作台";
            // 
            // button1
            // 
            button1.Location = new Point(1159, 22);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 12;
            button1.Text = "清空日志";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 86);
            label4.Name = "label4";
            label4.Size = new Size(56, 17);
            label4.TabIndex = 11;
            label4.Text = "Aria2c：";
            // 
            // Aria2
            // 
            Aria2.Location = new Point(67, 80);
            Aria2.Name = "Aria2";
            Aria2.Size = new Size(379, 23);
            Aria2.TabIndex = 10;
            Aria2.Text = "ws://localhost:6800/jsonrpc";
            // 
            // Logout
            // 
            Logout.Location = new Point(1159, 80);
            Logout.Name = "Logout";
            Logout.Size = new Size(75, 23);
            Logout.TabIndex = 9;
            Logout.Text = "注销";
            Logout.UseVisualStyleBackColor = true;
            Logout.Click += Logout_Click;
            // 
            // Logbox
            // 
            Logbox.Location = new Point(452, 22);
            Logbox.Name = "Logbox";
            Logbox.ReadOnly = true;
            Logbox.Size = new Size(701, 81);
            Logbox.TabIndex = 8;
            Logbox.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 57);
            label2.Name = "label2";
            label2.Size = new Size(46, 17);
            label2.TabIndex = 5;
            label2.Text = "Auth：";
            // 
            // Auth
            // 
            Auth.Location = new Point(67, 51);
            Auth.Name = "Auth";
            Auth.Size = new Size(379, 23);
            Auth.TabIndex = 4;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 28);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 3;
            label1.Text = "服务器：";
            // 
            // Endpoint
            // 
            Endpoint.Location = new Point(67, 22);
            Endpoint.Name = "Endpoint";
            Endpoint.Size = new Size(379, 23);
            Endpoint.TabIndex = 2;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(18, 142);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 10;
            label3.Text = "路径：";
            // 
            // UriPath
            // 
            UriPath.Location = new Point(79, 136);
            UriPath.Name = "UriPath";
            UriPath.ReadOnly = true;
            UriPath.Size = new Size(1173, 23);
            UriPath.TabIndex = 11;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1260, 677);
            Controls.Add(UriPath);
            Controls.Add(label3);
            Controls.Add(groupBox1);
            Controls.Add(listView1);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "AList下载器";
            contextMenuStrip1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listView1;
        private ColumnHeader columnHeader1;
        private Button Login;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private GroupBox groupBox1;
        private Label label2;
        private TextBox Auth;
        private Label label1;
        private TextBox Endpoint;
        private RichTextBox Logbox;
        private Button Logout;
        private Label label3;
        private TextBox UriPath;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem 下载ToolStripMenuItem;
        private ToolStripMenuItem 直接下载ToolStripMenuItem;
        private ToolStripMenuItem 发送到Aria2ToolStripMenuItem;
        private Label label4;
        private TextBox Aria2;
        private Button button1;
    }
}
