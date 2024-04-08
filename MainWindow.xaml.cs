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


namespace InterfaceEstadistUI8f;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }


    private TextBox[,] textBoxMatrix;

    private void CreateRandomGridWithRandomData()
    {
        Random random = new Random();

        // Generar aleatoriamente el número de filas y columnas en el rango del 1 al 15
        int numRows = random.Next(1, 16);
        int numCols = random.Next(1, 16);

        Grid dynamicGrid = new Grid();
        textBoxMatrix = new TextBox[numRows, numCols];

        // Agregar filas y columnas al Grid según el número aleatorio de filas y columnas
        for (int i = 0; i < numRows; i++)
        {
            dynamicGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        for (int i = 0; i < numCols; i++)
        {
            dynamicGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        int minValue = int.MaxValue; // Valor máximo posible para encontrar el mínimo
        int maxValue = int.MinValue; // Valor mínimo posible para encontrar el máximo

        // Llenar la matriz con TextBox y datos aleatorios
        for (int row = 0; row < numRows; row++)
        {
            for (int col = 0; col < numCols; col++)
            {
                int randomNumber = random.Next(1, 101); // Genera un número aleatorio entre 1 y 100
                TextBox textbox = new TextBox()
                {
                    Text = randomNumber.ToString(), // Asignar el número aleatorio como texto del TextBox
                    Foreground = Brushes.White,
                    FontSize = 18,
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsEnabled = false,
                    Name = $"textBox_{row}_{col}" // Asignar nombre único al TextBox
                };

                // Guardar el TextBox en la matriz
                textBoxMatrix[row, col] = textbox;

                // Obtener el valor numérico actual
                int currentValue = randomNumber;

                // Actualizar el valor mínimo y máximo
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

        // Obtener el Border existente por su nombre
        Border border1 = (Border)FindName("border1");

        if (border1 != null)
        {
            // Limpiar cualquier contenido previo del Border
            border1.Child = null;

            // Configurar el Border existente con el nuevo Grid
            border1.Child = dynamicGrid;
        }
        else
        {
            // El Border "border1" no se encontró
            // MessageBox.Show("No se encontró el Border 'border1'.");
        }

        // Mostrar la cantidad de datos en la matriz por medio de un MessageBox
        // MessageBox.Show($"La matriz contiene {numRows} filas y {numCols} columnas.");

        int CantidadTotal = numRows * numCols;

        // Mostrar el valor mínimo y máximo encontrados en la matriz
        // MessageBox.Show($"Valor mínimo: {minValue}\nValor máximo: {maxValue}");

        int valorMinimo = minValue;
        int valorMaximo = maxValue;
        k.Text = CalcularK(CantidadTotal).ToString();
        int Ka = CalcularK(CantidadTotal);
        int R = (valorMaximo - valorMinimo) + 1;
        // MessageBox.Show(R.ToString());

        float amplitud = (float)R / Ka;

        float amplitud2 = (float)Math.Ceiling(amplitud);

        /*
        MessageBox.Show("amplitud", amplitud.ToString());
        MessageBox.Show("amplitud2", amplitud2.ToString());

        */
        CreateTextBlocksInGrid(valorMinimo, Ka, amplitud2, textBoxMatrix);
    }

    public int CalcularK(int n)
    {
        // Calcular k usando la fórmula de Sturges
        double kDouble = Math.Log(n, 2) + 1;

        // Obtener parte entera y decimal de kDouble
        double decimalPart = kDouble - Math.Floor(kDouble);

        // Redondear kDouble condicionalmente
        int k;
        if (decimalPart < 0.5)
        {
            // Decimales menores a 0.5: redondear hacia abajo
            k = (int)Math.Floor(kDouble);
        }
        else
        {
            // Decimales 0.5 o mayores: redondear hacia arriba
            k = (int)Math.Ceiling(kDouble);
        }

        return k;
    }

    private void CreateTextBlocksInGrid(int valorMinimo, int k, float amplitud, TextBox[,] textBoxMatrix)
    {
        // Crear un nuevo Grid
        Grid grid = new Grid();

        // Definir las columnas en el Grid
        for (int col = 0; col < 7; col++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Definir las filas en el Grid
        for (int i = 0; i < k + 1; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        // Calcular el número total de elementos en la matriz
        int totalElementos = textBoxMatrix.GetLength(0) * textBoxMatrix.GetLength(1);

        int fiAcumulado = 0;

        // Calcular y agregar los intervalos en filas del Grid
        for (int fila = 1; fila <= k; fila++)
        {
            double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);
            double intervaloMaximo = valorMinimo + (fila * amplitud);

            // Crear TextBlock para el intervalo y agregar al Grid
            TextBlock intervaloTextBlock = new TextBlock()
            {
                Text = $"[{intervaloMinimo}, {intervaloMaximo})", // Mostrar el intervalo
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(intervaloTextBlock, 0); // Columna "Intervalos"
            Grid.SetRow(intervaloTextBlock, fila);
            grid.Children.Add(intervaloTextBlock);

            // Contar números en el intervalo [intervaloMinimo, intervaloMaximo)
            int count = CountNumbersInRange(textBoxMatrix, intervaloMinimo, intervaloMaximo);

            // Crear TextBlock para mostrar el conteo (debajo de la columna "fi")
            TextBlock countTextBlock = new TextBlock()
            {
                Text = count.ToString(), // Mostrar el conteo como texto
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(countTextBlock, 1); // Columna "fi"
            Grid.SetRow(countTextBlock, fila);
            grid.Children.Add(countTextBlock);

            // Calcular la frecuencia relativa (hi)
            double frecuenciaRelativa = (double)count / totalElementos;

            // Crear TextBlock para mostrar la frecuencia relativa (debajo de la columna "hi")
            TextBlock relativeFrequencyTextBlock = new TextBlock()
            {
                Text = frecuenciaRelativa.ToString("0.000"), // Mostrar la frecuencia relativa formateada
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(relativeFrequencyTextBlock, 2); // Columna "hi"
            Grid.SetRow(relativeFrequencyTextBlock, fila);
            grid.Children.Add(relativeFrequencyTextBlock);

            // Calcular el porcentaje (pi) multiplicando la frecuencia relativa por 100
            double porcentaje = frecuenciaRelativa * 100;

            // Crear TextBlock para mostrar el porcentaje (debajo de la columna "pi")
            TextBlock percentageTextBlock = new TextBlock()
            {
                Text = porcentaje.ToString("0.00") + "%", // Mostrar el porcentaje con formato de porcentaje
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(percentageTextBlock, 3); // Columna "pi"
            Grid.SetRow(percentageTextBlock, fila);
            grid.Children.Add(percentageTextBlock);

            // Calcular FI (Frecuencia Acumulada)
            fiAcumulado += count;

            // Crear TextBlock para mostrar FI (debajo de la columna "FI")
            TextBlock cumulativeFrequencyTextBlock = new TextBlock()
            {
                Text = fiAcumulado.ToString(), // Mostrar FI como texto
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativeFrequencyTextBlock, 4); // Columna "FI"
            Grid.SetRow(cumulativeFrequencyTextBlock, fila);
            grid.Children.Add(cumulativeFrequencyTextBlock);

            // Calcular HI (Frecuencia Acumulada Relativa)
            double hi = (double)fiAcumulado / totalElementos;

            // Crear TextBlock para mostrar HI (debajo de la columna "HI")
            TextBlock cumulativeRelativeFrequencyTextBlock = new TextBlock()
            {
                Text = hi.ToString("0.000"), // Mostrar HI como texto formateado
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativeRelativeFrequencyTextBlock, 5); // Columna "HI"
            Grid.SetRow(cumulativeRelativeFrequencyTextBlock, fila);
            grid.Children.Add(cumulativeRelativeFrequencyTextBlock);

            // Calcular PI (Frecuencia Acumulada Relativa en porcentaje)
            double pi = hi * 100;

            // Crear TextBlock para mostrar PI (debajo de la columna "PI")
            TextBlock cumulativePercentageTextBlock = new TextBlock()
            {
                Text = pi.ToString("0.00") + "%", // Mostrar PI como texto formateado de porcentaje
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(cumulativePercentageTextBlock, 6); // Columna "PI"
            Grid.SetRow(cumulativePercentageTextBlock, fila);
            grid.Children.Add(cumulativePercentageTextBlock);
        }

        // Definir los nombres que deseas asignar a cada TextBlock en un orden específico
        string[] textBlockNames = { "Intervalos", "fi", "hi", "pi", "FI", "HI", "PI" };

        // Agregar encabezados en la primera fila del Grid (Row = 0)
        for (int col = 0; col < 7; col++)
        {
            TextBlock textBlock = new TextBlock()
            {
                Text = textBlockNames[col], // Asignar el nombre específico al TextBlock
                Foreground = Brushes.White, // Color de texto blanco
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5) // Margen entre TextBlocks
            };

            Grid.SetColumn(textBlock, col);
            Grid.SetRow(textBlock, 0); // Primera fila para los encabezados
            grid.Children.Add(textBlock);
        }

        // Crear un nuevo Border
        Border border = new Border()
        {
            Name = "border2",
            Margin = new Thickness(10, 10, 10, 0),
            Height = double.NaN, // Auto height
            Width = double.NaN, // Auto width
            CornerRadius = new CornerRadius(10),
            Padding = new Thickness(10),
            Background = Brushes.Black
        };

        // Agregar el Grid al Border
        border.Child = grid;

        // Obtener el StackPanel existente por su nombre
        StackPanel panel = (StackPanel)FindName("Panel2");

        if (panel != null)
        {
            // Limpiar cualquier contenido previo del StackPanel
            panel.Children.Clear();

            // Agregar el Border al StackPanel
            panel.Children.Add(border);
        }
        else
        {
            // El StackPanel "Panel2" no se encontró
            MessageBox.Show("No se encontró el StackPanel 'Panel2'.");
        }
    }

    private int CountNumbersInRange(TextBox[,] textBoxMatrix, double minIntervalo, double maxIntervalo)
    {
        int count = 0;

        // Recorrer la matriz textBoxMatrix
        for (int row = 0; row < textBoxMatrix.GetLength(0); row++)
        {
            for (int col = 0; col < textBoxMatrix.GetLength(1); col++)
            {
                // Obtener el valor numérico del TextBox
                if (double.TryParse(textBoxMatrix[row, col].Text, out double value))
                {
                    // Verificar si el valor está dentro del rango [minIntervalo, maxIntervalo)
                    if (value >= minIntervalo && value < maxIntervalo)
                    {
                        count++; // Incrementar el contador
                    }
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
        // Obtener la visibilidad actual de los botones adicionales
        Visibility buttonVisibility = MediaAritmeticaButton.Visibility;

        // Cambiar la visibilidad de los botones adicionales
        if (buttonVisibility == Visibility.Collapsed)
        {
            DropUpIcon.Visibility = Visibility.Visible;
            // Ocultar el ícono de bajada (?)
            DropDownIcon.Visibility = Visibility.Collapsed;

            MediaAritmeticaButton.Visibility = Visibility.Visible;
            MediaGeometricaButton.Visibility = Visibility.Visible;
            MediaPonderadaButton.Visibility = Visibility.Visible;
        }
        else
        {
            // Mostrar el ícono de bajada (?)
            DropDownIcon.Visibility = Visibility.Visible;
            // Ocultar el ícono de subida (?)
            DropUpIcon.Visibility = Visibility.Collapsed;

            MediaAritmeticaButton.Visibility = Visibility.Collapsed;
            MediaGeometricaButton.Visibility = Visibility.Collapsed;
            MediaPonderadaButton.Visibility = Visibility.Collapsed;
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void ButtonBase2_OnClick(object sender, RoutedEventArgs e)
    {
    }

    private void ButtonBase3_OnClick(object sender, RoutedEventArgs e)
    {
        CreateRandomGridWithRandomData();
    }
}