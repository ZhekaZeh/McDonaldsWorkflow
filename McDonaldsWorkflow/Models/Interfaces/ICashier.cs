namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICashier: IEmployee
    {
        double Takings { get; }
        int LineCount { get; }
        void StandOnLine(Client client);
    }
}
