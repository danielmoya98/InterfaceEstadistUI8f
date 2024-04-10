using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace InterfaceEstadistUI8f
{
    public partial class profile : Window
    {
        private Button selectedButton;

        public profile()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (selectedButton != null)
            {
                selectedButton.BorderBrush = Brushes.Transparent;
            }

            selectedButton = (Button)sender;
            selectedButton.BorderBrush = Brushes.Yellow;
        }

        private void Iniciar_Click(object sender, RoutedEventArgs e)
        {
            string nombre = txtNombre.Text;

            if (selectedButton != null)
            {
                string botonSeleccionado = selectedButton.Name;

                MainWindow ventanaNueva = new MainWindow(nombre, botonSeleccionado);
                ventanaNueva.Show();

                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un botón antes de iniciar.");
            }
        }
    }
}