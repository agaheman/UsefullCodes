using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgahClassLibrary
{
    public class Notification
    {
       

        /// <param name="icon">new System.Drawing.Icon(System.IO.Path.GetFullPath(@"Path"))</param>
        public void DisplayNotify(System.Drawing.Icon icon ,string text,string balloonTipTitle="",string balloonTipText="",int timeout=100)
        {
            var notifyIcon = new NotifyIcon();
            try
            {
                notifyIcon.Icon = icon;
                notifyIcon.Text = text;
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipTitle = balloonTipTitle;
                notifyIcon.BalloonTipText = balloonTipText;
                notifyIcon.ShowBalloonTip(timeout);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

        public void DisplayNotify(string text, string balloonTipTitle = "", string balloonTipText = "", int timeout = 100)
        {
            var notifyIcon = new NotifyIcon();
            try
            {
                notifyIcon.Text = text;
                notifyIcon.Visible = true;
                notifyIcon.BalloonTipTitle = balloonTipTitle;
                notifyIcon.BalloonTipText = balloonTipText;
                notifyIcon.ShowBalloonTip(timeout);
            }
            catch (Exception ex)
            {
                // ignored
            }
        }

    }
}
