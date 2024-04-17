using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Lab3_4
{
    public partial class Service1 : ServiceBase
    {
        // Import the WTSSendMessage function from wtsapi32.dll
        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
                IntPtr hServer,
                [MarshalAs(UnmanagedType.I4)] int SessionId,
                String pTitle,
                [MarshalAs(UnmanagedType.U4)] int TitleLength,
                String pMessage,
                [MarshalAs(UnmanagedType.U4)] int MessageLength,
                [MarshalAs(UnmanagedType.U4)] int Style,
                [MarshalAs(UnmanagedType.U4)] int Timeout,
                [MarshalAs(UnmanagedType.U4)] out int pResponse,
                bool bWait
            );

        public Service1()
        {
            // Set CanHandleSessionChangeEvent to true to handle session change events
            this.CanHandleSessionChangeEvent = true;
            InitializeComponent();
        }

        protected override void OnSessionChange(SessionChangeDescription changeDescription)
        {
            if (changeDescription != null && (changeDescription.Reason == SessionChangeReason.SessionLogon || changeDescription.Reason == SessionChangeReason.SessionUnlock))
            {
                // Get the session ID of the triggered event
                int sessionId = changeDescription.SessionId;

                try
                {
                    // Construct message details
                    string title = "Your Student ID!!!";
                    string message = "21522297 - 21522583 - 21522598";

                    int response;
                    // Call WTSSendMessage function to send the message
                    bool result = WTSSendMessage(
                        IntPtr.Zero,
                        sessionId,
                        title,
                        title.Length,
                        message,
                        message.Length,
                        0,
                        0,
                        out response,
                        true
                    );

                    // Check if sending the message was successful
                    if (!result)
                    {
                        // Log an error if message sending fails
                        Console.WriteLine("Failed to send message to session: " + sessionId);
                    }
                }
                catch (Exception ex)
                {
                    // Log any exceptions that occur during message sending
                    Console.WriteLine("Error occurred while sending message: " + ex.Message);
                }

                base.OnSessionChange(changeDescription);
            }
        }
    }
}
