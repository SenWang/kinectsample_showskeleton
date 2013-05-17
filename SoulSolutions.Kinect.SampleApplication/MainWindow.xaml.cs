#region

using System;
using System.Linq;
using System.Windows;
using Microsoft.Kinect;

#endregion

namespace SoulSolutions.Kinect.SampleApplication
{
    public partial class MainWindow : Window
    {
        private KinectSensor nui = KinectSensor.KinectSensors[0];

        public MainWindow()
        {
            InitializeComponent();
            Loaded += WindowLoaded;
            Closed += WindowClosed;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            //Cleanup
            nui.Stop();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            nui.Start();

            #region TransformSmooth

            ////Use to transform and reduce jitter
            var parameters = new TransformSmoothParameters
                                 {
                                     Smoothing = 0.75f,
                                     Correction = 0.0f,
                                     Prediction = 0.0f,
                                     JitterRadius = 0.05f,
                                     MaxDeviationRadius = 0.04f
                                 };

            nui.SkeletonStream.Enable(parameters);

            #endregion

            //add event to receive skeleton data
            nui.SkeletonFrameReady += NuiSkeletonFrameReady;
        }

        private void NuiSkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
        {
            SkeletonFrame allSkeletons = e.OpenSkeletonFrame();

            using (SkeletonFrame skframe = e.OpenSkeletonFrame())
            {
                if (skframe != null)
                {
                    Skeleton[] FrameSkeletons = new Skeleton[skframe.SkeletonArrayLength];
                    skframe.CopySkeletonDataTo(FrameSkeletons);
                    //get the first tracked skeleton
                    skeleton.SkeletonData = (from s in FrameSkeletons
                                             where s.TrackingState == SkeletonTrackingState.Tracked
                                             select s).FirstOrDefault();
                }
            }

            
        }
    }
}