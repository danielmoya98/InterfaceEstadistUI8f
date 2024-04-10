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
        

        // Mostrar el nombre en los TextBlock correspondientes
        nombreuser.Text = nombre;
        nombreuser2.Text = "HOLA " + nombre + "!";

        // Determinar qué imagen mostrar según el botón presionado
        if (boton == "M")
        {
            // Cambiar la imagen para el botón "M"
            genero.Text = "M";
            imagen.Source =
                new BitmapImage(new Uri("C:\\Users\\Alienware\\RiderProjects\\InterfaceEstadistUI8f\\img\\user.png"));
        }
        else if (boton == "F")
        {
            genero.Text = "F";

            // Cambiar la imagen para el botón "F"
            imagen.Source =
                new BitmapImage(new Uri("C:\\Users\\Alienware\\RiderProjects\\InterfaceEstadistUI8f\\img\\user2.png"));
        }

        DateTime fechaActual = DateTime.Now;

        // Formatear la fecha como una cadena legible
        string fechaFormateada = fechaActual.ToString("dd/MM/yyyy");

        // Asignar la fecha formateada al TextBlock llamado "fecha"
        fecha.Text = fechaFormateada;


        DateTime horaActual = DateTime.Now;

        // Formatear la hora en un formato específico (HH:mm tt)
        string horaFormateada = horaActual.ToString("hh:mm");

        // Asignar la hora formateada al TextBlock llamado "HoraActual"
        HoraActual.Text = horaFormateada + " AM";
        // Inicializar el temporizador
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1); // Intervalo de 1 segundo
        timer.Tick += Timer_Tick;

        // Iniciar el temporizador cuando se carga la ventana
        Loaded += (sender, e) => { StartTimer(); };
    }

    private void Timer_Tick(object sender, EventArgs e)
    {
        // Incrementar el tiempo transcurrido cada segundo
        elapsedTime = elapsedTime.Add(TimeSpan.FromSeconds(1));

        // Actualizar el TextBlock para mostrar el tiempo transcurrido
        UpdateElapsedTime();
    }

    private void StartTimer()
    {
        // Reiniciar el tiempo transcurrido
        elapsedTime = TimeSpan.Zero;

        // Iniciar el temporizador
        timer.Start();
    }

    private void UpdateElapsedTime()
    {
        // Actualizar el TextBlock con el tiempo transcurrido formateado
        contador.Text = $"{elapsedTime.Hours:00}:{elapsedTime.Minutes:00}:{elapsedTime.Seconds:00}";
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        // Detener el temporizador cuando se cierra la ventana
        timer.Stop();
    }

    private TextBox[,] textBoxMatrix;

    private void CreateRandomGridWithRandomData()
    {
        Random random = new Random();

        // Generar aleatoriamente el número de filas y columnas en el rango del 1 al 15
        int numRows = random.Next(5, 16);
        int numCols = random.Next(5, 16);

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
        CreateTextBlocksInGrid(valorMinimo, Ka, amplitud2, textBoxMatrix, CantidadTotal);
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


    private void CreateTextBlocksInGrid(int valorMinimo, int k, float amplitud, TextBox[,] textBoxMatrix, int cantidadT)
    {
        // Crear un nuevo Grid
        Grid grid = new Grid();

        // Definir las columnas en el Grid (8 columnas en total)
        for (int col = 0; col < 8; col++)
        {
            grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
        }

        // Definir las filas en el Grid (k + 2 filas en total)
        for (int i = 0; i < k + 2; i++)
        {
            grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
        }

        // Calcular el número total de elementos en la matriz
        int totalElementos = textBoxMatrix.GetLength(0) * textBoxMatrix.GetLength(1);

        int fiAcumulado = 0;
        double sumatoriaFiXi = 0.0; // Sumatoria de fi * xi
        double productoriafiXi = 1.0;
        double sumatoriaFiDivXi = 0.0; // Sumatoria de fi / xi
        double identificarIntervalo = cantidadT / 2;

        // Calcular y agregar los intervalos en filas del Grid
        for (int fila = 1; fila <= k; fila++)
        {
            double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);
            double intervaloMaximo = valorMinimo + (fila * amplitud);

            // Calcular xi para la fila actual
            double xi = (intervaloMinimo + intervaloMaximo - 1) / 2.0;

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

            // Crear TextBlock para mostrar xi (debajo de la columna "xi")
            TextBlock xiTextBlock = new TextBlock()
            {
                Text = xi.ToString("0.00"), // Mostrar xi como texto formateado
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

            Grid.SetColumn(xiTextBlock, 7); // Columna "xi"
            Grid.SetRow(xiTextBlock, fila);
            grid.Children.Add(xiTextBlock);

            // Calcular fi * xi y agregarlo a la sumatoria
            sumatoriaFiXi += count * xi;

            // Calcular fi / xi y agregarlo a la sumatoria
            sumatoriaFiDivXi += (double)count / xi;

            productoriafiXi *= Math.Pow(xi, count);
        }

        // Calcular media aritmética
        double mediaAritmetica = sumatoriaFiXi / cantidadT;

        // Calcular media geométrica
        double mediaGeometrica = Math.Pow(productoriafiXi, 1.0 / cantidadT);

        // Calcular media armónica
        double mediaArmonica = cantidadT / sumatoriaFiDivXi;

        /*// Mostrar media aritmética en un TextBlock dentro del StackPanel "Panel2"
        TextBlock mediaAritmeticaTextBlock = new TextBlock()
        {
            Text = $"M A: {mediaAritmetica.ToString("0.00")}",
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5)
        };

        Grid.SetColumn(mediaAritmeticaTextBlock, 0);
        Grid.SetRow(mediaAritmeticaTextBlock, k + 1); // Última fila para mostrar la media aritmética
        grid.Children.Add(mediaAritmeticaTextBlock);*/

        mediaAritmeticaRes.Text = mediaAritmetica.ToString("0.00");

        mediaArmonicaRes.Text = mediaArmonica.ToString("0.00");
        /*
        // Mostrar media armónica en un TextBlock dentro del StackPanel "Panel2"
        TextBlock mediaArmonicaTextBlock = new TextBlock()
        {
            Text = $"MeAr: {mediaArmonica.ToString("0.00")}",
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5)
        };

        Grid.SetColumn(mediaArmonicaTextBlock, 1);
        Grid.SetRow(mediaArmonicaTextBlock, k + 1); // Última fila para mostrar la media armónica
        grid.Children.Add(mediaArmonicaTextBlock);
        */

        mediaGeometricaRes.Text = mediaGeometrica.ToString("0.00");
        /*// Mostrar media geométrica en un TextBlock dentro del StackPanel "Panel2"
        TextBlock mediaGeometricaTextBlock = new TextBlock()
        {
            Text = $"MG: {mediaGeometrica.ToString("0.00")}",
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5)
        };

        Grid.SetColumn(mediaGeometricaTextBlock, 2);
        Grid.SetRow(mediaGeometricaTextBlock, k + 1); // Última fila para mostrar la media geométrica
        grid.Children.Add(mediaGeometricaTextBlock);*/

        // Definir los nombres que deseas asignar a cada TextBlock en un orden específico
        string[] textBlockNames = { "Intervalos", "fi", "hi", "pi", "FI", "HI", "PI", "xi" };

        // Agregar encabezados en la primera fila del Grid (Row = 0)
        for (int col = 0; col < textBlockNames.Length; col++)
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

        int numeroMasCercano = -1; // Valor predeterminado en caso de no encontrar ningún número
        int filaNumeroMasCercano = -1; // Fila donde se encuentra el número más cercano
        int valorColumna1NumeroMasCercano = -1; // Valor de la columna 1 para el número más cercano

        for (int fila = 1; fila <= k; fila++)
        {
            // Obtener el valor de FI en la fila actual (columna 4)
            int fiValue = int.Parse(grid.Children.Cast<TextBlock>()
                .First(tb => Grid.GetRow(tb) == fila && Grid.GetColumn(tb) == 4)
                .Text);

            // Obtener el valor de la columna 1 en la fila actual
            int fiValueColumna1 = int.Parse(grid.Children.Cast<TextBlock>()
                .First(tb => Grid.GetRow(tb) == fila && Grid.GetColumn(tb) == 1)
                .Text);

            // Verificar si FI es mayor o igual a identificarIntervalo
            if (fiValue >= identificarIntervalo)
            {
                numeroMasCercano = fiValue;
                filaNumeroMasCercano = fila;
                valorColumna1NumeroMasCercano = fiValueColumna1; // Obtener el valor de la columna 1 para esta fila

                // MessageBox.Show($"Fila del número más cercano: {filaNumeroMasCercano}");
                // MessageBox.Show($"Número más cercano: {numeroMasCercano}");
                // MessageBox.Show($"Valor de la columna 1 para el número más cercano: {valorColumna1NumeroMasCercano}");

                break; // Detener la búsqueda después de encontrar el primer valor
            }
        }


        // Recuperar el valor de FI en la fila anterior al número más cercano
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

        /*MessageBox.Show($"Límite Inferior del Intervalo: {limiteInferior}");
        */
        // Mostrar el valor de FI en la posición anterior en un MessageBox
        if (fiAnterior != -1)
        {
            // MessageBox.Show($"Valor de FI en la posición anterior: {fiAnterior}");
            // // Obtener el límite inferior del intervalo para la fila número más cercano
            //
        }
        else
        {
            MessageBox.Show("No se encontró un valor de FI en la posición anterior.");
        }

        // Valor de la mediana
        double mediana = limiteInferior + (cantidadT / 2.0 - fiAnterior) * (amplitud / valorColumna1NumeroMasCercano);

// Encontrar el intervalo que contiene la mediana
        int filaMediana = -1; // Valor inicial para indicar que no se ha encontrado el intervalo

        medianaRes.Text = mediana.ToString("0.00");
        /*TextBlock medianaTextBlock = new TextBlock()
        {
            Text = $"Mediana: {mediana.ToString("0.00")}",
            Foreground = Brushes.White,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            Margin = new Thickness(5)
        };

    // Agregar el TextBlock de mediana al Grid
        Grid.SetColumn(medianaTextBlock, 4); // Por ejemplo, añadirlo a la columna 4
        Grid.SetRow(medianaTextBlock, k + 1); // Última fila para mostrar la mediana
        grid.Children.Add(medianaTextBlock);*/

        for (int fila = 1; fila <= k; fila++)
        {
            // Calcular los límites del intervalo actual
            double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);
            double intervaloMaximo = valorMinimo + (fila * amplitud);

            // Verificar si la mediana está dentro del intervalo actual
            if (mediana >= intervaloMinimo && mediana < intervaloMaximo)
            {
                // La mediana se encuentra en este intervalo
                filaMediana = fila;
                break; // Terminar la búsqueda
            }
        }

        if (filaMediana != -1)
        {
            // Calcular los límites del intervalo que contiene la mediana
            double limiteInferiorMediana = valorMinimo + ((filaMediana - 1) * amplitud);
            double limiteSuperiorMediana = valorMinimo + (filaMediana * amplitud);

            // Obtener el valor de fi en la columna 1 para el intervalo de la mediana
            int valorColumna1Mediana = int.Parse(grid.Children.Cast<TextBlock>()
                .First(tb => Grid.GetRow(tb) == filaMediana && Grid.GetColumn(tb) == 1)
                .Text);

            // Calcular el valor de fi para el intervalo anterior (filaMediana - 1)
            int valorColumna1Anterior = -1;
            if (filaMediana > 1)
            {
                valorColumna1Anterior = int.Parse(grid.Children.Cast<TextBlock>()
                    .First(tb => Grid.GetRow(tb) == filaMediana - 1 && Grid.GetColumn(tb) == 1)
                    .Text);
            }

            // Calcular el valor de fi para el intervalo posterior (filaMediana + 1)
            int valorColumna1Posterior = -1;
            if (filaMediana < k)
            {
                valorColumna1Posterior = int.Parse(grid.Children.Cast<TextBlock>()
                    .First(tb => Grid.GetRow(tb) == filaMediana + 1 && Grid.GetColumn(tb) == 1)
                    .Text);
            }

            // Calcular d1 y d2
            double d1 = valorColumna1Mediana - valorColumna1Anterior;
            double d2 = valorColumna1Posterior - valorColumna1Mediana;

            // // Mostrar los datos recuperados en un MessageBox
            // MessageBox.Show($"Intervalo que contiene la mediana:\n" +
            //                 $"Límite Inferior: {limiteInferiorMediana}\n" +
            //                 $"Límite Superior: {limiteSuperiorMediana}\n" +
            //                 $"Valor de fi (Columna 1): {valorColumna1Mediana}\n" +
            //                 $"d1 (Columna 1 Mediana - Columna 1 Anterior): {d1}\n" +
            //                 $"d2 (Columna 1 Posterior - Columna 1 Mediana): {d2}");

            double moda = limiteInferiorMediana + ((amplitud * d1) / (d1 + d2));

            /*TextBlock modaTextBlock = new TextBlock()
            {
                Text = $"Modo: {moda.ToString("0.00")}",
                Foreground = Brushes.White,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(5)
            };

    // Agregar el TextBlock de mediana al Grid
            Grid.SetColumn(modaTextBlock, 5); // Por ejemplo, añadirlo a la columna 4
            Grid.SetRow(modaTextBlock, k + 1); // Última fila para mostrar la mediana
            grid.Children.Add(modaTextBlock);*/
            modaRes.Text = moda.ToString("0.00");
        }
        else
        {
            // La mediana no se encuentra en ningún intervalo (esto debería ser poco probable si los cálculos son precisos)
            MessageBox.Show("No se encontró el intervalo que contiene la mediana.");
        }
    }


    private double GetLimiteInferiorIntervalo(int fila, int valorMinimo, float amplitud)
    {
        // Calcular el intervalo mínimo para la fila especificada
        double intervaloMinimo = valorMinimo + ((fila - 1) * amplitud);

        return intervaloMinimo;
    }


// Función para contar números en un rango específico en una matriz de TextBox
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
        // Obtener la visibilidad actual de los botones adicionales
        Visibility buttonVisibility = MediaAritmeticaButton.Visibility;

        // Cambiar la visibilidad de los botones adicionales
        if (buttonVisibility == Visibility.Collapsed)
        {
            DropUpIcon.Visibility = Visibility.Visible;
            DropDownIcon.Visibility = Visibility.Collapsed;
            
            MediaAritmeticaButton.Visibility = Visibility.Visible;
            SturgesButton.Visibility = Visibility.Visible;
        }
        else
        {
            // Mostrar el ícono de bajada (?)
            DropDownIcon.Visibility = Visibility.Visible;
            // Ocultar el ícono de subida (?)
            DropUpIcon.Visibility = Visibility.Collapsed;

            MediaAritmeticaButton.Visibility = Visibility.Collapsed;
            SturgesButton.Visibility = Visibility.Collapsed;
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

    private void GenerarTextbox_OnClick(object sender, RoutedEventArgs e)
    {
        if (int.TryParse(generartextbox.Text, out int cantidad) && cantidad > 0)
        {
            // Limpiar WrapPanel antes de agregar nuevos TextBox y botón de media
            Wrap1.Children.Clear();

            // Lista para almacenar los TextBox generados dinámicamente
            List<TextBox> textBoxes = new List<TextBox>();

            // Generar y agregar TextBox dinámicamente al WrapPanel
            for (int i = 0; i < cantidad; i++)
            {
                TextBox newTextBox = new TextBox();
                newTextBox.Margin = new Thickness(10, 15, 10, 0);
                newTextBox.Foreground = Brushes.White;
                newTextBox.BorderBrush = Brushes.White;
                newTextBox.Width = 100;
                Wrap1.Children.Add(newTextBox);
                textBoxes.Add(newTextBox); // Agregar TextBox a la lista
            }

            // Crear botón para calcular media aritmética
            Button calcularMediaButton = new Button();
            calcularMediaButton.Content = "Calcular Media aritmética";
            calcularMediaButton.Click += (s, args) =>
            {
                // Calcular la suma de los valores ingresados en los TextBox
                double suma = 0;
                int count = 0; // Contador para contar los TextBox válidos

                foreach (var textBox in textBoxes)
                {
                    // Intentar convertir el valor del TextBox a double
                    if (double.TryParse(textBox.Text, out double valor))
                    {
                        suma += valor;
                        count++; // Incrementar el contador de TextBox válidos
                    }
                    else
                    {
                        // Mostrar un mensaje de error si el valor no es válido
                        MessageBox.Show($"Valor no válido en el TextBox: {textBox.Text}", "Error", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return; // Salir del evento de clic si hay un valor no válido
                    }
                }

                // Verificar si hay al menos un TextBox con valor numérico válido
                if (count > 0)
                {
                    // Calcular la media aritmética
                    double media = suma / count;

                    // Mostrar la media aritmética al lado del botón
                    TextBlock resultadoMedia = new TextBlock();
                    resultadoMedia.Text = media.ToString(); // Mostrar la media con dos decimales
                    resultadoMedia.Foreground = Brushes.White;
                    resultadoMedia.Margin = new Thickness(10, 15, 10, 0);
                    Wrap1.Children.Add(resultadoMedia); // Agregar resultado al WrapPanel
                }
                else
                {
                    MessageBox.Show("Por favor, introduzca valores numéricos válidos en los TextBox.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            };


            // Establecer propiedades del botón para calcular media
            LinearGradientBrush gradientBrush = new LinearGradientBrush();
            gradientBrush.StartPoint = new Point(0, 0);
            gradientBrush.EndPoint = new Point(1, 1);
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(58, 97, 134), 1)); // Color principal
            gradientBrush.GradientStops.Add(new GradientStop(Color.FromRgb(137, 37, 62), 0)); // Color de fondo
            calcularMediaButton.Background = gradientBrush;
            calcularMediaButton.Margin = new Thickness(0, 15, 0, 0);
            calcularMediaButton.BorderBrush = Brushes.Transparent;

            // Agregar botón de calcular media al WrapPanel
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