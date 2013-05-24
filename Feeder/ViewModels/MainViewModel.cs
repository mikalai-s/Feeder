using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Feeder.Resources;

namespace Feeder.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.Items = new ObservableCollection<ItemViewModel>();
        }

        /// <summary>
        /// A collection for ItemViewModel objects.
        /// </summary>
        public ObservableCollection<ItemViewModel> Items { get; private set; }

        private string _sampleProperty = "Sample Runtime Property Value";
        /// <summary>
        /// Sample ViewModel property; this property is used in the view to display its value using a Binding
        /// </summary>
        /// <returns></returns>
        public string SampleProperty
        {
            get
            {
                return _sampleProperty;
            }
            set
            {
                if (value != _sampleProperty)
                {
                    _sampleProperty = value;
                    NotifyPropertyChanged("SampleProperty");
                }
            }
        }

        /// <summary>
        /// Sample property that returns a localized string
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// Creates and adds a few ItemViewModel objects into the Items collection.
        /// </summary>
        public async void LoadData()
        {
            // Sample data; replace with real data
            var r = new WebClient();
            var xml = await DownloadString("http://habrahabr.ru/rss/hubs/");
            /*this.Items.Add(new ItemViewModel()
                {
                    LineOne = "HELLO! Online: celeb & royal news, magazines, babies,",
                    LineTwo = "HELLO! Onliner brings you the latest celebrity",
                    Url = "asdg"
                });*/
            foreach (var item in XDocument.Parse(xml).Element("rss").Element("channel").Elements("item"))
            {
                this.Items.Add(new ItemViewModel()
                    {
                        LineOne = item.Element("title").Value,
                        LineTwo = item.Element("description").Value.Substring(0, 200),
                        Url = item.Element("guid").Value
                    });

            }
            this.IsDataLoaded = true;
        }

        public static Task<string> DownloadString(string uri)
        {
            var tcs = new TaskCompletionSource<string>();
            var r = new WebClient();
            r.DownloadStringCompleted += (sender, args) =>
                {
                    if (args.Error != null)
                    {
                        tcs.SetException(args.Error);
                    }
                    else
                    {
                        tcs.SetResult(args.Result);
                    }
                };
           
            r.DownloadStringAsync(new Uri(uri));
           
            return tcs.Task;
        } 

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}