using CommunityToolkit.Mvvm.ComponentModel;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore;
using SkiaSharp;
using Sigma_Trainer.Services;
using CommunityToolkit.Mvvm.Input;
using Sigma_Trainer.Resources.Languages;

namespace Sigma_Trainer.ViewModel
{
    public partial class ExerciseViewModel : ObservableObject
    {
        private readonly int _exerciseId;
        private readonly StatisticsService _statisticsService;
        private readonly ExerciseService _exerciseService;
        [ObservableProperty]
        public string name;
        public Axis[] XAxes { get; set; }
        public Axis[] YAxes { get; set; }
        public List<string> Dates { get; set; }
        public ISeries[] Series { get; set; }
        private int StatisticsPageNumber { get; set; }
        private int StatisticsPageSize { get; set; }
        [ObservableProperty]
        public int statisticsForPeriod;
        [ObservableProperty]
        public int allTimeStatistics;
        public ExerciseViewModel(int exerciseId, ExerciseService exerciseService, StatisticsService statisticsService)
        {
            _exerciseId = exerciseId;
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
            StatisticsPageNumber = 0;
            StatisticsPageSize = 7;
        }
        public async Task LoadStatistics()
        {
            var exercise = await _exerciseService.GetExerciseAsync(_exerciseId);
            var exerciseStatistics = await _statisticsService.GetExerciseStatisticsAsync(exercise.Id, StatisticsPageSize, StatisticsPageNumber);
            var values = exerciseStatistics.Select(es => es.count).ToArray();
            StatisticsForPeriod = values.Sum();
            AllTimeStatistics = await _statisticsService.GetAllExerciseValue(_exerciseId);
            Dates = exerciseStatistics.Select(es => es.DateTime.ToString("dd:MM:yy")).ToList();
            Name = exercise.Name;
            var series = new LineSeries<int>
            {
                Name = exercise.Name,
                Values = values,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 3 },
                GeometrySize = 2,
            };
            Series = [series];
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
        public async Task EditStatisticsPageSizer(string size)
        {
            StatisticsPageSize = int.Parse(size);
            await LoadStatistics();
        }
        [RelayCommand]
        public async Task GetNextStatisticsPage()
        {
            if (StatisticsPageNumber > 0)
                StatisticsPageNumber--;
            else
                StatisticsPageNumber = 0;
           await LoadStatistics();
        }
        [RelayCommand]
        public async Task GetPreviousStatisticsPage()
        {
            StatisticsPageNumber++;
            await  LoadStatistics();
        }
        [RelayCommand]
        public async Task AddScore()
        {
            var result = await Application.Current.MainPage.DisplayPromptAsync(Strings.Add_a_score,
                Strings.Enter_the_number_of_points_,
                Strings.OK,
                Strings.Cancel,
                keyboard: Keyboard.Numeric);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (result != null && int.TryParse(result, out int score))
            {
                if (score > 0)
                {
                    await _statisticsService.AddExerciseStatisticsAsync(_exerciseId, score);
                }
            }
            await LoadStatistics();
        }
        [RelayCommand]
        public async Task RenameExercise()
        {
            var newName = await Application.Current.MainPage.DisplayPromptAsync(Strings.Rename_exercise,
                Strings.Enter_a_new_name,
                Strings.OK,
                Strings.Cancel,
                keyboard: Keyboard.Text);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (newName != null)
            {
                await _exerciseService.RenameExerciseAsync(_exerciseId, newName);
                Name = newName;
            }
        }
        [RelayCommand]
        public async Task DeleteExercise()
        {
            // Отображаем диалоговое окно с вопросом о подтверждении удаления
            bool isConfirmed = await Application.Current.MainPage.DisplayAlert(
                Strings.DeleteExerciseAnswer, // Заголовок
                Strings.This_action_cannot_be_undone_, // Сообщение
                Strings.OK, // Текст кнопки "Да"
                Strings.Cancel // Текст кнопки "Нет"
            );

            // Проверяем, подтвердил ли пользователь удаление
            if (isConfirmed)
            {
                await _exerciseService.DeleteExerciseAsync(_exerciseId);
                await Shell.Current.Navigation.PopToRootAsync(true);
            }
        }
    }
}
