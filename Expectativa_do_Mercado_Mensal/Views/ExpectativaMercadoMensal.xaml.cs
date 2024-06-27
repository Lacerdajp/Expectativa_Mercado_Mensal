using Expectativa_do_Mercado_Mensal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Expectativa_do_Mercado_Mensal.Views
{
    /// <summary>
    /// Interaction logic for ExpectativaMercadoMensal.xaml
    /// </summary>
    public partial class ExpectativaMercadoMensal : Page
    {
        public ExpectativaMercadoMensal()
        {
            InitializeComponent();
            DataContext = new MainViewModel(ComboBox.SelectedItem.ToString(),datainicio.DisplayDate,datafim.DisplayDate);
        }

    }
}
