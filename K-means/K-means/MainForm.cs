using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace K_means
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Graphics graph;
        private ClassesData[] classes;
        private bool[] Flags;
        private Color[] colors = new Color[6] {Color.Green, Color.Purple, Color.Salmon, Color.SeaGreen, Color.Silver, Color.Red };
        private PointList pointlist;
        private void RandomButton_Click(object sender, EventArgs e)
        {
            PaintPanel.Refresh();
            Random r = new Random();
            graph = PaintPanel.CreateGraphics();
            pointlist = new PointList { };
            classes = new ClassesData[Convert.ToInt32(ClassesLabel.Text)];
            Flags = new bool[Convert.ToInt32(ClassesLabel.Text)];
            for (int i = 0; i < Flags.Length; i++)
            {
                Flags[i] = false;
            }
            for (int i = 0; i < Convert.ToInt32(PointsLabel.Text); i++)
            {
                Point p = new Point(r.Next(1, 200),r.Next(1, 300));
                pointlist.pointcollection.Add(new PointData(p.X, p.Y, Color.Red));
            }
            for (int i = 0; i < Convert.ToInt32(ClassesLabel.Text); i++)
            {
                classes[i] = new ClassesData(r.Next(1, 200),
                                            r.Next(1, 300), Color.Black);
            }
            
            foreach (PointData pd in pointlist.pointcollection)
            {
                int numCluster = Calculate(classes, pd.point);
                
                classes[numCluster].ObjectClass.Add(pd);
                graph.DrawRectangle(new Pen(colors[numCluster]),
                                            pd.point.X,pd.point.Y,2,2);
            }
            for (int i = 0; i < classes.Length; i++)
            {
                Rectangle rect = new Rectangle(classes[i].center.point.X,
                                classes[i].center.point.Y, 10, 10);

                graph.DrawRectangle(new Pen(classes[i].center.color),
                                            rect);
                graph.FillRectangle(Brushes.Black, rect);
            }

        }
        private void ClassesTrackBar_ValueChanged(object sender, EventArgs e)
        {
            ClassesLabel.Text = ClassesTrackBar.Value.ToString();
        }

        private void PointsTrackBar_ValueChanged(object sender, EventArgs e)
        {
            PointsLabel.Text = PointsTrackBar.Value.ToString();
        }

        private void ResultButton_Click(object sender, EventArgs e)
        {
            PaintPanel.Refresh();
            while (!flag(Flags))
            {
                for (int i = 0; i < classes.Length; i++)
                {
                    int X = 0;
                    int Y = 0;
                    foreach (PointData pd in classes[i].ObjectClass)
                    {
                        X += pd.point.X;
                        Y += pd.point.Y;
                    }
                    X /= classes[i].ObjectClass.Count;
                    Y /= classes[i].ObjectClass.Count;
                    if (classes[i].center.point.X == X && classes[i].center.point.Y == Y)
                    {
                        Flags[i] = true;
                    }
                    else
                        classes[i] = new ClassesData(X, Y, Color.Black);
                }
                foreach (PointData pd in pointlist.pointcollection)
                {
                    int numCluster = Calculate(classes, pd.point);
                    pd.color = colors[numCluster];
                    classes[numCluster].ObjectClass.Add(pd);
                }
            }
            foreach (PointData pd in pointlist.pointcollection)
            {
                graph.DrawRectangle(new Pen(pd.color),
                                            pd.point.X,pd.point.Y, 1,1);
            }
            for (int i = 0; i < classes.Length; i++)
            {
                Rectangle rect = new Rectangle(classes[i].center.point.X,
                                classes[i].center.point.Y, 10, 10);

                graph.DrawRectangle(new Pen(classes[i].center.color),
                                            rect);
                graph.FillRectangle(Brushes.Black, rect);
            }
        }
        private int Calculate(ClassesData[] classes, Point point )
        {
            double tempVar = 0;
            double minDistance = 0;
            int numCluster = 0;
            for (int j = 0; j < classes.Length; j++)
            {
                if (j == 0)
                {
                    tempVar = Math.Sqrt(Math.Pow(classes[j].center.point.X - point.X, 2) + Math.Pow(classes[j].center.point.Y - point.Y, 2));
                    minDistance = tempVar;
                }
                else
                {
                    tempVar = Math.Sqrt(Math.Pow(classes[j].center.point.X - point.X, 2) + Math.Pow(classes[j].center.point.Y - point.Y, 2));
                    if (minDistance > tempVar)
                    {
                        minDistance = tempVar;
                        numCluster = j;
                    }
                }
            }
            return numCluster;
        }
        private bool flag(bool [] flags)
        {
            for (int i = 0; i < Flags.Length; i++)
            {
                if (flags[i] == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
