﻿using Sigma_Trainer.ViewModel;

namespace Sigma_Trainer.View;
public partial class SummaryPage : ContentPage
{
    public SummaryPage(SummaryViewModel vm)
    {
        BindingContext = vm;
        InitializeComponent();
    }
}
