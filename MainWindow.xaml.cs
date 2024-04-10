using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace InterfaceEstadistUI8f;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private DispatcherTimer timer;
    private TimeSpan elapsedTime;

    public MainWindow(string nombre, string boton)
    {
        InitializeComponent();

        dasborder.Visibility = Visibility.Visible;
        sturges.Visibility = Visibility.Hidden;
        mediaVIsta.Visibility = Visibility.Hidden;

        nombreuser.Text = nombre;
        nombreuser2.Text = "HOLA " + nombre + "!";

        if (boton == "M")
        {
            genero.Text = "M";
            imagen.Source =
                new BitmapImage(new Uri("C:\\Users\\Alienware\\RiderProjects\\InterfaceEstadistUI8f\\img\\user.png"));
        }
        else if (boton == "F")
        {
            genero.Text = "F";
            imagen.Source =
                new BitmapImage(new Uri("C:\\Users\\Alienware\\RiderProjects\\InterfaceEstadistUI8f\\img\\user2.png"));
        }

        DateTime fechaActual = DateTime.Now;
        string fechaFormateada = fechaActual.ToString("dd/MM/yyyy");
        fecha.Text = fechaFormateada;

        DateTime horaActual = DateTime.Now;
        string horaFormateada = horaActual.ToString("hh:mm");
        HoraActual.Text = horaFormateada + " AM";

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += Timer_Tick;

        Loaded += (sender, e) => { StartTimer(); };
    }


    private void Timer_Tick(object sender, EventArgs e)
    {
        elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));

        UpdateElapsedTime();
    }

    private void StartTimer()
    {
        elapsedTime = TimeSpan.Zero;

        timer.Start();
    }

    private void UpdateElapsedTime()
    {
        contador.Text = $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        timer.Stop();
    }

    private TextBox[,] textBoxMatrix;

    private void CreateRandomGridWithRandomData()
    {
        Random random = new Random();

        int numRows = random.Next(5, 16);
        int numCols = random.Next(5, 16);

        Grid dynamicGrid = new Grid();
        textBoxMatrix = new TextBox[numRows, numCols];

        for (int i = 0; i < numRows; i++)
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        for (int i = 0; i < numCols; i++)
        {
            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        int minValue = int.MaxValue;
        int maxValue = int.MinValue;

        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int randomNumber = random.Next(1, 101);
                TextBox textbox = new TextBox()
                {
                    Text = randomNumber.ToString(),
                    Foreground = Brushes.White,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsEnabled = false,
                    Name = $"textBox_{row}_{col}"
                };

                textBoxMatrix[row, col] = textbox;

                int currentValue = randomNumber;

                if (currentValue < minValue)
                {
                    minValue = currentValue;
                }

                if (currentValue > maxValue)
                {
                    maxValue = currentValue;
                }

                Grid.SetRow(textbox, row);
                Grid.SetColumn(textbox, col);
                dynamicGrid.Children.Add(textbox);
            }
        }

        Border border1 = (Border)FindName("border1");

        if (border1 != null)
        {
            border1.Child = null;
            border1.Child = dynamicGrid;
        }

        int CantidadTotal = numRows * numCols;

        int valorMinimo = minValue;
        int valorMaximo = maxValue;
        k.Text = CalcularK(CantidadTotal).ToString();
        int Ka = CalcularK(CantidadTotal);
        int R = (valorMaximo - valorMinimo) + 1;

        float amplitud = (float)R / Ka;
        float amplitud2 = (float)Math.Ceiling(amplitud);

        CreateTextBlocksInGrid(valorMinimo, Ka, amplitud2, textBoxMatrix, CantidadTotal);
    }

    public int CalcularK(int n)
    {
        double kDouble = Math.Log(n, 2) + 1;

        double decimalPart = kDouble - Math.Floor(kDouble);

        int k;
        if (decimalPart < 0.5)
        {
            k = (int)Math.Floor(kDouble);
        }
        else
        {
            k = (int)Math.Ceiling(kDouble);
        }

        return k;
    }


    private void CreateTextBlocksInGrid(int valorMinimo, int k, float amplitud, TextBox[,] textBoxMatrix, int cantidadT)
    {
        Grid grid = new Grid();

        for (int col = 0; col < 8; col++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        for (int i = 0; i < k + 2; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        int totalElementos = textBoxMatrix.GetLength(0) * textBoxMatrix.GetLength(1);

        int fiAcumulado = 0;
        double sumatoriaFiXi = 0.0;
        double productoriafiXi = 1.0;
        double sumatoriaFiDivXi = 0.0;
        double identificarIntervalo = cantidadT / 2;

        for (int fila = 1; fila <= k; fila++)
        {
            double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);
            double intervaloMaximo = valorMinimo + (fila * amplitud);

            double xi = (intervaloMinimo + intervaloMaximo - 1) / 2.0;

            TextBlock intervaloTextBlock = new TextBlock()
            {
                Text = $"[{intervaloMinimo}, {intervaloMaximo})",
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(intervaloTextBlock, 0);
            Grid.SetRow(intervaloTextBlock, fila);
            grid.Children.Add(intervaloTextBlock);

            int count = CountNumbersInRange(textBoxMatrix, intervaloMinimo, intervaloMaximo);

            TextBlock countTextBlock = new TextBlock()
            {
                Text = count.ToString(),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(countTextBlock, 1);
            Grid.SetRow(countTextBlock, fila);
            grid.Children.Add(countTextBlock);

            double frecuenciaRelativa = (double)count / totalElementos;

            TextBlock relativeFrequencyTextBlock = new TextBlock()
            {
                Text = frecuenciaRelativa.ToString("0.000"),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(relativeFrequencyTextBlock, 2);
            Grid.SetRow(relativeFrequencyTextBlock, fila);
            grid.Children.Add(relativeFrequencyTextBlock);

            double porcentaje = frecuenciaRelativa * 100;

            TextBlock percentageTextBlock = new TextBlock()
            {
                Text = porcentaje.ToString("0.00") + "%",
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(percentageTextBlock, 3);
            Grid.SetRow(percentageTextBlock, fila);
            grid.Children.Add(percentageTextBlock);

            fiAcumulado += count;

            TextBlock cumulativeFrequencyTextBlock = new TextBlock()
            {
                Text = fiAcumulado.ToString(),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativeFrequencyTextBlock, 4);
            Grid.SetRow(cumulativeFrequencyTextBlock, fila);
            grid.Children.Add(cumulativeFrequencyTextBlock);

            double hi = (double)fiAcumulado / totalElementos;

            TextBlock cumulativeRelativeFrequencyTextBlock = new TextBlock()
            {
                Text = hi.ToString("0.000"),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativeRelativeFrequencyTextBlock, 5);
            Grid.SetRow(cumulativeRelativeFrequencyTextBlock, fila);
            grid.Children.Add(cumulativeRelativeFrequencyTextBlock);

            double pi = hi * 100;

            TextBlock cumulativePercentageTextBlock = new TextBlock()
            {
                Text = pi.ToString("0.00") + "%",
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativePercentageTextBlock, 6);
            Grid.SetRow(cumulativePercentageTextBlock, fila);
            grid.Children.Add(cumulativePercentageTextBlock);

            TextBlock xiTextBlock = new TextBlock()
            {
                Text = xi.ToString("0.00"),
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(xiTextBlock, 7);
            Grid.SetRow(xiTextBlock, fila);
            grid.Children.Add(xiTextBlock);

            sumatoriaFiXi += count * xi;
            sumatoriaFiDivXi += (double)count / xi;
            productoriafiXi *= Math.Pow(xi, count);
        }

        double mediaAritmetica = sumatoriaFiXi / cantidadT;
        double mediaGeometrica = Math.Pow(productoriafiXi, 1.0 / cantidadT);
        double mediaArmonica = cantidadT / sumatoriaFiDivXi;

        mediaAritmeticaRes.Text = mediaAritmetica.ToString("0.00");
        mediaArmonicaRes.Text = mediaArmonica.ToString("0.00");
        mediaGeometricaRes.Text = mediaGeometrica.ToString("0.00");

        string[] textBlockNames = { "Intervalos", "fi", "hi", "pi", "FI", "HI", "PI", "xi" };

        for (int col = 0; col < textBlockNames.Length; col++)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = textBlockNames[col],
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(textBlock, col);
            Grid.SetRow(textBlock, 0);
            grid.Children.Add(textBlock);
        }

        Border border = new Border()
        {
            Name = "border2",
            Margin = new Thickness(10, 10, 10, 0),
            Height = double.NaN,
            Width = double.NaN,
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(10),
            Background = Brushes.Black
        };

        border.Child = grid;

        StackPanel panel = (StackPanel)FindName("Panel2");

        if (panel != null)
        {
            panel.Children.Clear();
            panel.Children.Add(border);
        }
        else
        {
            MessageBox.Show("No se encontró el StackPanel 'Panel2'.");
        }

        int numeroMasCercano = -1;
        int filaNumeroMasCercano = -1;
        int valorColumna1NumeroMasCercano = -1;

        for (int fila = 1; fila <= k; fila++)
        {
            int fiValue = int.Parse(grid.Children.Cast<TextBlock>()
                .First(tb => Grid.GetRow(tb) == fila && Grid.GetColumn(tb) == 4)
                .Text);

            int fiValueColumna1 = int.Parse(grid.Children.Cast<TextBlock>()
                .First(tb => Grid.GetRow(tb) == fila && Grid.GetColumn(tb) == 1)
                .Text);

            if (fiValue >= identificarIntervalo)
            {
                numeroMasCercano = fiValue;
                filaNumeroMasCercano = fila;
                valorColumna1NumeroMasCercano = fiValueColumna1;
                break;
            }
        }

        int fiAnterior = -1;
        if (filaNumeroMasCercano > 1)
        {
            int filaAnterior = filaNumeroMasCercano - 1;
            TextBlock fiAnteriorTextBlock = grid.Children.Cast<TextBlock>()
                .FirstOrDefault(tb => Grid.GetRow(tb) == filaAnterior && Grid.GetColumn(tb) == 4);

            if (fiAnteriorTextBlock != null)
            {
                fiAnterior = int.Parse(fiAnteriorTextBlock.Text);
            }
        }

        double limiteInferior = GetLimiteInferiorIntervalo(filaNumeroMasCercano, valorMinimo, amplitud);

        if (fiAnterior != -1)
        {
            // Cálculo de la mediana
            double mediana = limiteInferior +
                             (cantidadT / 2.0 - fiAnterior) * (amplitud / valorColumna1NumeroMasCercano);
            medianaRes.Text = mediana.ToString("0.00");

            int filaMediana = -1;

            for (int fila = 1; fila <= k; fila++)
            {
                double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);
                double intervaloMaximo = valorMinimo + (fila * amplitud);

                if (mediana >= intervaloMinimo && mediana < intervaloMaximo)
                {
                    filaMediana = fila;
                    break;
                }
            }

            if (filaMediana != -1)
            {
                double limiteInferiorMediana = valorMinimo + ((filaMediana - 1) * amplitud);
                double limiteSuperiorMediana = valorMinimo + (filaMediana * amplitud);

                int valorColumna1Mediana = int.Parse(grid.Children.Cast<TextBlock>()
                    .First(tb => Grid.GetRow(tb) == filaMediana && Grid.GetColumn(tb) == 1)
                    .Text);

                int valorColumna1Anterior = -1;
                if (filaMediana > 1)
                {
                    valorColumna1Anterior = int.Parse(grid.Children.Cast<TextBlock>()
                        .First(tb => Grid.GetRow(tb) == filaMediana - 1 && Grid.GetColumn(tb) == 1)
                        .Text);
                }

                int valorColumna1Posterior = -1;
                if (filaMediana < k)
                {
                    valorColumna1Posterior = int.Parse(grid.Children.Cast<TextBlock>()
                        .First(tb => Grid.GetRow(tb) == filaMediana + 1 && Grid.GetColumn(tb) == 1)
                        .Text);
                }

                double d1 = valorColumna1Mediana - valorColumna1Anterior;
                double d2 = valorColumna1Posterior - valorColumna1Mediana;

                double moda = limiteInferiorMediana + ((amplitud * d1) / (d1 + d2));
                modaRes.Text = moda.ToString("0.00");
            }
            else
            {
                MessageBox.Show("No se encontró el intervalo que contiene la mediana.");
            }
        }
        else
        {
            MessageBox.Show("No se encontró un valor de FI en la posición anterior.");
        }
    }


    private double GetLimiteInferiorIntervalo(int fila, int valorMinimo, float amplitud)
    {
        double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);

        return intervaloMinimo;
    }


    private int CountNumbersInRange(TextBox[,] matrix, double min, double max)
    {
        int count = 0;
        foreach (TextBox textBox in matrix)
        {
            double value;
            if (double.TryParse(textBox.Text, out value))
            {
                if (value >= min && value < max)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void OperacionesButton_Click(object sender, RoutedEventArgs e)
    {
        ToggleVisibility();
    }

    private void ToggleVisibility()
    {
        Visibility buttonVisibility = MediaAritmeticaButton.Visibility;

        if (buttonVisibility == Visibility.Collapsed)
        {
            DropUpIcon.Visibility = Visibility.Visible;
            DropDownIcon.Visibility = Visibility.Collapsed;
            MediaAritmeticaButton.Visibility = Visibility.Visible;
            SturgesButton.Visibility = Visibility.Visible;
        }
        else
        {
            DropDownIcon.Visibility = Visibility.Visible;
            DropUpIcon.Visibility = Visibility.Collapsed;
            MediaAritmeticaButton.Visibility = Visibility.Collapsed;
            SturgesButton.Visibility = Visibility.Collapsed;
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
    {
        CreateRandomGridWithRandomData();
    }

    private void GenerarTextbox_OnClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(generartextbox.Text, out int cantidad) && cantidad > 0)
        {
            Wrap1.Children.Clear();

            List<TextBox> textBoxes = new List<TextBox>();

            for (int i = 0; i < cantidad; i++)
            {
                TextBox newTextBox = new TextBox();
                newTextBox.Margin = new Thickness(10, 15, 10, 0);
                newTextBox.Foreground = Brushes.White;
                newTextBox.BorderBrush = Brushes.White;
                newTextBox.Width = 100;
                Wrap1.Children.Add(newTextBox);
                textBoxes.Add(newTextBox);
            }

            Button calcularMediaButton = new Button();
            calcularMediaButton.Content = "Calcular Media aritmética";
            calcularMediaButton.Click += (s, args) =>
            {
                double suma = 0;
                int count = 0;

                foreach (var textBox in textBoxes)
                {
                    if (double.TryParse(textBox.Text, out double valor))
                    {
                        suma += valor;
                        count++;
                    }
                    else
                    {
                        MessageBox.Show($"Valor no válido en el TextBox: {textBox.Text}", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                }

                if (count > 0)
                {
                    double media = suma / count;

                    TextBlock resultadoMedia = new TextBlock();
                    resultadoMedia.Text = media.ToString();
                    resultadoMedia.Foreground = Brushes.White;
                    resultadoMedia.Margin = new Thickness(10, 15, 10, 0);
                    Wrap1.Children.Add(resultadoMedia);
                }
                else
                {
                    MessageBox.Show("Por favor, introduzca valores numéricos válidos en los TextBox.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };

            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 0);
            gradientBrush.EndPoint = new Point(1, 1);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(58, 97, 134), 1));
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(137, 37, 62), 0));
            calcularMediaButton.Background = gradientBrush;
            calcularMediaButton.Margin = new Thickness(0, 15, 0, 0);
            calcularMediaButton.BorderBrush = Brushes.Transparent;

            Wrap1.Children.Add(calcularMediaButton);
        }
        else
        {
            MessageBox.Show("Por favor, introduzca un número válido mayor que cero.");
        }
    }

    private void dashborde_OnClick(object sender, RoutedEventArgs e)
    {
        dasborder.Visibility = Visibility.Visible;
        sturges.Visibility = Visibility.Hidden;
        mediaVIsta.Visibility = Visibility.Hidden;
    }

    private void SturgesButton_OnClick(object sender, RoutedEventArgs e)
    {
        dasborder.Visibility = Visibility.Hidden;
        sturges.Visibility = Visibility.Visible;
        mediaVIsta.Visibility = Visibility.Hidden;
    }


    private void MediaAritmeticaButton_OnClick(object sender, RoutedEventArgs e)
    {
        dasborder.Visibility = Visibility.Hidden;
        sturges.Visibility = Visibility.Hidden;
        mediaVIsta.Visibility = Visibility.Visible;
    }
}