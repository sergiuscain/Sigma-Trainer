﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DBLibrary.Entities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Sigma_Trainer.Resources.Languages;
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
        private int StatisticsPageNumber { get; set; }
        private int StatisticsPageSize { get; set; }
        public WorkoutViewModel(StatisticsService statisticsService, ExerciseService exerciseService)
        {
            _statisticsService = statisticsService;
            _exerciseService = exerciseService;
            StatisticsPageNumber = 0;
            StatisticsPageSize = 7;
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
                    await _statisticsService.AddExerciseStatisticsAsync(exerciseId, score);
                    await LoadStatistics();
                }
            }
        }
        [RelayCommand]
        public async Task EditExercise(int exerciseId)
        {
            var newName = await Application.Current.MainPage.DisplayPromptAsync(Strings.Rename_exercise,
                Strings.Enter_a_new_name,
                Strings.OK,
                Strings.Cancel,
                keyboard: Keyboard.Text);
            // Проверяем, что пользователь ввёл значение и оно корректно преобразуется в число
            if (newName != null)
            {
                await _exerciseService.RenameExerciseAsync(exerciseId, newName);
                await UpdateExerciseList();
                await LoadStatistics();
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
                var exerciseStatistics = await _statisticsService.GetExerciseStatisticsAsync(exercisesList[i].Id, StatisticsPageSize, StatisticsPageNumber);
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
        [RelayCommand]
        public async Task GetNextStatisticsPage()
        {
            if (StatisticsPageNumber > 0)
                StatisticsPageNumber--;
            else 
                StatisticsPageNumber = 0;
            var exercisesList = await _exerciseService.GetExercises();
            var series = new List<LineSeries<int>>();
            for (int i = 0; i < exercisesList.Count; i++)
            {
                var exerciseStatistics = await _statisticsService.GetExerciseStatisticsAsync(exercisesList[i].Id, StatisticsPageSize, StatisticsPageNumber);
                var name = exercisesList[i].Name;
                var values = exerciseStatistics.Select(es => es.count).ToArray();
                series.Add(CreateLineSeries(values, name));
                if (i == 0)
                {
                    Dates = exerciseStatistics.Select(es => es.DateTime.ToString("dd:MM:yy")).ToList();
                }
            }
            Series = series.ToArray();
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

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(XAxes));
        }
        [RelayCommand]
        public async Task GetPreviousStatisticsPage()
        {
            StatisticsPageNumber++;
            var exercisesList = await _exerciseService.GetExercises();
            var series = new List<LineSeries<int>>();
            for (int i = 0; i < exercisesList.Count; i++)
            {
                var exerciseStatistics = await _statisticsService.GetExerciseStatisticsAsync(exercisesList[i].Id, StatisticsPageSize, StatisticsPageNumber);
                var name = exercisesList[i].Name;
                var values = exerciseStatistics.Select(es => es.count).ToArray();
                series.Add(CreateLineSeries(values, name));
                if (i == 0)
                {
                    Dates = exerciseStatistics.Select(es => es.DateTime.ToString("dd:MM:yy")).ToList();
                }
            }
            Series = series.ToArray();
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

            OnPropertyChanged(nameof(Series));
            OnPropertyChanged(nameof(XAxes));
        }
        [RelayCommand]
        public async Task EditStatisticsPageSizer(string size)
        {
            StatisticsPageSize = int.Parse(size);
            await LoadStatistics();
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
                var exercise1 = new Exercises { Name = Strings.Push_ups};
                var exercise2 = new Exercises { Name = Strings.Pull_ups};
                var exercise3 = new Exercises { Name = Strings.Squats};
                await _exerciseService.AddExerciseAsync(exercise1);
                await _exerciseService.AddExerciseAsync(exercise2);
                await _exerciseService.AddExerciseAsync(exercise3);
            }
        }
    }
}
