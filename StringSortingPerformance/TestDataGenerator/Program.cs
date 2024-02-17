using TestDataGenerator;

var generator = new Generator();
await generator.GenerateTestFile(@"D:\TestData",
    "original_text.txt",
    "test_data",
    1,
    100,
    new CancellationTokenSource().Token);