using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageViewerApplication
{
    public partial class viewer : Form
    {
        int currentIndex;

        public viewer()
        {
            InitializeComponent();
        }

        /************We pass opened Image Index via Constructor****************/
        public viewer(int selectedIndex) {

            InitializeComponent();

            currentIndex = selectedIndex;

            
        }

        /*************Setter for picture box****************/
        public Image ImageBox
        {
            set
            {
                this.pictureBox1.Image = value;
                this.pictureBox1.Size = value.Size;
            }

        }

        /**********if this form closes***********/
        private void Viewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if form is closed Dispose pictureBox1
            if (pictureBox1.Image != null)
                pictureBox1.Dispose();  //pictureBox1 is object on panel.

        }


        /***********if Mext is clicked*********/
        private void btnNext_Click(object sender, EventArgs e)
        {
            currentIndex += 1;

            if (currentIndex  < frm.filenames.Count)
            {
               

                using (Image i = Image.FromFile(frm.filenames[currentIndex]))
                {
                    
                    pictureBox1.ImageLocation = frm.filenames[currentIndex];
                    pictureBox1.Size = i.Size;
                    //i.Dispose();

                }

            }
            else
            {
                currentIndex = frm.filenames.Count-1;
            }
              
             
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {

            currentIndex -= 1;

            if (currentIndex >= 0)
            {


                using (Image i = Image.FromFile(frm.filenames[currentIndex]))
                {
                    pictureBox1.ImageLocation = frm.filenames[currentIndex];
                    pictureBox1.Size = i.Size;
                   // i.Dispose();
                }

            }
            else
            {
                currentIndex = 0;
            }


        }
        /***************************/
        private void ZoomOutFunc()
        {
            SizeF f = new SizeF(0.8f, 0.8f);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.Scale(f);
            NumOfZoomOutClick++;


            //preserve original location
            pictureBox1.Location = new Point(25, 23);

           

        }


        int NumOfZoomOutClick = 0;
        /*************if zoom out is clicked**********/
        private void btnZoomOut_Click(object sender, EventArgs e)
        {

            if (NumOfZoomOutClick < 5)
            {
                ZoomOutFunc();
            }
            else
            {
                NumOfZoomOutClick = 5; //limit zoom out scaling
            }


            Console.WriteLine("zoom out :" +NumOfZoomOutClick);

            if (trackBar1.Value > -5)
                trackBar1.Value--;

        }



        /*********************/
        private void ZoomInFunc()
        {
            SizeF f = new SizeF(1.2f, 1.2f);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            pictureBox1.Scale(f);
            NumOfZoomInClick++;


            //preserve original location
            pictureBox1.Location = new Point(25, 23);

           
        }


        
        int NumOfZoomInClick = 0;
        /*****************if Zoom in is clicked****/
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            if (NumOfZoomInClick < 5)
            {
                ZoomInFunc();

            }
            else
            {
                NumOfZoomInClick = 5;
            }

            Console.WriteLine("zoom in : "+NumOfZoomInClick);
            if (trackBar1.Value < 5)
                trackBar1.Value++;
        }



        /****************if Original is clicked***********/
        private void button1_Click(object sender, EventArgs e)
        {
            //reload image
            using (Image i = Image.FromFile(frm.filenames[currentIndex]))
            {
                pictureBox1.ImageLocation = frm.filenames[currentIndex];
                pictureBox1.Size = i.Size;
                //i.Dispose();
            }

            //clear numbers
            NumOfZoomInClick = 0;
            NumOfZoomOutClick = 0;

            //set pictureBox1 Location
            pictureBox1.Location = new Point(25, 23);

            //updata trackbar
            trackBar1.Value = 0;

            

        }

        /*************if track is scrolled****************/
        int val;
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            val = trackBar1.Value;

            if (val == 0)
            {
                button1_Click(this, null);
                trackBar1.Value = val; 
            }
            if (val > 0)
            {
                button1_Click(this, null);
                for(int i =0;  i< val ; i++)
                {
                    ZoomInFunc();
                }
                trackBar1.Value = val;
                trackBar1.Refresh();
                trackBar1.Update();
            }
            if (val < 0)
            {
                button1_Click(this, null);
                for (int i = 0; i > val; i--)
                {
                    ZoomOutFunc();
                }
                trackBar1.Value = val;
                trackBar1.Refresh();
                trackBar1.Update();

            }


            
        }

        

        /************** if Rotate 90 is clicked ************/
        private void btnRotatePos_Click(object sender, EventArgs e)
        {
            if (comboBoxRotPos.SelectedItem != null)
            {
                RotateFlipType t =RotateFlipType.RotateNoneFlipNone;

                if (comboBoxRotPos.SelectedIndex == 0)
                    t = RotateFlipType.Rotate90FlipNone;
                if (comboBoxRotPos.SelectedIndex == 1)
                    t = RotateFlipType.Rotate90FlipX;
                if (comboBoxRotPos.SelectedIndex == 2)
                    t = RotateFlipType.Rotate90FlipY;
                if (comboBoxRotPos.SelectedIndex == 3)
                    t = RotateFlipType.Rotate90FlipXY;


                //img is here is like copy variable.
                //it is alias and copies pictureBox1.Image object to a new address!!
                //after change is done we must set it back to pictureBox1.Image
                Image img = pictureBox1.Image;

                int maxDim = Math.Max(img.Width, img.Height);
                pictureBox1.Width = maxDim;
                pictureBox1.Height = maxDim;

                img.RotateFlip(t);
                pictureBox1.Image = img;

            }
            }


        /************** if Rotate -90 is clicked ************/
        private void btnRotateNeg_Click(object sender, EventArgs e)
        {
            RotateFlipType t = RotateFlipType.RotateNoneFlipNone;

            if (comboBoxRotNEg.SelectedIndex == 0)
                t = RotateFlipType.Rotate270FlipNone;
            if (comboBoxRotNEg.SelectedIndex == 1)
                t = RotateFlipType.Rotate270FlipX;
            if (comboBoxRotNEg.SelectedIndex == 2)
                t = RotateFlipType.Rotate270FlipY;
            if (comboBoxRotNEg.SelectedIndex == 3)
                t = RotateFlipType.Rotate270FlipXY;


            //copy existing Image
            Image img = pictureBox1.Image;

            int maxDim = Math.Max(img.Width, img.Height);
            pictureBox1.Width = maxDim;
            pictureBox1.Height = maxDim;

            img.RotateFlip(t);
          
            pictureBox1.Image = img;


        }

        private Timer timer;
        /***********if SlideShow is clicked**************/
        private void button2_Click(object sender, EventArgs e)
        {
            timer = new Timer();

            //if val is 0 set it to 1 s.
            if (numericUpDownInterval.Value == 0)
                numericUpDownInterval.Value = 1;
            
            timer.Interval = 1000 * ((int)numericUpDownInterval.Value);

            //start timer
            timer.Start();
            //triggered after 1 s
            timer.Tick += Change_Image;



        }

        int slideIndex = 0;
        //timer's handler
        private void Change_Image(object sender, EventArgs e)
        {


            if (slideIndex < frm.filenames.Count)
            {

                pictureBox1.Size = panel1.Size;
                pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
                pictureBox1.ImageLocation = frm.filenames[slideIndex];
                slideIndex++;
            }
            else
            {
                slideIndex = 0;
            }
        }


        /**********stop slide show**********/
        private void button3_Click(object sender, EventArgs e)
        {
            //stop timer
            timer.Stop();

            //reload image bfore starting slide show
            using (Image i = Image.FromFile(frm.filenames[currentIndex]))
            {
                pictureBox1.ImageLocation = frm.filenames[currentIndex];
                pictureBox1.Size = i.Size;
                // i.Dispose();
            }

        }


    }
}
