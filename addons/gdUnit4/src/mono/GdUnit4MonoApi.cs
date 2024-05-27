using Godot;
using Godot.Collections;

// GdUnit4 C# API wrapper
public partial class GdUnit4MonoApi : GdUnit4.GdUnit4NetAPI
{
	public new string Version() => GdUnit4.GdUnit4NetAPI.Version();
	
	public new bool IsTestSuite(string classPath) => GdUnit4.GdUnit4NetAPI.IsTestSuite(classPath);
	
	public new RefCounted Executor(Node listener) => (RefCounted)GdUnit4.GdUnit4NetAPI.Executor(listener);
	
	public new GdUnit4.CsNode? ParseTestSuite(string classPath) => GdUnit4.GdUnit4NetAPI.ParseTestSuite(classPath);
	
	public new Dictionary CreateTestSuite(string sourcePath, int lineNumber, string testSuitePath) =>
		GdUnit4.GdUnit4NetAPI.CreateTestSuite(sourcePath, lineNumber, testSuitePath);
}
