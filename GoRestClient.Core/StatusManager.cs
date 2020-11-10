using System;

namespace GoRestClient.Core
{
    public class StatusManager : IStatusManager
    {
        public event EventHandler<string> OnNewStatusReceived;

        public void ReportException(Exception ex)
        {
            OnNewStatusReceived?.Invoke(this, $"[ERROR] {ex.Message}");
        }

        public void ReportInfo(string message)
        {
            OnNewStatusReceived?.Invoke(this, $"[INFO] {message}");
        }
    }
}
