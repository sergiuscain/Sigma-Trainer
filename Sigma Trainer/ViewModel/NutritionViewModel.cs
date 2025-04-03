using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using Sigma_Trainer.Resources.Languages;
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
                CreateLineSeries(FoodStatistics.Select(fs => fs.Calories).Reverse().ToArray(), Strings.Kcal, SeriesColors[0]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Proteins).Reverse().ToArray(), Strings.Proteins__g_, SeriesColors[1]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Fats).Reverse().ToArray(), Strings.Fats__g_, SeriesColors[2]),
                CreateLineSeries(FoodStatistics.Select(fs => fs.Carbohydrates).Reverse().ToArray(), Strings.Carbohydrates__g_, SeriesColors[3]),
            };
            if(FoodStatistics.FirstOrDefault(fs => fs.Date == DateTime.Today).Calories < 1)
            {
                SeriesToday = new ISeries[]
                {
                    new PieSeries<double> { Values = [1], Name = Strings.You_haven_t_eaten_anything_today_ },
                };
            }
            else
            {
                SeriesToday = new ISeries[]
                {
                    CreatePieSeries(FoodStatistics.FirstOrDefault().Proteins, SeriesColors[1], Strings.Proteins__g_),
                    CreatePieSeries(FoodStatistics.FirstOrDefault().Fats, SeriesColors[2], Strings.Fats__g_),
                    CreatePieSeries(FoodStatistics.FirstOrDefault().Carbohydrates, SeriesColors[3], Strings.Carbohydrates__g_)
                };
            }
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
                Stroke = new SolidColorPaint(color) { StrokeThickness = 3 }, // Цвет обводки
                Fill = new LiveChartsCore.SkiaSharpView.Painting.LinearGradientPaint(
                    new SKColor[] { color.WithAlpha(100), color.WithAlpha(50) },
                    new SKPoint(0.5f, 0),
                    new SKPoint(0.5f, 1)
                ),
                GeometrySize = 2,
                GeometryStroke = new SolidColorPaint(color) { StrokeThickness = 3 }, // Цвет обводки для геометрии
                GeometryFill = new SolidColorPaint(color) // Цвет заливки для геометрии
            };
        }
        private PieSeries<double>  CreatePieSeries(int value, SKColor color, string name)
        {
            return new PieSeries<double> 
            {
                Values = [value],
                Name = name, 
                Stroke = new SolidColorPaint(color) { StrokeThickness = 3 },
                Fill = new LiveChartsCore.SkiaSharpView.Painting.LinearGradientPaint(
                    new SKColor[] { color.WithAlpha(100), color.WithAlpha(75) },
                    new SKPoint(0.1f, 0),
                    new SKPoint(0.5f, 1)
                ),
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
