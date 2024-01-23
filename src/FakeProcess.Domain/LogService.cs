using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FakeProcess.Domain
{
    public class LogService
    {
        private static LogService _instance;
        private static object syncRoot = new();

        public List<string> events = new List<string>();
        public event Action OnLogUpdated;
        private LogService() { }

        public static LogService getInstance()
        {
            if (_instance == null)
            {
                lock (syncRoot)
                {
                    if (_instance == null)
                        _instance = new LogService();
                }
            }
            return _instance;
        }

        public void AddEvent(string message)
        {
            events.Add($"{DateTime.Now.ToString("HH:mm:ss")} {message}");
            OnLogUpdated?.Invoke();
        }
    }
}
