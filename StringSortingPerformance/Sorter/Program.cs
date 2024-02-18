using Sorter;

var sortProcessor = new SortProcessor();
sortProcessor.Sort(
    @"D:\TestData",
    "test_data_1Gb.txt",
    "result",
    new CancellationTokenSource().Token
);