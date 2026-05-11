using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public sealed class RedBallBuildWorkspace : EditorWindow
{
    private const string ScenePath = "Assets/Scenes/RedBall.unity";
    private const string BuildPath = "Builds/iOS/RedBall-iOS";
    private const string BundleId = "com.yusufbayindir.redball";
    private const string TeamId = "P8C3928J3Z";
    private const string WorkspaceName = "Unity-iPhone.xcworkspace";

    [MenuItem("RedBall/Build Workspace %#b", priority = 0)]
    public static void BuildWorkspaceMenu()
    {
        BuildIOSWorkspace(true);
    }

    [MenuItem("RedBall/Open Build Panel", priority = 1)]
    public static void OpenWindow()
    {
        var window = GetWindow<RedBallBuildWorkspace>("RedBall");
        window.minSize = new Vector2(360f, 170f);
        window.Show();
    }

    public static void BuildIOSFromCommandLine()
    {
        BuildIOSWorkspace(false);
    }

    private void OnGUI()
    {
        GUILayout.Space(10f);
        EditorGUILayout.LabelField("RedBall iOS", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Build Workspace tusu iOS icin Xcode workspace'i uretir, otomatik imzayi YUSUF BAYINDIR team'i ile ayarlar ve Xcode'da acar.", MessageType.Info);

        GUILayout.Space(8f);
        if (GUILayout.Button("Build Workspace", GUILayout.Height(44f)))
        {
            BuildIOSWorkspace(true);
        }

        GUILayout.Space(6f);
        if (GUILayout.Button("Open Build Workspace", GUILayout.Height(28f)))
        {
            OpenBuildWorkspace();
        }
    }

    private static void BuildIOSWorkspace(bool openWhenDone)
    {
        ConfigurePlayerSettings();
        EnsureSceneInBuildSettings();

        if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.iOS)
        {
            if (!Application.isBatchMode)
            {
                EditorUtility.DisplayProgressBar("RedBall", "iOS platformuna geciliyor...", 0.15f);
            }

            bool switched = EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.iOS, BuildTarget.iOS);

            if (!Application.isBatchMode)
            {
                EditorUtility.ClearProgressBar();
            }

            if (!switched)
            {
                const string message = "iOS build target'a gecilemedi. Unity Hub'dan iOS Build Support modulunun kurulu oldugunu kontrol et.";
                if (Application.isBatchMode)
                {
                    throw new BuildFailedException(message);
                }

                Debug.LogError(message);
                return;
            }
        }

        EnsureCleanBuildFolder();

        var options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            target = BuildTarget.iOS,
            locationPathName = BuildPath,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        BuildSummary summary = report.summary;

        if (summary.result == BuildResult.Succeeded)
        {
            PatchXcodeSigning();
            EnsureWorkspace();
            Debug.Log($"RedBall iOS workspace hazir: {Path.GetFullPath(GetWorkspacePath())} ({summary.totalSize / (1024f * 1024f):0.0} MB)");
            if (openWhenDone)
            {
                OpenBuildWorkspace();
            }
        }
        else
        {
            string message = $"RedBall iOS build basarisiz: {summary.result}";
            Debug.LogError(message);
            if (Application.isBatchMode)
            {
                throw new BuildFailedException(message);
            }

        }
    }

    private static void ConfigurePlayerSettings()
    {
        PlayerSettings.companyName = "Yusuf Bayindir";
        PlayerSettings.productName = "RedBall";
        PlayerSettings.bundleVersion = "1.0";
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, BundleId);
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        PlayerSettings.iOS.appleDeveloperTeamID = TeamId;
    }

    private static void EnsureSceneInBuildSettings()
    {
        EditorBuildSettings.scenes = new[]
        {
            new EditorBuildSettingsScene(ScenePath, true)
        };
    }

    private static void EnsureCleanBuildFolder()
    {
        if (Directory.Exists(BuildPath))
        {
            FileUtil.DeleteFileOrDirectory(BuildPath);
            FileUtil.DeleteFileOrDirectory(BuildPath + ".meta");
        }

        Directory.CreateDirectory(BuildPath);
    }

    private static void PatchXcodeSigning()
    {
        string projectPath = PBXProject.GetPBXProjectPath(BuildPath);
        var project = new PBXProject();
        project.ReadFromFile(projectPath);

        string mainTarget = project.GetUnityMainTargetGuid();
        string frameworkTarget = project.GetUnityFrameworkTargetGuid();
        ConfigureTargetSigning(project, mainTarget, BundleId);
        ConfigureTargetSigning(project, frameworkTarget, string.Empty);
        project.WriteToFile(projectPath);

        PatchProvisioningStyle(projectPath);
    }

    private static void ConfigureTargetSigning(PBXProject project, string targetGuid, string bundleId)
    {
        if (string.IsNullOrEmpty(targetGuid))
        {
            return;
        }

        project.SetTeamId(targetGuid, TeamId);
        project.SetBuildProperty(targetGuid, "CODE_SIGN_STYLE", "Automatic");
        project.SetBuildProperty(targetGuid, "DEVELOPMENT_TEAM", TeamId);
        project.SetBuildProperty(targetGuid, "CODE_SIGN_IDENTITY", "Apple Development");
        project.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE_SPECIFIER", string.Empty);
        project.SetBuildProperty(targetGuid, "PROVISIONING_PROFILE", string.Empty);

        if (!string.IsNullOrEmpty(bundleId))
        {
            project.SetBuildProperty(targetGuid, "PRODUCT_BUNDLE_IDENTIFIER", bundleId);
        }
    }

    private static void PatchProvisioningStyle(string projectPath)
    {
        string text = File.ReadAllText(projectPath);
        text = text.Replace("ProvisioningStyle = Manual;", "ProvisioningStyle = Automatic;");
        File.WriteAllText(projectPath, text);
    }

    private static void EnsureWorkspace()
    {
        string workspacePath = GetWorkspacePath();
        string contentsPath = Path.Combine(workspacePath, "contents.xcworkspacedata");
        Directory.CreateDirectory(workspacePath);

        if (!File.Exists(contentsPath))
        {
            File.WriteAllText(contentsPath,
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?>\n" +
                "<Workspace version=\"1.0\">\n" +
                "   <FileRef location=\"group:Unity-iPhone.xcodeproj\"></FileRef>\n" +
                "</Workspace>\n");
        }
    }

    private static void OpenBuildWorkspace()
    {
        EnsureWorkspace();
        string fullPath = Path.GetFullPath(GetWorkspacePath());
        var process = new System.Diagnostics.ProcessStartInfo
        {
            FileName = "open",
            Arguments = Quote(fullPath),
            UseShellExecute = false
        };
        System.Diagnostics.Process.Start(process);
    }

    private static string GetWorkspacePath()
    {
        return Path.Combine(BuildPath, WorkspaceName);
    }

    private static string Quote(string value)
    {
        return "\"" + value.Replace("\"", "\\\"") + "\"";
    }
}
