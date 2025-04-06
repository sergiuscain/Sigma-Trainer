using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.Painting;
using SkiaSharp;
using DBLibrary.Entities;
using Sigma_Trainer.Services;
using CommunityToolkit.Mvvm.Input;

namespace Sigma_Trainer.ViewModel
{
    public partial class SummaryViewModel : ObservableObject
    {
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public List<string> Dates { get; set; }
        public ISeries[] Series { get; set; }
        public List<WeightRecord> Weights { get; set; }
        [ObservableProperty]
        public double weight;
        private int StatisticsPageNumber { get; set; }
        private int StatisticsPageSize { get; set; }
        private readonly WeightService _weightService;
        public SummaryViewModel(WeightService weightService)
        {
            _weightService = weightService;
            StatisticsPageNumber = 0;
            StatisticsPageSize = 14;
            LoadStatistics().Wait();
        }
        public async Task LoadStatistics()
        {
            Weights = await _weightService.GetWeight(StatisticsPageSize, StatisticsPageNumber);
            Series = [new LineSeries<double>(Weights.Select(wr => wr.Weight).ToArray())];
            Dates = Weights.Select(wr => wr.Date.ToString("dd:MM:yy")).ToList();
            // Настройка осей
            XAxes = new Axis[]
            {
                new Axis
                {
                    Labels = Dates,
                    LabelsRotation = 45,
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                    SeparatorsAtCenter = false,
                    TicksPaint = new SolidColorPaint(SKColors.LightGray),
                    TicksAtCenter = true
                }
            };

            YAxes = new Axis[]
            {
                new Axis
                {
                    TextSize = 12,
                    SeparatorsPaint = new SolidColorPaint(SKColors.LightGray.WithAlpha(100)),
                    TicksPaint = new SolidColorPaint(SKColors.LightGray),
                    Labeler = value => value.ToString("N0")
                }
            };

            // Уведомляем об изменении Series, XAxes и YAxes
            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(XAxes));
            OnPropertyChanged(nameof(YAxes));
        }
        [RelayCommand]
        public async Task EditPageSize(string size)
        {
            StatisticsPageSize = int.Parse(size);
            await LoadStatistics();
        }
        [RelayCommand]
        public async Task NextPage(string size)
        {
            if (StatisticsPageNumber > 0)
            {
                StatisticsPageNumber--;
                await LoadStatistics();
            }
        }
        [RelayCommand]
        public async Task PreviousPage(string size)
        {
            StatisticsPageNumber++;
            await LoadStatistics();
        }
        [RelayCommand]
        public async Task AddWeight()
        {
            if(Weight > 2)
            {
                var weightRecord = new WeightRecord { Weight = Weight, Date = DateTime.Today};
                await _weightService.AddWeightAsync(weightRecord);

                await LoadStatistics();
            }
        }
    }
}
