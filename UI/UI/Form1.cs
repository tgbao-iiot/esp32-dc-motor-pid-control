using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace giaodien
{
    public partial class Form1 : Form
    {
        private const string esp32IP = "http://192.168.4.1";
        private const int filterSize = 20;

        private readonly HttpClient httpClient = new HttpClient();
        private readonly Queue<double> speedBuffer = new Queue<double>();
        private double elapsedTime = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var area = chart1.ChartAreas[0];
            chart1.Series[0].ChartType = SeriesChartType.Line;
            area.AxisX.Title = "Thời gian (s)";
            area.AxisY.Title = "Tốc độ (RPM)";
            area.AxisX.MajorGrid.LineColor = Color.LightGray;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            chart1.Series[0].Color = Color.Blue;
            chart1.Series[0].BorderWidth = 2;
            area.AxisX.LabelStyle.Format = "0.00";
        }

        private async Task SendHttp(string path)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(esp32IP + path);
                if (response.IsSuccessStatusCode)
                {
                    lbl_pid.Text = "Đã gửi: " + path;
                    lbl_pid.ForeColor = Color.Green;
                }
                else
                {
                    lbl_pid.Text = "Lỗi gửi";
                    lbl_pid.ForeColor = Color.Red;
                }
            }
            catch
            {
                status_connect.Text = "Mất kết nối ESP32";
                status_connect.ForeColor = Color.Red;
            }
        }

        private async void connect_btn_Click(object sender, EventArgs e)
        {
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(esp32IP + "/");
                UpdateUIStatus(true);
                status_connect.Text = "Đã kết nối ESP32";
                status_connect.ForeColor = Color.Green;

                await LoadESP32Config();
            }
            catch
            {
                status_connect.Text = "Không thể kết nối ESP32";
                status_connect.ForeColor = Color.Red;
            }
        }

        private void disconnect_btn_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            UpdateUIStatus(false);
            status_connect.Text = "Đã ngắt kết nối ESP32";
            status_connect.ForeColor = Color.Gray;
        }

        private async void config_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Kiểm tra tính hợp lệ dữ liệu nhập từ các TextBox
                if (!double.TryParse(kp.Text, out double kpVal) ||
                    !double.TryParse(ki.Text, out double kiVal) ||
                    !double.TryParse(kd.Text, out double kdVal) ||
                    !int.TryParse(txtSpeed.Text, out int speedVal))
                {
                    MessageBox.Show("Vui lòng nhập đúng định dạng số cho các thông số!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Đóng gói thành chuỗi JSON
                string jsonPayload = $"{{\"kp\":{kpVal}, \"ki\":{kiVal}, \"kd\":{kdVal}, \"target\":{speedVal}}}";

                // 3. Tạo Content với Header application/json
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                // 4. Gửi DUY NHẤT 1 request HTTP POST sang ESP32
                HttpResponseMessage response = await httpClient.PostAsync(esp32IP + "/api/config", content);

                if (response.IsSuccessStatusCode)
                {
                    lbl_pid.Text = "Đã cập nhật cấu hình (POST JSON thành công)!";
                    lbl_pid.ForeColor = Color.Green;
                }
                else
                {
                    lbl_pid.Text = "Lỗi cập nhật cấu hình: " + response.StatusCode;
                    lbl_pid.ForeColor = Color.Red;
                }
            }
            catch (Exception ex)
            {
                status_connect.Text = "Mất kết nối ESP32";
                status_connect.ForeColor = Color.Red;
                MessageBox.Show("Lỗi kết nối: " + ex.Message);
            }
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string result = await httpClient.GetStringAsync(esp32IP + "/rpm");
                if (!double.TryParse(result, out double speed)) return;

                // Cập nhật bộ đệm lọc trung bình
                speedBuffer.Enqueue(speed);
                if (speedBuffer.Count > filterSize)
                    speedBuffer.Dequeue();

                double filteredSpeed = speedBuffer.Average();

                // Cập nhật trạng thái động cơ
                if (filteredSpeed > 0)
                {
                    rotation.Text = "Forward rotation";
                    status_motor.BackColor = Color.Green;
                }
                else if (filteredSpeed < 0)
                {
                    rotation.Text = "Reverse rotation";
                    status_motor.BackColor = Color.Green;
                }
                else
                {
                    rotation.Text = "motor stop";
                    status_motor.BackColor = Color.Red;
                }

                // Vẽ đồ thị
                elapsedTime += 0.1;
                chart1.Series[0].Points.AddXY(elapsedTime, filteredSpeed);

                if (elapsedTime > 10)
                {
                    while (chart1.Series[0].Points.Count > 0 &&
                           chart1.Series[0].Points[0].XValue < elapsedTime - 10)
                    {
                        chart1.Series[0].Points.RemoveAt(0);
                    }

                    chart1.ChartAreas[0].AxisX.Minimum = elapsedTime - 10;
                    chart1.ChartAreas[0].AxisX.Maximum = elapsedTime;
                }

                lbl_pid.Text = "Đang nhận SPEED (đã lọc trung bình)";
                lbl_pid.ForeColor = Color.Green;
            }
            catch
            {
                timer1.Stop();
                status_connect.Text = "Mất kết nối ESP32";
                status_connect.ForeColor = Color.Red;
            }
        }

        private void draw_chart_Click(object sender, EventArgs e) => timer1.Start();

        private void stop_chart_Click(object sender, EventArgs e) => timer1.Stop();

        private void re_chart_Click(object sender, EventArgs e)
        {
            chart1.Series[0].Points.Clear();
            elapsedTime = 0;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 10;
        }

        private async void on_btn_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSpeed.Text, out int speedVal))
            {
                await SendPostConfig(speedVal);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập tốc độ dạng số nguyên!");
            }
        }

        private async void off_btn_Click(object sender, EventArgs e)
        {
            await SendPostConfig(0);
        }

        private async void reverse_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtSpeed.Text, out int speedM))
            {
                speedM = -speedM; 
                txtSpeed.Text = speedM.ToString();
                await SendPostConfig(speedM); 
            }
        }

        private void speed_bar_Scroll(object sender, EventArgs e)
        {
            txtSpeed.Text = speed_bar.Value.ToString();
        }

        private void UpdateUIStatus(bool connected)
        {
            on_btn.Enabled = connected;
            off_btn.Enabled = connected;
            config.Enabled = connected;
            draw_chart.Enabled = connected;
            stop_chart.Enabled = connected;
            re_chart.Enabled = connected;
            disconnect_btn.Enabled = connected;
            connect_btn.Enabled = !connected;
        }

        private async Task LoadESP32Config()
        {
            try
            {
                // Yêu cầu ESP32 trả về thông số Kp, Ki, Kd và Target Speed         
                string kpVal = await httpClient.GetStringAsync(esp32IP + "/kp");
                string kiVal = await httpClient.GetStringAsync(esp32IP + "/ki");
                string kdVal = await httpClient.GetStringAsync(esp32IP + "/kd");
                string targetVal = await httpClient.GetStringAsync(esp32IP + "/target");

                // Đổ dữ liệu đọc được lên giao diện C#
                kp.Text = kpVal;
                ki.Text = kiVal;
                kd.Text = kdVal;
                txtSpeed.Text = targetVal;

                if (int.TryParse(targetVal, out int speedValue))
                {
                    // Cập nhật thanh trượt TrackBar nếu giá trị nằm trong khoảng
                    if (speedValue >= speed_bar.Minimum && speedValue <= speed_bar.Maximum)
                    {
                        speed_bar.Value = speedValue;
                    }
                }

                lbl_pid.Text = "Đã tải cấu hình từ ESP32!";
                lbl_pid.ForeColor = Color.Green;
            }
            catch
            {
                lbl_pid.Text = "Không thể tải cấu hình PID từ ESP32";
                lbl_pid.ForeColor = Color.Orange;
            }
        }

        private async Task SendPostConfig(int targetSpeed)
        {
            try
            {
                // 1. Lấy thông số Kp, Ki, Kd hiện có từ TextBox (hoặc gán mặc định nếu rỗng)
                double.TryParse(kp.Text, out double kpVal);
                double.TryParse(ki.Text, out double kiVal);
                double.TryParse(kd.Text, out double kdVal);

                // 2. Đóng gói dữ liệu dạng JSON
                string jsonPayload = $"{{\"kp\":{kpVal}, \"ki\":{kiVal}, \"kd\":{kdVal}, \"target\":{targetSpeed}}}";
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                // 3. Gửi HTTP POST sang ESP32
                HttpResponseMessage response = await httpClient.PostAsync(esp32IP + "/api/config", content);

                if (response.IsSuccessStatusCode)
                {
                    lbl_pid.Text = "Đã cập nhật Tốc độ: " + targetSpeed;
                    lbl_pid.ForeColor = Color.Green;
                }
                else
                {
                    lbl_pid.Text = "Lỗi cài đặt tốc độ!";
                    lbl_pid.ForeColor = Color.Red;
                }
            }
            catch
            {
                status_connect.Text = "Mất kết nối ESP32";
                status_connect.ForeColor = Color.Red;
            }
        }
    }
}