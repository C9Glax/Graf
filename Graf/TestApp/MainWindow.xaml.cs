using Graf;
using System.Windows;
using System;

namespace TestApp
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            double[] values = new double[10];

            Random rnd = new Random();
            for (int i = 0; i < values.Length; i++)
                values[i] = rnd.NextDouble() * 5;

            string[] idents = new string[values.Length];
            for (int i = 0; i < values.Length; i++)
                idents[i] = values[i].ToString();

            this.canvas = Graph.DrawGraph(this.canvas, values, idents);
        }
    }
}
