namespace McDonaldsWorkflow.Models.Interfaces
{
    public interface ICashier: IEmployee
    {
        int Takings { get; }
        int LineCount { get; }
        void StandOnLine(Client client);
    }
}
