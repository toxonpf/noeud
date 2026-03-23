using CleaNoteMd.Domain.Models.Blocks;
using CleaNoteMd.Domain.Models.Inlines;
using Noeud.Infrastructure.Markdown_Parser;
using Xunit;

namespace Noeud.Tests;

public class MarkdigParserTests
{
    [Fact]
    public void Parse_BasicMarkdown_ReturnsCorrectDomainModels()
    {
        // 1. ARRANGE (Подготовка)
        var parser = new MarkdigParser();
        // Специально делаем строку с заголовком и параграфом, внутри которого есть жирный текст
        string markdownText = "# Привет\n\nЭто **важный** тест.";

        // 2. ACT (Действие)
        // Вызываем парсер и сразу превращаем результат в List, чтобы было удобно проверять по индексам
        var resultBlocks = parser.Parse(markdownText).ToList();

        // 3. ASSERT (Проверка)

        // Проверяем, что парсер нашел ровно 2 блока (Заголовок и Параграф)
        Assert.Equal(2, resultBlocks.Count);

        // --- Проверяем первый блок (Заголовок) ---
        // Assert.IsType проверяет, что объект имеет нужный класс, и возвращает его приведенным к этому классу
        var heading = Assert.IsType<MdHeading>(resultBlocks[0]);
        Assert.Equal(1, heading.Level); // Проверяем уровень (# = 1)
        Assert.Single(heading.Inlines); // Проверяем, что внутри ровно 1 кусок текста

        var headingText = Assert.IsType<MdPlainText>(heading.Inlines[0]);
        Assert.Equal("Привет", headingText.Text);

        // --- Проверяем второй блок (Параграф) ---
        var paragraph = Assert.IsType<MdParagraph>(resultBlocks[1]);

        // В строке "Это **важный** тест." должно быть 3 инлайна:
        // 1. "Это " (PlainText)
        // 2. "важный" (BoldText)
        // 3. " тест." (PlainText)
        Assert.Equal(3, paragraph.Inlines.Count);

        var firstText = Assert.IsType<MdPlainText>(paragraph.Inlines[0]);
        Assert.Equal("Это ", firstText.Text);

        var boldText = Assert.IsType<MdBoldText>(paragraph.Inlines[1]);
        Assert.Equal("важный", boldText.Text);

        var lastText = Assert.IsType<MdPlainText>(paragraph.Inlines[2]);
        Assert.Equal(" тест.", lastText.Text);
    }
}
