using BenchmarkDotNet.Running;
using EfCoreDemo.Benchmarks;

// BenchmarkRunner.Run<SimpleQueryBenchmarks>();
BenchmarkRunner.Run<JoinedQueryBenchmarks>();