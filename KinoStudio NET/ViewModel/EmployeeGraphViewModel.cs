using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using KinoStudio_NET.Annotations;
using LiveCharts;
using LiveCharts.Wpf;

namespace KinoStudio_NET.ViewModel;

internal class EmployeeGraphViewModel : INotifyPropertyChanged
{
    private EmployeesViewModel _employees = new();

    private int _maxValue;

    private SeriesCollection _series = new();

    public EmployeesViewModel Employees
    {
        get => _employees;
        set
        {
            _employees = value;
            SetValue(value);
            OnPropertyChanged();
        }
    }

    public SeriesCollection Series
    {
        get => _series;
        set
        {
            _series = value;
            OnPropertyChanged();
        }
    }

    public int MaxValue
    {
        get => _maxValue;
        set
        {
            _maxValue = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public void SetValue(EmployeesViewModel value)
    {
        MaxValue = value.Employees.Count < 10 ? 10 : value.Employees.Count + 1;

        var val = value.Employees.GroupBy(employee =>
            (new DateTime(1) + DateTime.Now.Subtract(employee.PersonalInfo.Passport.DateOfBirth)).Year - 1);

        foreach (var group in val)
            Series.Add(
                new ColumnSeries
                {
                    Title = $"Возраст: {@group.Key}",
                    Values = new ChartValues<int>(new[] {@group.AsEnumerable().Count()})
                });
    }
}