using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Patterns;
using Animalerie.WPF.Interfaces;
using Animalerie.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages
{
    public partial class AnimalAddPage : Page
    {
        // On injecte maintenant le ViewModel au lieu des Services !
        public AnimalAddPage(AnimalAddViewModel viewModel)
        {
            InitializeComponent();

            // C'est ici que la magie opère : on lie la Vue au ViewModel
            this.DataContext = viewModel;
        }

        // Constructeur par défaut pour l'injection de dépendances
        public AnimalAddPage()
            : this(App.ServiceProvider.GetRequiredService<AnimalAddViewModel>())
        {
        }
    }
}