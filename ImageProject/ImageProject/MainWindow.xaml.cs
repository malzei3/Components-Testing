using ImageDiff;
using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using Vintasoft;

namespace ImageProject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        /// <summary>
        /// Bit map Comparer 
        /// </summary>
        private BitmapComparer comparer;

        /// <summary>
        /// First image 
        /// </summary>
        private Bitmap image1;

        /// <summary>
        /// Second image
        /// </summary>
        private Bitmap image2;

        /// <summary>
        /// Is standred selected flag
        /// </summary>
        private bool isStandredSelected;

        /// <summary>
        /// Is not standred selected flag
        /// </summary>
        private bool isNotStandredSelected;

        /// <summary>
        /// Is damaged selected flag
        /// </summary>
        private bool isDamagedSelected;

        /// <summary>
        /// Is not damged selected flag
        /// </summary>
        private bool isNotDamagedSelected;

        /// <summary>
        /// Is detected flag
        /// </summary>
        private bool isDetected;

        /// <summary>
        /// Is not detected flag.
        /// </summary>
        private bool isNotDetected;

        /// <summary>
        /// Property Changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //setup campare options
            var options = new CompareOptions
            {
                AnalyzerType = AnalyzerTypes.ExactMatch,
                JustNoticeableDifference = 2.3,
                DetectionPadding = 2,
                Labeler = LabelerTypes.ConnectedComponentLabeling,
                BoundingBoxColor = System.Drawing.Color.Red,
                BoundingBoxPadding = 10,
                BoundingBoxMode = BoundingBoxModes.Multiple
            };

            this.comparer = new BitmapComparer(options);

            //insilize flags
            this.isNotStandredSelected = true;
            this.isNotDamagedSelected = true;
            this.isNotDetected = true;
        }


        /// <summary>
        /// Close the application
        /// </summary>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }


        /// <summary>
        /// Open Def Image
        /// </summary>
        private void OpenDefImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                this.imgDef.Source = new BitmapImage(new Uri(op.FileName));

                this.image1 = new Bitmap(op.FileName);
                this.isStandredSelected = true;
                this.isNotStandredSelected = false;
                RaisePropertyChanged("IsStandredSelected");
                RaisePropertyChanged("isNotStandredSelected");

            }
        }

        /// <summary>
        /// Open second image
        /// </summary>
        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (op.ShowDialog() == true)
            {
                image2 = new Bitmap(op.FileName);

                this.isDamagedSelected = true;
                this.isNotDamagedSelected = false;
                RaisePropertyChanged("IsDamagedSelected");
                RaisePropertyChanged("IsNotDamagedSelected");


                try
                {
                    image2 = comparer.Compare(image1, image2);
                    if (image2.Flags != image1.Flags)
                    {
                        this.isDetected = true;
                        this.isNotDetected = false;
                        RaisePropertyChanged("IsDetected");
                        RaisePropertyChanged("IsNotDetected");
                    }
                    else
                    {
                        this.isDetected = false;
                        this.isNotDetected = true;
                        RaisePropertyChanged("IsDetected");
                        RaisePropertyChanged("IsNotDetected");
                    }

                    imgPhoto.Source = BitmapToImageSource(image2);

                }
                catch
                {
                    MessageBox.Show("Please enter same size images!");
                    this.isDamagedSelected = false;
                    this.isNotDamagedSelected = true;
                }
                
            }
        }


        /// <summary>
        /// Converts Bitmap to image source.
        /// </summary>
        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        /// <summary>
        /// Is Standred Selected Property
        /// </summary>
        public bool IsStandredSelected
        {
            get
            {
                return isStandredSelected;
            }
            set
            {
                if (isStandredSelected != value)
                {
                    isStandredSelected = value;
                    RaisePropertyChanged("IsStandredSelected");  // To notify when the property is changed
                }
            }
        }

        /// <summary>
        /// Is Not Standred Selected Property
        /// </summary>
        public bool IsNotStandredSelected
        {
            get
            {
                return isNotStandredSelected;
            }
            set
            {
                if (isNotStandredSelected != value)
                {
                    isNotStandredSelected = value;
                    RaisePropertyChanged("IsNotStandredSelected");  // To notify when the property is changed
                }
            }
        }

        /// <summary>
        /// Is Damaged Selected Property 
        /// </summary>
        public bool IsDamagedSelected
        {
            get
            {
                return isDamagedSelected;
            }
            set
            {
                if (isDamagedSelected != value)
                {
                    isDamagedSelected = value;
                    RaisePropertyChanged("IsDamagedSelected");  // To notify when the property is changed
                }
            }
        }

        /// <summary>
        /// Is Not Damaged Selected property 
        /// </summary>
        public bool IsNotDamagedSelected
        {
            get
            {
                return isNotDamagedSelected;
            }
            set
            {
                if (isNotDamagedSelected != value)
                {
                    isNotDamagedSelected = value;
                    RaisePropertyChanged("IsNotDamagedSelected");  // To notify when the property is changed
                }
            }
        }

        /// <summary>
        /// Is Detected flag.
        /// </summary>
        public bool IsDetected
        {
            get
            {
                return isDetected;
            }
            set
            {
                if (isDetected != value)
                {
                    isDetected = value;
                    RaisePropertyChanged("IsDetected");  // To notify when the property is changed
                }
            }
        }

        /// <summary>
        /// Is Not Detected Property 
        /// </summary>
        public bool IsNotDetected
        {
            get
            {
                return isNotDetected;
            }
            set
            {
                if (isNotDetected != value)
                {
                    isNotDetected = value;
                    RaisePropertyChanged("IsNotDetected");  // To notify when the property is changed
                }
            }
        }


        /// <summary>
        /// Raise Property Changed
        /// </summary>
        /// <param name="propertyName"></param>
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
