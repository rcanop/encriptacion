using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.Text.Json;
using Encriptacion.wpf.Domain;
using System.Windows.Controls;

namespace Encriptacion.wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private UrlInfo Registro;
        public UrlInfo Ficha;

        public MainWindow()
        {
            InitializeComponent();
            Ficha = new UrlInfo();
            DataContext = Ficha;

        }
        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog dlgAbrirFichero = new OpenFileDialog
            {
                Title = "Abrir fichero de datos",
                DefaultExt = ".json",
                FileName = "claves.json",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dlgAbrirFichero.ShowDialog() ?? false)
            {
                try
                {
                    LeerEscribir lector = new LeerEscribir();
                    string jsonString = lector.LeerFicheroUTF8(dlgAbrirFichero.FileName, dlgAbrirFichero.FileName.EndsWith(".cod"));

                    JsonSerializerOptions options = new JsonSerializerOptions
                    {
                        WriteIndented = true,
                        //PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                        IgnoreNullValues = true
                    };

                    List<UrlInfo> sitios;
                    if (jsonString.Length > 0)
                        sitios = (List<UrlInfo>)JsonSerializer.Deserialize(jsonString, typeof(List<UrlInfo>), options);
                    else
                    {
                        sitios = new List<UrlInfo>();
                    }
                    RegCount.Text = sitios.Count().ToString();
                    lstSites.ItemsSource = sitios.OrderBy(si => si.Sitio);
                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                catch (JsonException ex)
                {
                    MessageBox.Show($"Error en la carga del fichero {dlgAbrirFichero.FileName}.\r\n{ex.Message}"
                        , "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                if (MessageBox.Show("No se ha seleccionado fichero de datos.\r\n¿Desea crear uno nuevo?"
                    , "Pregunta", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    List<UrlInfo> sitios = new List<UrlInfo>();
                    lstSites.ItemsSource = sitios;
                    RegCount.Text = "";
                }
            }

        }

        private void LstSitesSelectionChanged(object sender, RoutedEventArgs e)
        {
            Registro = null;
            Registro = (sender as ListBox).SelectedItems[0] as UrlInfo;
            Ficha.GetData(Registro);

        }

        private void ActualizarLista(object sender, RoutedEventArgs e)
        {
            if (Ficha != null)
            {
                Registro.GetData(Ficha);
                //lstSites.
            }
        }
    }
}
