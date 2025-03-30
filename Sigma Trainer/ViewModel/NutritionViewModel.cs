using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Sigma_Trainer.Services;
using Sigma_Trainer.View;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Sigma_Trainer.ViewModel
{
    public partial class NutritionViewModel : ObservableObject
    {
        private readonly FoodService _foodService;
        private readonly StatisticsService _statisticsService;
        Random random = new Random();
        [ObservableProperty]
        public ObservableCollection<FoodRecord> todayFoodRecords;
        [ObservableProperty]
        public ObservableCollection<DailyFoodStatistics> foodStatistics;
        public ISeries[] Series { get; set; }
        [ObservableProperty]
        public IEnumerable<ISeries> seriesToday;
        public List<string> Dates { get; set; }
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        private static readonly SKColor[] SeriesColors = new[]
        {
            new SKColor(0, 255, 0, 255),   // Яркий зеленый - ккал
            new SKColor(0, 150, 255, 255), // Ярко-синий - белок
            new SKColor(255, 204, 0, 255), // Яркий желтый - жиры
            new SKColor(0, 102, 204, 255)  // Яркий голубой - углеводы
        };
        [ObservableProperty]
        public LabelVisual titleTodayStat;
        public NutritionViewModel(FoodService foodService, StatisticsService statistics)
        {
            _foodService = foodService;
            _statisticsService = statistics;
        }
        public async Task LoadFoodRecords() 
            => TodayFoodRecords = new ObservableCollection<FoodRecord>(await _foodService.GetFoodRecordsAsync(DateTime.Now.Date));
        public async Task LoadStatistics()
        {
            FoodStatistics = new ObservableCollection<DailyFoodStatistics>(await _statisticsService.GetFoodStatisticsAsync(7));
            Dates = FoodStatistics.Select(x => x.Date.ToString("dd:MM:yyyy")).Reverse().ToList();
            Series = new ISeries[]
            {
                CreateLineSeries(FoodStatistics.Select(fs => fs.Calories).Reverse().ToArray(), "Ккал", SeriesColors[0]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Proteins).Reverse().ToArray(), "Протеин:", SeriesColors[1]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Fats).Reverse().ToArray(), "Жиры:", SeriesColors[2]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Carbohydrates).Reverse().ToArray(), "Углеводы:", SeriesColors[3]),
            };
            if(FoodStatistics.FirstOrDefault(fs => fs.Date == DateTime.Today).Calories < 1)
            {
                SeriesToday = new ISeries[]
                {
                    new PieSeries<double> { Values = [1], Name = "Вы ничего не ели сегодня" },
                };
            }
            else
            {
                SeriesToday = new ISeries[]
                {
                    new PieSeries<double> { Values = [FoodStatistics.FirstOrDefault().Proteins], Name = "Белки" },
                    new PieSeries<double> { Values = [FoodStatistics.FirstOrDefault().Fats], Name = "Жиры" },
                    new PieSeries<double> { Values = [FoodStatistics.FirstOrDefault().Carbohydrates], Name = "Углеводы" }
                };
            }
            TitleTodayStat =
            new LabelVisual
            {
                Text = "Сводка за сегодня",
                TextSize = 25,
                Padding = new LiveChartsCore.Drawing.Padding(15)
            };
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
        // Метод для создания линии с заданным цветом
        private LineSeries<int> CreateLineSeries(int[] values, string name, SKColor color)
        {
            return new LineSeries<int>
            {
                Values = values,
                Name = name,
                Stroke = new SolidColorPaint(color) { StrokeThickness = 3 },
                Fill = new LiveChartsCore.SkiaSharpView.Painting.LinearGradientPaint(
                    new SKColor[] { color.WithAlpha(100), color.WithAlpha(50) },
                    new SKPoint(0.5f, 0),
                    new SKPoint(0.5f, 1)
                ),
                GeometrySize = 2,
                GeometryStroke = new SolidColorPaint(color) { StrokeThickness = 3 },
                GeometryFill = new SolidColorPaint(color)
            };
        }
        [RelayCommand]
        public async Task AddFoodRecord()
        {
            var page = new AddFoodRecordPage(new AddFoodRecordViewModel(_foodService, _statisticsService));
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
