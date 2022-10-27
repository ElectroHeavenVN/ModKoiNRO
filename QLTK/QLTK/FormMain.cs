using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;

namespace QLTK
{
	public class FormMain : MaterialForm
	{
		public static string string_0 = Path.GetTempPath() + "koi occtiu957\\mod 222";

		public static string string_1 = string_0 + "\\data";

		public static string string_2 = string_0 + "\\login";

		public static string string_3 = string_0 + "\\auto";

		public static bool bool_0;

		public static string string_4 = Application.StartupPath + "\\Dragonboy_vn_v222.exe";

		public static List<Data> list_0 = new List<Data>();

		private IContainer icontainer_0 = null;

		private GroupBox groupBox1;

		private MaterialTextBox txt_username;

		private MaterialTextBox txt_password;

		private MaterialComboBox txt_server;

		private MaterialTextBox txt_note;

		private MaterialListView list_data;

		private ColumnHeader columnHeader_0;

		private ColumnHeader columnHeader_1;

		private ColumnHeader columnHeader_2;

		private ColumnHeader columnHeader_3;

		private GroupBox groupBox2;

		private MaterialButton btn_skill_1;

		private MaterialButton btn_skill_8;

		private MaterialButton btn_skill_7;

		private MaterialButton btn_skill_6;

		private MaterialButton btn_skill_5;

		private MaterialButton btn_skill_4;

		private MaterialButton btn_skill_3;

		private MaterialButton btn_skill_2;

		private MaterialButton btn_xoa;

		private MaterialButton btn_them;
        private IContainer components;
        private System.Windows.Forms.Timer timer_0;

		public FormMain()
		{
			InitializeComponent();
			MaterialSkinManager instance = MaterialSkinManager.Instance;
			instance.AddFormToManage(this);
			instance.Theme = MaterialSkinManager.Themes.DARK;
			instance.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
			if (!Directory.Exists(string_0))
				Directory.CreateDirectory(string_0);
			try
			{
				if (!Directory.Exists(string_0))
					Directory.CreateDirectory(string_0);
				if (File.Exists(string_3))
					File.Delete(string_3);
			}
			catch
			{
				Application.Exit();
			}
			method_0();
			timer_0.Start();
		}

		public void method_0()
		{
			try
			{
				if (!File.Exists(string_1))
					return;
				list_0.Clear();
				list_data.Items.Clear();
				string[] array = File.ReadAllLines(string_1);
				for (int i = 1; i <= array.Length; i++)
				{
					if (!array.Equals(""))
					{
						string[] array2 = array[i - 1].Split('|');
						Data gClass = new Data
						{
							username = array2[1],
							server = int.Parse(array2[2]),
							password = array2[3],
							note = array2[4]
						};
						list_0.Add(gClass);
						list_data.Items.Add(new ListViewItem(new string[4]
						{
							i.ToString(),
							gClass.username,
							gClass.server.ToString(),
							gClass.note
						}));
					}
				}
			}
			catch
			{
			}
		}

		public void method_1()
		{
			if (File.Exists(string_1))
				File.Delete(string_1);
			string text = "";
			for (int i = 0; i < list_0.Count; i++)
			{
				text = text + (i + 1) + "|" + list_0[i].getData() + "|" + list_0[i].note;
				if (i < list_0.Count - 1)
					text += "\n";
			}
			File.WriteAllText(string_1, text);
		}

		private void btn_them_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(txt_username.Text))
			{
				MessageBox.Show("Không được bỏ trống tài khoản!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (string.IsNullOrEmpty(txt_password.Text))
			{
				MessageBox.Show("Không được bỏ trống mật khẩu!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			string text = txt_note.Text;
			if (string.IsNullOrEmpty(text) || text == "Có thể bỏ qua")
				text = "none";
			btn_them.Enabled = false;
			Data gClass = new Data
			{
				username = txt_username.Text,
				server = smethod_0(txt_server.Text),
				password = smethod_1(txt_password.Text, "ud"),
				note = text
			};
			list_0.Add(gClass);
			list_data.Items.Add(new ListViewItem(new string[4]
			{
				(list_data.Items.Count + 1).ToString(),
				gClass.username,
				gClass.server.ToString(),
				gClass.note
			}));
			method_1();
			btn_them.Enabled = true;
		}

		public static int smethod_0(string string_5)
		{
			if (string_5.StartsWith("Vũ"))
				return int.Parse(string_5.Split(' ')[2]);
			if (string_5.StartsWith("Võ"))
				return 11;
			if (string_5.Equals("Naga"))
				return 12;
			return 13;
		}

		private void btn_xoa_Click(object sender, EventArgs e)
		{
			btn_xoa.Enabled = false;
			if (list_data.SelectedItems != null)
			{
				for (int i = 0; i < list_data.Items.Count; i++)
				{
					if (list_data.Items[i].Selected)
					{
						list_data.Items[i].Remove();
						list_0.RemoveAt(i);
						method_1();
						break;
					}
				}
			}
			method_0();
			btn_xoa.Enabled = true;
		}

		public static string smethod_1(string string_5, string string_6)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(string_5);
			byte[] key = new MD5CryptoServiceProvider().ComputeHash(Encoding.UTF8.GetBytes(string_6));
			byte[] array = new TripleDESCryptoServiceProvider
			{
				Key = key,
				Mode = CipherMode.ECB,
				Padding = PaddingMode.PKCS7
			}.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
			return Convert.ToBase64String(array, 0, array.Length);
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (MessageBox.Show("Do you want to exit?", "Notification", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
			{
				timer_0.Stop();
				e.Cancel = true;
				Application.Exit();
			}
		}

		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr FindWindow(string string_5, string string_6);

		[DllImport("user32.dll")]
		public static extern bool SetWindowText(IntPtr intptr_0, string string_5);

		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool MoveWindow(IntPtr intptr_0, int int_0, int int_1, int int_2, int int_3, bool bool_1);

		private void list_data_DoubleClick(object sender, EventArgs e)
		{
			int index = int.Parse(list_data.SelectedItems[0].SubItems[0].Text);
			Thread thread = new Thread((ThreadStart)delegate
			{
				smethod_3(index);
			});
			thread.IsBackground = true;
			thread.Start();
		}

		public static void smethod_2(int int_0)
		{
			if (bool_0)
				return;
			bool_0 = true;
			try
			{
				if (File.Exists(string_3))
					File.Delete(string_3);
				File.WriteAllText(string_3, int_0.ToString());
				Thread.Sleep(2000);
				File.Delete(string_3);
				Thread.Sleep(2000);
			}
			catch
			{
				try
				{
					if (File.Exists(string_3))
						File.Delete(string_3);
				}
				catch
				{
				}
			}
			bool_0 = false;
		}

		public static void smethod_3(int int_0)
		{
			if (!bool_0)
			{
				bool_0 = true;
				Process.Start(arguments: File.ReadAllLines(string_1)[int_0 - 1], fileName: string_4);
				Thread.Sleep(200);
				while (!smethod_4("Dragonboy222"))
				{
					Thread.Sleep(100);
				}
				SetWindowText(FindWindow(null, "Dragonboy222"), "DP" + (int_0 + 1));
				Thread.Sleep(2000);
				bool_0 = false;
			}
		}

		public static bool smethod_4(string string_5)
		{
			return FindWindow(null, string_5) != IntPtr.Zero;
		}

		public static void smethod_5(int int_0)
		{
			Thread thread = new Thread((ThreadStart)delegate
			{
				smethod_2(int_0);
			});
			thread.IsBackground = true;
			thread.Start();
		}

		private void btn_skill_1_Click(object sender, EventArgs e)
		{
			smethod_5(0);
		}

		private void btn_skill_2_Click(object sender, EventArgs e)
		{
			smethod_5(1);
		}

		private void btn_skill_3_Click(object sender, EventArgs e)
		{
			smethod_5(2);
		}

		private void btn_skill_4_Click(object sender, EventArgs e)
		{
			smethod_5(3);
		}

		private void btn_skill_5_Click(object sender, EventArgs e)
		{
			smethod_5(4);
		}

		private void btn_skill_6_Click(object sender, EventArgs e)
		{
			smethod_5(5);
		}

		private void btn_skill_7_Click(object sender, EventArgs e)
		{
			smethod_5(6);
		}

		private void btn_skill_8_Click(object sender, EventArgs e)
		{
			smethod_5(7);
		}

		private void timer_0_Tick(object sender, EventArgs e)
		{
			btn_skill_1.Enabled = !bool_0;
			btn_skill_2.Enabled = !bool_0;
			btn_skill_3.Enabled = !bool_0;
			btn_skill_4.Enabled = !bool_0;
			btn_skill_5.Enabled = !bool_0;
			btn_skill_6.Enabled = !bool_0;
			btn_skill_7.Enabled = !bool_0;
			btn_skill_8.Enabled = !bool_0;
			btn_them.Enabled = !bool_0;
			btn_xoa.Enabled = !bool_0;
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && icontainer_0 != null)
				icontainer_0.Dispose();
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_xoa = new MaterialSkin.Controls.MaterialButton();
            this.btn_them = new MaterialSkin.Controls.MaterialButton();
            this.txt_note = new MaterialSkin.Controls.MaterialTextBox();
            this.txt_server = new MaterialSkin.Controls.MaterialComboBox();
            this.txt_password = new MaterialSkin.Controls.MaterialTextBox();
            this.txt_username = new MaterialSkin.Controls.MaterialTextBox();
            this.list_data = new MaterialSkin.Controls.MaterialListView();
            this.columnHeader_0 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader_3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_skill_8 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_7 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_6 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_5 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_4 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_3 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_2 = new MaterialSkin.Controls.MaterialButton();
            this.btn_skill_1 = new MaterialSkin.Controls.MaterialButton();
            this.timer_0 = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_xoa);
            this.groupBox1.Controls.Add(this.btn_them);
            this.groupBox1.Controls.Add(this.txt_note);
            this.groupBox1.Controls.Add(this.txt_server);
            this.groupBox1.Controls.Add(this.txt_password);
            this.groupBox1.Controls.Add(this.txt_username);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(593, 72);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(391, 368);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cài đặt";
            // 
            // btn_xoa
            // 
            this.btn_xoa.AutoSize = false;
            this.btn_xoa.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_xoa.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_xoa.Depth = 0;
            this.btn_xoa.HighEmphasis = true;
            this.btn_xoa.Icon = ((System.Drawing.Image)(resources.GetObject("btn_xoa.Icon")));
            this.btn_xoa.Location = new System.Drawing.Point(197, 309);
            this.btn_xoa.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_xoa.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_xoa.Name = "btn_xoa";
            this.btn_xoa.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_xoa.Size = new System.Drawing.Size(108, 36);
            this.btn_xoa.TabIndex = 5;
            this.btn_xoa.Text = "Xóa";
            this.btn_xoa.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_xoa.UseAccentColor = false;
            this.btn_xoa.UseVisualStyleBackColor = true;
            this.btn_xoa.Click += new System.EventHandler(this.btn_xoa_Click);
            // 
            // btn_them
            // 
            this.btn_them.AutoSize = false;
            this.btn_them.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_them.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_them.Depth = 0;
            this.btn_them.HighEmphasis = true;
            this.btn_them.Icon = ((System.Drawing.Image)(resources.GetObject("btn_them.Icon")));
            this.btn_them.Location = new System.Drawing.Point(81, 309);
            this.btn_them.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_them.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_them.Name = "btn_them";
            this.btn_them.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_them.Size = new System.Drawing.Size(108, 36);
            this.btn_them.TabIndex = 4;
            this.btn_them.Text = "Thêm";
            this.btn_them.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_them.UseAccentColor = false;
            this.btn_them.UseVisualStyleBackColor = true;
            this.btn_them.Click += new System.EventHandler(this.btn_them_Click);
            // 
            // txt_note
            // 
            this.txt_note.AnimateReadOnly = false;
            this.txt_note.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_note.Depth = 0;
            this.txt_note.Font = new System.Drawing.Font("Roboto", 12F);
            this.txt_note.Hint = "Ghi chú";
            this.txt_note.LeadingIcon = null;
            this.txt_note.Location = new System.Drawing.Point(21, 175);
            this.txt_note.MaxLength = 50;
            this.txt_note.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_note.Multiline = false;
            this.txt_note.Name = "txt_note";
            this.txt_note.Size = new System.Drawing.Size(346, 50);
            this.txt_note.TabIndex = 3;
            this.txt_note.Text = "Có thể bỏ qua";
            this.txt_note.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("txt_note.TrailingIcon")));
            // 
            // txt_server
            // 
            this.txt_server.AutoResize = false;
            this.txt_server.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.txt_server.Depth = 0;
            this.txt_server.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.txt_server.DropDownHeight = 174;
            this.txt_server.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.txt_server.DropDownWidth = 121;
            this.txt_server.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.txt_server.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txt_server.FormattingEnabled = true;
            this.txt_server.Hint = "Máy chủ";
            this.txt_server.IntegralHeight = false;
            this.txt_server.ItemHeight = 43;
            this.txt_server.Items.AddRange(new object[] {
            "Vũ trụ 1 sao",
            "Vũ trụ 2 sao",
            "Vũ trụ 3 sao",
            "Vũ trụ 4 sao",
            "Vũ trụ 5 sao",
            "Vũ trụ 6 sao",
            "Vũ trụ 7 sao",
            "Vũ trụ 8 sao",
            "Vũ trụ 9 sao",
            "Vũ trụ 10 sao",
            "Võ đài liên vũ trụ",
            "Naga",
            "Universe 1"});
            this.txt_server.Location = new System.Drawing.Point(21, 242);
            this.txt_server.MaxDropDownItems = 4;
            this.txt_server.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_server.Name = "txt_server";
            this.txt_server.Size = new System.Drawing.Size(346, 49);
            this.txt_server.StartIndex = 0;
            this.txt_server.TabIndex = 2;
            // 
            // txt_password
            // 
            this.txt_password.AnimateReadOnly = false;
            this.txt_password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_password.Depth = 0;
            this.txt_password.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_password.Hint = "Mật khẩu";
            this.txt_password.LeadingIcon = null;
            this.txt_password.Location = new System.Drawing.Point(21, 108);
            this.txt_password.MaxLength = 50;
            this.txt_password.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_password.Multiline = false;
            this.txt_password.Name = "txt_password";
            this.txt_password.Password = true;
            this.txt_password.Size = new System.Drawing.Size(346, 50);
            this.txt_password.TabIndex = 1;
            this.txt_password.Text = "";
            this.txt_password.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("txt_password.TrailingIcon")));
            // 
            // txt_username
            // 
            this.txt_username.AnimateReadOnly = false;
            this.txt_username.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_username.Depth = 0;
            this.txt_username.Font = new System.Drawing.Font("Roboto", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txt_username.Hint = "Tài khoản";
            this.txt_username.LeadingIcon = null;
            this.txt_username.Location = new System.Drawing.Point(21, 41);
            this.txt_username.MaxLength = 50;
            this.txt_username.MouseState = MaterialSkin.MouseState.OUT;
            this.txt_username.Multiline = false;
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(346, 50);
            this.txt_username.TabIndex = 0;
            this.txt_username.Text = "";
            this.txt_username.TrailingIcon = ((System.Drawing.Image)(resources.GetObject("txt_username.TrailingIcon")));
            // 
            // list_data
            // 
            this.list_data.AutoSizeTable = false;
            this.list_data.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.list_data.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.list_data.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader_0,
            this.columnHeader_1,
            this.columnHeader_2,
            this.columnHeader_3});
            this.list_data.Depth = 0;
            this.list_data.FullRowSelect = true;
            this.list_data.HideSelection = false;
            this.list_data.Location = new System.Drawing.Point(16, 80);
            this.list_data.MinimumSize = new System.Drawing.Size(200, 100);
            this.list_data.MouseLocation = new System.Drawing.Point(-1, -1);
            this.list_data.MouseState = MaterialSkin.MouseState.OUT;
            this.list_data.Name = "list_data";
            this.list_data.OwnerDraw = true;
            this.list_data.Size = new System.Drawing.Size(567, 360);
            this.list_data.TabIndex = 1;
            this.list_data.UseCompatibleStateImageBehavior = false;
            this.list_data.View = System.Windows.Forms.View.Details;
            this.list_data.DoubleClick += new System.EventHandler(this.list_data_DoubleClick);
            // 
            // columnHeader_0
            // 
            this.columnHeader_0.Text = "STT";
            // 
            // columnHeader_1
            // 
            this.columnHeader_1.Text = "Tài khoản";
            this.columnHeader_1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_1.Width = 250;
            // 
            // columnHeader_2
            // 
            this.columnHeader_2.Text = "Máy chủ";
            this.columnHeader_2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_2.Width = 90;
            // 
            // columnHeader_3
            // 
            this.columnHeader_3.Text = "Ghi chú";
            this.columnHeader_3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader_3.Width = 150;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_skill_8);
            this.groupBox2.Controls.Add(this.btn_skill_7);
            this.groupBox2.Controls.Add(this.btn_skill_6);
            this.groupBox2.Controls.Add(this.btn_skill_5);
            this.groupBox2.Controls.Add(this.btn_skill_4);
            this.groupBox2.Controls.Add(this.btn_skill_3);
            this.groupBox2.Controls.Add(this.btn_skill_2);
            this.groupBox2.Controls.Add(this.btn_skill_1);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(16, 446);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(968, 93);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Điều khiển";
            // 
            // btn_skill_8
            // 
            this.btn_skill_8.AutoSize = false;
            this.btn_skill_8.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_8.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_8.Depth = 0;
            this.btn_skill_8.HighEmphasis = true;
            this.btn_skill_8.Icon = null;
            this.btn_skill_8.Location = new System.Drawing.Point(838, 34);
            this.btn_skill_8.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_8.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_8.Name = "btn_skill_8";
            this.btn_skill_8.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_8.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_8.TabIndex = 7;
            this.btn_skill_8.Text = "Skill 8";
            this.btn_skill_8.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_8.UseAccentColor = false;
            this.btn_skill_8.UseVisualStyleBackColor = true;
            this.btn_skill_8.Click += new System.EventHandler(this.btn_skill_8_Click);
            // 
            // btn_skill_7
            // 
            this.btn_skill_7.AutoSize = false;
            this.btn_skill_7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_7.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_7.Depth = 0;
            this.btn_skill_7.HighEmphasis = true;
            this.btn_skill_7.Icon = null;
            this.btn_skill_7.Location = new System.Drawing.Point(722, 34);
            this.btn_skill_7.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_7.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_7.Name = "btn_skill_7";
            this.btn_skill_7.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_7.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_7.TabIndex = 6;
            this.btn_skill_7.Text = "Skill 7";
            this.btn_skill_7.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_7.UseAccentColor = false;
            this.btn_skill_7.UseVisualStyleBackColor = true;
            this.btn_skill_7.Click += new System.EventHandler(this.btn_skill_7_Click);
            // 
            // btn_skill_6
            // 
            this.btn_skill_6.AutoSize = false;
            this.btn_skill_6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_6.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_6.Depth = 0;
            this.btn_skill_6.HighEmphasis = true;
            this.btn_skill_6.Icon = null;
            this.btn_skill_6.Location = new System.Drawing.Point(606, 34);
            this.btn_skill_6.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_6.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_6.Name = "btn_skill_6";
            this.btn_skill_6.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_6.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_6.TabIndex = 5;
            this.btn_skill_6.Text = "Skill 6";
            this.btn_skill_6.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_6.UseAccentColor = false;
            this.btn_skill_6.UseVisualStyleBackColor = true;
            this.btn_skill_6.Click += new System.EventHandler(this.btn_skill_6_Click);
            // 
            // btn_skill_5
            // 
            this.btn_skill_5.AutoSize = false;
            this.btn_skill_5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_5.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_5.Depth = 0;
            this.btn_skill_5.HighEmphasis = true;
            this.btn_skill_5.Icon = null;
            this.btn_skill_5.Location = new System.Drawing.Point(490, 34);
            this.btn_skill_5.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_5.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_5.Name = "btn_skill_5";
            this.btn_skill_5.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_5.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_5.TabIndex = 4;
            this.btn_skill_5.Text = "Skill 5";
            this.btn_skill_5.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_5.UseAccentColor = false;
            this.btn_skill_5.UseVisualStyleBackColor = true;
            this.btn_skill_5.Click += new System.EventHandler(this.btn_skill_5_Click);
            // 
            // btn_skill_4
            // 
            this.btn_skill_4.AutoSize = false;
            this.btn_skill_4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_4.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_4.Depth = 0;
            this.btn_skill_4.HighEmphasis = true;
            this.btn_skill_4.Icon = null;
            this.btn_skill_4.Location = new System.Drawing.Point(374, 34);
            this.btn_skill_4.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_4.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_4.Name = "btn_skill_4";
            this.btn_skill_4.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_4.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_4.TabIndex = 3;
            this.btn_skill_4.Text = "Skill 4";
            this.btn_skill_4.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_4.UseAccentColor = false;
            this.btn_skill_4.UseVisualStyleBackColor = true;
            this.btn_skill_4.Click += new System.EventHandler(this.btn_skill_4_Click);
            // 
            // btn_skill_3
            // 
            this.btn_skill_3.AutoSize = false;
            this.btn_skill_3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_3.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_3.Depth = 0;
            this.btn_skill_3.HighEmphasis = true;
            this.btn_skill_3.Icon = null;
            this.btn_skill_3.Location = new System.Drawing.Point(258, 34);
            this.btn_skill_3.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_3.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_3.Name = "btn_skill_3";
            this.btn_skill_3.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_3.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_3.TabIndex = 2;
            this.btn_skill_3.Text = "Skill 3";
            this.btn_skill_3.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_3.UseAccentColor = false;
            this.btn_skill_3.UseVisualStyleBackColor = true;
            this.btn_skill_3.Click += new System.EventHandler(this.btn_skill_3_Click);
            // 
            // btn_skill_2
            // 
            this.btn_skill_2.AutoSize = false;
            this.btn_skill_2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_2.Depth = 0;
            this.btn_skill_2.HighEmphasis = true;
            this.btn_skill_2.Icon = null;
            this.btn_skill_2.Location = new System.Drawing.Point(142, 34);
            this.btn_skill_2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_2.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_2.Name = "btn_skill_2";
            this.btn_skill_2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_2.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_2.TabIndex = 1;
            this.btn_skill_2.Text = "Skill 2";
            this.btn_skill_2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_2.UseAccentColor = false;
            this.btn_skill_2.UseVisualStyleBackColor = true;
            this.btn_skill_2.Click += new System.EventHandler(this.btn_skill_2_Click);
            // 
            // btn_skill_1
            // 
            this.btn_skill_1.AutoSize = false;
            this.btn_skill_1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btn_skill_1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.btn_skill_1.Depth = 0;
            this.btn_skill_1.HighEmphasis = true;
            this.btn_skill_1.Icon = null;
            this.btn_skill_1.Location = new System.Drawing.Point(26, 34);
            this.btn_skill_1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btn_skill_1.MouseState = MaterialSkin.MouseState.HOVER;
            this.btn_skill_1.Name = "btn_skill_1";
            this.btn_skill_1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.btn_skill_1.Size = new System.Drawing.Size(108, 36);
            this.btn_skill_1.TabIndex = 0;
            this.btn_skill_1.Text = "Skill 1";
            this.btn_skill_1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.btn_skill_1.UseAccentColor = false;
            this.btn_skill_1.UseVisualStyleBackColor = true;
            this.btn_skill_1.Click += new System.EventHandler(this.btn_skill_1_Click);
            // 
            // timer_0
            // 
            this.timer_0.Tick += new System.EventHandler(this.timer_0_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1000, 553);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.list_data);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "QLTK Mod Kòi Octiiu957 - By Dũng Phạm - version 2.2.2";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
	}
}
