namespace ReportPortal.Extensions.Insider.Core.Abstractions
{
    public interface IInstrumentationLogger
    {
        void Info(string message);

        void Error(string message);
    }
}
