using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterfaceEstadistUI8f;

public partial class profile : Window
{
    private Button selectedButton;

    public profile()
    {
        InitializeComponent();
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        // Cambiar el borde del botón seleccionado
        if (selectedButton != null)
        {
            selectedButton.BorderBrush = Brushes.Transparent; // Restaurar borde transparente para el botón previamente seleccionado
        }

        selectedButton = (Button)sender;
        selectedButton.BorderBrush = Brushes.Yellow; // Cambiar color de borde al botón seleccionado
    }

    private void Iniciar_Click(object sender, RoutedEventArgs e)
    {
        // Obtener el nombre ingresado en el TextBox
        string nombre = txtNombre.Text;

        // Validar si se ha seleccionado un botón
        if (selectedButton != null)
        {
            // Obtener la identificación del botón seleccionado
            string botonSeleccionado = selectedButton.Name;

            // Crear una nueva ventana y pasar los datos
            MainWindow ventanaNueva = new MainWindow(nombre, botonSeleccionado);
            ventanaNueva.Show();

            // Cerrar la ventana actual si es necesario
            this.Close();
        }
        else
        {
            MessageBox.Show("Por favor, seleccione un botón antes de iniciar.");
        }
    }
}