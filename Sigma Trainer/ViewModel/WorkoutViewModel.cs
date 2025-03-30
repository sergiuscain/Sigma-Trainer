using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Sigma_Trainer.Services;
using Sigma_Trainer.View;
using SkiaSharp;
using System.Collections.ObjectModel;

namespace Sigma_Trainer.ViewModel
{
    public partial class WorkoutViewModel : ObservableObject
    {
        private readonly StatisticsService _statisticsService;
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        ObservableCollection<Exercises> exercises;
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public List<string> Dates { get; set; }
        public ISeries[] Series { get; set; }
        public WorkoutViewModel(StatisticsService statisticsService, ExerciseService exerciseService)
        {
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
        }
        [RelayCommand]
        public async Task AddExercise()
        {
            var viewModel = new AddExerciseViewModel(_exerciseService);
            var page = new AddExercisePage(viewModel);
            await Shell.Current.Navigation.PushAsync(page);
            await UpdateExerciseList();
        }
        [RelayCommand]
        public async Task AddScore(int exerciseId)
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync("Добавить счёт",
                "Введите количество очков:",
                "OK",
                "Отмена",
                keyboard: Keyboard.Numeric);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (result != null && int.TryParse(result, out int score))
            {
                if (score > 0)
                {
                    await _statisticsService.AddExerciseStatisticsAsync(exerciseId, score);

                    var r = await _statisticsService.GetExerciseStatisticsAsync(exerciseId);
                    await LoadStatistics();
                }
            }
        }
        public async Task UpdateExerciseList()
        {
            Exercises = new ObservableCollection<Exercises>(await _exerciseService.GetExercises());
        }
        public async Task LoadStatistics()
        {
            var exercisesList = await _exerciseService.GetExercises();
            var series = new List<LineSeries<int>>();
            for (int i = 0; i < exercisesList.Count; i++)
            {
                var exerciseStatistics = await _statisticsService.GetExerciseStatisticsAsync(exercisesList[i].Id, 14);
                var name = exercisesList[i].Name;
                var values = exerciseStatistics.Select(es => es.count).ToArray();
                series.Add(CreateLineSeries(values, name));
                if(i == 0)
                {
                    Dates = exerciseStatistics.Select(es => es.DateTime.ToString("dd:MM:yy")).ToList();
                }
            }
            Series = series.ToArray();
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
        private LineSeries<int> CreateLineSeries(int[] values, string name)
        {
            return new LineSeries<int>
            {
                Values = values,
                Name = name,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 }, // Пример цвета
                GeometrySize = 2,
            };
        }
        public async Task InitExercises()
        {
            if ((await _exerciseService.GetExercises()).Count < 1)
            {
                var exercise1 = new Exercises { Name = "Отжимания",};
                var exercise2 = new Exercises { Name = "Подтягивания" };
                var exercise3 = new Exercises { Name = "Приседания"};
                await _exerciseService.AddExerciseAsync(exercise1);
                await _exerciseService.AddExerciseAsync(exercise2);
                await _exerciseService.AddExerciseAsync(exercise3);
            }
        }
    }
}
