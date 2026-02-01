using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages;
using Animalerie.WPF.Pages.Compatibilite;
using Animalerie.WPF.Pages.Contact;
using Animalerie.WPF.Pages.Interfaces;
using Animalerie.WPF.Pages.Vaccin;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Animalerie.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigating += MainFrame_Navigating;

            MainFrame.Navigate(new AnimalListPage());
        }

        private void MainFrame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            // 1. Récupérer la page qui est actuellement affichée (avant le changement)
            if (MainFrame.Content is ICanCheckDirty dirtyPage)
            {
                // 2. Vérifier si la page dit qu'elle a des modifs non sauvegardées
                if (dirtyPage.IsDirty)
                {
                    var result = MessageBox.Show(
                        "Vous avez des modifications non enregistrées. Voulez-vous vraiment quitter cette page ?",
                        "Confirmation de départ",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    // 3. Si l'utilisateur répond "Non", on annule la navigation
                    if (result == MessageBoxResult.No)
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        private void BtnListe_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AnimalListPage());
        }

        private void BtnAjout_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AnimalAddPage());
        }

        private void BtnCompatibilites_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new CompatibilitePage());
        }

        private void BtnContacts_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ContactListPage());
        }

        private void BtnAjoutContact_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new ContactFormPage());
        }

        private void BtnVaccins_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new VaccinPage());
        }
    }
}