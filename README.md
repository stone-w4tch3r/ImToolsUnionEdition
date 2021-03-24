# ImTools

[![license](https://img.shields.io/github/license/dadhi/ImTools.svg)](http://opensource.org/licenses/MIT)

- Windows, Linux, MacOS [![Windows build](https://ci.appveyor.com/api/projects/status/el9echuqfnl86u53?svg=true)](https://ci.appveyor.com/project/MaksimVolkau/imtools/branch/master)

- Lib package [![NuGet Badge](https://buildstats.info/nuget/ImTools.dll)](https://www.nuget.org/packages/ImTools.dll)
- Code package [![NuGet Badge](https://buildstats.info/nuget/ImTools)](https://www.nuget.org/packages/ImTools)

Fast and memory-efficient immutable collections and helper data structures.

Split from the [DryIoc](https://github.com/dadhi/dryioc).


## Benchmarks

The comparison is done against the `ImMap` V1 version, the V2 `ImMapSlots` bucketed version 
and a variety of BCL C# collections including the experimental `Microsoft.Collections.Extensions.DictionarySlim<K, V>`.

__Note:__ Keep in mind that immutable collections have a different use-case and a thread-safety guarantees compared to the 
`Dictionary`, `DictionarySlim` or even `ConcurrentDictionary`. The closest comparable would be the `ImmutableDictionary`. 
The benchmarks do not take the collection "nature" into account and run though a simplest available API path.

*Benchmark environment*:

```
BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=3.1.100
  [Host]     : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
  DefaultJob : .NET Core 3.1.0 (CoreCLR 4.700.19.56402, CoreFX 4.700.19.56404), X64 RyuJIT
```


### ImMap V2 with small string values

`ImMap<string>` stores the `int` keys and `string` values.


#### ImMap Population


[The benchmark](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImMapBenchmarks.cs) inserts from 1 to 10 000 of items into the `ImMap<string>`:

```md
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
Intel Core i9-8950HK CPU 2.90GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
.NET Core SDK=5.0.201
  [Host]     : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT
  DefaultJob : .NET Core 5.0.4 (CoreCLR 5.0.421.11614, CoreFX 5.0.421.11614), X64 RyuJIT

|                          Method | Count |             Mean |          Error |         StdDev |           Median | Ratio | RatioSD |     Gen 0 |    Gen 1 |    Gen 2 | Allocated |
|-------------------------------- |------ |-----------------:|---------------:|---------------:|-----------------:|------:|--------:|----------:|---------:|---------:|----------:|
|            V2_ImMap_AddOrUpdate |     1 |         15.14 ns |       0.464 ns |       1.323 ns |         14.84 ns |  1.00 |    0.00 |    0.0076 |        - |        - |      48 B |
|            V3_ImMap_AddOrUpdate |     1 |         11.02 ns |       0.299 ns |       0.743 ns |         10.87 ns |  0.74 |    0.07 |    0.0051 |        - |        - |      32 B |
| V3_PartitionedImMap_AddOrUpdate |     1 |        122.88 ns |       2.735 ns |       7.977 ns |        120.19 ns |  8.18 |    0.79 |    0.0496 |        - |        - |     312 B |
|       DictSlim_GetOrAddValueRef |     1 |         46.49 ns |       0.967 ns |       2.039 ns |         45.92 ns |  3.09 |    0.28 |    0.0204 |        - |        - |     128 B |
|                     Dict_TryAdd |     1 |         41.11 ns |       0.897 ns |       1.422 ns |         41.05 ns |  2.76 |    0.26 |    0.0344 |        - |        - |     216 B |
|           ConcurrentDict_TryAdd |     1 |        158.54 ns |       3.225 ns |       4.414 ns |        158.96 ns | 10.65 |    1.00 |    0.1376 |   0.0007 |        - |     864 B |
|       ImmutableDict_Builder_Add |     1 |        130.92 ns |       1.535 ns |       1.360 ns |        130.64 ns |  8.63 |    0.91 |    0.0253 |        - |        - |     160 B |
|               ImmutableDict_Add |     1 |        130.73 ns |       3.102 ns |       9.049 ns |        132.08 ns |  8.69 |    0.91 |    0.0165 |        - |        - |     104 B |
|                                 |       |                  |                |                |                  |       |         |           |          |          |           |
|            V2_ImMap_AddOrUpdate |    10 |        647.41 ns |      24.273 ns |      70.807 ns |        673.87 ns |  1.00 |    0.00 |    0.2823 |        - |        - |    1776 B |
|            V3_ImMap_AddOrUpdate |    10 |        242.78 ns |       4.904 ns |      11.069 ns |        240.92 ns |  0.36 |    0.03 |    0.1197 |        - |        - |     752 B |
| V3_PartitionedImMap_AddOrUpdate |    10 |        280.77 ns |       5.575 ns |       6.636 ns |        281.50 ns |  0.41 |    0.04 |    0.0954 |        - |        - |     600 B |
|       DictSlim_GetOrAddValueRef |    10 |        289.36 ns |       5.836 ns |      11.655 ns |        288.89 ns |  0.43 |    0.04 |    0.1326 |        - |        - |     832 B |
|                     Dict_TryAdd |    10 |        291.70 ns |       5.892 ns |      11.630 ns |        289.94 ns |  0.43 |    0.04 |    0.1578 |        - |        - |     992 B |
|           ConcurrentDict_TryAdd |    10 |        564.35 ns |      11.281 ns |      15.060 ns |        564.57 ns |  0.81 |    0.07 |    0.1945 |   0.0010 |        - |    1224 B |
|       ImmutableDict_Builder_Add |    10 |      1,612.84 ns |      30.964 ns |      35.658 ns |      1,617.42 ns |  2.33 |    0.20 |    0.1163 |        - |        - |     736 B |
|               ImmutableDict_Add |    10 |      2,715.60 ns |      41.873 ns |      32.692 ns |      2,726.71 ns |  3.94 |    0.10 |    0.4196 |        - |        - |    2640 B |
|                                 |       |                  |                |                |                  |       |         |           |          |          |           |
|            V2_ImMap_AddOrUpdate |   100 |     12,293.32 ns |     229.257 ns |     424.943 ns |     12,360.83 ns |  1.00 |    0.00 |    5.9357 |   0.2441 |        - |   37296 B |
|            V3_ImMap_AddOrUpdate |   100 |      8,818.77 ns |     175.424 ns |     373.843 ns |      8,791.68 ns |  0.72 |    0.04 |    3.4027 |   0.1373 |        - |   21352 B |
| V3_PartitionedImMap_AddOrUpdate |   100 |      3,234.23 ns |      64.352 ns |     117.671 ns |      3,237.60 ns |  0.26 |    0.01 |    1.4725 |   0.0839 |        - |    9240 B |
|       DictSlim_GetOrAddValueRef |   100 |      2,520.48 ns |      36.230 ns |      30.254 ns |      2,527.72 ns |  0.21 |    0.01 |    1.3275 |   0.0534 |        - |    8336 B |
|                     Dict_TryAdd |   100 |      3,038.78 ns |      60.333 ns |      86.528 ns |      3,053.22 ns |  0.25 |    0.01 |    2.0828 |   0.1335 |        - |   13072 B |
|           ConcurrentDict_TryAdd |   100 |     10,914.74 ns |     217.568 ns |     267.193 ns |     10,980.10 ns |  0.90 |    0.04 |    3.6316 |   0.3510 |        - |   22784 B |
|       ImmutableDict_Builder_Add |   100 |     25,755.21 ns |     510.198 ns |   1,007.081 ns |     25,798.85 ns |  2.09 |    0.11 |    1.4648 |   0.0916 |        - |    9376 B |
|               ImmutableDict_Add |   100 |     48,151.62 ns |     950.427 ns |   1,451.403 ns |     48,550.19 ns |  3.95 |    0.22 |    7.9346 |   0.3662 |        - |   49952 B |
|                                 |       |                  |                |                |                  |       |         |           |          |          |           |
|            V2_ImMap_AddOrUpdate |  1000 |    202,184.79 ns |   4,030.081 ns |   6,032.035 ns |    201,787.26 ns |  1.00 |    0.00 |   84.9609 |   0.4883 |        - |  534144 B |
|            V3_ImMap_AddOrUpdate |  1000 |    171,126.16 ns |   3,405.240 ns |   4,883.694 ns |    171,917.09 ns |  0.85 |    0.04 |   58.3496 |   0.4883 |        - |  366496 B |
| V3_PartitionedImMap_AddOrUpdate |  1000 |     95,031.20 ns |   1,826.035 ns |   1,793.410 ns |     95,489.40 ns |  0.48 |    0.02 |   35.8887 |  11.9629 |        - |  225496 B |
|       DictSlim_GetOrAddValueRef |  1000 |     25,387.70 ns |     492.313 ns |     971.778 ns |     25,388.93 ns |  0.12 |    0.01 |   11.6272 |   2.8992 |        - |   73120 B |
|                     Dict_TryAdd |  1000 |     31,939.82 ns |     636.682 ns |   1,148.068 ns |     31,787.71 ns |  0.16 |    0.01 |   21.2402 |   0.0610 |        - |  133896 B |
|           ConcurrentDict_TryAdd |  1000 |    114,283.00 ns |   1,884.712 ns |   2,579.815 ns |    113,859.86 ns |  0.57 |    0.02 |   32.7148 |   0.1221 |        - |  205368 B |
|       ImmutableDict_Builder_Add |  1000 |    329,232.01 ns |   6,559.466 ns |   8,529.159 ns |    326,383.94 ns |  1.63 |    0.07 |   15.1367 |   0.4883 |        - |   95776 B |
|               ImmutableDict_Add |  1000 |    725,486.29 ns |  14,467.219 ns |  19,313.322 ns |    726,475.10 ns |  3.59 |    0.13 |  112.3047 |   0.9766 |        - |  710208 B |
|                                 |       |                  |                |                |                  |       |         |           |          |          |           |
|            V2_ImMap_AddOrUpdate | 10000 |  4,426,278.12 ns |  87,006.155 ns |  77,128.690 ns |  4,404,791.02 ns |  1.00 |    0.00 | 1109.3750 | 226.5625 | 101.5625 | 6972672 B |
|            V3_ImMap_AddOrUpdate | 10000 |  4,423,388.25 ns |  83,777.390 ns | 122,799.812 ns |  4,381,706.25 ns |  1.00 |    0.03 |  835.9375 | 328.1250 | 148.4375 | 5247424 B |
| V3_PartitionedImMap_AddOrUpdate | 10000 |  3,739,595.26 ns |  73,002.219 ns |  92,324.433 ns |  3,718,667.58 ns |  0.85 |    0.03 |  613.2813 | 265.6250 |  70.3125 | 3856344 B |
|       DictSlim_GetOrAddValueRef | 10000 |    407,978.45 ns |   7,661.713 ns |  13,815.649 ns |    406,788.38 ns |  0.09 |    0.00 |  124.5117 | 124.5117 | 124.5117 |  975712 B |
|                     Dict_TryAdd | 10000 |    536,228.64 ns |  10,556.052 ns |  21,081.577 ns |    535,227.64 ns |  0.12 |    0.01 |  221.6797 | 221.6797 | 221.6797 | 1261688 B |
|           ConcurrentDict_TryAdd | 10000 |  2,860,109.91 ns |  28,230.649 ns |  25,025.735 ns |  2,868,305.66 ns |  0.65 |    0.01 |  273.4375 | 121.0938 |  42.9688 | 1645302 B |
|       ImmutableDict_Builder_Add | 10000 |  4,656,480.71 ns |  71,840.690 ns |  59,990.230 ns |  4,643,750.78 ns |  1.05 |    0.02 |  148.4375 |  70.3125 |        - |  959776 B |
|               ImmutableDict_Add | 10000 | 11,641,070.08 ns | 207,715.718 ns | 213,308.750 ns | 11,612,375.78 ns |  2.63 |    0.08 | 1468.7500 | 265.6250 | 125.0000 | 9271168 B |
```


### ImMap Lookup

[The benchmark](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImMapBenchmarks.cs) lookups for the last added index in the `ImMap<string>` 
containing the specified Count of elements.

```md
|                      Method | Count |       Mean |     Error |    StdDev |     Median | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|---------------------------- |------ |-----------:|----------:|----------:|-----------:|------:|--------:|------:|------:|------:|----------:|
|            V2_ImMap_TryFind |     1 |  0.6303 ns | 0.0454 ns | 0.0425 ns |  0.6381 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|            V3_ImMap_TryFind |     1 |  0.9290 ns | 0.0439 ns | 0.0411 ns |  0.9284 ns |  1.48 |    0.12 |     - |     - |     - |         - |
| V3_PartitionedImMap_TryFind |     1 |  1.0952 ns | 0.0355 ns | 0.0332 ns |  1.0951 ns |  1.74 |    0.12 |     - |     - |     - |         - |
|        DictSlim_TryGetValue |     1 |  3.9227 ns | 0.1003 ns | 0.0938 ns |  3.9245 ns |  6.25 |    0.42 |     - |     - |     - |         - |
|            Dict_TryGetValue |     1 |  7.2612 ns | 0.1122 ns | 0.1050 ns |  7.2657 ns | 11.57 |    0.76 |     - |     - |     - |         - |
|  ConcurrentDict_TryGetValue |     1 |  8.1538 ns | 0.1168 ns | 0.1035 ns |  8.1563 ns | 13.01 |    0.93 |     - |     - |     - |         - |
|   ImmutableDict_TryGetValue |     1 | 15.6600 ns | 0.1452 ns | 0.1287 ns | 15.6729 ns | 24.98 |    1.75 |     - |     - |     - |         - |
|                             |       |            |           |           |            |       |         |       |       |       |           |
|            V2_ImMap_TryFind |    10 |  3.2915 ns | 0.0955 ns | 0.0847 ns |  3.2810 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|            V3_ImMap_TryFind |    10 |  3.1971 ns | 0.0764 ns | 0.0677 ns |  3.1851 ns |  0.97 |    0.04 |     - |     - |     - |         - |
| V3_PartitionedImMap_TryFind |    10 |  1.1737 ns | 0.0437 ns | 0.0387 ns |  1.1668 ns |  0.36 |    0.01 |     - |     - |     - |         - |
|        DictSlim_TryGetValue |    10 |  3.9646 ns | 0.0951 ns | 0.1057 ns |  3.9467 ns |  1.21 |    0.04 |     - |     - |     - |         - |
|            Dict_TryGetValue |    10 |  7.1259 ns | 0.1011 ns | 0.0946 ns |  7.1384 ns |  2.16 |    0.06 |     - |     - |     - |         - |
|  ConcurrentDict_TryGetValue |    10 |  7.9278 ns | 0.1065 ns | 0.0944 ns |  7.9291 ns |  2.41 |    0.06 |     - |     - |     - |         - |
|   ImmutableDict_TryGetValue |    10 | 18.0258 ns | 0.2642 ns | 0.2472 ns | 17.9529 ns |  5.49 |    0.17 |     - |     - |     - |         - |
|                             |       |            |           |           |            |       |         |       |       |       |           |
|            V2_ImMap_TryFind |   100 |  5.2554 ns | 0.3458 ns | 0.9109 ns |  4.6748 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|            V3_ImMap_TryFind |   100 |  5.6595 ns | 0.1128 ns | 0.1055 ns |  5.6377 ns |  1.07 |    0.19 |     - |     - |     - |         - |
| V3_PartitionedImMap_TryFind |   100 |  2.5605 ns | 0.0626 ns | 0.0523 ns |  2.5697 ns |  0.50 |    0.08 |     - |     - |     - |         - |
|        DictSlim_TryGetValue |   100 |  4.3392 ns | 0.0588 ns | 0.0521 ns |  4.3430 ns |  0.83 |    0.14 |     - |     - |     - |         - |
|            Dict_TryGetValue |   100 |  6.6163 ns | 0.0773 ns | 0.0604 ns |  6.6331 ns |  1.30 |    0.22 |     - |     - |     - |         - |
|  ConcurrentDict_TryGetValue |   100 |  7.5732 ns | 0.0827 ns | 0.0733 ns |  7.5850 ns |  1.45 |    0.24 |     - |     - |     - |         - |
|   ImmutableDict_TryGetValue |   100 | 20.0410 ns | 0.3129 ns | 0.2927 ns | 19.9944 ns |  3.80 |    0.67 |     - |     - |     - |         - |
|                             |       |            |           |           |            |       |         |       |       |       |           |
|            V2_ImMap_TryFind |  1000 |  6.9524 ns | 0.1210 ns | 0.1073 ns |  6.9368 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|            V3_ImMap_TryFind |  1000 | 11.1095 ns | 0.1589 ns | 0.1487 ns | 11.0952 ns |  1.60 |    0.03 |     - |     - |     - |         - |
| V3_PartitionedImMap_TryFind |  1000 |  5.5250 ns | 0.1084 ns | 0.1014 ns |  5.5125 ns |  0.79 |    0.01 |     - |     - |     - |         - |
|        DictSlim_TryGetValue |  1000 |  4.3399 ns | 0.0754 ns | 0.0669 ns |  4.3214 ns |  0.62 |    0.01 |     - |     - |     - |         - |
|            Dict_TryGetValue |  1000 |  6.7385 ns | 0.0783 ns | 0.0612 ns |  6.7491 ns |  0.97 |    0.02 |     - |     - |     - |         - |
|  ConcurrentDict_TryGetValue |  1000 |  8.0877 ns | 0.0798 ns | 0.0707 ns |  8.0794 ns |  1.16 |    0.03 |     - |     - |     - |         - |
|   ImmutableDict_TryGetValue |  1000 | 23.2900 ns | 0.2741 ns | 0.2429 ns | 23.3068 ns |  3.35 |    0.06 |     - |     - |     - |         - |
|                             |       |            |           |           |            |       |         |       |       |       |           |
|            V2_ImMap_TryFind | 10000 | 11.8258 ns | 0.2418 ns | 0.4773 ns | 11.7575 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|            V3_ImMap_TryFind | 10000 | 15.9824 ns | 0.2152 ns | 0.1797 ns | 15.9901 ns |  1.36 |    0.02 |     - |     - |     - |         - |
| V3_PartitionedImMap_TryFind | 10000 | 10.2560 ns | 0.0970 ns | 0.0907 ns | 10.2246 ns |  0.87 |    0.01 |     - |     - |     - |         - |
|        DictSlim_TryGetValue | 10000 |  4.2128 ns | 0.0493 ns | 0.0412 ns |  4.2186 ns |  0.36 |    0.01 |     - |     - |     - |         - |
|            Dict_TryGetValue | 10000 |  6.4634 ns | 0.0628 ns | 0.0524 ns |  6.4662 ns |  0.55 |    0.01 |     - |     - |     - |         - |
|  ConcurrentDict_TryGetValue | 10000 |  7.4370 ns | 0.0582 ns | 0.0486 ns |  7.4572 ns |  0.63 |    0.01 |     - |     - |     - |         - |
|   ImmutableDict_TryGetValue | 10000 | 29.5492 ns | 0.6021 ns | 0.5632 ns | 29.3975 ns |  2.52 |    0.07 |     - |     - |     - |         - |
```

**Interpreting results:** `ImMap` holds very good against `ImmutableDictionary` sibling and even against `Dictionary`(s) up to certain count, 
indicating that immutable collection could be quite fast for lookups.

### ImMap Enumeration

[The benchmark source code](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImMapBenchmarks.cs)

```md
|                              Method | Count |            Mean |        Error |       StdDev | Ratio | RatioSD |   Gen 0 |   Gen 1 |   Gen 2 | Allocated |
|------------------------------------ |------ |----------------:|-------------:|-------------:|------:|--------:|--------:|--------:|--------:|----------:|
|              ImMap_EnumerateToArray |     1 |       111.10 ns |     0.490 ns |     0.435 ns |  1.00 |    0.00 |  0.0340 |       - |       - |     160 B |
|           ImMap_V1_EnumerateToArray |     1 |       120.70 ns |     0.436 ns |     0.386 ns |  1.09 |    0.01 |  0.0391 |       - |       - |     184 B |
|                   ImMap_FoldToArray |     1 |        35.21 ns |     0.227 ns |     0.212 ns |  0.32 |    0.00 |  0.0255 |       - |       - |     120 B |
| ImMap_FoldToArray_FoldReducerStruct |     1 |        35.07 ns |     0.125 ns |     0.111 ns |  0.32 |    0.00 |  0.0255 |       - |       - |     120 B |
|      Experimental_ImMap_FoldToArray |     1 |        41.52 ns |     0.322 ns |     0.301 ns |  0.37 |    0.00 |  0.0255 |       - |       - |     120 B |
|              ImMapSlots_FoldToArray |     1 |        57.45 ns |     0.269 ns |     0.251 ns |  0.52 |    0.00 |  0.0254 |       - |       - |     120 B |
| Experimental_ImMapSlots_FoldToArray |     1 |       143.24 ns |     0.685 ns |     0.641 ns |  1.29 |    0.01 |  0.0253 |       - |       - |     120 B |
|                    DictSlim_ToArray |     1 |       113.84 ns |     0.330 ns |     0.292 ns |  1.02 |    0.00 |  0.0373 |       - |       - |     176 B |
|                        Dict_ToArray |     1 |        31.78 ns |     0.260 ns |     0.230 ns |  0.29 |    0.00 |  0.0085 |       - |       - |      40 B |
|              ConcurrentDict_ToArray |     1 |       228.83 ns |     0.857 ns |     0.802 ns |  2.06 |    0.01 |  0.0083 |       - |       - |      40 B |
|               ImmutableDict_ToArray |     1 |       455.85 ns |     1.836 ns |     1.717 ns |  4.10 |    0.02 |  0.0081 |       - |       - |      40 B |
|                                     |       |                 |              |              |       |         |         |         |         |           |
|              ImMap_EnumerateToArray |    10 |       287.93 ns |     1.325 ns |     1.240 ns |  1.00 |    0.00 |  0.0949 |       - |       - |     448 B |
|           ImMap_V1_EnumerateToArray |    10 |       369.19 ns |     2.343 ns |     2.077 ns |  1.28 |    0.01 |  0.0968 |       - |       - |     456 B |
|                   ImMap_FoldToArray |    10 |       170.22 ns |     0.980 ns |     0.917 ns |  0.59 |    0.00 |  0.1001 |       - |       - |     472 B |
| ImMap_FoldToArray_FoldReducerStruct |    10 |       150.25 ns |     1.112 ns |     1.041 ns |  0.52 |    0.00 |  0.1001 |       - |       - |     472 B |
|      Experimental_ImMap_FoldToArray |    10 |       176.73 ns |     0.693 ns |     0.648 ns |  0.61 |    0.00 |  0.1001 |       - |       - |     472 B |
|              ImMapSlots_FoldToArray |    10 |       167.37 ns |     1.568 ns |     1.390 ns |  0.58 |    0.01 |  0.0918 |       - |       - |     432 B |
| Experimental_ImMapSlots_FoldToArray |    10 |       249.96 ns |     1.312 ns |     1.227 ns |  0.87 |    0.01 |  0.0916 |       - |       - |     432 B |
|                    DictSlim_ToArray |    10 |       340.28 ns |     1.950 ns |     1.824 ns |  1.18 |    0.01 |  0.1326 |       - |       - |     624 B |
|                        Dict_ToArray |    10 |        65.21 ns |     0.368 ns |     0.326 ns |  0.23 |    0.00 |  0.0391 |       - |       - |     184 B |
|              ConcurrentDict_ToArray |    10 |       257.08 ns |     1.457 ns |     1.138 ns |  0.89 |    0.01 |  0.0391 |       - |       - |     184 B |
|               ImmutableDict_ToArray |    10 |     1,799.44 ns |     3.894 ns |     3.251 ns |  6.25 |    0.03 |  0.0381 |       - |       - |     184 B |
|                                     |       |                 |              |              |       |         |         |         |         |           |
|              ImMap_EnumerateToArray |   100 |     1,620.80 ns |     5.929 ns |     5.546 ns |  1.00 |    0.00 |  0.4692 |  0.0019 |       - |    2216 B |
|           ImMap_V1_EnumerateToArray |   100 |     2,316.30 ns |     7.182 ns |     6.367 ns |  1.43 |    0.01 |  0.4692 |       - |       - |    2224 B |
|                   ImMap_FoldToArray |   100 |     1,080.23 ns |     3.741 ns |     2.921 ns |  0.67 |    0.00 |  0.6542 |  0.0038 |       - |    3080 B |
| ImMap_FoldToArray_FoldReducerStruct |   100 |       823.73 ns |     3.283 ns |     2.910 ns |  0.51 |    0.00 |  0.6542 |  0.0048 |       - |    3080 B |
|      Experimental_ImMap_FoldToArray |   100 |     1,055.48 ns |     4.004 ns |     3.745 ns |  0.65 |    0.00 |  0.6542 |  0.0038 |       - |    3080 B |
|              ImMapSlots_FoldToArray |   100 |       907.18 ns |     4.936 ns |     4.617 ns |  0.56 |    0.00 |  0.6475 |  0.0048 |       - |    3048 B |
| Experimental_ImMapSlots_FoldToArray |   100 |     1,202.60 ns |     4.109 ns |     3.643 ns |  0.74 |    0.00 |  0.6466 |  0.0038 |       - |    3048 B |
|                    DictSlim_ToArray |   100 |     2,011.05 ns |     8.163 ns |     7.636 ns |  1.24 |    0.01 |  0.8430 |  0.0076 |       - |    3984 B |
|                        Dict_ToArray |   100 |       398.15 ns |     2.618 ns |     2.449 ns |  0.25 |    0.00 |  0.3448 |  0.0019 |       - |    1624 B |
|              ConcurrentDict_ToArray |   100 |     2,065.41 ns |    14.971 ns |    14.004 ns |  1.27 |    0.01 |  0.3433 |       - |       - |    1624 B |
|               ImmutableDict_ToArray |   100 |    15,537.86 ns |    85.670 ns |    80.135 ns |  9.59 |    0.06 |  0.3357 |       - |       - |    1624 B |
|                                     |       |                 |              |              |       |         |         |         |         |           |
|              ImMap_EnumerateToArray |  1000 |    15,054.50 ns |    74.727 ns |    69.900 ns |  1.00 |    0.00 |  3.5553 |  0.1221 |       - |   16768 B |
|           ImMap_V1_EnumerateToArray |  1000 |    22,248.00 ns |    64.405 ns |    53.781 ns |  1.48 |    0.01 |  3.5400 |  0.1526 |       - |   16776 B |
|                   ImMap_FoldToArray |  1000 |    10,051.62 ns |    64.300 ns |    60.146 ns |  0.67 |    0.01 |  5.2490 |  0.2899 |       - |   24712 B |
| ImMap_FoldToArray_FoldReducerStruct |  1000 |     7,334.16 ns |    21.443 ns |    20.058 ns |  0.49 |    0.00 |  5.2490 |  0.2899 |       - |   24712 B |
|      Experimental_ImMap_FoldToArray |  1000 |     9,822.09 ns |    68.825 ns |    64.379 ns |  0.65 |    0.01 |  5.2490 |  0.2899 |       - |   24712 B |
|              ImMapSlots_FoldToArray |  1000 |     8,949.39 ns |    58.822 ns |    55.022 ns |  0.59 |    0.00 |  5.2338 |  0.3052 |       - |   24680 B |
| Experimental_ImMapSlots_FoldToArray |  1000 |    10,529.47 ns |    27.764 ns |    25.970 ns |  0.70 |    0.00 |  5.2338 |  0.3052 |       - |   24680 B |
|                    DictSlim_ToArray |  1000 |    16,649.68 ns |    63.599 ns |    59.491 ns |  1.11 |    0.01 |  6.9580 |  0.6104 |       - |   32880 B |
|                        Dict_ToArray |  1000 |     3,624.89 ns |    22.328 ns |    19.793 ns |  0.24 |    0.00 |  3.3875 |  0.1869 |       - |   16024 B |
|              ConcurrentDict_ToArray |  1000 |    16,913.26 ns |    75.327 ns |    70.461 ns |  1.12 |    0.01 |  3.3875 |  0.1831 |       - |   16024 B |
|               ImmutableDict_ToArray |  1000 |   152,451.63 ns |   509.580 ns |   476.662 ns | 10.13 |    0.05 |  3.1738 |       - |       - |   16024 B |
|                                     |       |                 |              |              |       |         |         |         |         |           |
|              ImMap_EnumerateToArray | 10000 |   158,659.55 ns |   492.752 ns |   384.709 ns |  1.00 |    0.00 | 44.6777 | 14.8926 |       - |  211928 B |
|           ImMap_V1_EnumerateToArray | 10000 |   238,534.87 ns | 1,317.101 ns | 1,232.017 ns |  1.50 |    0.01 | 44.6777 | 14.8926 |       - |  211936 B |
|                   ImMap_FoldToArray | 10000 |   185,608.77 ns | 1,156.637 ns | 1,081.919 ns |  1.17 |    0.01 | 60.0586 | 29.2969 | 15.3809 |  342659 B |
| ImMap_FoldToArray_FoldReducerStruct | 10000 |   153,426.27 ns |   258.939 ns |   242.211 ns |  0.97 |    0.00 | 60.0586 | 29.2969 | 15.3809 |  342659 B |
|      Experimental_ImMap_FoldToArray | 10000 |   181,060.33 ns |   921.031 ns |   861.533 ns |  1.14 |    0.00 | 60.0586 | 29.5410 | 15.3809 |  342657 B |
|              ImMapSlots_FoldToArray | 10000 |   193,669.62 ns | 1,706.895 ns | 1,596.630 ns |  1.22 |    0.01 | 60.0586 | 29.7852 | 15.3809 |  342622 B |
| Experimental_ImMapSlots_FoldToArray | 10000 |   197,295.56 ns |   960.392 ns |   898.351 ns |  1.24 |    0.01 | 60.0586 | 29.7852 | 15.3809 |  342619 B |
|                    DictSlim_ToArray | 10000 |   219,924.53 ns | 1,328.065 ns | 1,242.273 ns |  1.39 |    0.01 | 50.2930 | 27.8320 | 22.7051 |  422944 B |
|                        Dict_ToArray | 10000 |    75,081.85 ns | 1,492.327 ns | 1,596.774 ns |  0.47 |    0.01 |  0.8545 |  0.8545 |  0.8545 |  160018 B |
|              ConcurrentDict_ToArray | 10000 |    76,812.19 ns |   472.741 ns |   442.202 ns |  0.48 |    0.00 |  7.3242 |  7.3242 |  7.3242 |  160013 B |
|               ImmutableDict_ToArray | 10000 | 1,567,955.86 ns | 3,414.536 ns | 3,193.959 ns |  9.88 |    0.03 | 25.3906 | 25.3906 | 25.3906 |  160041 B |

```


### ImHashMap of Type keys and small string values

#### ImHashMap Population

[The benchmark](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImHashMapBenchmarks.cs) inserts from 10 to 1000
items into the `ImHashMap<Type, string>`:

```md
|                     Method | Count |         Mean |       Error |      StdDev | Ratio | RatioSD |    Gen 0 |   Gen 1 | Gen 2 | Allocated |
|--------------------------- |------ |-------------:|------------:|------------:|------:|--------:|---------:|--------:|------:|----------:|
|      ImHashMap_AddOrUpdate |     1 |     120.9 ns |     1.00 ns |     0.93 ns |  1.00 |    0.00 |   0.0577 |       - |     - |     272 B |
|   ImHashMap_V1_AddOrUpdate |     1 |     132.3 ns |     2.64 ns |     2.71 ns |  1.10 |    0.02 |   0.0610 |       - |     - |     288 B |
| ImHashMapSlots_AddOrUpdate |     1 |     213.6 ns |     0.32 ns |     0.27 ns |  1.76 |    0.01 |   0.1070 |  0.0002 |     - |     504 B |
|            DictSlim_TryAdd |     1 |     125.8 ns |     0.79 ns |     0.66 ns |  1.04 |    0.01 |   0.0408 |       - |     - |     192 B |
|                Dict_TryAdd |     1 |     130.6 ns |     0.65 ns |     0.61 ns |  1.08 |    0.01 |   0.0544 |       - |     - |     256 B |
|      ConcurrentDict_TryAdd |     1 |     290.6 ns |     1.27 ns |     1.19 ns |  2.40 |    0.02 |   0.2074 |  0.0014 |     - |     976 B |
|  ImmutableDict_Builder_Add |     1 |     398.0 ns |     1.15 ns |     1.08 ns |  3.29 |    0.03 |   0.0577 |       - |     - |     272 B |
|                            |       |              |             |             |       |         |          |         |       |           |
|      ImHashMap_AddOrUpdate |    10 |     812.8 ns |     4.65 ns |     3.88 ns |  1.00 |    0.00 |   0.4911 |  0.0029 |     - |    2312 B |
|   ImHashMap_V1_AddOrUpdate |    10 |   1,017.5 ns |     3.90 ns |     3.65 ns |  1.25 |    0.01 |   0.6218 |  0.0038 |     - |    2928 B |
| ImHashMapSlots_AddOrUpdate |    10 |     595.5 ns |     1.75 ns |     1.46 ns |  0.73 |    0.00 |   0.2956 |  0.0019 |     - |    1392 B |
|            DictSlim_TryAdd |    10 |     556.0 ns |     2.24 ns |     2.09 ns |  0.68 |    0.01 |   0.2375 |  0.0010 |     - |    1120 B |
|                Dict_TryAdd |    10 |     604.9 ns |     1.78 ns |     1.57 ns |  0.74 |    0.00 |   0.2193 |  0.0010 |     - |    1032 B |
|      ConcurrentDict_TryAdd |    10 |   1,387.4 ns |     5.62 ns |     4.69 ns |  1.71 |    0.01 |   0.6294 |  0.0095 |     - |    2968 B |
|  ImmutableDict_Builder_Add |    10 |   2,505.4 ns |    25.19 ns |    22.33 ns |  3.08 |    0.03 |   0.1793 |       - |     - |     848 B |
|                            |       |              |             |             |       |         |          |         |       |           |
|      ImHashMap_AddOrUpdate |   100 |  12,489.6 ns |    51.63 ns |    48.29 ns |  1.00 |    0.00 |   7.4005 |  0.3510 |     - |   34856 B |
|   ImHashMap_V1_AddOrUpdate |   100 |  15,068.0 ns |   121.82 ns |    95.11 ns |  1.21 |    0.01 |   8.5449 |  0.4272 |     - |   40320 B |
| ImHashMapSlots_AddOrUpdate |   100 |   6,526.1 ns |    16.45 ns |    14.58 ns |  0.52 |    0.00 |   3.1052 |  0.2060 |     - |   14640 B |
|            DictSlim_TryAdd |   100 |   4,200.5 ns |    28.71 ns |    23.97 ns |  0.34 |    0.00 |   1.5945 |  0.0458 |     - |    7536 B |
|                Dict_TryAdd |   100 |   5,021.9 ns |    18.09 ns |    16.92 ns |  0.40 |    0.00 |   2.1667 |  0.0916 |     - |   10232 B |
|      ConcurrentDict_TryAdd |   100 |  15,519.5 ns |    68.27 ns |    63.86 ns |  1.24 |    0.01 |   6.5613 |  0.0305 |     - |   30944 B |
|  ImmutableDict_Builder_Add |   100 |  34,182.6 ns |   106.60 ns |    99.71 ns |  2.74 |    0.01 |   1.4038 |  0.0610 |     - |    6608 B |
|                            |       |              |             |             |       |         |          |         |       |           |
|      ImHashMap_AddOrUpdate |  1000 | 265,875.9 ns |   897.54 ns |   839.56 ns |  1.00 |    0.00 | 108.3984 | 30.7617 |     - |  511209 B |
|   ImHashMap_V1_AddOrUpdate |  1000 | 307,754.2 ns | 1,962.45 ns | 1,739.66 ns |  1.16 |    0.01 | 121.0938 | 35.1563 |     - |  571250 B |
| ImHashMapSlots_AddOrUpdate |  1000 | 155,827.3 ns |   537.71 ns |   502.97 ns |  0.59 |    0.00 |  57.3730 | 19.0430 |     - |  270866 B |
|            DictSlim_TryAdd |  1000 |  38,470.1 ns |   334.14 ns |   312.56 ns |  0.14 |    0.00 |  12.2681 |  0.0610 |     - |   57856 B |
|                Dict_TryAdd |  1000 |  50,979.3 ns |   165.64 ns |   154.94 ns |  0.19 |    0.00 |  21.6064 |  5.3711 |     - |  102256 B |
|      ConcurrentDict_TryAdd |  1000 | 174,180.3 ns | 3,453.65 ns | 3,061.57 ns |  0.65 |    0.01 |  48.8281 | 23.9258 |     - |  260009 B |
|  ImmutableDict_Builder_Add |  1000 | 501,694.1 ns | 2,097.12 ns | 1,961.65 ns |  1.89 |    0.01 |  12.6953 |  2.9297 |     - |   64209 B |
```

### ImHashMap Lookup

[The benchmark](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImHashMapBenchmarks.cs) lookups for the specific key in the 
`ImHashMap<Type, string>` containing the specified Count of elements.

```md

|                           Method | Count |      Mean |     Error |    StdDev | Ratio | RatioSD | Gen 0 | Gen 1 | Gen 2 | Allocated |
|--------------------------------- |------ |----------:|----------:|----------:|------:|--------:|------:|------:|------:|----------:|
|                ImHashMap_TryFind |     1 |  4.094 ns | 0.0632 ns | 0.0591 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|             ImHashMap_TryFind_V1 |     1 |  5.340 ns | 0.0315 ns | 0.0295 ns |  1.30 |    0.02 |     - |     - |     - |         - |
|           ImHashMapSlots_TryFind |     1 |  2.880 ns | 0.0316 ns | 0.0264 ns |  0.70 |    0.01 |     - |     - |     - |         - |
|       DictionarySlim_TryGetValue |     1 |  6.372 ns | 0.0207 ns | 0.0172 ns |  1.56 |    0.02 |     - |     - |     - |         - |
|           Dictionary_TryGetValue |     1 | 16.292 ns | 0.3957 ns | 0.3701 ns |  3.98 |    0.11 |     - |     - |     - |         - |
| ConcurrentDictionary_TryGetValue |     1 | 15.540 ns | 0.0507 ns | 0.0475 ns |  3.80 |    0.06 |     - |     - |     - |         - |
|             ImmutableDict_TryGet |     1 | 23.682 ns | 0.1103 ns | 0.0978 ns |  5.78 |    0.08 |     - |     - |     - |         - |
|                                  |       |           |           |           |       |         |       |       |       |           |
|                ImHashMap_TryFind |    10 |  5.840 ns | 0.0366 ns | 0.0325 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|             ImHashMap_TryFind_V1 |    10 |  6.560 ns | 0.0558 ns | 0.0522 ns |  1.12 |    0.01 |     - |     - |     - |         - |
|           ImHashMapSlots_TryFind |    10 |  2.947 ns | 0.0483 ns | 0.0452 ns |  0.50 |    0.01 |     - |     - |     - |         - |
|       DictionarySlim_TryGetValue |    10 |  6.567 ns | 0.0103 ns | 0.0096 ns |  1.12 |    0.01 |     - |     - |     - |         - |
|           Dictionary_TryGetValue |    10 | 18.852 ns | 0.3877 ns | 0.3626 ns |  3.23 |    0.07 |     - |     - |     - |         - |
| ConcurrentDictionary_TryGetValue |    10 | 15.723 ns | 0.0745 ns | 0.0660 ns |  2.69 |    0.01 |     - |     - |     - |         - |
|             ImmutableDict_TryGet |    10 | 25.695 ns | 0.1539 ns | 0.1440 ns |  4.40 |    0.03 |     - |     - |     - |         - |
|                                  |       |           |           |           |       |         |       |       |       |           |
|                ImHashMap_TryFind |   100 |  7.733 ns | 0.0366 ns | 0.0306 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|             ImHashMap_TryFind_V1 |   100 |  9.432 ns | 0.0590 ns | 0.0552 ns |  1.22 |    0.01 |     - |     - |     - |         - |
|           ImHashMapSlots_TryFind |   100 |  5.618 ns | 0.0106 ns | 0.0099 ns |  0.73 |    0.00 |     - |     - |     - |         - |
|       DictionarySlim_TryGetValue |   100 |  6.286 ns | 0.0638 ns | 0.0566 ns |  0.81 |    0.01 |     - |     - |     - |         - |
|           Dictionary_TryGetValue |   100 | 18.963 ns | 0.3079 ns | 0.2571 ns |  2.45 |    0.04 |     - |     - |     - |         - |
| ConcurrentDictionary_TryGetValue |   100 | 16.410 ns | 0.0628 ns | 0.0557 ns |  2.12 |    0.01 |     - |     - |     - |         - |
|             ImmutableDict_TryGet |   100 | 28.138 ns | 0.1037 ns | 0.0970 ns |  3.64 |    0.02 |     - |     - |     - |         - |
|                                  |       |           |           |           |       |         |       |       |       |           |
|                ImHashMap_TryFind |  1000 | 12.019 ns | 0.0553 ns | 0.0518 ns |  1.00 |    0.00 |     - |     - |     - |         - |
|             ImHashMap_TryFind_V1 |  1000 | 12.757 ns | 0.0509 ns | 0.0425 ns |  1.06 |    0.01 |     - |     - |     - |         - |
|           ImHashMapSlots_TryFind |  1000 |  9.052 ns | 0.0212 ns | 0.0198 ns |  0.75 |    0.00 |     - |     - |     - |         - |
|       DictionarySlim_TryGetValue |  1000 |  6.293 ns | 0.0627 ns | 0.0586 ns |  0.52 |    0.01 |     - |     - |     - |         - |
|           Dictionary_TryGetValue |  1000 | 16.499 ns | 0.0173 ns | 0.0161 ns |  1.37 |    0.01 |     - |     - |     - |         - |
| ConcurrentDictionary_TryGetValue |  1000 | 16.311 ns | 0.1058 ns | 0.0990 ns |  1.36 |    0.00 |     - |     - |     - |         - |
|             ImmutableDict_TryGet |  1000 | 30.316 ns | 0.5058 ns | 0.4731 ns |  2.52 |    0.04 |     - |     - |     - |         - |
```
    
### ImHashMap Enumeration


[The benchmark source](https://github.com/dadhi/ImTools/blob/master/playground/ImTools.Benchmarks/ImHashMapBenchmarks.cs)

```md
|                        Method | Count |          Mean |        Error |       StdDev | Ratio | RatioSD |   Gen 0 |  Gen 1 | Gen 2 | Allocated |
|------------------------------ |------ |--------------:|-------------:|-------------:|------:|--------:|--------:|-------:|------:|----------:|
|    ImHashMap_EnumerateToArray |     1 |     151.24 ns |     1.077 ns |     1.008 ns |  1.00 |    0.00 |  0.0441 |      - |     - |     208 B |
| ImHashMap_V1_EnumerateToArray |     1 |     160.10 ns |     1.006 ns |     0.840 ns |  1.06 |    0.01 |  0.0560 |      - |     - |     264 B |
|         ImHashMap_FoldToArray |     1 |      56.37 ns |     0.115 ns |     0.096 ns |  0.37 |    0.00 |  0.0356 |      - |     - |     168 B |
|    ImHashMapSlots_FoldToArray |     1 |      71.05 ns |     0.351 ns |     0.328 ns |  0.47 |    0.00 |  0.0271 |      - |     - |     128 B |
|        DictionarySlim_ToArray |     1 |     141.78 ns |     0.651 ns |     0.577 ns |  0.94 |    0.01 |  0.0405 |      - |     - |     192 B |
|            Dictionary_ToArray |     1 |      39.32 ns |     0.178 ns |     0.158 ns |  0.26 |    0.00 |  0.0119 |      - |     - |      56 B |
|  ConcurrentDictionary_ToArray |     1 |     228.28 ns |     0.390 ns |     0.365 ns |  1.51 |    0.01 |  0.0117 |      - |     - |      56 B |
|         ImmutableDict_ToArray |     1 |     612.37 ns |     4.531 ns |     4.238 ns |  4.05 |    0.03 |  0.0114 |      - |     - |      56 B |
|                               |       |               |              |              |       |         |         |        |       |           |
|    ImHashMap_EnumerateToArray |    10 |     406.74 ns |     1.278 ns |     1.067 ns |  1.00 |    0.00 |  0.1001 |      - |     - |     472 B |
| ImHashMap_V1_EnumerateToArray |    10 |     473.14 ns |     3.844 ns |     3.408 ns |  1.16 |    0.01 |  0.1726 |      - |     - |     816 B |
|         ImHashMap_FoldToArray |    10 |     208.65 ns |     0.615 ns |     0.575 ns |  0.51 |    0.00 |  0.1054 |      - |     - |     496 B |
|    ImHashMapSlots_FoldToArray |    10 |     234.16 ns |     1.495 ns |     1.325 ns |  0.58 |    0.00 |  0.1016 |      - |     - |     480 B |
|        DictionarySlim_ToArray |    10 |     424.86 ns |     0.893 ns |     0.791 ns |  1.04 |    0.00 |  0.1359 |      - |     - |     640 B |
|            Dictionary_ToArray |    10 |      87.90 ns |     0.570 ns |     0.505 ns |  0.22 |    0.00 |  0.0424 |      - |     - |     200 B |
|  ConcurrentDictionary_ToArray |    10 |     507.57 ns |     5.354 ns |     5.008 ns |  1.25 |    0.01 |  0.0420 |      - |     - |     200 B |
|         ImmutableDict_ToArray |    10 |   2,041.72 ns |     5.804 ns |     5.145 ns |  5.02 |    0.01 |  0.0381 |      - |     - |     200 B |
|                               |       |               |              |              |       |         |         |        |       |           |
|    ImHashMap_EnumerateToArray |   100 |   2,892.03 ns |    17.595 ns |    16.458 ns |  1.00 |    0.00 |  0.4768 |      - |     - |    2248 B |
| ImHashMap_V1_EnumerateToArray |   100 |   3,539.25 ns |    12.053 ns |    11.274 ns |  1.22 |    0.01 |  1.1597 | 0.0267 |     - |    5472 B |
|         ImHashMap_FoldToArray |   100 |   1,456.77 ns |     7.952 ns |     6.641 ns |  0.50 |    0.00 |  0.6599 | 0.0038 |     - |    3112 B |
|    ImHashMapSlots_FoldToArray |   100 |   1,537.36 ns |     8.437 ns |     7.045 ns |  0.53 |    0.00 |  0.6733 | 0.0038 |     - |    3168 B |
|        DictionarySlim_ToArray |   100 |   2,538.43 ns |    15.330 ns |    14.340 ns |  0.88 |    0.01 |  0.8469 | 0.0076 |     - |    4000 B |
|            Dictionary_ToArray |   100 |     658.39 ns |     2.193 ns |     2.051 ns |  0.23 |    0.00 |  0.3481 | 0.0019 |     - |    1640 B |
|  ConcurrentDictionary_ToArray |   100 |   2,288.25 ns |     7.825 ns |     7.320 ns |  0.79 |    0.01 |  0.3471 |      - |     - |    1640 B |
|         ImmutableDict_ToArray |   100 |  16,009.36 ns |    46.706 ns |    41.404 ns |  5.53 |    0.03 |  0.3357 |      - |     - |    1640 B |
|                               |       |               |              |              |       |         |         |        |       |           |
|    ImHashMap_EnumerateToArray |  1000 |  27,630.71 ns |   160.441 ns |   150.077 ns |  1.00 |    0.00 |  3.5706 | 0.1526 |     - |   16808 B |
| ImHashMap_V1_EnumerateToArray |  1000 |  34,720.95 ns |    85.476 ns |    79.954 ns |  1.26 |    0.01 | 10.3149 | 1.7090 |     - |   48832 B |
|         ImHashMap_FoldToArray |  1000 |  14,661.14 ns |    64.614 ns |    57.279 ns |  0.53 |    0.00 |  5.2490 | 0.3204 |     - |   24752 B |
|    ImHashMapSlots_FoldToArray |  1000 |  15,407.60 ns |    25.918 ns |    21.643 ns |  0.56 |    0.00 |  5.2490 | 0.2747 |     - |   24784 B |
|        DictionarySlim_ToArray |  1000 |  22,157.65 ns |    40.801 ns |    36.169 ns |  0.80 |    0.01 |  6.9885 | 0.6714 |     - |   32896 B |
|            Dictionary_ToArray |  1000 |   6,235.23 ns |    24.355 ns |    21.590 ns |  0.23 |    0.00 |  3.3951 | 0.1831 |     - |   16040 B |
|  ConcurrentDictionary_ToArray |  1000 |  38,018.12 ns |   173.550 ns |   162.339 ns |  1.38 |    0.01 |  3.3569 | 0.1831 |     - |   16040 B |
|         ImmutableDict_ToArray |  1000 | 158,162.55 ns | 2,083.502 ns | 1,948.910 ns |  5.72 |    0.08 |  3.1738 |      - |     - |   16041 B |
```


## End-to-end Example

Let's assume you are implementing yet another DI container because why not :-)

Container should contain registry of `Type` to `Factory` mappings. 
On resolution `Factory` is compiled to the delegate which you would like to cache, because compilation is costly. 
The cache will store the mappings from `Type` to `Func<object>`.

__The requirements:__

- The container may be used in parallel from different threads including registrations and resolutions. 
- The container state should not be corrupted and the cache should correspond to the current state of registrations.

Let's design the basic container structure to support the requirements and __without locking__:

```cs
    public class Container
    {
        private readonly Ref<Registry> _registry = Ref.Of(new Registry());

        public void Register<TService, TImpl>() where TImpl : TService, new()
        {
            _registry.Swap(reg => reg.With(typeof(TService), new Factory(typeof(TImpl))));
        }

        public object Resolve<TService>()
        {
            return (TService)(_registry.Value.Resolve(typeof(TService)) ?? ThrowUnableToResolve(typeof(TService)));
        }
        
        public object ThrowUnableToResolve(Type t) { throw new InvalidOperationException("Unable to resolve: " + t); }

        class Registry 
        {
            ImHashMap<Type, Factory> _registrations = ImHashMap<Type, Factory>.Empty;
            Ref<ImHashMap<Type, Func<object>>> _resolutionCache = Ref.Of(ImHashMap<Type, Func<object>>.Empty);

            // Creating a new registry with +1 registration and new reference to cache value
            public Registry With(Type serviceType, Factory implFactory)
            {
                return new Registry() 
                {	
                    _registrations = _registrations.AddOrUpdate(serviceType, implFactory),
                        
                    // Here is most interesting part:
                    // We are creating new independent reference pointing to cache value,
                    // isolating it from possible parallel resolutions, 
                    // which will swap older version/ref of cache and won't touch the new one.
                    _resolutionCache = Ref.Of(_resolutionCache.Value)
                };
            }

            public object Resolve(Type serviceType)
            {
                var func = _resolutionCache.Value.GetValueOrDefault(serviceType);
                if (func != null)
                    return func();

                var reg = _registrations.GetValueOrDefault(serviceType);
                if (reg == null)
                    return null;
                
                func = reg.CompileDelegate();
                _resolutionCache.Swap(cache => cache.AddOrUpdate(serviceType, func));
                return func.Invoke();
            }
        }
        
        class Factory 
        {
            public readonly Type ImplType;
            public Factory(Type implType) { ImplType = implType; }
            public Func<object> CompileDelegate() { return () => Activator.CreateInstance(ImplType); }
        } 
    }
```
