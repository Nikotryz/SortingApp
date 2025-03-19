using SortingApp;

namespace SortingAppTests
{
    public class ProgramUnitTests
    {
        [Fact]
        public void Test_ReadParticipants_SimpleFile()
        {
            // Подготовка данных для теста
            var testData = @"Иванов Иван Иванович:10,2
                             Харламов Никита Владимирович:10,2";

            // Создание временного файла с тестовыми данными
            var tempFilePath = Path.GetTempFileName();
            File.WriteAllText(tempFilePath, testData);

            // Вызов тестируемого метода
            var actualResult = Program.ReadParticipants(tempFilePath);

            // Проверка результата
            Assert.NotNull(actualResult);
            Assert.Equal(2, actualResult.Count);
            Assert.True(actualResult.Exists(x => x.Name == "Иванов Иван Иванович" && x.Time == 10.2m));
            Assert.True(actualResult.Exists(x => x.Name == "Харламов Никита Владимирович" && x.Time == 10.2m));

            // Удаление временного файла после завершения теста
            File.Delete(tempFilePath);
        }

        [Theory]
        [InlineData(
            new[] { "Иванов Иван Иванович:10,2", "Харламов Никита Владимирович:10,2" },
            new[] { "Иванов Иван Иванович", "Харламов Никита Владимирович" },
            new[] { 1, 1 }
        )]
        [InlineData(
            new[] { "Иванов Иван Иванович:10,2", "Харламов Никита Владимирович:12,3" },
            new[] { "Иванов Иван Иванович", "Харламов Никита Владимирович" },
            new[] { 1, 2 }
        )]
        public void Test_SortParticipants_MultipleCases(string[] inputLines, string[] expectedNames, int[] expectedPlaces)
        {
            // Преобразование входных данных в список участников
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

            // Вызов тестируемого метода
            var actualResult = Program.SortParticipants(participants);

            // Проверка результата
            for (int i = 0; i < expectedNames.Length; i++)
            {
                Assert.Equal(expectedNames[i], actualResult[i].Name);
                Assert.Equal(expectedPlaces[i], actualResult[i].Place);
            }
        }
    }
}