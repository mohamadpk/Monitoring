using System;
using System.Collections.Generic;
using System.Text;
using DirectX.Capture;
using System.IO;
using System.Timers;
using Module_WebCam_Microphone_Capture.Response;
using System.Reflection;

namespace Module_WebCam_Microphone_Capture
{
    //TODO add exeption handling
    //TODO add encrypt to file with module encrypt
    public class Module_WebCam_Microphone_Capture
    {
        private Capture capture = null;
        private Filters filters = new Filters();
        public int MaxMinuteLimit { get; set; }
        public int MaxSizeLimit { get; set; }
        private Timer StopTimer;
        private DateTime EndTime;
        public OnStartRecordWebcamAndVoiceResponse OnStartRecordWebcamAndVoice(bool Webcam,bool Mic)
        {
            OnStartRecordWebcamAndVoiceResponse ROnStartRecordWebcamAndVoiceResponse = new OnStartRecordWebcamAndVoiceResponse();
            try
            {
                Directory.CreateDirectory("captures");
                if (MaxMinuteLimit > 0)
                    EndTime = DateTime.Now.AddMinutes(MaxMinuteLimit);

                StopTimer = new System.Timers.Timer();
                StopTimer.Interval = 2000;
                StopTimer.Elapsed += StopTimer_Elapsed;
                StopTimer.Enabled = true;
                Filter AudioDevice = null;
                Filter VideoDevice = null;
                foreach (Filter AD in filters.AudioInputDevices)
                {
                    AudioDevice = AD;
                    if (!AudioDevice.Name.Contains("DMO"))
                    {
                        if (!AudioDevice.Name.Contains("Creative"))
                        {
                            break;
                        }
                    }
                }
                foreach (Filter VD in filters.VideoInputDevices)
                {
                    VideoDevice = VD;
                    if (!VideoDevice.Name.Contains("DMO"))
                    {
                        break;
                    }
                }

                if (VideoDevice.Name.Contains("DMO"))
                {
                    Webcam = false;
                    ROnStartRecordWebcamAndVoiceResponse.Description = "VideoDeviceIsNotFind\r\n";
                }
                if (AudioDevice.Name.Contains("DMO") || AudioDevice.Name.Contains("Creative"))
                {
                    Mic = false;
                    ROnStartRecordWebcamAndVoiceResponse.Description = "AudioDeviceIsNotFind\r\n";
                }

                if (Webcam && Mic)
                {
                    capture = new Capture(VideoDevice, AudioDevice);
                }
                else if (Webcam)
                {
                    capture = new Capture(VideoDevice, null);
                }
                else if (Mic)
                {
                    capture = new Capture(null, AudioDevice);
                }
                else {
                    ROnStartRecordWebcamAndVoiceResponse.Description = "Video and Audio Device Is Empty";
                    return ROnStartRecordWebcamAndVoiceResponse;
                }

                //enable audio mic and set input volume to max
                AudioSource ADS = (AudioSource)capture.AudioSources[0];
                ADS.MixLevel = 1;
                ADS.Enabled = true;


                foreach (Filter vf in filters.VideoCompressors)
                {
                    if (vf.Name == "Cinepak Codec by Radius")
                    {
                        capture.VideoCompressor = vf;
                        break;
                    }
                    else if (vf.Name == "DV Video Encoder")
                    {
                        capture.VideoCompressor = vf;
                        break;
                    }
                }
                foreach (Filter af in filters.AudioCompressors)
                {
                    if (af.Name == "ATI MPEG Audio Encoder")
                    {
                        capture.AudioCompressor = af;
                        break;
                    }
                    else if (af.Name == "CCITT A-Law")
                    {
                        capture.AudioCompressor = af;
                        break;
                    }
                }
                capture.CaptureComplete += Capture_CaptureComplete; ;
                if (!capture.Cued)
                    capture.Filename = "captures\\" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".webmic";
                capture.Start();
                StopTimer.Start();
            }
            catch (Exception ex)
            {
                ROnStartRecordWebcamAndVoiceResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStartRecordWebcamAndVoiceResponse;

        }



        private void StopTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if(capture!=null)
            {
                if(MaxMinuteLimit>0 && MaxSizeLimit>0)
                {
                    if (CheckTime()|| CheckSize())
                    {
                        capture.Stop();
                        StopTimer.Stop();
                    }
                }
                else if(MaxMinuteLimit> 0)
                {
                    if(CheckTime())
                    {
                        capture.Stop();
                        StopTimer.Stop();
                    }
                }else if(MaxSizeLimit>0)
                {
                    if(CheckSize())
                    {
                        capture.Stop();
                        StopTimer.Stop();
                    }
                }else
                {
                    //not set max size or max time
                    //stop recording manualy
                }
                
                
            }
            else { StopTimer.Stop(); }
        }

        private bool CheckSize()
        {
            FileInfo fileinfo = new FileInfo(capture.Filename);
            if (fileinfo.Length > MaxSizeLimit)
            {
                return true;
            }
            return false;
        }
        private bool CheckTime()
        {
            DateTime NowTime = DateTime.Now;
            if (DateTime.Now.CompareTo(EndTime) >= 0)
            {

                return true;
            }
            return false;
        }
        private void Capture_CaptureComplete(object sender, EventArgs e)
        {
            OnRecordWebcamAndVoiceComplete(sender, e);
        }
        private OnRecordWebcamAndVoiceCompleteResponse OnRecordWebcamAndVoiceComplete(object sender, EventArgs e)
        {
            OnRecordWebcamAndVoiceCompleteResponse ROnRecordWebcamAndVoiceCompleteResponse = new OnRecordWebcamAndVoiceCompleteResponse();
            try
            {
                Utils._S_Utils_Crypto suc = new Utils._S_Utils_Crypto();
                ROnRecordWebcamAndVoiceCompleteResponse.EncryptedFileName= capture.Filename + "AES";
                ROnRecordWebcamAndVoiceCompleteResponse.Pass = "mpk";
                suc.EncryptFile(capture.Filename, ROnRecordWebcamAndVoiceCompleteResponse.EncryptedFileName,"mpk", suc.salt, 1);
                File.Delete(capture.Filename);
            }
            catch (Exception ex)
            {
                ROnRecordWebcamAndVoiceCompleteResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnRecordWebcamAndVoiceCompleteResponse;
        }
        public OnStopRecordWebcamAndVoiceResponse OnStopRecordWebcamAndVoice()
        {
            OnStopRecordWebcamAndVoiceResponse ROnStopRecordWebcamAndVoiceResponse = new OnStopRecordWebcamAndVoiceResponse();
            try
            {
                capture.Stop();
                StopTimer.Stop();
            }
            catch (Exception ex)
            {
                ROnStopRecordWebcamAndVoiceResponse.Errors.AddErrorToErrorList(MethodBase.GetCurrentMethod().ToString(), ex.Message);
            }
            return ROnStopRecordWebcamAndVoiceResponse;
        }
    }
}
