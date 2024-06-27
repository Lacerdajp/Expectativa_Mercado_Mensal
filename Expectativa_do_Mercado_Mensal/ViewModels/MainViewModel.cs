using Expectativa_do_Mercado_Mensal.Models;
using Expectativa_do_Mercado_Mensal.Service;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Expectativa_do_Mercado_Mensal.ViewModels
{
    internal class MainViewModel  {
        private readonly ExpectativasContext _dbContext;
        private readonly BacenService _bacenService;
        public ObservableCollection<ExpectativasMercado> Expectativas { get; set; }
        public ICommand LoadDataCommand { get; set; }
        public ICommand ExportCsvCommand { get; set; }
        public ICommand SaveToDataBaseCommand {  get; set; }
        private string _indicadorSelecionado;
        public string IndicadorSelecionado
        {
            get => _indicadorSelecionado;
            set
            {
                _indicadorSelecionado = value;
                OnPropertyChanged(nameof(IndicadorSelecionado));
            }
        }

        private DateTime _dataInicio;
        public DateTime DataInicio
        {
            get => _dataInicio;
            set
            {
                _dataInicio = value;
                OnPropertyChanged(nameof(DataInicio));
            }
        }

        private DateTime _dataFim;
        public DateTime DataFim
        {
            get => _dataFim;
            set
            {
                _dataFim = value;
                OnPropertyChanged(nameof(DataFim));
            }
        }
        private PlotModel _plotModel;
        public PlotModel PlotModel
        {
            get => _plotModel;
            set
            {
                _plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }

        public MainViewModel(string indicador, DateTime dateInicio, DateTime dateFim)
        {
            _dbContext = new ExpectativasContext();
            _bacenService = new BacenService();
            Expectativas = new ObservableCollection<ExpectativasMercado>();
            IndicadorSelecionado = indicador;
            DataInicio = dateInicio;
            DataFim = dateFim;
            LoadDataCommand = new RelayCommand(async (param) => await CarregarDados(IndicadorSelecionado,DataInicio, DataFim));
            ExportCsvCommand = new RelayCommand(async(param) => await ExportCsv());
            SaveToDataBaseCommand = new RelayCommand(async (param) => await SaveToDatabase());
            InitializePlotModel();
        }
        private async Task CarregarDados(string indicador, DateTime dateInicio, DateTime dateFim)
        {
            string message;
            indicador = indicador.Substring(indicador.IndexOf(": ") + 2);
            if (indicador == null || ((!indicador.Equals("IPCA")) && (!indicador.Equals("IGP-M")) && (!indicador.Equals("Selic"))))
            {
                message = "Erro no Indicador";
                MessageBox.Show(message);
            }
            else if (dateInicio > dateFim)
            {
                message = "Escolha uma Data Inicial menor que a Data Final";
                MessageBox.Show(message);

            }
            else
            {

                try
                {
                    var data = await _bacenService.GetExpectativasAsync(indicador, dateInicio, dateFim);

                    Expectativas.Clear();
                    foreach (var item in data)
                    {

                        Expectativas.Add(item);
                    }

                    UpdatePlot(data, DataFim, DataInicio, indicador);
                }catch (Exception ex)
                {
                }
             }
        }
        private void UpdatePlot(List<ExpectativasMercado> data,DateTime datafim,DateTime dataInicio, string indicador)
        {
            PlotModel.Series.Clear();
            List<DateTime> referencias = new List<DateTime>();
            if (indicador.Equals("Selic")&& (datafim<=dataInicio.AddMonths(2)))
            {
                double v = ((8 * datafim.Month) / 12);
                int m = (int)(Math.Ceiling(v));
                var x = new DateTime(datafim.Year,m,1);
                referencias.Add(x.AddMonths(+2));
                referencias.Add(x.AddMonths(+1));
            }else if(indicador.Equals("Selic") && (datafim > dataInicio.AddMonths(2)))
            {
                double v = ((8 * datafim.Month) / 12);
                int m = (int)(Math.Ceiling(v));
                var x = new DateTime(datafim.Year, m, 1);
                referencias.Add(x.AddMonths(0));
                referencias.Add(x.AddMonths(+1));
            }
            else
            {
                referencias.Add(datafim);
                referencias.Add(datafim.AddMonths(1));
                referencias.Add(datafim.AddMonths(2));
            }

            double xMin = 0;
            double xMax = 0;
            double yMin = 0;
            double yMax = 0;
            foreach (var item in referencias)
            {
                var y=item.ToString("MM/yyyy");
                var lineSeries = new LineSeries { Title = y };
                var pontos=data.Where(x=> { if (x.Indicador == "Selic") { 
                        return x.DataReferencia.Replace("R", "0").Equals(y); } 
                    else { return x.DataReferencia.Equals(y); } })
                    .OrderBy(x=>x.Data)
                    .Select(x=> new  DataPoint(DateTimeAxis
                    .ToDouble(DateTime.Parse(x.Data)), x.Mediana));
                xMax = pontos.Max(p => p.X)>xMax||xMax==0? pontos.Max(p => p.X):xMax;
                xMin = pontos.Min(p => p.X)<xMin||xMin==0? pontos.Min(p => p.X):xMin;
                yMax = pontos.Max(p => p.Y)>yMax || yMax == 0 ?pontos.Max(p=>p.Y):yMax;
                yMin = pontos.Min(p => p.Y)<yMin|| yMin==0? pontos.Min(p => p.Y) :yMin;
                lineSeries.Points.AddRange(pontos);
                if (indicador == "Selic")
                {
                    string title = y.Remove(0,1);
                title = "R"+title;
                lineSeries.Title = title;

                }
                
                PlotModel.Series.Add(lineSeries);
            }
            //
           

            double paddingX = (xMax - xMin) * 0.05;
            double paddingY = (yMax - yMin) * 0.05;

            xMin -= paddingX;
            xMax += paddingX;
            yMin -= paddingY;
            yMax += paddingY;

            PlotModel.Axes.Clear();
            PlotModel.Title = indicador;
            PlotModel.Axes.Add(new DateTimeAxis
            {
                Position = AxisPosition.Bottom,
                StringFormat = "dd/MM/yyyy",
                Minimum = xMin,
                Maximum = xMax,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });
            PlotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Minimum = yMin,
                Maximum = yMax,
                IsZoomEnabled = false,
                IsPanEnabled = false
            });

            PlotModel.InvalidatePlot(true);
        }
        private void InitializePlotModel()
        {
            PlotModel = new PlotModel { Title = IndicadorSelecionado.Substring(IndicadorSelecionado.IndexOf(": ") + 2)};
            PlotModel.Axes.Add(new DateTimeAxis { Position = AxisPosition.Bottom, StringFormat = "dd/MM/yyyy" });
            PlotModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left });
        }
        private async Task SaveToDatabase()
        {
            if (_dbContext.IsDatabaseConnected())
            {
                try
                {
                    foreach (var item in Expectativas)
                    {
                        _dbContext.Expectativas.Add(item);
                    }
                    await _dbContext.SaveChangesAsync();
                    MessageBox.Show("Os dados foram salvos com sucesso");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    // Tratar exceções aqui
                }
            }
            else
            {
                MessageBox.Show("Base não Connectada");
            }
        } 
        private async Task ExportCsv()
        {
            try
            {
                StringBuilder csv = new StringBuilder();
                csv.AppendLine("Indicador,Data,Data Referencia,Media,Mediana,Desvio Padrão,Minimo,Maximo,Numero Respondentes,base Calculo");
                foreach (var expectativa in Expectativas)
                {
                    csv.AppendLine($"{expectativa.Indicador},{expectativa.Data:yyyy-MM-dd},{expectativa.DataReferencia},{expectativa.Media},{expectativa.Mediana},{expectativa.DesvioPadrao},{expectativa.Minimo},{expectativa.Maximo},{expectativa.numeroRespondentes},{expectativa.baseCalculo}");
                }
                string pastaDownload = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string nomeArquivo = "Expectativas";
                string extensaoArquivo = ".csv";
                string nomeFinal = nomeArquivo + extensaoArquivo;
                string pastaArquivo = Path.Combine(pastaDownload, nomeFinal);
                int index = 1;

                // Check if the file already exists and increment the file name if it does
                while (File.Exists(pastaArquivo))
                {
                    nomeFinal = nomeArquivo + "(" + index + ")" + extensaoArquivo;
                    pastaArquivo = Path.Combine(pastaDownload, nomeFinal);
                    index++;
                }
                File.WriteAllText(pastaArquivo, csv.ToString());
                MessageBox.Show("Os dados foram salvos na pasta de Download como " + nomeFinal).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
