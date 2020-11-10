using System;

namespace GoRestClient.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStatusManager
    {
        /// <summary>
        /// Event triggered on every new status received.
        /// </summary>
        event EventHandler<string> OnNewStatusReceived;

        /// <summary>
        /// Report that an exception occurred.
        /// </summary>
        /// <param name="message">Friendly message related to the error.</param>
        /// <param name="exception">Captured exception.</param>
        void ReportException(string message, Exception exception);

        /// <summary>
        /// Report an regular event on the application that should be informed to de user..
        /// </summary>
        /// <param name="message">Message to be reported.</param>
        void ReportInfo(string message);
    }
}