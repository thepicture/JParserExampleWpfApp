using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace JParserExampleWpfApp
{
    /// <summary>
    /// Interaction logic for ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : Window
    {
        public ScheduleWindow()
        {
            InitializeComponent();
        }

        private void UpdateScheduleUsingDictionary(object sender, RoutedEventArgs e)
        {
            ScheduleView.ItemsSource = null;
            var schedules = new List<FromToSpan>();
            Dictionary<string, FromToSpan> scheduleDictionary =
                JsonConvert.DeserializeObject<Dictionary<string, FromToSpan>>(File.ReadAllText("schedules.txt"));
            foreach (var entry in scheduleDictionary)
            {
                entry.Value.Title = entry.Key.Split(null)[1];
                schedules.Add(entry.Value);
            }
            ScheduleView.ItemsSource = schedules;
        }

        private void UpdateScheduleUsingJObject(object sender, RoutedEventArgs e)
        {
            ScheduleView.ItemsSource = null;
            var schedules = new List<Schedule>();
            JObject scheduleObject = JObject.Parse(File.ReadAllText("schedules.txt"));
            foreach (var property in scheduleObject.Properties())
            {
                schedules.Add(new Schedule
                {
                    Title = property.Name.Split(new string[] { " " }, StringSplitOptions.None)[1],
                    From = TimeSpan.Parse(property.Value["from"].Value<string>()),
                    To = TimeSpan.Parse(property.Value["to"].Value<string>())
                });
            }
            ScheduleView.ItemsSource = schedules;
        }
    }
}
