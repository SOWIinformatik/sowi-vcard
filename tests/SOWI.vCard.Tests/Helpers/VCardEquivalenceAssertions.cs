// SOWI Informatik, www.sowi.ch
// Franz Schönbächler

using SOWI.vCard.Domain;
using SOWI.vCard.Domain.ValueObjects;
using Xunit;

namespace SOWI.vCard.Tests.Helpers
{
    /// <summary>
    /// Semantische Vergleichs-Assertions für Round-Trip-Tests (Parse → Serialize → Parse).
    /// </summary>
    internal static class VCardEquivalenceAssertions
    {
        internal static void AssertCoreContactDataEquivalent(VCard expected, VCard actual)
        {
            Assert.Equal(expected.Version, actual.Version);
            Assert.Equal(expected.FullName, actual.FullName);
            Assert.Equal(expected.Organization, actual.Organization);
            Assert.Equal(expected.Role, actual.Role);
            Assert.Equal(expected.Title, actual.Title);
            Assert.Equal(expected.Url, actual.Url);
            Assert.Equal(expected.Note, actual.Note);
            Assert.Equal(expected.Nickname, actual.Nickname);
            Assert.Equal(expected.Uid, actual.Uid);
            AssertPersonNameEquivalent(expected.PersonName, actual.PersonName);
            AssertAddressesEquivalent(expected.Addresses, actual.Addresses);
            AssertEmailsEquivalent(expected.Emails, actual.Emails);
            AssertPhoneNumbersEquivalent(expected.PhoneNumbers, actual.PhoneNumbers);
        }

        internal static void AssertReadmeExampleEquivalent(VCard expected, VCard actual)
        {
            AssertCoreContactDataEquivalent(expected, actual);

            if (expected.Photo != null)
            {
                Assert.NotNull(actual.Photo);
                Assert.Equal(expected.Photo!.Value, actual.Photo!.Value);
                Assert.Equal(expected.Photo.Type, actual.Photo.Type, ignoreCase: true);
            }

            if (expected.Revision.HasValue)
            {
                Assert.NotNull(actual.Revision);
                Assert.Equal(
                    expected.Revision.Value.ToUniversalTime(),
                    actual.Revision!.Value.ToUniversalTime());
            }
        }

        internal static void AssertExtensionsPreserved(VCard expected, VCard actual)
        {
            Assert.Equal(expected.Extensions.Count, actual.Extensions.Count);

            foreach (var extension in expected.Extensions.Values)
            {
                Assert.Contains(actual.Extensions.Values, value => value == extension);
            }
        }

        private static void AssertPersonNameEquivalent(PersonName expected, PersonName actual)
        {
            Assert.Equal(expected.LastName, actual.LastName);
            Assert.Equal(expected.FirstName, actual.FirstName);
            Assert.Equal(expected.MiddleName, actual.MiddleName);
            Assert.Equal(expected.Prefix, actual.Prefix);
            Assert.Equal(expected.Suffix, actual.Suffix);
        }

        private static void AssertAddressesEquivalent(IReadOnlyList<Address> expected, IReadOnlyList<Address> actual)
        {
            Assert.Equal(expected.Count, actual.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Type, actual[i].Type, ignoreCase: true);
                Assert.Equal(expected[i].Street, actual[i].Street);
                Assert.Equal(expected[i].Locality, actual[i].Locality);
                Assert.Equal(expected[i].Region, actual[i].Region);
                Assert.Equal(expected[i].PostalCode, actual[i].PostalCode);
                Assert.Equal(expected[i].Country, actual[i].Country);
                Assert.Equal(expected[i].Label, actual[i].Label);
            }
        }

        private static void AssertEmailsEquivalent(IReadOnlyList<Email> expected, IReadOnlyList<Email> actual)
        {
            Assert.Equal(expected.Count, actual.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(expected[i].Address, actual[i].Address);
                Assert.Equal(expected[i].Type, actual[i].Type, ignoreCase: true);
            }
        }

        private static void AssertPhoneNumbersEquivalent(IReadOnlyList<PhoneNumber> expected, IReadOnlyList<PhoneNumber> actual)
        {
            Assert.Equal(expected.Count, actual.Count);

            for (var i = 0; i < expected.Count; i++)
            {
                Assert.Equal(NormalizePhone(expected[i].Number), NormalizePhone(actual[i].Number));
                Assert.Equal(expected[i].Type, actual[i].Type, ignoreCase: true);
            }
        }

        private static string NormalizePhone(string number)
        {
            return new string(number.Where(char.IsDigit).ToArray());
        }
    }
}
