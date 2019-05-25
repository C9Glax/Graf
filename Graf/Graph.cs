using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Graf
{
    public enum GraphType { BAR, LINE }
    public class Graph
    {
        private const int borderLeft = 50;
        private const int borderBottom = 30;
        private const int tickSize = 5;
        private const int labelHeight = 30;

        public static Canvas DrawGraph(Canvas canvas, double[] values, string[] identifiers, int steps, GraphType type, bool fullLine, SolidColorBrush axisBrush, SolidColorBrush graphBrush)
        {
            if (values.Length < 1)
                throw new GraphParameterException("Graph has to get at least one value to Draw.");
            else if (steps < 1)
                throw new GraphParameterException("There has to be at least one step.");

            canvas = DrawAxes(canvas, MaxVal(values), steps, axisBrush, fullLine);

            double[] heights = new double[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                double multiplier = (canvas.Height - borderBottom) / MaxVal(values);
                heights[i] = multiplier * values[i];
            }

            switch (type)
            {
                case GraphType.BAR:
                    canvas = DrawBarGraph(canvas, heights, identifiers, axisBrush, graphBrush, fullLine);
                    break;
                case GraphType.LINE:
                    canvas = DrawLineGraph(canvas, heights, identifiers, axisBrush, graphBrush, fullLine);
                    break;
            }

            return canvas;
        }

        public static Canvas DrawGraph(Canvas canvas, double[] values, int steps, GraphType type, bool fullLine)
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

            string[] idents = new string[values.Length];

            return DrawGraph(canvas, values, idents, steps, type, fullLine, blackBrush, blueBrush);
        }

        public static Canvas DrawGraph(Canvas canvas, double[] values)
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

            string[] idents = new string[values.Length];

            return DrawGraph(canvas, values, idents, 5, GraphType.LINE, false, blackBrush, blueBrush);
        }

        public static Canvas DrawGraph(Canvas canvas, double[] values, string[] idents)
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

            return DrawGraph(canvas, values, idents, 5, GraphType.LINE, false, blackBrush, blueBrush);
        }

        public static Canvas DrawGraph(Canvas canvas, double[] values, string[] idents, GraphType type)
        {
            SolidColorBrush blackBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            SolidColorBrush blueBrush = new SolidColorBrush(Color.FromRgb(0, 0, 255));

            return DrawGraph(canvas, values, idents, 5, type, false, blackBrush, blueBrush);
        }

        private static Canvas DrawLineGraph(Canvas canvas, double[] heights, string[] identifiers, SolidColorBrush tickBrush, SolidColorBrush lineBrush , bool fullLine)
        {
            double valueWidth = (canvas.Width - borderLeft) / (heights.Length - 1);

            for(int i = 1; i < heights.Length; i++)
            {
                canvas.Children.Add(new Label()
                {
                    Margin = new System.Windows.Thickness(borderLeft + (valueWidth * i) - (valueWidth / 2.0), canvas.Height - borderBottom, 0, 0),
                    Height = borderBottom - tickSize,
                    Width = valueWidth,
                    Foreground = tickBrush,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
                    Content = identifiers[i]
                });

                canvas.Children.Add(new Line()
                {
                    X1 = borderLeft + (valueWidth * (i - 1)),
                    X2 = borderLeft + (valueWidth * i),
                    Y1 = canvas.Height - borderBottom - heights[i - 1],
                    Y2 = canvas.Height - borderBottom - heights[i],
                    Stroke = lineBrush,
                    StrokeThickness = 2
                });
                if (fullLine)
                {
                    canvas.Children.Add(new Line()
                    {
                        Y1 = 0,
                        Y2 = (canvas.Height - borderBottom) + tickSize,
                        X1 = borderLeft + (valueWidth * i),
                        X2 = borderLeft + (valueWidth * i),
                        Stroke = tickBrush,
                        StrokeThickness = 1
                    });
                }
                else
                {
                    canvas.Children.Add(new Line()
                    {
                        Y1 = (canvas.Height - borderBottom),
                        Y2 = (canvas.Height - borderBottom) + tickSize,
                        X1 = borderLeft + (valueWidth * i),
                        X2 = borderLeft + (valueWidth * i),
                        Stroke = tickBrush,
                        StrokeThickness = 2
                    });
                }
            }

            return canvas;
        }

        private static Canvas DrawBarGraph(Canvas canvas, double[] heights, string[] identifiers, SolidColorBrush tickBrush, SolidColorBrush barBrush, bool fullLine)
        {
            double valueWidth = (canvas.Width - borderLeft) / heights.Length;
            double barWidth = valueWidth * 0.8;
            double gapWidth = valueWidth * 0.1;

            for(int i = 0; i < heights.Length; i++)
            {
                canvas.Children.Add(new Rectangle()
                {
                    Height = heights[i],
                    Width = barWidth,
                    Margin = new System.Windows.Thickness(borderLeft + (valueWidth * i) + gapWidth, (canvas.Height - borderBottom) - heights[i], (canvas.Width - borderLeft) + (valueWidth * (i + 1)) - gapWidth, borderBottom),
                    Fill = barBrush
                });

                canvas.Children.Add(new Label()
                {
                    Margin = new System.Windows.Thickness(borderLeft + (valueWidth * i), canvas.Height - borderBottom, 0 ,0),
                    Height = borderBottom - tickSize,
                    Width = valueWidth,
                    Foreground = tickBrush,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Top,
                    Content = identifiers[i]
                });

                if (fullLine)
                {
                    canvas.Children.Add(new Line()
                    {
                        Y1 = 0,
                        Y2 = (canvas.Height - borderBottom) + tickSize,
                        X1 = borderLeft + (valueWidth * (i + 1)),
                        X2 = borderLeft + (valueWidth * (i + 1)),
                        Stroke = tickBrush,
                        StrokeThickness = 1
                    });
                }
                else
                {
                    canvas.Children.Add(new Line()
                    {
                        Y1 = (canvas.Height - borderBottom),
                        Y2 = (canvas.Height - borderBottom) + tickSize,
                        X1 = borderLeft + (valueWidth * (i + 1)),
                        X2 = borderLeft + (valueWidth * (i + 1)),
                        Stroke = tickBrush,
                        StrokeThickness = 2
                    });
                }
            }

            return canvas;
        }

        private static Canvas DrawAxes(Canvas canvas, double maxVal, int steps, SolidColorBrush brush, bool fullLine)
        {
            double stepSize = (canvas.Height - borderBottom) / steps;

            //yAxis
            canvas.Children.Add(new Line()
            {
                X1 = borderLeft,
                X2 = borderLeft,
                Y1 = 0,
                Y2 = canvas.Height - (borderBottom / 2.0),
                Stroke = brush,
                StrokeThickness = 2
            });

            //xAxis
            canvas.Children.Add(new Line()
            {
                X1 = (borderLeft / 2.0),
                X2 = canvas.Width,
                Y1 = (canvas.Height - borderBottom),
                Y2 = (canvas.Height - borderBottom),
                Stroke = brush,
                StrokeThickness = 2
            });

            for(int i = 1; i <= steps; i++)
            {
                if (fullLine)
                {
                    canvas.Children.Add(new Line()
                    {
                        X1 = borderLeft - tickSize,
                        X2 = canvas.Width,
                        Y1 = (canvas.Height - borderBottom) - (stepSize * i),
                        Y2 = (canvas.Height - borderBottom) - (stepSize * i),
                        Stroke = brush,
                        StrokeThickness = 1
                    });
                }
                else
                {
                    canvas.Children.Add(new Line()
                    {
                        X1 = borderLeft - tickSize,
                        X2 = borderLeft,
                        Y1 = (canvas.Height - borderBottom) - (stepSize * i),
                        Y2 = (canvas.Height - borderBottom) - (stepSize * i),
                        Stroke = brush,
                        StrokeThickness = 2
                    });
                }
                
                canvas.Children.Add(new Label()
                {
                    Margin = new System.Windows.Thickness(0, canvas.Height - borderBottom - (stepSize * i) - (labelHeight / 2.0), canvas.Width - borderLeft + tickSize,0),
                    Width = borderLeft - tickSize,
                    Content = maxVal / i,
                    Foreground = brush,
                    HorizontalContentAlignment = System.Windows.HorizontalAlignment.Right,
                    VerticalContentAlignment = System.Windows.VerticalAlignment.Center
                });
            }

            return canvas;
        }

        private static double MaxVal(double[] values)
        {
            double max = values[0];
            foreach (double val in values)
                if (val > max)
                    max = val;
            return max;
        }
    }
}
