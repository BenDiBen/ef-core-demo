```

BenchmarkDotNet v0.14.0, Windows 11 (10.0.26100.3775)
13th Gen Intel Core i7-13700KF, 1 CPU, 24 logical and 16 physical cores
.NET SDK 9.0.203
  [Host]     : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2
  DefaultJob : .NET 9.0.4 (9.0.425.16305), X64 RyuJIT AVX2


```
| Method                                | Job        | Toolchain              | Mean     | Error     | StdDev    |
|-------------------------------------- |----------- |----------------------- |---------:|----------:|----------:|
| GetAllCustomersToEmailNaive           | DefaultJob | Default                | 1.224 ms | 0.0224 ms | 0.0314 ms |
| GetAllCustomersToEmailNoTracking      | DefaultJob | Default                | 1.218 ms | 0.0237 ms | 0.0273 ms |
| GetAllCustomersToEmailPreFiltered     | DefaultJob | Default                | 1.218 ms | 0.0181 ms | 0.0169 ms |
| GetAllCustomersToEmailPreFilteredWith | DefaultJob | Default                | 1.236 ms | 0.0230 ms | 0.0384 ms |
| GetAllCustomersToEmailNaive           | InProcess  | InProcessEmitToolchain | 1.187 ms | 0.0233 ms | 0.0303 ms |
| GetAllCustomersToEmailNoTracking      | InProcess  | InProcessEmitToolchain | 1.230 ms | 0.0242 ms | 0.0376 ms |
| GetAllCustomersToEmailPreFiltered     | InProcess  | InProcessEmitToolchain | 1.218 ms | 0.0193 ms | 0.0161 ms |
| GetAllCustomersToEmailPreFilteredWith | InProcess  | InProcessEmitToolchain | 1.200 ms | 0.0149 ms | 0.0159 ms |
