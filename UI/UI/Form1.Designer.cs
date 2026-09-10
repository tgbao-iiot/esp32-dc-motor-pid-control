namespace giaodien
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.connect_btn = new System.Windows.Forms.Button();
            this.disconnect_btn = new System.Windows.Forms.Button();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label2 = new System.Windows.Forms.Label();
            this.on_btn = new System.Windows.Forms.Button();
            this.off_btn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.kp = new System.Windows.Forms.TextBox();
            this.ki = new System.Windows.Forms.TextBox();
            this.kd = new System.Windows.Forms.TextBox();
            this.config = new System.Windows.Forms.Button();
            this.status_motor = new System.Windows.Forms.Button();
            this.draw_chart = new System.Windows.Forms.Button();
            this.stop_chart = new System.Windows.Forms.Button();
            this.status_connect = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lbl_pid = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpeed = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rotation = new System.Windows.Forms.Label();
            this.reverse = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.re_chart = new System.Windows.Forms.Button();
            this.speed_bar = new System.Windows.Forms.TrackBar();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.speed_bar)).BeginInit();
            this.SuspendLayout();
            // 
            // connect_btn
            // 
            this.connect_btn.BackColor = System.Drawing.Color.Lime;
            this.connect_btn.Location = new System.Drawing.Point(4, 31);
            this.connect_btn.Margin = new System.Windows.Forms.Padding(2);
            this.connect_btn.Name = "connect_btn";
            this.connect_btn.Size = new System.Drawing.Size(76, 30);
            this.connect_btn.TabIndex = 3;
            this.connect_btn.Text = "CONNECT";
            this.connect_btn.UseVisualStyleBackColor = false;
            this.connect_btn.Click += new System.EventHandler(this.connect_btn_Click);
            // 
            // disconnect_btn
            // 
            this.disconnect_btn.BackColor = System.Drawing.Color.Red;
            this.disconnect_btn.Location = new System.Drawing.Point(86, 31);
            this.disconnect_btn.Margin = new System.Windows.Forms.Padding(2);
            this.disconnect_btn.Name = "disconnect_btn";
            this.disconnect_btn.Size = new System.Drawing.Size(92, 30);
            this.disconnect_btn.TabIndex = 4;
            this.disconnect_btn.Text = "DISCONNECT";
            this.disconnect_btn.UseVisualStyleBackColor = false;
            this.disconnect_btn.Click += new System.EventHandler(this.disconnect_btn_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(12, 66);
            this.chart1.Margin = new System.Windows.Forms.Padding(2);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(550, 282);
            this.chart1.TabIndex = 5;
            this.chart1.Text = "chart1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 117);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Speed";
            // 
            // on_btn
            // 
            this.on_btn.BackColor = System.Drawing.Color.Lime;
            this.on_btn.Location = new System.Drawing.Point(4, 17);
            this.on_btn.Margin = new System.Windows.Forms.Padding(2);
            this.on_btn.Name = "on_btn";
            this.on_btn.Size = new System.Drawing.Size(68, 28);
            this.on_btn.TabIndex = 7;
            this.on_btn.Text = "on motor";
            this.on_btn.UseVisualStyleBackColor = false;
            this.on_btn.Click += new System.EventHandler(this.on_btn_Click);
            // 
            // off_btn
            // 
            this.off_btn.BackColor = System.Drawing.Color.Red;
            this.off_btn.Location = new System.Drawing.Point(4, 50);
            this.off_btn.Margin = new System.Windows.Forms.Padding(2);
            this.off_btn.Name = "off_btn";
            this.off_btn.Size = new System.Drawing.Size(68, 27);
            this.off_btn.TabIndex = 8;
            this.off_btn.Text = "off motor";
            this.off_btn.UseVisualStyleBackColor = false;
            this.off_btn.Click += new System.EventHandler(this.off_btn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "kp";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 67);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "ki";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 92);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(21, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "kd";
            // 
            // kp
            // 
            this.kp.Location = new System.Drawing.Point(50, 39);
            this.kp.Margin = new System.Windows.Forms.Padding(2);
            this.kp.Name = "kp";
            this.kp.Size = new System.Drawing.Size(76, 19);
            this.kp.TabIndex = 12;
            // 
            // ki
            // 
            this.ki.Location = new System.Drawing.Point(50, 62);
            this.ki.Margin = new System.Windows.Forms.Padding(2);
            this.ki.Name = "ki";
            this.ki.Size = new System.Drawing.Size(76, 19);
            this.ki.TabIndex = 13;
            // 
            // kd
            // 
            this.kd.Location = new System.Drawing.Point(50, 87);
            this.kd.Margin = new System.Windows.Forms.Padding(2);
            this.kd.Name = "kd";
            this.kd.Size = new System.Drawing.Size(76, 19);
            this.kd.TabIndex = 14;
            // 
            // config
            // 
            this.config.BackColor = System.Drawing.Color.Lime;
            this.config.Location = new System.Drawing.Point(16, 138);
            this.config.Margin = new System.Windows.Forms.Padding(2);
            this.config.Name = "config";
            this.config.Size = new System.Drawing.Size(128, 28);
            this.config.TabIndex = 15;
            this.config.Text = "CONFIG CONTROL";
            this.config.UseVisualStyleBackColor = false;
            this.config.Click += new System.EventHandler(this.config_Click);
            // 
            // status_motor
            // 
            this.status_motor.Location = new System.Drawing.Point(86, 25);
            this.status_motor.Margin = new System.Windows.Forms.Padding(2);
            this.status_motor.Name = "status_motor";
            this.status_motor.Size = new System.Drawing.Size(56, 42);
            this.status_motor.TabIndex = 16;
            this.status_motor.Text = "status";
            this.status_motor.UseVisualStyleBackColor = true;
            // 
            // draw_chart
            // 
            this.draw_chart.BackColor = System.Drawing.Color.Lime;
            this.draw_chart.Location = new System.Drawing.Point(12, 27);
            this.draw_chart.Margin = new System.Windows.Forms.Padding(2);
            this.draw_chart.Name = "draw_chart";
            this.draw_chart.Size = new System.Drawing.Size(76, 31);
            this.draw_chart.TabIndex = 17;
            this.draw_chart.Text = "draw chart";
            this.draw_chart.UseVisualStyleBackColor = false;
            this.draw_chart.Click += new System.EventHandler(this.draw_chart_Click);
            // 
            // stop_chart
            // 
            this.stop_chart.BackColor = System.Drawing.Color.Red;
            this.stop_chart.Location = new System.Drawing.Point(109, 27);
            this.stop_chart.Margin = new System.Windows.Forms.Padding(2);
            this.stop_chart.Name = "stop_chart";
            this.stop_chart.Size = new System.Drawing.Size(76, 31);
            this.stop_chart.TabIndex = 18;
            this.stop_chart.Text = "stop chart";
            this.stop_chart.UseVisualStyleBackColor = false;
            this.stop_chart.Click += new System.EventHandler(this.stop_chart_Click);
            // 
            // status_connect
            // 
            this.status_connect.AutoSize = true;
            this.status_connect.Location = new System.Drawing.Point(55, 72);
            this.status_connect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.status_connect.Name = "status_connect";
            this.status_connect.Size = new System.Drawing.Size(41, 13);
            this.status_connect.TabIndex = 19;
            this.status_connect.Text = "status";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lbl_pid
            // 
            this.lbl_pid.AutoSize = true;
            this.lbl_pid.Location = new System.Drawing.Point(43, 20);
            this.lbl_pid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_pid.Name = "lbl_pid";
            this.lbl_pid.Size = new System.Drawing.Size(41, 13);
            this.lbl_pid.TabIndex = 20;
            this.lbl_pid.Text = "status";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "PID:";
            // 
            // txtSpeed
            // 
            this.txtSpeed.Location = new System.Drawing.Point(50, 115);
            this.txtSpeed.Margin = new System.Windows.Forms.Padding(2);
            this.txtSpeed.Name = "txtSpeed";
            this.txtSpeed.Size = new System.Drawing.Size(76, 19);
            this.txtSpeed.TabIndex = 22;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.connect_btn);
            this.groupBox1.Controls.Add(this.disconnect_btn);
            this.groupBox1.Controls.Add(this.status_connect);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(19, 10);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox1.Size = new System.Drawing.Size(182, 101);
            this.groupBox1.TabIndex = 23;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CONNECT WIFI";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rotation);
            this.groupBox2.Controls.Add(this.reverse);
            this.groupBox2.Controls.Add(this.on_btn);
            this.groupBox2.Controls.Add(this.off_btn);
            this.groupBox2.Controls.Add(this.status_motor);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(19, 116);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(161, 122);
            this.groupBox2.TabIndex = 24;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "ON/OFF MOTOR";
            // 
            // rotation
            // 
            this.rotation.AutoSize = true;
            this.rotation.Location = new System.Drawing.Point(80, 90);
            this.rotation.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.rotation.Name = "rotation";
            this.rotation.Size = new System.Drawing.Size(50, 13);
            this.rotation.TabIndex = 30;
            this.rotation.Text = "rotation";
            // 
            // reverse
            // 
            this.reverse.BackColor = System.Drawing.Color.Yellow;
            this.reverse.Location = new System.Drawing.Point(4, 83);
            this.reverse.Margin = new System.Windows.Forms.Padding(2);
            this.reverse.Name = "reverse";
            this.reverse.Size = new System.Drawing.Size(68, 27);
            this.reverse.TabIndex = 17;
            this.reverse.Text = "reverse";
            this.reverse.UseVisualStyleBackColor = false;
            this.reverse.Click += new System.EventHandler(this.reverse_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbl_pid);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.txtSpeed);
            this.groupBox3.Controls.Add(this.kp);
            this.groupBox3.Controls.Add(this.ki);
            this.groupBox3.Controls.Add(this.kd);
            this.groupBox3.Controls.Add(this.config);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(19, 241);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox3.Size = new System.Drawing.Size(161, 177);
            this.groupBox3.TabIndex = 25;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "PID CONTROL";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.re_chart);
            this.groupBox4.Controls.Add(this.chart1);
            this.groupBox4.Controls.Add(this.draw_chart);
            this.groupBox4.Controls.Add(this.stop_chart);
            this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(200, 12);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox4.Size = new System.Drawing.Size(574, 367);
            this.groupBox4.TabIndex = 26;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "CHART SPEED";
            // 
            // re_chart
            // 
            this.re_chart.BackColor = System.Drawing.Color.Yellow;
            this.re_chart.Location = new System.Drawing.Point(207, 27);
            this.re_chart.Margin = new System.Windows.Forms.Padding(2);
            this.re_chart.Name = "re_chart";
            this.re_chart.Size = new System.Drawing.Size(76, 31);
            this.re_chart.TabIndex = 19;
            this.re_chart.Text = "reset chart";
            this.re_chart.UseVisualStyleBackColor = false;
            this.re_chart.Click += new System.EventHandler(this.re_chart_Click);
            // 
            // speed_bar
            // 
            this.speed_bar.Location = new System.Drawing.Point(282, 394);
            this.speed_bar.Margin = new System.Windows.Forms.Padding(2);
            this.speed_bar.Maximum = 330;
            this.speed_bar.Name = "speed_bar";
            this.speed_bar.Size = new System.Drawing.Size(261, 45);
            this.speed_bar.TabIndex = 27;
            this.speed_bar.TickFrequency = 5;
            this.speed_bar.Value = 100;
            this.speed_bar.Scroll += new System.EventHandler(this.speed_bar_Scroll);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Control;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(197, 402);
            this.label6.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 17);
            this.label6.TabIndex = 28;
            this.label6.Text = "Set Speed: ";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 448);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.speed_bar);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.speed_bar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button connect_btn;
        private System.Windows.Forms.Button disconnect_btn;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button on_btn;
        private System.Windows.Forms.Button off_btn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox kp;
        private System.Windows.Forms.TextBox ki;
        private System.Windows.Forms.TextBox kd;
        private System.Windows.Forms.Button config;
        private System.Windows.Forms.Button status_motor;
        private System.Windows.Forms.Button draw_chart;
        private System.Windows.Forms.Button stop_chart;
        private System.Windows.Forms.Label status_connect;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lbl_pid;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSpeed;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button re_chart;
        private System.Windows.Forms.TrackBar speed_bar;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button reverse;
        private System.Windows.Forms.Label rotation;
    }
}