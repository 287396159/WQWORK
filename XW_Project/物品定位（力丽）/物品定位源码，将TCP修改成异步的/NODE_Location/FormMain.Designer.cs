using CiXinLocation.bean;
namespace CiXinLocation
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.button11 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.moveThreeAbleList1 = new MoveableListLib.MoveThreeAbleList();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.moveableList1 = new MoveableListLib.MoveableList();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label_company = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(215)))), ((int)(((byte)(215)))), ((int)(((byte)(217)))));
            this.panel3.Controls.Add(this.label8);
            this.panel3.Controls.Add(this.button11);
            this.panel3.Controls.Add(this.button10);
            this.panel3.Controls.Add(this.button9);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Location = new System.Drawing.Point(200, 144);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(787, 573);
            this.panel3.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(639, 465);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 12);
            this.label8.TabIndex = 9;
            this.label8.Text = "同步結束";
            this.label8.Visible = false;
            // 
            // button11
            // 
            this.button11.BackColor = System.Drawing.SystemColors.Control;
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button11.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button11.Location = new System.Drawing.Point(639, 430);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(90, 30);
            this.button11.TabIndex = 8;
            this.button11.Text = "同步資訊";
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Visible = false;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button10
            // 
            this.button10.BackColor = System.Drawing.SystemColors.Control;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button10.ForeColor = System.Drawing.Color.Black;
            this.button10.Location = new System.Drawing.Point(639, 457);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(90, 30);
            this.button10.TabIndex = 7;
            this.button10.Text = "歷史信息";
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Visible = false;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.SystemColors.Control;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button9.ForeColor = System.Drawing.Color.Crimson;
            this.button9.Location = new System.Drawing.Point(639, 263);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(90, 30);
            this.button9.TabIndex = 6;
            this.button9.Text = "0";
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click_1);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(609, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(124, 27);
            this.label5.TabIndex = 5;
            this.label5.Text = "報警信息";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.Control;
            this.panel4.Controls.Add(this.button8);
            this.panel4.Controls.Add(this.button7);
            this.panel4.Controls.Add(this.moveThreeAbleList1);
            this.panel4.Location = new System.Drawing.Point(0, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(603, 568);
            this.panel4.TabIndex = 4;
            // 
            // button8
            // 
            this.button8.BackgroundImage = global::CiXinLocation.Properties.Resources.quyu_down1;
            this.button8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button8.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(521, 513);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(53, 49);
            this.button8.TabIndex = 2;
            this.button8.Tag = "quyu_down";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            this.button8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_mouseDown);
            this.button8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_mouseUp);
            // 
            // button7
            // 
            this.button7.BackgroundImage = global::CiXinLocation.Properties.Resources.quyu_up1;
            this.button7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button7.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(523, 8);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(53, 49);
            this.button7.TabIndex = 1;
            this.button7.Tag = "quyu_up";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            this.button7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_mouseDown);
            this.button7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_mouseUp);
            // 
            // moveThreeAbleList1
            // 
            this.moveThreeAbleList1.Colors = null;
            this.moveThreeAbleList1.Location = new System.Drawing.Point(6, 10);
            this.moveThreeAbleList1.Name = "moveThreeAbleList1";
            this.moveThreeAbleList1.Size = new System.Drawing.Size(507, 532);
            this.moveThreeAbleList1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(748, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(18, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "台";
            // 
            // button4
            // 
            this.button4.BackColor = System.Drawing.SystemColors.Control;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button4.ForeColor = System.Drawing.Color.RoyalBlue;
            this.button4.Location = new System.Drawing.Point(639, 63);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(90, 30);
            this.button4.TabIndex = 2;
            this.button4.Text = "0";
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(609, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(124, 27);
            this.label2.TabIndex = 1;
            this.label2.Text = "布車總數";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.moveableList1);
            this.panel2.Controls.Add(this.button6);
            this.panel2.Controls.Add(this.button5);
            this.panel2.Location = new System.Drawing.Point(0, 144);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 573);
            this.panel2.TabIndex = 1;
            // 
            // moveableList1
            // 
            this.moveableList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(43)))), ((int)(((byte)(54)))));
            this.moveableList1.Location = new System.Drawing.Point(0, 35);
            this.moveableList1.Name = "moveableList1";
            this.moveableList1.Size = new System.Drawing.Size(200, 502);
            this.moveableList1.TabIndex = 1;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(29)))), ((int)(((byte)(38)))));
            this.button6.BackgroundImage = global::CiXinLocation.Properties.Resources.manu_down1;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(206)))), ((int)(((byte)(178)))));
            this.button6.Location = new System.Drawing.Point(0, 537);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(200, 35);
            this.button6.TabIndex = 2;
            this.button6.Tag = "CHANGQU_DOWN";
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            this.button6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_mouseDown);
            this.button6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_mouseUp);
            // 
            // button5
            // 
            this.button5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(29)))), ((int)(((byte)(38)))));
            this.button5.BackgroundImage = global::CiXinLocation.Properties.Resources.manu_up1;
            this.button5.Dock = System.Windows.Forms.DockStyle.Top;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(206)))), ((int)(((byte)(178)))));
            this.button5.Location = new System.Drawing.Point(0, 0);
            this.button5.Margin = new System.Windows.Forms.Padding(0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(200, 35);
            this.button5.TabIndex = 1;
            this.button5.Tag = "CHANGQU_UP";
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            this.button5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.button_mouseDown);
            this.button5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.button_mouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(91)))), ((int)(((byte)(199)))), ((int)(((byte)(199)))));
            this.panel1.Controls.Add(this.panel8);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.panel7);
            this.panel1.Controls.Add(this.panel6);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label_company);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(200, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(787, 144);
            this.panel1.TabIndex = 0;
            // 
            // panel8
            // 
            this.panel8.BackgroundImage = global::CiXinLocation.Properties.Resources.localogo;
            this.panel8.Location = new System.Drawing.Point(36, 33);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(73, 78);
            this.panel8.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(247)))), ((int)(((byte)(196)))));
            this.label4.Location = new System.Drawing.Point(171, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(278, 12);
            this.label4.TabIndex = 7;
            this.label4.Text = "- - - - - - - - - - - - - - - - - - - -";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // panel7
            // 
            this.panel7.BackgroundImage = global::CiXinLocation.Properties.Resources.pc_1;
            this.panel7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel7.Location = new System.Drawing.Point(476, 59);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(93, 73);
            this.panel7.TabIndex = 6;
            // 
            // panel6
            // 
            this.panel6.BackgroundImage = global::CiXinLocation.Properties.Resources.tag_1;
            this.panel6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panel6.Location = new System.Drawing.Point(111, 79);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(35, 53);
            this.panel6.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label1.Location = new System.Drawing.Point(150, 38);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(295, 34);
            this.label1.TabIndex = 4;
            this.label1.Text = "布車定位管理系統";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label_company
            // 
            this.label_company.AutoSize = true;
            this.label_company.Font = new System.Drawing.Font("宋体", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_company.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.label_company.Location = new System.Drawing.Point(186, 11);
            this.label_company.Margin = new System.Windows.Forms.Padding(0);
            this.label_company.Name = "label_company";
            this.label_company.Size = new System.Drawing.Size(219, 20);
            this.label_company.TabIndex = 3;
            this.label_company.Text = "力麗科技股份有限公司";
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::CiXinLocation.Properties.Resources.start_1;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button3.ForeColor = System.Drawing.SystemColors.Control;
            this.button3.Location = new System.Drawing.Point(611, 97);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(174, 35);
            this.button3.TabIndex = 2;
            this.button3.Tag = "green";
            this.button3.Text = "查詢";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            this.button3.MouseEnter += new System.EventHandler(this.button_mouse);
            this.button3.MouseLeave += new System.EventHandler(this.button_mouse_over);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::CiXinLocation.Properties.Resources.start_1;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.ForeColor = System.Drawing.SystemColors.Control;
            this.button2.Location = new System.Drawing.Point(611, 55);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(174, 35);
            this.button2.TabIndex = 1;
            this.button2.Tag = "green";
            this.button2.Text = "參數設定";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            this.button2.MouseEnter += new System.EventHandler(this.button_mouse);
            this.button2.MouseLeave += new System.EventHandler(this.button_mouse_over);
            // 
            // button1
            // 
            this.button1.BackgroundImage = global::CiXinLocation.Properties.Resources.start_1;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(611, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(174, 35);
            this.button1.TabIndex = 0;
            this.button1.Tag = "green";
            this.button1.Text = "開始連接監控";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.button1.MouseEnter += new System.EventHandler(this.button_mouse);
            this.button1.MouseLeave += new System.EventHandler(this.button_mouse_over);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(219)))), ((int)(((byte)(222)))));
            this.panel5.BackgroundImage = global::CiXinLocation.Properties.Resources.logo1;
            this.panel5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel5.Location = new System.Drawing.Point(16, 41);
            this.panel5.Margin = new System.Windows.Forms.Padding(0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(166, 52);
            this.panel5.TabIndex = 3;
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(219)))), ((int)(((byte)(222)))));
            this.panel9.Controls.Add(this.label7);
            this.panel9.Controls.Add(this.label6);
            this.panel9.Controls.Add(this.panel5);
            this.panel9.Location = new System.Drawing.Point(0, 0);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(200, 144);
            this.panel9.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.SystemColors.Control;
            this.label7.Location = new System.Drawing.Point(53, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(20, 10);
            this.label7.TabIndex = 5;
            this.label7.Text = "...";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label6.ForeColor = System.Drawing.SystemColors.Control;
            this.label6.Location = new System.Drawing.Point(12, 11);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 4;
            this.label6.Text = "[登錄]";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // timer2
            // 
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // timer3
            // 
            this.timer3.Interval = 180000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 717);
            this.Controls.Add(this.panel9);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FormMain";
            this.Text = "LIBOLON_";
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label_company;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private MoveableListLib.MoveableList moveableList1;
        private System.Windows.Forms.Panel panel5;
        private MoveableListLib.MoveThreeAbleList moveThreeAbleList1;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Timer timer3;
    }
}