using Xunit;
using VerifyIG = InteropGenerator.Tests.Helpers.IncrementalGeneratorVerifier<InteropGenerator.Generator.InteropGenerator>;

namespace InteropGenerator.Tests.Generator;

public class GeneralInteropAttributeTests {
    [Fact]
    public async Task NoMarkerAttribute() {
        const string code = """
                            public partial struct TestStruct
                            {
                            }
                            """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            []);
    }

    [Fact]
    public async Task MarkerAttribute() {
        const string code = """
                            [GenerateInterop]
                            public partial struct TestStruct
                            {
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task StructWithNamespace() {
        const string code = """
                            namespace TestNamespace.InnerNamespace;

                            [GenerateInterop]
                            public partial struct TestStruct
                            {
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              namespace TestNamespace.InnerNamespace;

                              unsafe partial struct TestStruct
                              {
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestNamespace.InnerNamespace.TestStruct.InteropGenerator.g.cs", result));
    }

    [Fact]
    public async Task NestedStruct() {
        const string code = """
                            public partial struct TestStruct
                            {
                                [GenerateInterop]
                                public partial struct InnerStruct
                                {
                                }
                            }
                            """;

        const string result = """
                              // <auto-generated/>
                              unsafe partial struct TestStruct
                              {
                                  unsafe partial struct InnerStruct
                                  {
                                  }
                              }
                              """;

        await VerifyIG.VerifyGeneratorAsync(
            code,
            ("TestStruct+InnerStruct.InteropGenerator.g.cs", result));
    }
}
