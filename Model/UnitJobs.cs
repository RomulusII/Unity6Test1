using System.ComponentModel.DataAnnotations;

namespace Model
{
    public class UnitJob
    {
        [Key]
        public int Id { get; set; }

        public JobType JobType { get; set; }
        public long StartTick { get; set; }

    }

    public enum JobScheduleType
    {
        OnceInQueue,
        Repeat,
        MostProfitableFirst
    }

    public class UnitJobs
    {
        [Key]
        public int Id { get; set; }

        //public List<JobType> JobTypes { get; set; } = new List<JobType>();
        public JobScheduleType JobScheduleType { get; set; }


    }
}
