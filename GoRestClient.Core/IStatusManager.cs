using System;

namespace GoRestClient.Core
{
    public interface IStatusManager
    {
        event EventHandler<string> OnNewStatusReceived;
        void ReportException(Exception ex);
        void ReportInfo(string message);
    }
}