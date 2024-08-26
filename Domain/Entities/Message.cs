namespace Koshelekpy_Test.Domain.Entities
{
    public class Message
    {
        public Message(int id, string text, DateTime timestamp, int sequenceNumber)
        {
            Id = id;
            Text = text;
            Timestamp = timestamp;
            SequenceNumber = sequenceNumber;
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
        public int SequenceNumber { get; set; }

        public override string ToString() => $"Message Id:{Id}, text: {Text}, send time: {Timestamp}, Sequence number: {SequenceNumber}";
    }

}
