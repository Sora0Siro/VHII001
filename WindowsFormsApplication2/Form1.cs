using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        int states;
        int s1, s2, s3;
        
       
        public Form1()
        {
            InitializeComponent();
            states = 0;
            s1 = 5;
            s2 = s1 * 2;
            s3 = s2 * 2;
        }

        public void moving()
        {
            for (int w=0,h=0; w<SystemInformation.VirtualScreen.Width-500; w+=200,h+=100)
            {
                this.Location = new Point(w, h);
                Thread.Sleep(2000);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            line(0, 0, 500, 500);
        }

        public void line(int x, int y, int x2, int y2)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1;
                else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                this.pictureBox1.Location = new Point(x, y);
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TransparencyKey = BackColor;
            //timer1.Start();
            //timer1.Interval = 1;
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            line(pictureBox1.Location.X, pictureBox1.Location.Y, e.X, e.Y);
        }

        Mutex checking = new Mutex(false);
        AutoResetEvent are = new AutoResetEvent(false);
        //You could create just one handler, but this is to show what you need to link to
        private void Form1_MouseLeave(object sender, EventArgs e)
        {
            StartWaitingForClickFromOutside();
        }

        private void Form1_Leave(object sender, EventArgs e)
        {
            StartWaitingForClickFromOutside();
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
            StartWaitingForClickFromOutside();
        }

        private void StartWaitingForClickFromOutside()
        {
            if (checking.WaitOne(10))
            {
                var ctx = new SynchronizationContext();
                are.Reset();

                Task.Factory.StartNew(() =>
                {
                    while (true)
                    {
                        if (are.WaitOne(1))
                        {
                            break;
                        }
                        if (MouseButtons == MouseButtons.Left)
                        {
                            ctx.Send(CLickFromOutside, null);
                            //you might need to put in a delay here and not break depending on what you want to accomplish
                            
                            break;
                        }
                    }

                    checking.ReleaseMutex();
                });
            }
        }

        private void CLickFromOutside(object state)
        {
            MessageBox.Show("Clicked from outside of the window");
        }

        private void Form1_MouseEnter(object sender, EventArgs e)
        {
            are.Set();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            are.Set();
        }

        private void Form1_Enter(object sender, EventArgs e)
        {
            are.Set();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible == true)
            {
                are.Set();
            }
            else
            {
                StartWaitingForClickFromOutside();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Location = Cursor.Position;
            if (states == 0)
            {
                pictureBox1.BackgroundImage = WindowsFormsApplication2.Properties.Resources.VirtualFriendMaket;
            }
            else if (states == s1)
            {
                pictureBox1.BackgroundImage = WindowsFormsApplication2.Properties.Resources.VirtualFriendMaket2;
            }
            else if (states == s2)
            {
                pictureBox1.BackgroundImage = WindowsFormsApplication2.Properties.Resources.VirtualFriendMaket3;
            }
            else if (states == s3)
            {
                pictureBox1.BackgroundImage = WindowsFormsApplication2.Properties.Resources.VirtualFriendMaket4;
                states = -1;
            }
            states++;
        }

        private void fu()
        {
          /* int j = 0;
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                if (listBox1.Items[i].ToString() == GetBrowserURL())
                {
                    break;
                }
                else
                {
                    j++;
                }
            }
            if (j == listBox1.Items.Count)
            {
                listBox1.Items.Add(GetBrowserURL());
            }*/
        }

    }
}