namespace CommandsService.Dtos
{
    public class CommandReadDto
    {
        public long Id { get; set; }
        public string HowTo { get; set; }
        public string CommandLine { get; set; }
        public long Platform_Id { get; set; }
    }
}