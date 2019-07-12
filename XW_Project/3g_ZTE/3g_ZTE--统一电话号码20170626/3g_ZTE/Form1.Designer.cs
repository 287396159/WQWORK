namespace _3g_ZTE
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.ComPort = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.bt_serial_open_close = new System.Windows.Forms.Button();
            this.bt_enter_config = new System.Windows.Forms.Button();
            this.gb_io_in = new System.Windows.Forms.GroupBox();
            this.checkedListBoxInput_IO_Setting = new System.Windows.Forms.CheckedListBox();
            this.bt_set_io_input = new System.Windows.Forms.Button();
            this.bt_get_io_input = new System.Windows.Forms.Button();
            this.bt_clear_message = new System.Windows.Forms.Button();
            this.tb_PhoneMessage = new System.Windows.Forms.TextBox();
            this.bt_del_all = new System.Windows.Forms.Button();
            this.bt_delete = new System.Windows.Forms.Button();
            this.bt_add = new System.Windows.Forms.Button();
            this.lb_phonenumber_show = new System.Windows.Forms.ListBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tb_phonenumberinput = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tb_MsInput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gb_io_out = new System.Windows.Forms.GroupBox();
            this.cb_Pin8_State = new System.Windows.Forms.ComboBox();
            this.bt_get_io_output = new System.Windows.Forms.Button();
            this.bt_set_io_output = new System.Windows.Forms.Button();
            this.cb_Pin7_State = new System.Windows.Forms.ComboBox();
            this.cb_Pin6_State = new System.Windows.Forms.ComboBox();
            this.cb_Pin5_State = new System.Windows.Forms.ComboBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.bt_del_all2 = new System.Windows.Forms.Button();
            this.bt_delete2 = new System.Windows.Forms.Button();
            this.bt_add2 = new System.Windows.Forms.Button();
            this.lb_phonenumber_show2 = new System.Windows.Forms.ListBox();
            this.tb_phonenumberinput2 = new System.Windows.Forms.TextBox();
            this.cb_IsAckMessage = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbParity = new System.Windows.Forms.ComboBox();
            this.cbDataBits = new System.Windows.Forms.ComboBox();
            this.cbStop = new System.Windows.Forms.ComboBox();
            this.cbBaudRate = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.bt_save_excel = new System.Windows.Forms.Button();
            this.bt_read_excel = new System.Windows.Forms.Button();
            this.gb_io_in.SuspendLayout();
            this.gb_io_out.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(4, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Com Port：";
            // 
            // ComPort
            // 
            this.ComPort.FormattingEnabled = true;
            this.ComPort.Location = new System.Drawing.Point(75, 20);
            this.ComPort.Name = "ComPort";
            this.ComPort.Size = new System.Drawing.Size(55, 20);
            this.ComPort.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(284, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 3;
            // 
            // bt_serial_open_close
            // 
            this.bt_serial_open_close.Location = new System.Drawing.Point(500, 39);
            this.bt_serial_open_close.Name = "bt_serial_open_close";
            this.bt_serial_open_close.Size = new System.Drawing.Size(90, 23);
            this.bt_serial_open_close.TabIndex = 4;
            this.bt_serial_open_close.Text = "OpenComm";
            this.bt_serial_open_close.UseVisualStyleBackColor = true;
            this.bt_serial_open_close.Click += new System.EventHandler(this.bt_serial_open_close_Click);
            // 
            // bt_enter_config
            // 
            this.bt_enter_config.Location = new System.Drawing.Point(682, 20);
            this.bt_enter_config.Name = "bt_enter_config";
            this.bt_enter_config.Size = new System.Drawing.Size(90, 23);
            this.bt_enter_config.TabIndex = 6;
            this.bt_enter_config.Text = "Enter Config";
            this.bt_enter_config.UseVisualStyleBackColor = true;
            this.bt_enter_config.Click += new System.EventHandler(this.bt_enter_config_Click);
            // 
            // gb_io_in
            // 
            this.gb_io_in.Controls.Add(this.checkedListBoxInput_IO_Setting);
            this.gb_io_in.Controls.Add(this.bt_clear_message);
            this.gb_io_in.Controls.Add(this.tb_PhoneMessage);
            this.gb_io_in.Controls.Add(this.bt_del_all);
            this.gb_io_in.Controls.Add(this.bt_delete);
            this.gb_io_in.Controls.Add(this.bt_add);
            this.gb_io_in.Controls.Add(this.lb_phonenumber_show);
            this.gb_io_in.Controls.Add(this.label7);
            this.gb_io_in.Controls.Add(this.tb_phonenumberinput);
            this.gb_io_in.Controls.Add(this.label6);
            this.gb_io_in.Controls.Add(this.tb_MsInput);
            this.gb_io_in.Controls.Add(this.label5);
            this.gb_io_in.Controls.Add(this.label4);
            this.gb_io_in.Location = new System.Drawing.Point(14, 78);
            this.gb_io_in.Name = "gb_io_in";
            this.gb_io_in.Size = new System.Drawing.Size(635, 225);
            this.gb_io_in.TabIndex = 7;
            this.gb_io_in.TabStop = false;
            this.gb_io_in.Text = "IO_IN";
            // 
            // checkedListBoxInput_IO_Setting
            // 
            this.checkedListBoxInput_IO_Setting.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkedListBoxInput_IO_Setting.FormattingEnabled = true;
            this.checkedListBoxInput_IO_Setting.Items.AddRange(new object[] {
            "Pin1 Low To High",
            "Pin1 High To Low",
            "Pin2 Low To High",
            "Pin2 High To Low",
            "Pin3 Low To High",
            "Pin3 High To Low",
            "Pin4 Low To High",
            "Pin4 High To Low"});
            this.checkedListBoxInput_IO_Setting.Location = new System.Drawing.Point(0, 53);
            this.checkedListBoxInput_IO_Setting.Name = "checkedListBoxInput_IO_Setting";
            this.checkedListBoxInput_IO_Setting.Size = new System.Drawing.Size(138, 148);
            this.checkedListBoxInput_IO_Setting.TabIndex = 15;
            this.checkedListBoxInput_IO_Setting.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxInput_IO_Setting_SelectedIndexChanged);
            // 
            // bt_set_io_input
            // 
            this.bt_set_io_input.Location = new System.Drawing.Point(655, 256);
            this.bt_set_io_input.Name = "bt_set_io_input";
            this.bt_set_io_input.Size = new System.Drawing.Size(60, 23);
            this.bt_set_io_input.TabIndex = 14;
            this.bt_set_io_input.Text = "Set All";
            this.bt_set_io_input.UseVisualStyleBackColor = true;
            this.bt_set_io_input.Click += new System.EventHandler(this.bt_set_io_input_Click);
            // 
            // bt_get_io_input
            // 
            this.bt_get_io_input.Location = new System.Drawing.Point(655, 299);
            this.bt_get_io_input.Name = "bt_get_io_input";
            this.bt_get_io_input.Size = new System.Drawing.Size(60, 23);
            this.bt_get_io_input.TabIndex = 13;
            this.bt_get_io_input.Text = "Get All";
            this.bt_get_io_input.UseVisualStyleBackColor = true;
            this.bt_get_io_input.Click += new System.EventHandler(this.bt_get_io_input_Click);
            // 
            // bt_clear_message
            // 
            this.bt_clear_message.Location = new System.Drawing.Point(362, 178);
            this.bt_clear_message.Name = "bt_clear_message";
            this.bt_clear_message.Size = new System.Drawing.Size(87, 23);
            this.bt_clear_message.TabIndex = 12;
            this.bt_clear_message.Text = "ClearMessage";
            this.bt_clear_message.UseVisualStyleBackColor = true;
            this.bt_clear_message.Click += new System.EventHandler(this.bt_clear_message_Click);
            // 
            // tb_PhoneMessage
            // 
            this.tb_PhoneMessage.Location = new System.Drawing.Point(362, 52);
            this.tb_PhoneMessage.MaxLength = 17;
            this.tb_PhoneMessage.Multiline = true;
            this.tb_PhoneMessage.Name = "tb_PhoneMessage";
            this.tb_PhoneMessage.Size = new System.Drawing.Size(239, 120);
            this.tb_PhoneMessage.TabIndex = 11;
            // 
            // bt_del_all
            // 
            this.bt_del_all.Location = new System.Drawing.Point(281, 178);
            this.bt_del_all.Name = "bt_del_all";
            this.bt_del_all.Size = new System.Drawing.Size(60, 23);
            this.bt_del_all.TabIndex = 10;
            this.bt_del_all.Text = "Del All";
            this.bt_del_all.UseVisualStyleBackColor = true;
            this.bt_del_all.Click += new System.EventHandler(this.bt_del_all_Click);
            // 
            // bt_delete
            // 
            this.bt_delete.Location = new System.Drawing.Point(281, 149);
            this.bt_delete.Name = "bt_delete";
            this.bt_delete.Size = new System.Drawing.Size(60, 23);
            this.bt_delete.TabIndex = 9;
            this.bt_delete.Text = "Del";
            this.bt_delete.UseVisualStyleBackColor = true;
            this.bt_delete.Click += new System.EventHandler(this.bt_delete_Click);
            // 
            // bt_add
            // 
            this.bt_add.Location = new System.Drawing.Point(281, 120);
            this.bt_add.Name = "bt_add";
            this.bt_add.Size = new System.Drawing.Size(60, 23);
            this.bt_add.TabIndex = 8;
            this.bt_add.Text = "Add";
            this.bt_add.UseVisualStyleBackColor = true;
            this.bt_add.Click += new System.EventHandler(this.bt_add_Click);
            // 
            // lb_phonenumber_show
            // 
            this.lb_phonenumber_show.FormattingEnabled = true;
            this.lb_phonenumber_show.ItemHeight = 12;
            this.lb_phonenumber_show.Location = new System.Drawing.Point(150, 101);
            this.lb_phonenumber_show.Name = "lb_phonenumber_show";
            this.lb_phonenumber_show.Size = new System.Drawing.Size(125, 100);
            this.lb_phonenumber_show.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(150, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(125, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "Phone Number Entry：";
            // 
            // tb_phonenumberinput
            // 
            this.tb_phonenumberinput.Location = new System.Drawing.Point(150, 70);
            this.tb_phonenumberinput.MaxLength = 15;
            this.tb_phonenumberinput.Name = "tb_phonenumberinput";
            this.tb_phonenumberinput.Size = new System.Drawing.Size(125, 21);
            this.tb_phonenumberinput.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(360, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(95, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "Phone Message：";
            // 
            // tb_MsInput
            // 
            this.tb_MsInput.Location = new System.Drawing.Point(197, 12);
            this.tb_MsInput.MaxLength = 5;
            this.tb_MsInput.Name = "tb_MsInput";
            this.tb_MsInput.Size = new System.Drawing.Size(78, 21);
            this.tb_MsInput.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(144, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 2;
            this.label5.Text = "Sustain";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(119, 12);
            this.label4.TabIndex = 0;
            this.label4.Text = "IO Control Select：";
            // 
            // gb_io_out
            // 
            this.gb_io_out.Controls.Add(this.cb_Pin8_State);
            this.gb_io_out.Controls.Add(this.cb_Pin7_State);
            this.gb_io_out.Controls.Add(this.cb_Pin6_State);
            this.gb_io_out.Controls.Add(this.cb_Pin5_State);
            this.gb_io_out.Controls.Add(this.label17);
            this.gb_io_out.Controls.Add(this.label16);
            this.gb_io_out.Controls.Add(this.label15);
            this.gb_io_out.Controls.Add(this.label14);
            this.gb_io_out.Controls.Add(this.label13);
            this.gb_io_out.Controls.Add(this.label12);
            this.gb_io_out.Controls.Add(this.label11);
            this.gb_io_out.Controls.Add(this.label10);
            this.gb_io_out.Controls.Add(this.label9);
            this.gb_io_out.Controls.Add(this.bt_del_all2);
            this.gb_io_out.Controls.Add(this.bt_delete2);
            this.gb_io_out.Controls.Add(this.bt_add2);
            this.gb_io_out.Controls.Add(this.lb_phonenumber_show2);
            this.gb_io_out.Controls.Add(this.tb_phonenumberinput2);
            this.gb_io_out.Controls.Add(this.cb_IsAckMessage);
            this.gb_io_out.Controls.Add(this.label8);
            this.gb_io_out.Location = new System.Drawing.Point(14, 309);
            this.gb_io_out.Name = "gb_io_out";
            this.gb_io_out.Size = new System.Drawing.Size(635, 179);
            this.gb_io_out.TabIndex = 8;
            this.gb_io_out.TabStop = false;
            this.gb_io_out.Text = "IO_OUT";
            // 
            // cb_Pin8_State
            // 
            this.cb_Pin8_State.FormattingEnabled = true;
            this.cb_Pin8_State.Items.AddRange(new object[] {
            "Low",
            "High"});
            this.cb_Pin8_State.Location = new System.Drawing.Point(461, 154);
            this.cb_Pin8_State.Name = "cb_Pin8_State";
            this.cb_Pin8_State.Size = new System.Drawing.Size(51, 20);
            this.cb_Pin8_State.TabIndex = 14;
            this.cb_Pin8_State.Text = "High";
            // 
            // bt_get_io_output
            // 
            this.bt_get_io_output.Location = new System.Drawing.Point(655, 405);
            this.bt_get_io_output.Name = "bt_get_io_output";
            this.bt_get_io_output.Size = new System.Drawing.Size(60, 23);
            this.bt_get_io_output.TabIndex = 13;
            this.bt_get_io_output.Text = "Get All";
            this.bt_get_io_output.UseVisualStyleBackColor = true;
            this.bt_get_io_output.Visible = false;
            this.bt_get_io_output.Click += new System.EventHandler(this.bt_get_io_output_Click);
            // 
            // bt_set_io_output
            // 
            this.bt_set_io_output.Location = new System.Drawing.Point(655, 376);
            this.bt_set_io_output.Name = "bt_set_io_output";
            this.bt_set_io_output.Size = new System.Drawing.Size(60, 23);
            this.bt_set_io_output.TabIndex = 13;
            this.bt_set_io_output.Text = "Set All";
            this.bt_set_io_output.UseVisualStyleBackColor = true;
            this.bt_set_io_output.Visible = false;
            this.bt_set_io_output.Click += new System.EventHandler(this.bt_set_io_output_Click);
            // 
            // cb_Pin7_State
            // 
            this.cb_Pin7_State.FormattingEnabled = true;
            this.cb_Pin7_State.Items.AddRange(new object[] {
            "Low",
            "High"});
            this.cb_Pin7_State.Location = new System.Drawing.Point(461, 114);
            this.cb_Pin7_State.Name = "cb_Pin7_State";
            this.cb_Pin7_State.Size = new System.Drawing.Size(51, 20);
            this.cb_Pin7_State.TabIndex = 14;
            this.cb_Pin7_State.Text = "High";
            // 
            // cb_Pin6_State
            // 
            this.cb_Pin6_State.FormattingEnabled = true;
            this.cb_Pin6_State.Items.AddRange(new object[] {
            "Low",
            "High"});
            this.cb_Pin6_State.Location = new System.Drawing.Point(461, 75);
            this.cb_Pin6_State.Name = "cb_Pin6_State";
            this.cb_Pin6_State.Size = new System.Drawing.Size(51, 20);
            this.cb_Pin6_State.TabIndex = 14;
            this.cb_Pin6_State.Text = "High";
            // 
            // cb_Pin5_State
            // 
            this.cb_Pin5_State.FormattingEnabled = true;
            this.cb_Pin5_State.Items.AddRange(new object[] {
            "Low",
            "High"});
            this.cb_Pin5_State.Location = new System.Drawing.Point(461, 37);
            this.cb_Pin5_State.Name = "cb_Pin5_State";
            this.cb_Pin5_State.Size = new System.Drawing.Size(51, 20);
            this.cb_Pin5_State.TabIndex = 14;
            this.cb_Pin5_State.Text = "High";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(405, 40);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(41, 12);
            this.label17.TabIndex = 13;
            this.label17.Text = "State:";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(402, 157);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(41, 12);
            this.label16.TabIndex = 13;
            this.label16.Text = "State:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(402, 117);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(41, 12);
            this.label15.TabIndex = 13;
            this.label15.Text = "State:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(402, 78);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(41, 12);
            this.label14.TabIndex = 12;
            this.label14.Text = "State:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(350, 157);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 11;
            this.label13.Text = "Pin8";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(350, 117);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 12);
            this.label12.TabIndex = 10;
            this.label12.Text = "Pin7";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(350, 78);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 9;
            this.label11.Text = "Pin6";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(350, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 8;
            this.label10.Text = "Pin5";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(387, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 12);
            this.label9.TabIndex = 7;
            this.label9.Text = "Pin Output Init";
            // 
            // bt_del_all2
            // 
            this.bt_del_all2.Location = new System.Drawing.Point(144, 146);
            this.bt_del_all2.Name = "bt_del_all2";
            this.bt_del_all2.Size = new System.Drawing.Size(60, 23);
            this.bt_del_all2.TabIndex = 6;
            this.bt_del_all2.Text = "Del All";
            this.bt_del_all2.UseVisualStyleBackColor = true;
            this.bt_del_all2.Click += new System.EventHandler(this.bt_del_all2_Click);
            // 
            // bt_delete2
            // 
            this.bt_delete2.Location = new System.Drawing.Point(144, 117);
            this.bt_delete2.Name = "bt_delete2";
            this.bt_delete2.Size = new System.Drawing.Size(60, 23);
            this.bt_delete2.TabIndex = 5;
            this.bt_delete2.Text = "Del";
            this.bt_delete2.UseVisualStyleBackColor = true;
            this.bt_delete2.Click += new System.EventHandler(this.bt_delete2_Click);
            // 
            // bt_add2
            // 
            this.bt_add2.Location = new System.Drawing.Point(144, 88);
            this.bt_add2.Name = "bt_add2";
            this.bt_add2.Size = new System.Drawing.Size(60, 23);
            this.bt_add2.TabIndex = 4;
            this.bt_add2.Text = "Add";
            this.bt_add2.UseVisualStyleBackColor = true;
            this.bt_add2.Click += new System.EventHandler(this.bt_add2_Click);
            // 
            // lb_phonenumber_show2
            // 
            this.lb_phonenumber_show2.FormattingEnabled = true;
            this.lb_phonenumber_show2.ItemHeight = 12;
            this.lb_phonenumber_show2.Location = new System.Drawing.Point(6, 69);
            this.lb_phonenumber_show2.Name = "lb_phonenumber_show2";
            this.lb_phonenumber_show2.Size = new System.Drawing.Size(132, 100);
            this.lb_phonenumber_show2.TabIndex = 3;
            // 
            // tb_phonenumberinput2
            // 
            this.tb_phonenumberinput2.Location = new System.Drawing.Point(6, 37);
            this.tb_phonenumberinput2.MaxLength = 15;
            this.tb_phonenumberinput2.Name = "tb_phonenumberinput2";
            this.tb_phonenumberinput2.Size = new System.Drawing.Size(132, 21);
            this.tb_phonenumberinput2.TabIndex = 2;
            // 
            // cb_IsAckMessage
            // 
            this.cb_IsAckMessage.AutoSize = true;
            this.cb_IsAckMessage.Location = new System.Drawing.Point(197, 36);
            this.cb_IsAckMessage.Name = "cb_IsAckMessage";
            this.cb_IsAckMessage.Size = new System.Drawing.Size(138, 16);
            this.cb_IsAckMessage.TabIndex = 1;
            this.cb_IsAckMessage.Text = "ACK When Setting IO";
            this.cb_IsAckMessage.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(125, 12);
            this.label8.TabIndex = 0;
            this.label8.Text = "Phone Number Entry：";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbParity);
            this.groupBox1.Controls.Add(this.cbDataBits);
            this.groupBox1.Controls.Add(this.cbStop);
            this.groupBox1.Controls.Add(this.cbBaudRate);
            this.groupBox1.Controls.Add(this.label20);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.ComPort);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(480, 66);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Serial port Settings";
            // 
            // cbParity
            // 
            this.cbParity.FormattingEnabled = true;
            this.cbParity.Items.AddRange(new object[] {
            "无",
            "奇校验",
            "偶校验"});
            this.cbParity.Location = new System.Drawing.Point(360, 45);
            this.cbParity.Name = "cbParity";
            this.cbParity.Size = new System.Drawing.Size(63, 20);
            this.cbParity.TabIndex = 4;
            // 
            // cbDataBits
            // 
            this.cbDataBits.FormattingEnabled = true;
            this.cbDataBits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.cbDataBits.Location = new System.Drawing.Point(361, 20);
            this.cbDataBits.Name = "cbDataBits";
            this.cbDataBits.Size = new System.Drawing.Size(63, 20);
            this.cbDataBits.TabIndex = 4;
            // 
            // cbStop
            // 
            this.cbStop.FormattingEnabled = true;
            this.cbStop.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.cbStop.Location = new System.Drawing.Point(203, 46);
            this.cbStop.Name = "cbStop";
            this.cbStop.Size = new System.Drawing.Size(63, 20);
            this.cbStop.TabIndex = 4;
            // 
            // cbBaudRate
            // 
            this.cbBaudRate.FormattingEnabled = true;
            this.cbBaudRate.Items.AddRange(new object[] {
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "115200"});
            this.cbBaudRate.Location = new System.Drawing.Point(203, 20);
            this.cbBaudRate.Name = "cbBaudRate";
            this.cbBaudRate.Size = new System.Drawing.Size(63, 20);
            this.cbBaudRate.TabIndex = 4;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(287, 52);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 12);
            this.label20.TabIndex = 3;
            this.label20.Text = "ParityBits：";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(287, 24);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(65, 12);
            this.label19.TabIndex = 2;
            this.label19.Text = "DataBits：";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(142, 52);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 1;
            this.label18.Text = "StopBits：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(142, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "BaudRate：";
            // 
            // bt_save_excel
            // 
            this.bt_save_excel.Location = new System.Drawing.Point(648, 487);
            this.bt_save_excel.Name = "bt_save_excel";
            this.bt_save_excel.Size = new System.Drawing.Size(117, 23);
            this.bt_save_excel.TabIndex = 17;
            this.bt_save_excel.Text = "Save the Settings";
            this.bt_save_excel.UseVisualStyleBackColor = true;
            this.bt_save_excel.Click += new System.EventHandler(this.bt_save_excel_Click);
            // 
            // bt_read_excel
            // 
            this.bt_read_excel.Location = new System.Drawing.Point(648, 516);
            this.bt_read_excel.Name = "bt_read_excel";
            this.bt_read_excel.Size = new System.Drawing.Size(117, 23);
            this.bt_read_excel.TabIndex = 17;
            this.bt_read_excel.Text = "Read the Settings";
            this.bt_read_excel.UseVisualStyleBackColor = true;
            this.bt_read_excel.Click += new System.EventHandler(this.bt_read_excel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(784, 551);
            this.Controls.Add(this.bt_read_excel);
            this.Controls.Add(this.bt_get_io_output);
            this.Controls.Add(this.bt_set_io_input);
            this.Controls.Add(this.bt_set_io_output);
            this.Controls.Add(this.bt_save_excel);
            this.Controls.Add(this.bt_get_io_input);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gb_io_out);
            this.Controls.Add(this.gb_io_in);
            this.Controls.Add(this.bt_enter_config);
            this.Controls.Add(this.bt_serial_open_close);
            this.Controls.Add(this.label3);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "通信設置";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.gb_io_in.ResumeLayout(false);
            this.gb_io_in.PerformLayout();
            this.gb_io_out.ResumeLayout(false);
            this.gb_io_out.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComPort;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bt_serial_open_close;
        private System.Windows.Forms.Button bt_enter_config;
        private System.Windows.Forms.GroupBox gb_io_in;
        private System.Windows.Forms.Button bt_set_io_input;
        private System.Windows.Forms.Button bt_get_io_input;
        private System.Windows.Forms.Button bt_clear_message;
        private System.Windows.Forms.TextBox tb_PhoneMessage;
        private System.Windows.Forms.Button bt_del_all;
        private System.Windows.Forms.Button bt_delete;
        private System.Windows.Forms.Button bt_add;
        private System.Windows.Forms.ListBox lb_phonenumber_show;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tb_phonenumberinput;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tb_MsInput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gb_io_out;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button bt_del_all2;
        private System.Windows.Forms.Button bt_delete2;
        private System.Windows.Forms.Button bt_add2;
        private System.Windows.Forms.ListBox lb_phonenumber_show2;
        private System.Windows.Forms.TextBox tb_phonenumberinput2;
        private System.Windows.Forms.CheckBox cb_IsAckMessage;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cb_Pin8_State;
        private System.Windows.Forms.Button bt_get_io_output;
        private System.Windows.Forms.Button bt_set_io_output;
        private System.Windows.Forms.ComboBox cb_Pin7_State;
        private System.Windows.Forms.ComboBox cb_Pin6_State;
        private System.Windows.Forms.ComboBox cb_Pin5_State;
        private System.Windows.Forms.CheckedListBox checkedListBoxInput_IO_Setting;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cbParity;
        private System.Windows.Forms.ComboBox cbDataBits;
        private System.Windows.Forms.ComboBox cbStop;
        private System.Windows.Forms.ComboBox cbBaudRate;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bt_save_excel;
        private System.Windows.Forms.Button bt_read_excel;
    }
}

