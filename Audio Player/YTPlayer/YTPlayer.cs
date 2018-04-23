using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.IO;

namespace Player
{
    public class YTPlayer
    {
        public LinkResolver.Video current;

        private static void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
              ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

        public string StartStreaming()
        {
            var task = current.GetBestStreamableAudioFormat();
            task.Wait();
            string url = task.Result.Url;
            string tempPath = "temp/PerpetumMusica/playing/" + DateTime.Now.Ticks.ToString() + ".mp3";
            FileInfo tmpFileInfo = new FileInfo(tempPath);
            tmpFileInfo.Directory.Create();
            DirectoryInfo tmpFolder = tmpFileInfo.Directory;
            foreach(var x in tmpFolder.GetFiles())
            {
                x.Delete();
            }
            foreach(var x in tmpFolder.GetDirectories())
            {
                x.Delete(true);
            }
            var playerProcess = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "ffmpeg",
                    Arguments = $"-i \"{url}\" {tempPath} -y",
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            playerProcess.Start();

            return tempPath;
        }

    }
}