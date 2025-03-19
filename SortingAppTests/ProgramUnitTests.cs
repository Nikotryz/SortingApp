using SortingApp;

namespace SortingAppTests
{
    public class ProgramUnitTests
    {
        [Fact]
        public void Test_ReadParticipants_SimpleFile()
        {
            // ���������� ������ ��� �����
            var testData = @"������ ���� ��������:10,2
                             �������� ������ ������������:10,2";

            // �������� ���������� ����� � ��������� �������
            var tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, testData);

            // ����� ������������ ������
            var actualResult = Program.ReadParticipants(tempFilePath);

            // �������� ����������
            Assert.NotNull(actualResult);
            Assert.Equal(2, actualResult.Count);
            Assert.True(actualResult.Exists(x => x.Name == "������ ���� ��������" && x.Time == 10.2m));
            Assert.True(actualResult.Exists(x => x.Name == "�������� ������ ������������" && x.Time == 10.2m));

            // �������� ���������� ����� ����� ���������� �����
            File.Delete(tempFilePath);
        }

        [Theory]
        [InlineData(
            new[] { "������ ���� ��������:10,2", "�������� ������ ������������:10,2" },
            new[] { "������ ���� ��������", "�������� ������ ������������" },
            new[] { 1, 1 }
        )]
        [InlineData(
            new[] { "������ ���� ��������:10,2", "�������� ������ ������������:12,3" },
            new[] { "������ ���� ��������", "�������� ������ ������������" },
            new[] { 1, 2 }
        )]
        public void Test_SortParticipants_MultipleCases(string[] inputLines, string[] expectedNames, int[] expectedPlaces)
        {
            // �������������� ������� ������ � ������ ����������
            var participants = new List<Participant>();
            for (int i = 0; i < inputLines.Length; i++)
            {
                var parts = inputLines[i].Split(':');
                participants.Add(new Participant
                {
                    Name = parts[0],
                    Time = decimal.Parse(parts[1]),
                    Place = expectedPlaces[i]
                });
            }

            // ����� ������������ ������
            var actualResult = Program.SortParticipants(participants);

            // �������� ����������
            for (int i = 0; i < expectedNames.Length; i++)
            {
                Assert.Equal(expectedNames[i], actualResult[i].Name);
                Assert.Equal(expectedPlaces[i], actualResult[i].Place);
            }
        }
    }
}