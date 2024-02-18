using Sorter;

var sortProcessor = new SortProcessor();
sortProcessor.Sort(
    @"C:\TestData",
    "test_data_1Gb.txt",
    "result",
    new CancellationTokenSource().Token
);