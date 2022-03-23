using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Toolchains.CsProj;
using BenchmarkDotNet.Toolchains.DotNetCli;

var config = DefaultConfig.Instance
    .AddColumn( StatisticColumn.Max )
    .AddColumn( StatisticColumn.Min )
    .AddDiagnoser( MemoryDiagnoser.Default )
    .AddDiagnoser( ThreadingDiagnoser.Default )
    .AddExporter( MarkdownExporter.GitHub )
    .AddExporter( DefaultExporters.JsonFullCompressed )
    .AddJob(
        Job.ShortRun
            .AsBaseline()
            .WithId( "default" )
            .WithJit( Jit.RyuJit )
            .WithPlatform( Platform.AnyCpu )
    )
    .AddJob(
        Job.RyuJitX64
            .UnfreezeCopy()
            .ApplyAndFreeze( Job.ShortRun )
            .WithId( "x64" )
            .WithToolchain(
                CsProjCoreToolchain.From(
                    NetCoreAppSettings.NetCoreApp60
                          .WithCustomDotNetCliPath( @"C:\Program Files\dotnet\dotnet.exe", ".NET (x64)" )
                )
            )
    )
    .AddJob(
        Job.RyuJitX86
            .UnfreezeCopy()
            .ApplyAndFreeze( Job.ShortRun )
            .WithId( "x86" )
            .WithToolchain(
                CsProjCoreToolchain.From(
                    NetCoreAppSettings.NetCoreApp60
                          .WithCustomDotNetCliPath( @"C:\Program Files (x86)\dotnet\dotnet.exe", ".NET (x86)" )
                )
            )
    )
    .AddLogger( ConsoleLogger.Unicode );

BenchmarkSwitcher.FromAssembly( typeof( Program ).Assembly )
    .Run( args, config );
