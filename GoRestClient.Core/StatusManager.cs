using System;

namespace GoRestClient.Core
{
    /// <inheritdoc cref="IStatusManager" />
    public class StatusManager : IStatusManager
    {
        ///<inheritdoc/>
        public event EventHandler<string> OnNewStatusReceived;

        ///<inheritdoc/>
        public void ReportException(string message, Exception exception)
        {
            OnNewStatusReceived?.Invoke(this, $"[ERROR] {message ?? string.Empty} \r\n. Exception: {exception?.Message}");
        }

        ///<inheritdoc/>
        public void ReportInfo(string message)
        {
            OnNewStatusReceived?.Invoke(this, $"[INFO] {message ?? string.Empty}");
        }
    }
}
