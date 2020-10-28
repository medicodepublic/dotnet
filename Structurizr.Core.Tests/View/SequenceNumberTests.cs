using Xunit;

namespace Structurizr.Core.Tests
{
    public class SequenceNumberTests
    {
        [Fact]
        public void Test_Increment()
        {
            var sequenceNumber = new SequenceNumber();
            Assert.Equal("1", sequenceNumber.GetNext());
            Assert.Equal("2", sequenceNumber.GetNext());
        }

        [Fact]
        public void Test_ParallelSequences()
        {
            var sequenceNumber = new SequenceNumber();
            Assert.Equal("1", sequenceNumber.GetNext());

            sequenceNumber.StartParallelSequence();
            Assert.Equal("2", sequenceNumber.GetNext());
            sequenceNumber.EndParallelSequence(false);

            sequenceNumber.StartParallelSequence();
            Assert.Equal("2", sequenceNumber.GetNext());
            sequenceNumber.EndParallelSequence(true);

            Assert.Equal("3", sequenceNumber.GetNext());
        }
    }
}