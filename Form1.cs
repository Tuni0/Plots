using ScottPlot;
using System;
using System.Globalization;

namespace Graphs
{
    public partial class Form1 : Form
    {
        string file;
        double xmax=0.0;
        double ymax=0.0;
        double xmin = 0.0;
        double ymin = 0.0;
        double xfwhm, yfwhm,xfwhm2,xfwhm3,xfwhm4;
        double z,m,c,v;
        public double x,y;
        int b=0;
        int k = 0;
        List<double> xlist = new List<double>();
        List<double> ylist = new List<double>();
        public Form1()
        {
            InitializeComponent();
           
        }

        public void button1_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                label1.Text = dialog.SafeFileName.ToString();
                file = File.ReadAllText(dialog.FileName);
             
                CultureInfo usCulture = new CultureInfo("en-US");
                NumberFormatInfo decimalDotFormat = usCulture.NumberFormat;


                foreach (string line in file.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    k += 1;
                    var values = line.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    double x = double.Parse(values[0],decimalDotFormat);
                    double y = double.Parse(values[1],decimalDotFormat);


                   
                    xlist.Add(x);
                    ylist.Add(y);


                    formsPlot1.Plot.AddPoint(x, y, Color.Black, 6);



                   

                    if (y > ymax)
                    {
                        ymax = y;
                        xmax = x;
                        b = k;
                        yfwhm = ymax/2;
                    }
                    if (ymin > y)
                    {
                        ymin = y;
                        xmin = x;
                    }
                    
                 }
                for (int i = 0; i <= b; i++)
                {

                    if (ylist[i] <= yfwhm)
                    {
                        xfwhm = xlist[i];
                        z = ylist[i];
                    }

                }
                for (int o = b-1; o >= 0; o--)
                {

                    if (ylist[o] >= yfwhm)
                    {
                        xfwhm2 = xlist[o];
                        m = ylist[o];

                    }
                }
                for (int p = b-1; p <=xlist.Count-1; p++)
                {

                    if (ylist[p] >= yfwhm)
                    {
                        xfwhm3 = xlist[p];
                        c= ylist[p];    
                    }
                }
                for (int u = xlist.Count-1; u >=b ; u--)
                {

                    if (ylist[u] <= yfwhm)
                    {
                        xfwhm4 = xlist[u];
                        v = ylist[u]; 
                    }
                }
                

               formsPlot1.Plot.AddLine(xfwhm, z, xfwhm2, m,Color.Red,5);
               formsPlot1.Plot.AddLine(xfwhm4, v, xfwhm3, c, Color.Green, 5);
               formsPlot1.Plot.AddLine(xlist.First(), yfwhm, xlist.Last(), yfwhm);

                double angle = (m-z) / (xfwhm2-xfwhm);
                double angle2 = (v-c) / (xfwhm4-xfwhm3);

                
                double XXfwhm = -1*((m-yfwhm)/angle)+xfwhm2;
                double XXfwhm2 = -1 * ((c - yfwhm) / angle2) + xfwhm3;

                formsPlot1.Plot.AddText("FWHM= " + (XXfwhm2 - XXfwhm).ToString(), xlist[b+10], yfwhm+20);

                formsPlot1.Plot.AddPoint(XXfwhm,yfwhm,Color.Blue,8);
                formsPlot1.Plot.AddPoint(XXfwhm2, yfwhm, Color.Blue, 8);


                var rect = formsPlot1.Plot.AddMarker(xmax, ymax, ScottPlot.MarkerShape.openCircle, 10, Color.Green);
                formsPlot1.Refresh();

                rect.Text = ("           Pmax: (" + xmax.ToString() + " , " + ymax.ToString() + " )");
                rect.TextFont.Size = 10;
                rect.TextFont.Color = Color.Red;
                rect.TextFont.Alignment = Alignment.MiddleLeft;
                
                
                
            }
        }
    }
}