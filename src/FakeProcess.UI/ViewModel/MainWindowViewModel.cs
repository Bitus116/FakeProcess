using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FakeProcess.Domain;
using FakeProcess.Domain.Enums;
using FakeProcess.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Input;

namespace FakeProcess.UI.ViewModel
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableCollection<IProcess> _processes;
        public ObservableCollection<IProcess> Processes
        {
            get => _processes;
            set => SetProperty(ref _processes, value);
        }
        private ObservableCollection<IProcess> _processesInternal;
        public ObservableCollection<IProcess> ProcessesInternal
        {
            get => _processesInternal;
            set => SetProperty(ref _processesInternal, value);
        }
        private string log;
        public string Log
        {
            get
            {
                log = String.Empty;
                foreach (var @event in logService.events)
                {
                    log += $"{@event}\n\n";
                }
                return log;

            }
            set => SetProperty(ref log, value);
        }

        public IRelayCommand AddTypeACommand { get; set; }
        public IRelayCommand AddTypeBCommand { get; set; }
        public IRelayCommand DeleteAllCommand { get; set; }
        public IRelayCommand<int> DeleteCommand { get; set; }
        public IRelayCommand StartAllCommand { get; set; }
        public IRelayCommand StopAllCommand { get; set; }
        public IRelayCommand TypeAToggleCommand { get; set; }
        public IRelayCommand TypeBToggleCommand { get; set; }

        private readonly LogService logService;

        private bool isTypeA = false;
        private bool isTypeB = false;
        public MainWindowViewModel()
        {
            logService = LogService.getInstance();
            logService.OnLogUpdated += LogService_OnLogUpdated;

            ProcessesInternal = new ObservableCollection<IProcess>();
            Processes = new ObservableCollection<IProcess>();

            AddTypeACommand = new RelayCommand(addProcess(ProcessType.TypeA));
            AddTypeBCommand = new RelayCommand(addProcess(ProcessType.TypeB));
            DeleteAllCommand = new RelayCommand(deleteAll, () => ProcessesInternal.Count() > 0);
            StartAllCommand = new RelayCommand(startAll, () => ProcessesInternal.Count > 0);
            StopAllCommand = new RelayCommand(stopAll, () => ProcessesInternal.Where(x => x.State == ProcessState.Running).Count() > 0);
            DeleteCommand = new RelayCommand<int>(delete, (x) => true );
            TypeAToggleCommand = new RelayCommand<bool>(typeAToggle);
            TypeBToggleCommand = new RelayCommand<bool>(typeBToggle);
        }

        private void deleteAll()
        {
            ProcessesInternal.Clear();
            logService.AddEvent("Все задачи были удалены");
            UpdateList();
            StartAllCommand.NotifyCanExecuteChanged();
            StopAllCommand.NotifyCanExecuteChanged();
            DeleteAllCommand.NotifyCanExecuteChanged();
        }

        private void typeAToggle(bool e)
        {
            isTypeA = e;
            
            UpdateList();
        }
        private void typeBToggle(bool e)
        {
            isTypeB = e;
            UpdateList();
        }


        private void UpdateList()
        {
            Processes.Clear();
            List<IProcess> p = new List<IProcess>();
            if (isTypeA)
            {
                p.AddRange(ProcessesInternal.Where(x => x.Type == ProcessType.TypeA));
            }
            if (isTypeB)
            {
                p.AddRange(ProcessesInternal.Where(x => x.Type == ProcessType.TypeB));
            }
            Processes = new ObservableCollection<IProcess>(p.OrderBy(x => x.Id).ToList());
        }

        
        private void LogService_OnLogUpdated()
        {
            OnPropertyChanged(nameof(Log));
        }

        private void delete(int id)
        {
            var process = ProcessesInternal.FirstOrDefault(p => p.Id == id);
            if (process != null)
            {
                ProcessesInternal.Remove(process);
                logService.AddEvent($"Задача №{id} была удалена");
                UpdateList();
            }
        }

        private void stopAll()
        {
            foreach (var process in ProcessesInternal)
            {
                if (process.State == ProcessState.Running)
                {
                    process.Stop();
                }
            }
            logService.AddEvent("Все задачи были остановлены");
        }

        private void startAll()
        {
            foreach (var process in ProcessesInternal)
            {
                if(process.State == ProcessState.NotActive || process.State == ProcessState.Stoped)
                {
                    process.Start();
                }
            }
            logService.AddEvent($"Все задачи были запущены");
            StopAllCommand.NotifyCanExecuteChanged();
        }

        private Action addProcess(ProcessType type)
        {
            return () =>
            {
                var id = ProcessesInternal.Count > 0 ? ProcessesInternal.Count + 1 : 1;
                ProcessesInternal.Add(new MockProcess(id, $"Задача №{id}", type));
                logService.AddEvent($"Была добавлена задача {(type == ProcessType.TypeA? "Типа А" : "Типа В")}");
                StartAllCommand.NotifyCanExecuteChanged();
                StopAllCommand.NotifyCanExecuteChanged();
                DeleteAllCommand.NotifyCanExecuteChanged();
                DeleteCommand.NotifyCanExecuteChanged();
                UpdateList();
            };
        }
    }
}
