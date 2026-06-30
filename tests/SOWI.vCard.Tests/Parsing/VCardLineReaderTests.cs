// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Abstractions;
using SOWI.vCard.Domain.Exceptions;
using SOWI.vCard.Parsing;

namespace SOWI.vCard.Tests.Parsing
{
    /// <summary>
    /// Tests für <see cref="VCardLineReader"/> (Line Folding, Escaping).
    /// </summary>
    public class VCardLineReaderTests
    {
        private readonly IVCardLineReader _reader = new VCardLineReader();

        [Fact]
        public void ReadLines_UnfoldsContinuationLine()
        {
            const string input = "NOTE:Zeile eins\r\n Zeile zwei";

            var lines = _reader.ReadLines(input);

            Assert.Single(lines);
            Assert.Equal("NOTE:Zeile einsZeile zwei", lines[0]);
        }

        [Fact]
        public void ReadLines_UnescapesNewline()
        {
            const string input = @"NOTE:Zeile eins\nZeile zwei";

            var lines = _reader.ReadLines(input);

            Assert.Single(lines);
            Assert.Equal("NOTE:Zeile eins\nZeile zwei", lines[0]);
        }

        [Fact]
        public void ReadLines_UnescapesSemicolonAndComma()
        {
            const string input = @"ORG:Firma\; GmbH\, Zürich";

            var lines = _reader.ReadLines(input);

            Assert.Single(lines);
            Assert.Equal("ORG:Firma; GmbH, Zürich", lines[0]);
        }
    }
}
