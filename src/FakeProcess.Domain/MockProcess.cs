using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using FakeProcess.Domain.Enums;
using FakeProcess.Domain.Interfaces;

namespace FakeProcess.Domain
{
    public class MockProcess : ObservableObject, IProcess, IDisposable
    {
        private int id;
        public int Id { get => id; private set => SetProperty(ref id, value); }
        private string name;
        public string Name { get=> name; private set => SetProperty(ref name, value); }
        private ProcessType processType;
        public ProcessType Type { get => processType; private set => SetProperty(ref processType, value); }
        private ProcessState processState = ProcessState.NotActive;
        public ProcessState State { get => processState; private set => SetProperty(ref processState, value); }
        private int progress = 0;
        public int Progress { get => progress; private set => SetProperty(ref progress, value); }
        private string progressLable;
        public string ProgressLable { get => progressLable; set=>SetProperty(ref progressLable, value); }


        CancellationTokenSource cancelTokenSource = new CancellationTokenSource();

        private readonly int fullTime;
        private int SpentTime;
        private readonly LogService logService;
        public MockProcess(int id, string name, ProcessType type)
        {
            logService = LogService.getInstance();

            Id = id;
            Name = name;
            Type = type;

            Random rnd = new Random();
            fullTime = rnd.Next(10, 40);

            UpdateLable();
        }

        public async void Start()
        {
            cancelTokenSource = new CancellationTokenSource();
            State = ProcessState.Running;
            while (!cancelTokenSource.IsCancellationRequested)
            {
                double p = (SpentTime / (double)fullTime);
                if(p > 1)
                {
                    p = 1;
                }
                Progress = (int)(p * 100);
                UpdateLable();
                if(Progress >= 100)
                {
                    CompleteProcess();
                }

                await Task.Delay(1000);
                SpentTime++;
            }
        }
        private void UpdateLable()
        {
            switch (State)
            {
                case ProcessState.Running:
                    ProgressLable = $"{Progress} / 100";
                    break;
                case ProcessState.NotActive:
                    ProgressLable = "Не активно";
                    break;
                case ProcessState.Error:
                    progressLable = "0 / 100";
                    break;
                case ProcessState.Stoped:
                    progressLable = $"{Progress} / 100";
                    break;    
            }
        }
        private void CompleteProcess()
        {
            Random rnd = new Random();
            cancelTokenSource.Cancel();
            if (rnd.Next(0, 2) != 0)
            {
                State = ProcessState.Error;
                UpdateLable();
                logService.AddEvent($"{Name} завешилась с ошибкой");
            }
            else
            {
                logService.AddEvent($"{Name} успешно завершилась");
            }
        }

        public void Stop()
        {
            cancelTokenSource.Cancel();
            State = ProcessState.Stoped;
            UpdateLable();
        }
        public void Dispose()
        {
            Stop();
            cancelTokenSource.Dispose();
        }
    }
}
