using System;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace Emgu_TemplateMatching_Working_
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Image to sreach, Source (Big Image), Similarity
            pictureBox1.Image = FindImage(pictureBox2.Image as Bitmap, pictureBox1.Image as Bitmap, 0.85f).ToBitmap();
        }

        public static Image<Bgr, byte> FindImage(Bitmap imgTemplate, Bitmap imgSource, float Similarity = 0.75f)
        {
            Image<Bgr, byte> source = new Image<Bgr, byte>(imgSource);
            Image<Bgr, byte> template = new Image<Bgr, byte>(imgTemplate);
            Image<Bgr, byte> imageToShow = source.Copy();

            using (Image<Gray, float> result = source.MatchTemplate(template, TemplateMatchingType.CcoeffNormed))
            {
                double[] minValues, maxValues;
                Point[] minLocations, maxLocations;
                result.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

                if (maxValues[0] >= Similarity)
                {
                    Rectangle match = new Rectangle(maxLocations[0], template.Size);
                    imageToShow.Draw(match, new Bgr(Color.Green), 5);
                }
                else
                {
                    MessageBox.Show("Match Can't be found. MaxValue=[" + maxValues[0] + "]");
                    return null; // I don't know if it will crash the program or not
                }
            }

            return imageToShow;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Properties.Resources.SourceImage;
            pictureBox2.Image = Properties.Resources.Template;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "חפש תמונה";
            of.Filter = "Image Files (*.bmp; *.jpg; *.jpeg; *.png|*.BMP;*.JPG;*.JPEG;*.PNG|All Files (*.*)|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox2.ImageLocation = of.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog();
            of.Title = "חפש תמונה";
            of.Filter = "Image Files (*.bmp; *.jpg; *.jpeg; *.png|*.BMP;*.JPG;*.JPEG;*.PNG|All Files (*.*)|*.*";

            if (of.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = of.FileName;
            }
        }
    }
}
